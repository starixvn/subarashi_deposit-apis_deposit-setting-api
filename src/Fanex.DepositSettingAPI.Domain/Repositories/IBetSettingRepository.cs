namespace Fanex.DepositSettingAPI.Domain.Repositories;

using Entities;
using Params;
using Results;

public interface IBetSettingRepository
{
    Task<ServiceResult<BetSetting>> GetBetSettingAsync(GetBetSettingParam param);
}