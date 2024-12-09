namespace Fanex.DepositSettingAPI.Application.Handlers;

using AsyncKeyedLock;
using Constants;
using Domain.Entities;
using Domain.Errors;
using Domain.Params;
using Domain.Repositories;
using Domain.Results;
using Logging;
using MediatR;
using Queries;
using Services;
using Utils;

public class GetProductStatusHandler(
    ILogger logger,
    IProductRepository productRepository,
    ICacheProvider cacheProvider)
    : IRequestHandler<GetProductStatusQuery, ServiceResult<ProductStatus>>
{
    private const int Timeout = 500;

    private static readonly AsyncKeyedLocker<string> AsyncKeyedLocker = new(new AsyncKeyedLockOptions
    {
        MaxCount = 1, // max request call at the same time
        PoolSize = 0 // disable pooling
    });

    private static readonly HashSet<int> SupportedProductStatus =
    [
        Products.CO
    ];

    public async Task<ServiceResult<ProductStatus>> Handle(GetProductStatusQuery query, CancellationToken cancellationToken)
    {
        try
        {
            if (!IsSupportProduct(query.ProductId))
            {
                return ServiceResult<ProductStatus>.Failure(ProductStatusErrors.NotSupported);
            }

            GetProductStatusParam param = ParamMapper.MapGetProductStatusParam(query);
            string cacheKey = CacheKeys.ProductStatus(query);
            ServiceResult<ProductStatus> result = await GetResult(param, cacheKey);

            return result;
        }
        catch (Exception exception)
        {
            await logger.ErrorAsync(ErrorMessageUtils.GenerateMessage(exception));

            return ServiceResult<ProductStatus>.Failure(ServiceErrors.SystemError);
        }
    }

    private async Task<ServiceResult<ProductStatus>> GetResult(GetProductStatusParam param, string cacheKey)
    {
        ProductStatus productStatus = cacheProvider.GetValueFromCache<ProductStatus>(cacheKey);

        if (productStatus is not null)
        {
            return ServiceResult<ProductStatus>.Success(productStatus);
        }

        using (await AsyncKeyedLocker.LockOrNullAsync(cacheKey, Timeout))
        {
            productStatus = await cacheProvider
                .GetOrCreateFromCacheAsync(
                    cacheKey,
                    () => productRepository.GetProductStatusAsync(param),
                    TimeSpan.FromMinutes(5));

            return ServiceResult<ProductStatus>.Success(productStatus);
        }
    }

    private static bool IsSupportProduct(int productId)
    {
        return SupportedProductStatus.Contains(productId);
    }
}