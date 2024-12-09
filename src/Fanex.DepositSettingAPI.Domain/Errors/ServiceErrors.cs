namespace Fanex.DepositSettingAPI.Domain.Errors;

public sealed record ServiceErrors(int Code, string Message)
{
    public static readonly ServiceErrors None = new(0, string.Empty);

    public static readonly ServiceErrors SystemError = new(999, "System error.");
}