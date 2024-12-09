namespace Fanex.DepositSettingAPI.Domain.Errors;

public static class ProductStatusErrors
{
    public static readonly ServiceErrors NotSupported = new(101, "The product is not supported.");
}