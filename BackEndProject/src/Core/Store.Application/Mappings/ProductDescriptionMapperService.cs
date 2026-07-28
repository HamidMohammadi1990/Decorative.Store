using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductDescriptions.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductDescriptions;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductDescriptionMapperService : IProductDescriptionMapperService
{
    public GetAllProductDescriptionRequestDto Map(GetAllProductDescriptionRequest model)
    {
        return new GetAllProductDescriptionRequestDto
        {
            ProductId = model.ProductId,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductDescription, GetAllProductDescriptionRequestDto>(model);
    }

    public SearchProductDescriptionRequestDto Map(SearchProductDescriptionRequest model)
    {
        return new SearchProductDescriptionRequestDto
        {
            ProductId = model.ProductId,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductDescription, SearchProductDescriptionRequestDto>(model);
    }

    public GetProductDescriptionResponse Map(ProductDescription model)
    {
        return new GetProductDescriptionResponse
        {
            Id = model.ProductId,
            ProductId = model.ProductId,
            Description = model.Description
        };
    }

    public PagedResult<GetAllProductDescriptionResponse> Map(PagedResult<GetAllProductDescriptionResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllProductDescriptionResponse
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Description = x.Description,
                ProductTitle = x.ProductTitle
            })
            .ToList();

        return PagedResult<GetAllProductDescriptionResponse>.Create(items, model);
    }

    public PagedResult<SearchProductDescriptionResponse> Map(PagedResult<SearchProductDescriptionResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchProductDescriptionResponse
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Description = x.Description
            })
            .ToList();

        return PagedResult<SearchProductDescriptionResponse>.Create(items, model);
    }
}