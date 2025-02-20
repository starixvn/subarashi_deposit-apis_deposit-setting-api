namespace Fanex.DepositSettingAPI.Application.Handlers;

using Domain.Entities;
using Domain.Errors;
using Domain.Options;
using Domain.Params;
using Domain.Repositories;
using Domain.Results;
using Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Queries;
using Utils;

public class GetBetSettingHandler(
    IOptions<AppSetting> appSetting,
    ILogger logger,
    IHttpContextAccessor httpContextAccessor,
    IServerDbConfigRepository serverDbConfigRepository,
    IBetSettingRepository betSettingRepository)
    : BaseHandler<GetBetSettingQuery, ServiceResult<BetSetting>>(logger)
{
    protected override async Task<ServiceResult<BetSetting>> DoWork(GetBetSettingQuery query, CancellationToken cancellationToken)
    {
        var serverDbConfig = await serverDbConfigRepository.GetServerDbConfigByAgentIdAsync(query.AgentId);
        if (serverDbConfig is null)
        {
            return ServiceResult<BetSetting>.Failure(BetSettingErrors.InvalidAgentId);
        }

        ServerDbConfigUtils.SetServerDbConfigToHttpContext(serverDbConfig, httpContextAccessor);

        GetBetSettingParam param = ParamMapper.MapGetBetSettingParam(query, appSetting.Value);
        ServiceResult<BetSetting> result = await betSettingRepository.GetBetSettingAsync(param);

        return result;
    }
}