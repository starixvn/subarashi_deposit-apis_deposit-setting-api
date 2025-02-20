namespace Fanex.DepositSettingAPI.Presentation.DTOs.Response;

using Domain.Entities;
using Interfaces;

public class BetSettingResponse : IResultResponse<BetSetting>
{
    public int ErrorCode { get; set; }

    public string ErrorMessage { get; set; }

    public BetSetting Result { get; set; }
}