namespace Fanex.DepositSettingAPI.Presentation.Filters;

using DTOs.Request;
using Microsoft.AspNetCore.Http;
using Utils;

public class ValidatorProductStatusFilter : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var query = context.HttpContext.Request.Query;
        var request = new ProductStatusRequest
        {
            AgentId = int.TryParse(query["AgentId"], out int agentId) ? agentId : 0,
            ProductId = int.TryParse(query["ProductId"], out int productId) ? productId : 0
        };

        var (isValid, validationResults) = RequestValidator.IsValidRequest(request);

        if (isValid)
        {
            return await next(context);
        }

        return Results.BadRequest(new
        {
            code = 400,
            type = "INVALID_PARAMETER_VALUE",
            message = $"Invalid value or required parameter list: {RequestValidator.GetInvalidParamList(validationResults)}"
        });
    }
}