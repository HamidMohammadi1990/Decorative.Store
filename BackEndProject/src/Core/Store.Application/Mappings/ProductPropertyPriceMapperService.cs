using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductPropertyPrices.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductPropertyPrices;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductPropertyPriceMapperService : IProductPropertyPriceMapperService
{
    public GetAllProductPropertyPriceRequestDto Map(GetAllProductPropertyPriceRequest model)
    {
        return new GetAllProductPropertyPriceRequestDto
        {
            IsActive = model.IsActive,
            ProductId = model.ProductId,
            Pagination = model.Pagination,
            ProductPropertyId = model.ProductPropertyId
        }.WithContentPolicy<ProductPropertyPrice, GetAllProductPropertyPriceRequestDto>(model);
    }

    public GetProductPropertyPriceResponse Map(ProductPropertyPrice model)
    {
        return new GetProductPropertyPriceResponse
        {
            Id = model.Id,
            Price = model.Price,
            IsActive = !model.IsActive,
            CreatedOnUtc = model.CreatedOnUtc,
            ProductPropertyId = model.ProductPropertyId,
            CooperationPrice = model.CooperationPrice
        };
    }

    public PagedResult<GetAllProductPropertyPriceResponse> Map(PagedResult<GetAllProductPropertyPriceDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllProductPropertyPriceResponse
            {
                Id = x.Id,
                Price = x.Price,
                IsActive = x.IsActive,
                ProductId = x.ProductId,
                CreatedOnUtc = x.CreatedOnUtc,
                ProductTitle = x.ProductTitle,
                CooperationPrice = x.CooperationPrice,
                ProductPropertyId = x.ProductPropertyId
            })
            .ToList();

        return PagedResult<GetAllProductPropertyPriceResponse>.Create(items, model);
    }
}
