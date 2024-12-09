namespace Fanex.DepositSettingAPI.Domain.Entities;

public class ProductStatus
{
    public int AgentId { get; set; }

    public int ProductId { get; set; }

    public bool IsEnabled { get; set; }
}