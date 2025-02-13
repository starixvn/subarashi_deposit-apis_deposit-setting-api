namespace Fanex.DepositSettingAPI.Domain.Params;

public class GetBetSettingParam
{
    public int AgentId { get; set; }

    public byte SettingStatus { get; set; }
}