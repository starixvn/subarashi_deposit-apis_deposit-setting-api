namespace Fanex.DepositSettingAPI.Application.Queries;

using Domain.Entities;
using Domain.Results;
using MediatR;

public class GetBetSettingQuery : IRequest<ServiceResult<BetSetting>>
{
    public int AgentId { get; set; }
}