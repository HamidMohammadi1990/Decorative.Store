using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Products.Queries;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductMapperService : IProductMapperService
{
    public GetProductResponse Map(Product model)
    {
        return new GetProductResponse
        {
            Id = model.Id,
            Title = model.Title,
            IsActive = model.IsActive,
            ProductCode = model.ProductCode,
            Description = model.Description,
            CreationDate = model.CreatedOnUtc
        };
    }

    public PagedResult<GetAllProductResponse> Map(PagedResult<Product> model)
    {
        var items = model
            .Items
           .Select(x => new GetAllProductResponse
           {
               Id = x.Id,
               Title = x.Title,
               IsActive = x.IsActive,
               Description = x.Description,
               CreationDate = x.CreatedOnUtc,
               ProductCode = x.ProductCode
           })
           .ToList();

        return PagedResult<GetAllProductResponse>.Create(items, model);
    }

    public GetAllProductRequestDto Map(GetAllProductRequest model)
    {
        return new GetAllProductRequestDto
        {
            Slug = model.Slug,
            Title = model.Title,
            CategoryId = model.CategoryId,
            Pagination = model.Pagination,
            ProductCode = model.ProductCode,
            CategorySlug = model.CategorySlug,
            SubCategoryId = model.SubCategoryId,
            SubCategorySlug = model.SubCategorySlug
        }.WithContentPolicy<Product, GetAllProductRequestDto>(model);
    }
}