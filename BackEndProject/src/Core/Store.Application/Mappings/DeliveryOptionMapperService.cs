using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.DeliveryOptions.Queries;
using Store.Domain.Dtos.DeliveryOptions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class DeliveryOptionMapperService : IDeliveryOptionMapperService
{
    public GetDeliveryOptionResponse Map(DeliveryOption model)
    {
        return new GetDeliveryOptionResponse
        {
            Id = model.Id,
            Title = model.Title,
            DeliveryDays = model.DeliveryDays
        };
    }

    public PagedResult<GetAllDeliveryOptionResponse> Map(PagedResult<GetAllDeliveryOptionResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllDeliveryOptionResponse
            {
                Id = x.Id,
                Title = x.Title,
                IsActive = x.IsActive,
                DeliveryDays = x.DeliveryDays
            })
            .ToList();

        return PagedResult<GetAllDeliveryOptionResponse>.Create(items, model);
    }

    public PagedResult<SearchDeliveryOptionResponse> Map(PagedResult<SearchDeliveryOptionResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchDeliveryOptionResponse
            {
                Id = x.Id,
                Title = x.Title,
                DeliveryDays = x.DeliveryDays
            })
            .ToList();

        return PagedResult<SearchDeliveryOptionResponse>.Create(items, model);
    }

    public GetAllDeliveryOptionRequestDto Map(GetAllDeliveryOptionRequest model)
    {
        return new GetAllDeliveryOptionRequestDto
        {
            Title = model.Title,
            IsActive = model.IsActive,
            Pagination = model.Pagination,
            DeliveryDays = model.DeliveryDays
        }.WithContentPolicy<DeliveryOption, GetAllDeliveryOptionRequestDto>(model);
    }

    public SearchDeliveryOptionRequestDto Map(SearchDeliveryOptionRequest model)
    {
        return new SearchDeliveryOptionRequestDto
        {
            Title = model.Title,
            Pagination = model.Pagination
        }.WithContentPolicy<DeliveryOption, SearchDeliveryOptionRequestDto>(model);
    }
}