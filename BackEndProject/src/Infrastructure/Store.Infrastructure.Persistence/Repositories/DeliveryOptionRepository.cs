using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.DeliveryOptions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class DeliveryOptionRepository
    (EditionDbContext context)
    : Repository<DeliveryOption>(context), IDeliveryOptionRepository
{
    public async Task<PagedResult<GetAllDeliveryOptionResponseDto>> GetAllAsync(GetAllDeliveryOptionRequestDto request)
    {
        var deliveryOptions = Context.DeliveryOption
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        var result =
            await deliveryOptions
            .Select(x => new GetAllDeliveryOptionResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                IsActive = x.IsActive,
                DeliveryDays = x.DeliveryDays
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchDeliveryOptionResponseDto>> SearchAsync(SearchDeliveryOptionRequestDto request)
    {
        var deliveryOptions = Context.DeliveryOption
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        var result =
            await deliveryOptions
            .Select(x => new SearchDeliveryOptionResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                DeliveryDays = x.DeliveryDays
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}