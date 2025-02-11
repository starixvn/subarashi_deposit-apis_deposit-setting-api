namespace Fanex.DepositSettingAPI.Domain.Errors;

public static class BetSettingErrors
{
    public static readonly ServiceErrors InvalidAgentId = new(10, "Invalid AgentId.");
}