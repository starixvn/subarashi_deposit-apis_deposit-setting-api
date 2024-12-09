namespace Fanex.DepositSettingAPI.Presentation.UnitTests.Filters;

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Presentation.Filters;

public class ValidatorProductStatusFilterTest
{
    private readonly EndpointFilterDelegate _filterDelegate = Substitute.For<EndpointFilterDelegate>();
    private readonly EndpointFilterInvocationContext _invocationContext = Substitute.For<EndpointFilterInvocationContext>();
    private readonly IEndpointFilter _validatorFilter = new ValidatorProductStatusFilter();

    [Fact]
    public async Task ValidateRequestParameters_Should_ReturnBadRequestForInvalidValues()
    {
        // Arrange
        _invocationContext.HttpContext.Request.Query["AgentId"].Returns((StringValues)"-1");
        _invocationContext.HttpContext.Request.Query["ProductId"].Returns((StringValues)"-1");

        var expected = Results.BadRequest(new
        {
            code = 400,
            type = "INVALID_PARAMETER_VALUE",
            message = "Invalid value or required parameter list: AgentId, ProductId"
        });

        // Action
        var result = await _validatorFilter.InvokeAsync(_invocationContext, _filterDelegate);

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
    }

    [Fact]
    public async Task ValidateRequestParameters_Should_ReturnBadRequestForMissingRequiredValues()
    {
        // Arrange
        _invocationContext.HttpContext.Request.Query["AgentId"].Returns((StringValues)"1");

        var expected = Results.BadRequest(new
        {
            code = 400,
            type = "INVALID_PARAMETER_VALUE",
            message = "Invalid value or required parameter list: ProductId"
        });

        // Action
        var result = await _validatorFilter.InvokeAsync(_invocationContext, _filterDelegate);

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
    }

    [Fact]
    public async Task ValidateRequestParameters_Should_CallNextDelegateForValidParams()
    {
        // Arrange
        _invocationContext.HttpContext.Request.Query["AgentId"].Returns((StringValues)"1");
        _invocationContext.HttpContext.Request.Query["ProductId"].Returns((StringValues)"1");

        // Action
        await _validatorFilter.InvokeAsync(_invocationContext, _filterDelegate);

        // Assert
        await _filterDelegate.Received(1).Invoke(_invocationContext);
    }
}