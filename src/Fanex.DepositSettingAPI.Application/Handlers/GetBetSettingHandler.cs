namespace Fanex.DepositSettingAPI.Application.Handlers;

using Domain.Entities;
using Domain.Errors;
using Domain.Params;
using Domain.Repositories;
using Domain.Results;
using Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Queries;
using Utils;

public class GetBetSettingHandler(
    ILogger logger,
    IHttpContextAccessor httpContextAccessor,
    IServerDbConfigRepository serverDbConfigRepository,
    IBetSettingRepository betSettingRepository)
    : IRequestHandler<GetBetSettingQuery, ServiceResult<BetSetting>>
{
    public async Task<ServiceResult<BetSetting>> Handle(GetBetSettingQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var serverDbConfig = await serverDbConfigRepository.GetServerDbConfigByAgentIdAsync(query.AgentId);
            if (serverDbConfig is null)
            {
                return ServiceResult<BetSetting>.Failure(BetSettingErrors.InvalidAgentId);
            }

            ServerDbConfigUtils.SetServerDbConfigToHttpContext(serverDbConfig, httpContextAccessor);

            GetBetSettingParam param = ParamMapper.MapGetBetSettingParam(query);
            ServiceResult<BetSetting> result = await betSettingRepository.GetBetSettingAsync(param);

            return result;
        }
        catch (Exception exception)
        {
            await logger.ErrorAsync(ErrorMessageUtils.GenerateMessage(exception));

            return ServiceResult<BetSetting>.Failure(ServiceErrors.SystemError);
        }
    }
}