using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductProperties.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductProperties;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductPropertyMapperService : IProductPropertyMapperService
{
    public GetAllProductPropertyRequestDto Map(GetAllProductPropertyRequest model)
        => new GetAllProductPropertyRequestDto
        {
            ProductId = model.ProductId,
            PropertyId = model.PropertyId,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductProperty, GetAllProductPropertyRequestDto>(model);

    public SearchProductPropertyRequestDto Map(SearchProductPropertyRequest model)
        => new SearchProductPropertyRequestDto
        {
            ProductId = model.ProductId,
            PropertyId = model.PropertyId,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductProperty, SearchProductPropertyRequestDto>(model);

    public GetProductPropertyResponse Map(ProductProperty model)
    {
        return new GetProductPropertyResponse
        {
            Id = model.Id,
            ProductId = model.ProductId,
            PropertyId = model.PropertyId,
            PropertyItemId = model.PropertyItemId,
            IsActive = model.IsActive
        };
    }

    public PagedResult<GetAllProductPropertyResponse> Map(PagedResult<ProductProperty> model)
    {
        var items = model.Items.Select(x => new GetAllProductPropertyResponse
        {
            Id = x.Id,
            ProductId = x.ProductId,
            PropertyId = x.PropertyId,
            PropertyItemId = x.PropertyItemId,
            IsActive = x.IsActive
        }).ToList();
        return PagedResult<GetAllProductPropertyResponse>.Create(items, model);
    }

    public PagedResult<SearchProductPropertyResponse> MapToSearch(PagedResult<ProductProperty> model)
    {
        var items = model.Items.Select(x => new SearchProductPropertyResponse
        {
            Id = x.Id,
            ProductId = x.ProductId,
            PropertyId = x.PropertyId
        }).ToList();
        return PagedResult<SearchProductPropertyResponse>.Create(items, model);
    }
}