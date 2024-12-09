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
}