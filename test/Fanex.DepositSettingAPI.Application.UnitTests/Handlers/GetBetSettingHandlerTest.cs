namespace Fanex.DepositSettingAPI.Application.UnitTests.Handlers;

using Application.Handlers;
using AutoFixture;
using Constants;
using Domain.Entities;
using Domain.Errors;
using Domain.Options;
using Domain.Params;
using Domain.Repositories;
using Domain.Results;
using FluentAssertions;
using Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NSubstitute;
using Queries;

public class GetBetSettingHandlerTest : IDisposable
{
    private readonly IOptions<AppSetting> _appSetting;
    private readonly IBetSettingRepository _betSettingRepository;

    private readonly IFixture _fixture;
    private readonly GetBetSettingHandler _handler;
    private readonly ILogger _logger;
    private readonly IServerDbConfigRepository _serverDbConfigRepository;

    public GetBetSettingHandlerTest()
    {
        _appSetting = Substitute.For<IOptions<AppSetting>>();
        _logger = Substitute.For<ILogger>();
        _fixture = new Fixture();
        _serverDbConfigRepository = Substitute.For<IServerDbConfigRepository>();
        _betSettingRepository = Substitute.For<IBetSettingRepository>();
        _handler = new GetBetSettingHandler(
            _appSetting,
            _logger,
            Substitute.For<IHttpContextAccessor>(),
            _serverDbConfigRepository,
            _betSettingRepository);
        _appSetting.Value.Returns(new AppSetting { Environment = "Test" });
    }

    public void Dispose()
    {
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenNotHaveServerDbConfig()
    {
        // Arrange
        var query = _fixture.Create<GetBetSettingQuery>();
        _serverDbConfigRepository.GetServerDbConfigByAgentIdAsync(query.AgentId).Returns((ServerDbConfig)null!);

        // Action
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(BetSettingErrors.InvalidAgentId);
        await _betSettingRepository.DidNotReceive().GetBetSettingAsync(Arg.Any<GetBetSettingParam>());
    }

    [Theory]
    [InlineData("LOCAL")]
    [InlineData("SIT")]
    [InlineData("UAT")]
    [InlineData("PROD")]
    [InlineData("KLDR")]
    public async Task Handle_Should_CallGetBetSettingAsyncCorrect(string environment)
    {
        // Arrange
        var query = _fixture.Create<GetBetSettingQuery>();
        var data = _fixture.Create<BetSetting>();
        var settingStatus = string.Equals(environment, "PROD", StringComparison.OrdinalIgnoreCase)
            ? SettingStatus.ExcludeDataTest
            : SettingStatus.IncludeDataTest;
        _appSetting.Value.Returns(new AppSetting { Environment = environment });
        _serverDbConfigRepository.GetServerDbConfigByAgentIdAsync(query.AgentId).Returns(new ServerDbConfig());
        _betSettingRepository
            .GetBetSettingAsync(Arg.Is<GetBetSettingParam>(x => x.AgentId == query.AgentId && x.SettingStatus == settingStatus))
            .Returns(ServiceResult<BetSetting>.Success(data));

        // Action
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Result.Should().BeEquivalentTo(data);
        await _betSettingRepository.Received(1)
            .GetBetSettingAsync(Arg.Is<GetBetSettingParam>(x => x.AgentId == query.AgentId && x.SettingStatus == settingStatus));
    }

    [Fact]
    public async Task Handle_Should_ReturnCorrectData()
    {
        // Arrange
        var query = _fixture.Create<GetBetSettingQuery>();
        var data = _fixture.Create<BetSetting>();
        _serverDbConfigRepository.GetServerDbConfigByAgentIdAsync(query.AgentId).Returns(new ServerDbConfig());
        _betSettingRepository
            .GetBetSettingAsync(Arg.Is<GetBetSettingParam>(x => x.AgentId == query.AgentId))
            .Returns(ServiceResult<BetSetting>.Success(data));

        // Action
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Result.Should().BeEquivalentTo(data);
        await _betSettingRepository.Received(1).GetBetSettingAsync(Arg.Is<GetBetSettingParam>(x => x.AgentId == query.AgentId));
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorAndLoggingError_WhenErrorHasOccurred()
    {
        // Arrange
        var exception = new Exception("Testing");
        var query = _fixture.Create<GetBetSettingQuery>();

        _serverDbConfigRepository
            .When(x => x.GetServerDbConfigByAgentIdAsync(query.AgentId))
            .Do(x => throw exception);

        // Action
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(ServiceErrors.SystemError);
        await _logger.Received(1).ErrorAsync(Arg.Any<string>());
    }
}