namespace Fanex.DepositSettingAPI.Infrastructure.UnitTests.DbSettingConfig;

using Domain.Entities;
using Infrastructure.DbSettingConfig;
using Microsoft.AspNetCore.Http;
using NSubstitute;

public class ShardDbInfoAccessorTest
{
    private readonly HttpContext _httpContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ShardDbInfoAccessor _shardDbInfoAccessor;

    public ShardDbInfoAccessorTest()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _httpContext = Substitute.ForPartsOf<HttpContext>();
        _httpContextAccessor.HttpContext.Returns(_httpContext);
        _shardDbInfoAccessor = new ShardDbInfoAccessor(_httpContextAccessor);
    }

    [Fact]
    public void GetSpKey_ShouldRemoveShardPrefixes()
    {
        // Arrange
        var key = "ShardCustDB_TestKey";

        // Act
        var result = _shardDbInfoAccessor.GetSpKey(key);

        // Assert
        Assert.Equal("TestKey", result);
    }

    [Fact]
    public void ToShardCustDbSp_ShouldAddShardCustDbPrefix()
    {
        // Arrange
        var key = "TestKey";

        // Act
        var result = _shardDbInfoAccessor.ToShardCustDbSp(key);

        // Assert
        Assert.Equal("ShardCustDB_TestKey", result);
    }

    [Fact]
    public void ToShardSiteDbSp_ShouldAddShardSiteDbPrefix()
    {
        // Arrange
        var key = "TestKey";

        // Act
        var result = _shardDbInfoAccessor.ToShardSiteDbSp(key);

        // Assert
        Assert.Equal("ShardSiteDB_TestKey", result);
    }

    [Fact]
    public void TryUseShardDb_ShouldReturnTrueAndSetDbServer_WhenKeyIsShardSiteDbAndSiteDbServerIsNotNullOrEmpty()
    {
        // Arrange
        var key = "ShardSiteDB_TestKey";
        var serverDbConfig = new ServerDbConfig { SiteDbServer = "SiteDbServer" };
        ArrangeHttpContext(serverDbConfig);

        // Act
        var result = _shardDbInfoAccessor.TryUseShardDb(key, out var dbServer);

        // Assert
        Assert.True(result);
        Assert.Equal("SiteDbServer", dbServer);
    }

    [Fact]
    public void TryUseShardDb_ShouldReturnTrueAndSetDbServer_WhenKeyIsShardCustDbAndCustDbServerIsNotNullOrEmpty()
    {
        // Arrange
        var key = "ShardCustDB_TestKey";
        var serverDbConfig = new ServerDbConfig { CustDbServer = "CustDbServer" };
        ArrangeHttpContext(serverDbConfig);

        // Act
        var result = _shardDbInfoAccessor.TryUseShardDb(key, out var dbServer);

        // Assert
        Assert.True(result);
        Assert.Equal("CustDbServer", dbServer);
    }

    [Fact]
    public void TryUseShardDb_ShouldReturnFalseAndSetDbServerToNull_WhenKeyIsNotShardDb()
    {
        // Arrange
        var key = "TestKey";
        var serverDbConfig = new ServerDbConfig();
        ArrangeHttpContext(serverDbConfig);

        // Act
        var result = _shardDbInfoAccessor.TryUseShardDb(key, out var dbServer);

        // Assert
        Assert.False(result);
        Assert.Null(dbServer);
    }

    private void ArrangeHttpContext(ServerDbConfig serverDbConfig)
    {
        AssignMockValue("CustDbServerKey", serverDbConfig.CustDbServer);
        AssignMockValue("SiteDbServerKey", serverDbConfig.SiteDbServer);
        AssignMockValue("CustDbGroupIdKey", serverDbConfig.CustDbGroupId);
        AssignMockValue("SiteDbGroupIdKey", serverDbConfig.SiteDbGroupId);
        AssignMockValue("SiteKey", serverDbConfig.Site);
        return;

        void AssignMockValue(object key, object value)
        {
            _httpContext.Items.TryGetValue(key, out Arg.Any<object>()).Returns(x =>
            {
                x[1] = value;
                return true;
            });
        }
    }
}