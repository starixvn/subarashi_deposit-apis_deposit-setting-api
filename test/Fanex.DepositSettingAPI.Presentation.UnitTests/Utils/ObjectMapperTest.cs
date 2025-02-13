namespace Fanex.DepositSettingAPI.Presentation.UnitTests.Utils;

using AutoFixture;
using DTOs.Request;
using FluentAssertions;
using Presentation.Utils;

public class ObjectMapperTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void MapGetProductStatusQuery_Should_MapPropertiesCorrectly()
    {
        // Arrange
        var request = _fixture.Create<ProductStatusRequest>();

        // Action
        var getProductStatusQuery = ObjectMapper.MapGetProductStatusQuery(request);

        // Assert
        getProductStatusQuery.AgentId.Should().Be(request.AgentId);
        getProductStatusQuery.ProductId.Should().Be(request.ProductId);
    }

    [Fact]
    public void MapGetBetSettingQuery_Should_MapPropertiesCorrectly()
    {
        // Arrange
        var request = _fixture.Create<BetSettingRequest>();

        // Action
        var getBetSettingQuery = ObjectMapper.MapGetBetSettingQuery(request);

        // Assert
        getBetSettingQuery.AgentId.Should().Be(request.AgentId);
    }
}