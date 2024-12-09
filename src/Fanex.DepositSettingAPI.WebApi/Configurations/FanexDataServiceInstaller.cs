namespace Fanex.DepositSettingAPI.WebApi.Configurations;

using Data;
using Data.SqlServer;

public class FanexDataServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        DbSettingProviderManager
            .StartNewSession()
            .WithSqlServer(configuration)
            .UseDefaultDbSettingProvider("~/FanexSpName.xml")
            .Run();
    }
}