namespace Fanex.DepositSettingAPI.Infrastructure.Repositories;

using Constants;
using DbSettingConfig;
using Domain.Entities;
using Domain.Errors;
using Domain.Params;
using Domain.Repositories;
using Domain.Results;
using Utils;

public class BetSettingRepository(IDbFactory dbFactory, IShardDbInfoAccessor shardDbInfoAccessor) : IBetSettingRepository
{
    public async Task<ServiceResult<BetSetting>> GetBetSettingAsync(GetBetSettingParam param)
    {
        const string spKey = "API_AgentBetSetting_Sel";

        using var objectDb = dbFactory.GetObjectDb(shardDbInfoAccessor.ToShardCustDbSp(spKey));

        objectDb.SetParameterValue(DbParam.AgentId, param.AgentId);
        objectDb.SetParameterValue(DbParam.SettingStatus, param.SettingStatus);

        var betLimits = await objectDb.QueryAsync<BetLimit>();

        var output = objectDb.GetOutputParams();
        var (code, message) = DbUtils.GetErrorOutputParameters(output);

        if (code != 0)
        {
            return ServiceResult<BetSetting>.Failure(ServiceErrors.GetFromDbError(code, message));
        }

        var result = new BetSetting
        {
            SiteName = Converter.ToString(output[DbParam.Site]),
            Currency = Converter.ToInt(output[DbParam.Currency]),
            BetLimit = betLimits
        };

        return ServiceResult<BetSetting>.Success(result);
    }
}