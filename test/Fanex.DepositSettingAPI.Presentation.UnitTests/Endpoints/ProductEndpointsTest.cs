namespace Fanex.DepositSettingAPI.Presentation.UnitTests.Endpoints;

using Application.Queries;
using AutoFixture;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;
using DTOs.Response;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Presentation.Endpoints;

public class ProductEndpointsTest
{
    private readonly IFixture _fixture = new Fixture();
    private readonly IMediator _mediator = Substitute.For<IMediator>();

    [Fact]
    public async Task GetProductStatus_Should_ReturnSuccess()
    {
        // Arrange
        int productId = _fixture.Create<int>();
        int agentId = _fixture.Create<int>();
        var productStatus = _fixture.Create<ProductStatus>();
        _mediator
            .Send(Arg.Any<GetProductStatusQuery>())
            .Returns(ServiceResult<ProductStatus>.Success(productStatus));

        // Action
        var result = await ProductEndpoints.GetProductStatus(productId, agentId, _mediator);
        var okResult = (Ok<ProductStatusResponse>)result;

        // Assert
        okResult.StatusCode.Should().Be(200);
        okResult.Value.ErrorCode.Should().Be(0);
        okResult.Value.ErrorMessage.Should().BeEmpty();
        okResult.Value.Result.Should().Be(productStatus);
    }

    [Fact]
    public async Task GetTicketInformation_Should_ReturnFailure()
    {
        // Arrange
        int productId = _fixture.Create<int>();
        int agentId = _fixture.Create<int>();
        _mediator
            .Send(Arg.Any<GetProductStatusQuery>())
            .Returns(ServiceResult<ProductStatus>.Failure(ServiceErrors.SystemError));

        // Action
        var result = await ProductEndpoints.GetProductStatus(productId, agentId, _mediator);
        var okResult = (Ok<ProductStatusResponse>)result;

        // Assert
        okResult.StatusCode.Should().Be(200);
        okResult.Value.ErrorCode.Should().Be(ServiceErrors.SystemError.Code);
        okResult.Value.ErrorMessage.Should().Be(ServiceErrors.SystemError.Message);
        okResult.Value.Result.Should().BeNull();
    }
}