namespace Fanex.DepositSettingAPI.Application.Utils;

using Constants;
using Domain.Options;
using Domain.Params;
using Queries;

public static class ParamMapper
{
    public static GetProductStatusParam MapGetProductStatusParam(GetProductStatusQuery query)
    {
        return new GetProductStatusParam
        {
            AgentId = query.AgentId,
            ProductId = query.ProductId
        };
    }

    public static GetBetSettingParam MapGetBetSettingParam(GetBetSettingQuery query, AppSetting appSetting)
    {
        return new GetBetSettingParam
        {
            AgentId = query.AgentId,
            SettingStatus = string.Equals(appSetting.Environment, "PROD", StringComparison.OrdinalIgnoreCase)
                ? SettingStatus.ExcludeDataTest
                : SettingStatus.IncludeDataTest
        };
    }
}