namespace Fanex.DepositSettingAPI.Presentation.Endpoints;

using System.Net;
using Domain.Extensions;
using DTOs.Request;
using DTOs.Response;
using Extensions;
using Filters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Utils;

public class ProductEndpoints : IEndpoints
{
    public string GroupName => "Products";

    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this);

        group
            .MapGet(GetProductStatus, "status")
            .AddEndpointFilter<ValidatorProductStatusFilter>();
    }

    [Produces("application/json")]
    [ProducesResponseType(typeof(ProductStatusResponse), (int)HttpStatusCode.OK)]
    internal static async Task<IResult> GetProductStatus(
        int productId,
        int agentId,
        IMediator mediator)
    {
        var request = new ProductStatusRequest
        {
            AgentId = agentId,
            ProductId = productId
        };

        var query = ObjectMapper.MapGetProductStatusQuery(request);
        var result = await mediator.Send(query);

        return result.Match(
            _ => Results.Ok(new ProductStatusResponse
            {
                ErrorCode = 0,
                ErrorMessage = string.Empty,
                Result = result.Result
            }),
            (error, _) => Results.Ok(new ProductStatusResponse
            {
                ErrorCode = error.Code,
                ErrorMessage = error.Message
            })
        );
    }
}