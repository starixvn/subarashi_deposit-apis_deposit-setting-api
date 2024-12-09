namespace Fanex.DepositSettingAPI.Application.Constants;

using Queries;

public static class CacheKeys
{
    public static Func<GetProductStatusQuery, string> ProductStatus => query => $"ProductStatus_{query.AgentId}_{query.ProductId}";
}