using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.PropertyItemPrices.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyItemPrices;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class PropertyItemPriceMapperService : IPropertyItemPriceMapperService
{
    public GetAllPropertyItemPriceRequestDto Map(GetAllPropertyItemPriceRequest model)
    {
        return new GetAllPropertyItemPriceRequestDto
        {
            IsActive = model.IsActive,
            Pagination = model.Pagination,
            PropertyId = model.PropertyId,
            PropertyItemId = model.PropertyItemId,
            PropertyCategoryId = model.PropertyCategoryId
        }.WithContentPolicy<PropertyItemPrice, GetAllPropertyItemPriceRequestDto>(model);
    }

    public PagedResult<GetAllPropertyItemPriceResponse> Map(PagedResult<GetAllPropertyItemPriceDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllPropertyItemPriceResponse
            {
                Id = x.Id,
                IsActive = x.IsActive,
                Price = x.Price,
                PropertyId = x.PropertyId,
                CreatedOnUtc = x.CreatedOnUtc,
                PropertyTitle = x.PropertyTitle,
                PropertyItemId = x.PropertyItemId,
                CooperationPrice = x.CooperationPrice,
                PropertyItemTitle = x.PropertyItemTitle,
                PropertyCategoryId = x.PropertyCategoryId,
                PropertyCategoryTitle = x.PropertyCategoryTitle
            })
            .ToList();

        return PagedResult<GetAllPropertyItemPriceResponse>.Create(items, model);
    }

    public GetPropertyItemPriceResponse Map(PropertyItemPrice model)
    {
        return new GetPropertyItemPriceResponse
        {
            Id = model.Id,
            Price = model.Price,
            IsActive = model.IsActive,
            CreatedOnUtc = model.CreatedOnUtc,
            PropertyItemId = model.PropertyItemId,
            CooperationPrice = model.CooperationPrice
        };
    }
}
