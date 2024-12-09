namespace Fanex.DepositSettingAPI.Infrastructure.Repositories;

using Constants;
using Domain.Entities;
using Domain.Params;
using Domain.Repositories;
using Utils;

public class ProductRepository(IDbFactory dbFactory) : IProductRepository
{
    public async Task<ProductStatus> GetProductStatusAsync(GetProductStatusParam param)
    {
        const string spKey = "API_Dep_3rdProductCustStatusSel";

        using var objectDb = dbFactory.GetObjectDb(spKey);

        objectDb.SetParameterValue(DbParam.AgentId, param.AgentId);
        objectDb.SetParameterValue(DbParam.ProductId, param.ProductId);

        await objectDb.ExecuteNonQueryAsync();

        var output = objectDb.GetOutputParams();

        var result = new ProductStatus
        {
            ProductId = param.ProductId,
            AgentId = param.AgentId,
            IsEnabled = Converter.ToBoolean(output[DbParam.IsEnabled], false)
        };

        return result;
    }
}