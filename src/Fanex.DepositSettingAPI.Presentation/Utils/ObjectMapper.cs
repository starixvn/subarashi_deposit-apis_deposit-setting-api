namespace Fanex.DepositSettingAPI.Presentation.Utils;

using Application.Queries;
using DTOs.Request;

public static class ObjectMapper
{
    public static GetProductStatusQuery MapGetProductStatusQuery(ProductStatusRequest request)
    {
        return new GetProductStatusQuery
        {
            ProductId = request.ProductId,
            AgentId = request.AgentId
        };
    }
}