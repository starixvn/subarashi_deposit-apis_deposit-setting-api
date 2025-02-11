namespace Fanex.DepositSettingAPI.Domain.Entities;

public class BetSetting
{
    public string SiteName { get; set; }

    public int Currency { get; set; }

    public IEnumerable<BetLimit> BetLimit { get; set; }
}