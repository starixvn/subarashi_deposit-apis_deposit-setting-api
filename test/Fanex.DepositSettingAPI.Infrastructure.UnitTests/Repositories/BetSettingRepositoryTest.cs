namespace Fanex.DepositSettingAPI.Infrastructure.UnitTests.Repositories;

using System.Collections;
using AutoFixture;
using Constants;
using Domain.Entities;
using Domain.Errors;
using Domain.Params;
using Domain.Repositories;
using FluentAssertions;
using Infrastructure.Repositories;
using NSubstitute;

[Collection("DbFactoryFixture")]
public class BetSettingRepositoryTest : RepositoryBaseTest
{
    private readonly IBetSettingRepository _betSettingRepository;

    public BetSettingRepositoryTest(DbFactoryFixture dbFactoryFixture) : base(dbFactoryFixture)
    {
        _betSettingRepository = new BetSettingRepository(dbFactoryFixture, ShardDbInfoAccessor);
    }

    [Fact]
    public async Task GetBetSettingAsync_Should_SetParameterValueCorrect()
    {
        // Arrange
        var param = Fixture.Create<GetBetSettingParam>();

        // Action
        await _betSettingRepository.GetBetSettingAsync(param);

        // Assert
        ObjectDb.Received(1).SetParameterValue(DbParam.AgentId, param.AgentId);
        ObjectDb.Received(1).SetParameterValue(DbParam.SettingStatus, param.SettingStatus);
    }

    [Theory]
    [InlineData(10, "Invalid AgentId.")]
    [InlineData(999, "System error.")]
    public async Task GetBetSettingAsync_Should_ReturnError_WhenDBReturnError(int errorCode, string errorMessage)
    {
        // Arrange
        var param = Fixture.Create<GetBetSettingParam>();
        ObjectDb.GetOutputParams().Returns(new Hashtable
        {
            { DbParam.ErrCode, errorCode },
            { DbParam.ErrMsg, errorMessage }
        });

        // Action
        var result = await _betSettingRepository.GetBetSettingAsync(param);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(ServiceErrors.GetFromDbError(errorCode, errorMessage));
    }

    [Fact]
    public async Task GetBetSettingAsync_Should_ReturnSuccess()
    {
        // Arrange
        var param = Fixture.Create<GetBetSettingParam>();
        var data = Fixture.Create<BetSetting>();

        ObjectDb.GetOutputParams().Returns(new Hashtable
        {
            { DbParam.ErrCode, 0 },
            { DbParam.ErrMsg, "" },
            { DbParam.Site, data.SiteName },
            { DbParam.Currency, data.Currency }
        });

        ObjectDb.QueryAsync<BetLimit>().Returns(data.BetLimit);

        // Action
        var result = await _betSettingRepository.GetBetSettingAsync(param);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(ServiceErrors.None);
        result.Result.Should().BeEquivalentTo(data);
    }
}