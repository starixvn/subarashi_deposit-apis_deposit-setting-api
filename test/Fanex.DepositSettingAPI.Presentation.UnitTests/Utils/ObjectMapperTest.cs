namespace Fanex.DepositSettingAPI.Presentation.UnitTests.Utils;

using AutoFixture;
using DTOs.Request;
using FluentAssertions;
using Presentation.Utils;

public class ObjectMapperTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void MapNewPlayerCommand_Should_MapPropertiesCorrectly()
    {
        // Arrange
        var request = _fixture.Create<ProductStatusRequest>();

        // Action
        var getTicketInformationQuery = ObjectMapper.MapGetProductStatusQuery(request);

        // Assert
        getTicketInformationQuery.AgentId.Should().Be(request.AgentId);
        getTicketInformationQuery.ProductId.Should().Be(request.ProductId);
    }
}