using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductFeatureTypes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductFeatureTypes;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductFeatureTypeMapperService : IProductFeatureTypeMapperService
{
    public GetAllProductFeatureTypeRequestDto Map(GetAllProductFeatureTypeRequest model)
        => new GetAllProductFeatureTypeRequestDto
        {
            Name = model.Name,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductFeatureType, GetAllProductFeatureTypeRequestDto>(model);

    public SearchProductFeatureTypeRequestDto Map(SearchProductFeatureTypeRequest model)
        => new SearchProductFeatureTypeRequestDto
        {
            Name = model.Name,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductFeatureType, SearchProductFeatureTypeRequestDto>(model);

    public GetProductFeatureTypeResponse Map(ProductFeatureType model)
    {
        return new GetProductFeatureTypeResponse
        {
            Id = model.Id,
            Name = model.Name,
            Type = model.Type,
            DataType = model.DataType,
            Description = model.Description,
            IsActive = model.IsActive
        };
    }

    public PagedResult<GetAllProductFeatureTypeResponse> Map(PagedResult<ProductFeatureType> model)
    {
        var items = model.Items.Select(x => new GetAllProductFeatureTypeResponse
        {
            Id = x.Id,
            Name = x.Name,
            Type = x.Type,
            DataType = x.DataType,
            Description = x.Description,
            IsActive = x.IsActive
        }).ToList();

        return PagedResult<GetAllProductFeatureTypeResponse>.Create(items, model);
    }

    public PagedResult<SearchProductFeatureTypeResponse> MapToSearch(PagedResult<ProductFeatureType> model)
    {
        var items = model.Items.Select(x => new SearchProductFeatureTypeResponse
        {
            Id = x.Id,
            Name = x.Name,
            Type = x.Type,
            DataType = x.DataType,
            Description = x.Description
        }).ToList();

        return PagedResult<SearchProductFeatureTypeResponse>.Create(items, model);
    }
}