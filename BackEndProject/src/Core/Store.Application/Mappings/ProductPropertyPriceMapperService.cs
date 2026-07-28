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
            UserId = model.UserId,
            IsActive = model.IsActive,
            CompanyId = model.CompanyId,
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
            CompanyId = model.CompanyId,
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
                UserId = x.UserId,
                IsActive = x.IsActive,
                ProductId = x.ProductId,
                CompanyId = x.CompanyId,
                CreatedOnUtc = x.CreatedOnUtc,
                ProductTitle = x.ProductTitle,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                CooperationPrice = x.CooperationPrice,
                ProductPropertyId = x.ProductPropertyId
            })
            .ToList();

        return PagedResult<GetAllProductPropertyPriceResponse>.Create(items, model);
    }
}