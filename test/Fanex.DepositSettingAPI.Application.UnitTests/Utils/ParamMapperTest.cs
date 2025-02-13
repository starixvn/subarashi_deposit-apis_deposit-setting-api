namespace Fanex.DepositSettingAPI.Application.UnitTests.Utils;

using Application.Utils;
using AutoFixture;
using Constants;
using Domain.Options;
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

    [Theory]
    [InlineData("LOCAL")]
    [InlineData("SIT")]
    [InlineData("UAT")]
    [InlineData("PROD")]
    [InlineData("KLDR")]
    public void MapGetBetSettingParam_Should_ReturnCorrectValue(string environment)
    {
        // Arrange
        var query = _fixture.Create<GetBetSettingQuery>();
        var appSetting = _fixture.Build<AppSetting>().With(x => x.Environment, environment).Create();
        var settingStatus = string.Equals(environment, "PROD", StringComparison.OrdinalIgnoreCase)
            ? SettingStatus.ExcludeDataTest
            : SettingStatus.IncludeDataTest;

        // Action
        var param = ParamMapper.MapGetBetSettingParam(query, appSetting);

        // Assert
        param.AgentId.Should().Be(query.AgentId);
        param.SettingStatus.Should().Be(settingStatus);
    }
}