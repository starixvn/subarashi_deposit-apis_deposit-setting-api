namespace Fanex.DepositSettingAPI.Application.UnitTests.Utils;

using Application.Utils;
using AutoFixture;
using FluentAssertions;
using Queries;

public class ParamMapperTest
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public void MapGetProductStatusParam_Should_ReturnCorrectValue()
    {
        // Arrange
        var query = _fixture.Create<GetProductStatusQuery>();

        // Action
        var param = ParamMapper.MapGetProductStatusParam(query);

        // Assert
        param.AgentId.Should().Be(query.AgentId);
        param.ProductId.Should().Be(query.ProductId);
    }
}