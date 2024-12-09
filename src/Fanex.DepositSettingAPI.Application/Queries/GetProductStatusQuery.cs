namespace Fanex.DepositSettingAPI.Application.Queries;

using Domain.Entities;
using Domain.Results;
using MediatR;

public class GetProductStatusQuery : IRequest<ServiceResult<ProductStatus>>
{
    public int AgentId { get; set; }

    public int ProductId { get; set; }
}