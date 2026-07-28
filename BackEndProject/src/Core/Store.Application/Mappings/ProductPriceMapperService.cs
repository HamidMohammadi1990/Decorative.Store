using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductPrices.Queries;
using Store.Domain.Dtos.ProductPrices;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductPriceMapperService : IProductPriceMapperService
{
    public GetAllProductPriceRequestDto Map(GetAllProductPriceRequest model)
    {
        return new GetAllProductPriceRequestDto
        {
            UserId = model.UserId,
            IsActive = model.IsActive,
            ProductId = model.ProductId,
            CompanyId = model.CompanyId,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductPrice, GetAllProductPriceRequestDto>(model);
    }

    public GetProductPriceResponse Map(ProductPrice model)
    {
        return new GetProductPriceResponse
        {
            Id = model.Id,
            Price = model.Price,
            IsActive = model.IsActive,
            ProductId = model.ProductId,
            CompanyId = model.CompanyId,
            CreatedOnUtc = model.CreatedOnUtc,
            CooperationPrice = model.CooperationPrice
        };
    }

    public PagedResult<GetAllProductPriceResponse> Map(PagedResult<GetAllProductPriceDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllProductPriceResponse
            {
                Id = x.Id,
                Price = x.Price,
                UserId = x.UserId,
                IsActive = x.IsActive,
                CompanyId = x.CompanyId,
                ProductId = x.ProductId,
                ProductTitle = x.ProductTitle,
                CompanyName = x.CompanyName,
                CreatedOnUtc = x.CreatedOnUtc,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                CooperationPrice = x.CooperationPrice,
            })
            .ToList();

        return PagedResult<GetAllProductPriceResponse>.Create(items, model);
    }
}