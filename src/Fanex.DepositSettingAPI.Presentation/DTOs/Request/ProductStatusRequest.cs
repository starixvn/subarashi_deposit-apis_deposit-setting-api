namespace Fanex.DepositSettingAPI.Presentation.DTOs.Request;

using System.ComponentModel.DataAnnotations;

public class ProductStatusRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int AgentId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }
}