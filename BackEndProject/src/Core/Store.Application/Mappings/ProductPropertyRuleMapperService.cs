using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductPropertyRules.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Dtos.ProductPropertyRules;

namespace Edition.Application.Mappings;

public class ProductPropertyRuleMapperService : IProductPropertyRuleMapperService
{
    public GetAllProductPropertyRuleRequestDto Map(GetAllProductPropertyRuleRequest model)
        => new GetAllProductPropertyRuleRequestDto
        {
            ProductPropertyId = model.ProductPropertyId,
            PropertyType = model.PropertyType,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductPropertyRule, GetAllProductPropertyRuleRequestDto>(model);

    public SearchProductPropertyRuleRequestDto Map(SearchProductPropertyRuleRequest model)
        => new SearchProductPropertyRuleRequestDto
        {
            ProductPropertyId = model.ProductPropertyId,
            PropertyType = model.PropertyType,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductPropertyRule, SearchProductPropertyRuleRequestDto>(model);

    public GetProductPropertyRuleResponse Map(ProductPropertyRule model)
    {
        var payload = model.Get();
        return new GetProductPropertyRuleResponse
        {
            Id = model.Id,
            ProductPropertyId = model.ProductPropertyId,
            PropertyType = model.PropertyType,
            IsMandatory = model.IsMandatory,
            Description = model.Description,
            IsActive = model.IsActive,
            MinLength = payload.MinLength,
            MaxLength = payload.MaxLength,
            MinQuantity = payload.MinQuantity,
            MaxQuantity = payload.MaxQuantity,
            MinWidth = payload.MinWidth,
            MaxWidth = payload.MaxWidth,
            MinHeight = payload.MinHeight,
            MaxHeight = payload.MaxHeight
        };
    }

    public PagedResult<GetAllProductPropertyRuleResponse> Map(PagedResult<ProductPropertyRule> model)
    {
        var items = model.Items.Select(x =>
        {
            var payload = x.Get();
            return new GetAllProductPropertyRuleResponse
            {
                Id = x.Id,
                ProductPropertyId = x.ProductPropertyId,
                PropertyType = x.PropertyType,
                IsMandatory = x.IsMandatory,
                Description = x.Description,
                IsActive = x.IsActive,
                MinLength = payload.MinLength,
                MaxLength = payload.MaxLength,
                MinQuantity = payload.MinQuantity,
                MaxQuantity = payload.MaxQuantity,
                MinWidth = payload.MinWidth,
                MaxWidth = payload.MaxWidth,
                MinHeight = payload.MinHeight,
                MaxHeight = payload.MaxHeight
            };
        }).ToList();

        return PagedResult<GetAllProductPropertyRuleResponse>.Create(items, model);
    }

    public PagedResult<SearchProductPropertyRuleResponse> MapToSearch(PagedResult<ProductPropertyRule> model)
    {
        var items = model.Items.Select(x =>
        {
            var payload = x.Get();
            return new SearchProductPropertyRuleResponse
            {
                Id = x.Id,
                ProductPropertyId = x.ProductPropertyId,
                PropertyType = x.PropertyType,
                IsMandatory = x.IsMandatory,
                Description = x.Description,
                MinLength = payload.MinLength,
                MaxLength = payload.MaxLength,
                MinQuantity = payload.MinQuantity,
                MaxQuantity = payload.MaxQuantity,
                MinWidth = payload.MinWidth,
                MaxWidth = payload.MaxWidth,
                MinHeight = payload.MinHeight,
                MaxHeight = payload.MaxHeight
            };
        }).ToList();

        return PagedResult<SearchProductPropertyRuleResponse>.Create(items, model);
    }
}