namespace Fanex.DepositSettingAPI.Presentation.DTOs.Response;

using Domain.Entities;
using Interfaces;

public class ProductStatusResponse : IResultResponse<ProductStatus>
{
    public int ErrorCode { get; set; }

    public string ErrorMessage { get; set; }

    public ProductStatus Result { get; set; }
}