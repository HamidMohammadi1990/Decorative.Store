using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Localization;
using Edition.Application.Features.Products.Queries;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductMapperService : IProductMapperService
{
    public GetProductResponse Map(Product model, string title, string slug, string description)
    {
        return new GetProductResponse
        {
            Id = model.Id,
            Title = title,
            Slug = slug,
            IsActive = model.IsActive,
            ProductCode = model.ProductCode,
            Description = description,
            CreationDate = model.CreatedOnUtc,
            Price = model.Price,
            CompareAtPrice = model.CompareAtPrice
        };
    }

    public PagedResult<GetAllProductResponse> Map(PagedResult<GetAllProductResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllProductResponse
            {
                Id = x.Id,
                IsActive = x.IsActive,
                CreationDate = x.CreatedOnUtc,
                ProductCode = x.ProductCode,
                Translations = MapTranslations(x.Translations)
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

    private static List<ProductTranslationItemResponse> MapTranslations(
        IReadOnlyList<Store.Domain.Dtos.Localization.ProductTranslationItemDto> translations)
        => translations
            .Select(t => new ProductTranslationItemResponse
            {
                LanguageId = t.LanguageId,
                Title = t.Title,
                Slug = t.Slug,
                Description = t.Description
            })
            .ToList();
}
