using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductPriceDeliveryOptions.Queries;
using Store.Domain.Dtos.ProductPriceDeliveryOptions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductPriceDeliveryOptionMapperService : IProductPriceDeliveryOptionMapperService
{
    public GetProductPriceDeliveryOptionResponse Map(ProductPriceDeliveryOption model)
    {
        return new GetProductPriceDeliveryOptionResponse
        {
            Id = model.Id,
            Price = model.Price,
            ProductPriceId = model.ProductPriceId,
            DeliveryOptionId = model.DeliveryOptionId,
            CooperationPrice = model.CooperationPrice
        };
    }

    public GetAllProductPriceDeliveryOptionRequestDto Map(GetAllProductPriceDeliveryOptionRequest model)
    {
        return new GetAllProductPriceDeliveryOptionRequestDto
        {
            UserId = model.UserId,
            IsActive = model.IsActive,
            CompanyId = model.CompanyId,
            Pagination = model.Pagination,
            ProductPriceId = model.ProductPriceId,
            DeliveryOptionId = model.DeliveryOptionId
        }.WithContentPolicy<ProductPriceDeliveryOption, GetAllProductPriceDeliveryOptionRequestDto>(model);
    }

    public PagedResult<GetAllProductPriceDeliveryOptionResponse> Map(PagedResult<GetAllProductPriceDeliveryOptionResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllProductPriceDeliveryOptionResponse
            {
                Id = x.Id,
                Price = x.Price,
                CompanyId = x.CompanyId,
                ProductId = x.ProductId,
                CompanyName = x.CompanyName,
                ProductTitle = x.ProductTitle,
                CreatedOnUtc = x.CreatedOnUtc,
                ProductPriceId = x.ProductPriceId,
                DeliveryOptionId = x.DeliveryOptionId,
                CooperationPrice = x.CooperationPrice,
                DeliveryOptionTitle = x.DeliveryOptionTitle
            })
            .ToList();

        return PagedResult<GetAllProductPriceDeliveryOptionResponse>.Create(items, model);
    }
}