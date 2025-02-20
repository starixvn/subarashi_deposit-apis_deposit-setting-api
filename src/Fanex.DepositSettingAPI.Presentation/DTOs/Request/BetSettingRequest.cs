namespace Fanex.DepositSettingAPI.Presentation.DTOs.Request;

using System.ComponentModel.DataAnnotations;

public class BetSettingRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int AgentId { get; set; }
}