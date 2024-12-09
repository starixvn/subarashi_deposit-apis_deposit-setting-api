namespace Fanex.DepositSettingAPI.Infrastructure;

using Application.Services;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repositories;
using Services;
using Settings;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSetting>(configuration.GetSection(AppSetting.SettingName));
        services.Configure<SentrySetting>(configuration.GetSection(SentrySetting.SettingName));

        services.AddMemoryCache(options => { options.TrackStatistics = true; });

        // repositories

        services.TryAddScoped<IProductRepository, ProductRepository>();
        services.TryAddScoped<ICacheProvider, CacheProvider>();
        services.TryAddSingleton<IDbFactory, DbFactory>();

        return services;
    }
}