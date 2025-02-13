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

public class BetSettingEndpointsTest
{
    private readonly IFixture _fixture = new Fixture();
    private readonly IMediator _mediator = Substitute.For<IMediator>();

    [Fact]
    public async Task GetBetSetting_Should_ReturnSuccess()
    {
        // Arrange
        int agentId = _fixture.Create<int>();
        var betSetting = _fixture.Create<BetSetting>();
        _mediator
            .Send(Arg.Any<GetBetSettingQuery>())
            .Returns(ServiceResult<BetSetting>.Success(betSetting));

        // Action
        var result = await BetSettingEndpoints.GetBetSetting(agentId, _mediator);
        var okResult = (Ok<BetSettingResponse>)result;

        // Assert
        okResult.StatusCode.Should().Be(200);
        okResult.Value!.ErrorCode.Should().Be(0);
        okResult.Value.ErrorMessage.Should().BeEmpty();
        okResult.Value.Result.Should().Be(betSetting);
    }

    [Fact]
    public async Task GetBetSetting_Should_ReturnFailure()
    {
        // Arrange
        int agentId = _fixture.Create<int>();
        _mediator
            .Send(Arg.Any<GetBetSettingQuery>())
            .Returns(ServiceResult<BetSetting>.Failure(ServiceErrors.SystemError));

        // Action
        var result = await BetSettingEndpoints.GetBetSetting(agentId, _mediator);
        var okResult = (Ok<BetSettingResponse>)result;

        // Assert
        okResult.StatusCode.Should().Be(200);
        okResult.Value!.ErrorCode.Should().Be(ServiceErrors.SystemError.Code);
        okResult.Value.ErrorMessage.Should().Be(ServiceErrors.SystemError.Message);
        okResult.Value.Result.Should().BeNull();
    }
}