using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.DeliveryTypes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.DeliveryTypes;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class DeliveryTypeMapperService : IDeliveryTypeMapperService
{
    public GetAllDeliveryTypeRequestDto Map(GetAllDeliveryTypeRequest model)
    {
        return new GetAllDeliveryTypeRequestDto
        {
            Title = model.Title,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<DeliveryType, GetAllDeliveryTypeRequestDto>(model);
    }

    public SearchDeliveryTypeRequestDto Map(SearchDeliveryTypeRequest model)
    {
        return new SearchDeliveryTypeRequestDto
        {
            Title = model.Title,
            Pagination = model.Pagination
        }.WithContentPolicy<DeliveryType, SearchDeliveryTypeRequestDto>(model);
    }

    public GetDeliveryTypeResponse Map(DeliveryType model)
    {
        return new GetDeliveryTypeResponse
        {
            Id = model.Id,
            Title = model.Title,
            Priority = model.Priority,
            IsActive = model.IsActive
        };
    }

    public PagedResult<GetAllDeliveryTypeResponse> Map(PagedResult<GetAllDeliveryTypeResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllDeliveryTypeResponse
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority,
                IsActive = x.IsActive
            })
            .ToList();

        return PagedResult<GetAllDeliveryTypeResponse>.Create(items, model);
    }

    public PagedResult<SearchDeliveryTypeResponse> Map(PagedResult<SearchDeliveryTypeResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchDeliveryTypeResponse
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority
            })
            .ToList();

        return PagedResult<SearchDeliveryTypeResponse>.Create(items, model);
    }
}