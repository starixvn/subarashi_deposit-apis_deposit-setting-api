namespace Fanex.DepositSettingAPI.Domain.Repositories;

using Entities;
using Params;

public interface IProductRepository
{
    Task<ProductStatus> GetProductStatusAsync(GetProductStatusParam param);
}