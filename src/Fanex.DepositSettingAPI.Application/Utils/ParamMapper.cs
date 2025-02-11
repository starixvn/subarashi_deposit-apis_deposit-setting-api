namespace Fanex.DepositSettingAPI.Application.Utils;

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

    public static GetBetSettingParam MapGetBetSettingParam(GetBetSettingQuery query)
    {
        return new GetBetSettingParam
        {
            AgentId = query.AgentId
        };
    }
}