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

public class BetSettingEndpoints : IEndpoints
{
    public string GroupName => "BetSettings";

    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this);

        group
            .MapGet(GetBetSetting)
            .AddEndpointFilter<ValidatorBetSettingFilter>();
    }

    [Produces("application/json")]
    [ProducesResponseType(typeof(BetSettingResponse), (int)HttpStatusCode.OK)]
    internal static async Task<IResult> GetBetSetting(
        int agentId,
        IMediator mediator)
    {
        var request = new BetSettingRequest
        {
            AgentId = agentId
        };

        var query = ObjectMapper.MapGetBetSettingQuery(request);
        var result = await mediator.Send(query);

        return result.Match(
            _ => Results.Ok(new BetSettingResponse
            {
                ErrorCode = 0,
                ErrorMessage = string.Empty,
                Result = result.Result
            }),
            (error, _) => Results.Ok(new BetSettingResponse
            {
                ErrorCode = error.Code,
                ErrorMessage = error.Message
            })
        );
    }
}