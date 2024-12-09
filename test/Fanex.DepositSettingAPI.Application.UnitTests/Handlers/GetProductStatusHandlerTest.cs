namespace Fanex.DepositSettingAPI.Application.UnitTests.Handlers;

using Application.Handlers;
using AutoFixture;
using Constants;
using Domain.Entities;
using Domain.Errors;
using Domain.Params;
using Domain.Repositories;
using FluentAssertions;
using Logging;
using NSubstitute;
using Queries;
using Services;

public class GetProductStatusHandlerTest : IDisposable
{
    private readonly ICacheProvider _cacheProvider;
    private readonly IFixture _fixture;
    private readonly GetProductStatusHandler _handler;
    private readonly ILogger _logger;
    private readonly IProductRepository _productRepository;

    public GetProductStatusHandlerTest()
    {
        _logger = Substitute.For<ILogger>();
        _productRepository = Substitute.For<IProductRepository>();
        _cacheProvider = Substitute.For<ICacheProvider>();
        _fixture = new Fixture();

        _handler = new GetProductStatusHandler(_logger, _productRepository, _cacheProvider);
    }

    public void Dispose()
    {
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Handle_Should_ReturnError_WhenProductIdIsNotSupported(int productId)
    {
        // Arrange
        var query = _fixture.Build<GetProductStatusQuery>().With(x => x.ProductId, productId).Create();

        // Action
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(ProductStatusErrors.NotSupported);
        await _productRepository.DidNotReceive().GetProductStatusAsync(Arg.Any<GetProductStatusParam>());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_Should_NotCallGetProductStatusAsync_WhenHitCache(bool isEnabled)
    {
        var query = _fixture.Build<GetProductStatusQuery>().With(x => x.ProductId, 25).Create();
        var data = new ProductStatus
        {
            AgentId = query.AgentId,
            ProductId = query.ProductId,
            IsEnabled = isEnabled
        };
        _cacheProvider
            .GetValueFromCache(CacheKeys.ProductStatus(query), Arg.Any<ProductStatus>())
            .Returns(data);

        // Action
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Result.Should().BeEquivalentTo(data);
        await _productRepository.DidNotReceive().GetProductStatusAsync(Arg.Any<GetProductStatusParam>());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_Should_CallGetProductStatusAsync_WhenNotHitCache(bool isEnabled)
    {
        // Arrange
        var query = _fixture.Build<GetProductStatusQuery>().With(x => x.ProductId, 25).Create();
        var data = new ProductStatus
        {
            AgentId = query.AgentId,
            ProductId = query.ProductId,
            IsEnabled = isEnabled
        };
        _cacheProvider
            .GetValueFromCache(CacheKeys.ProductStatus(query), Arg.Any<ProductStatus>())
            .Returns((ProductStatus)null!);

        _cacheProvider
            .GetOrCreateFromCacheAsync(CacheKeys.ProductStatus(query), Arg.Any<Func<Task<ProductStatus>>>(), Arg.Any<TimeSpan>())
            .Returns(async callInfo => await ((Func<Task<ProductStatus>>)callInfo[1])());

        _productRepository
            .GetProductStatusAsync(Arg.Is<GetProductStatusParam>(x =>
                x.AgentId == query.AgentId && x.ProductId == query.ProductId))
            .Returns(data);

        // Action
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Result.Should().BeEquivalentTo(data);
        await _productRepository.Received(1).GetProductStatusAsync(Arg.Is<GetProductStatusParam>(x =>
            x.AgentId == query.AgentId && x.ProductId == query.ProductId));
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorAndLoggingError_WhenErrorHasOccurred()
    {
        // Arrange
        var exception = new Exception("Testing");
        var query = _fixture.Build<GetProductStatusQuery>().With(x => x.ProductId, 25).Create();

        _cacheProvider
            .GetValueFromCache(CacheKeys.ProductStatus(query), Arg.Any<ProductStatus>())
            .Returns((ProductStatus)null!);

        _cacheProvider
            .GetOrCreateFromCacheAsync(CacheKeys.ProductStatus(query), Arg.Any<Func<Task<ProductStatus>>>(), Arg.Any<TimeSpan>())
            .Returns(async callInfo => await ((Func<Task<ProductStatus>>)callInfo[1])());

        _productRepository
            .When(x => x.GetProductStatusAsync(Arg.Is<GetProductStatusParam>(
                param => param.AgentId == query.AgentId && param.ProductId == query.ProductId)))
            .Do(x => throw exception);

        // Action
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(ServiceErrors.SystemError);
        await _logger.Received(1).ErrorAsync(Arg.Any<string>());
    }
}