using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.DeliveryTypes;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class DeliveryTypeRepository
    (EditionDbContext context)
    : Repository<DeliveryType>(context), IDeliveryTypeRepository
{
    public async Task<PagedResult<GetAllDeliveryTypeResponseDto>> GetAllAsync(GetAllDeliveryTypeRequestDto request)
    {
        var deliveryTypes = Context.DeliveryType            
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        var result =
            await deliveryTypes
            .Select(x => new GetAllDeliveryTypeResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority,
                IsActive = x.IsActive
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchDeliveryTypeResponseDto>> SearchAsync(SearchDeliveryTypeRequestDto request)
    {
        var deliveryTypes = Context.DeliveryType            
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        var result =
            await deliveryTypes
            .Select(x => new SearchDeliveryTypeResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<List<DeliveryType>> GetAllAsync()
    {
        var deliveryTypes =
            await Context
            .DeliveryType
            .Where(x => x.IsActive)
            .AsNoTracking()
            .ToListAsync();

        return deliveryTypes;
    }
}