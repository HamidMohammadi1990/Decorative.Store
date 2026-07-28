using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductOrderItemAttachmentTypes;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductOrderItemAttachmentTypeMapperService : IProductOrderItemAttachmentTypeMapperService
{
    public GetAllProductOrderItemAttachmentTypeRequestDto Map(GetAllProductOrderItemAttachmentTypeRequest model)
        => new GetAllProductOrderItemAttachmentTypeRequestDto
        {
            ProductId = model.ProductId,
            OrderItemAttachmentTypeId = model.OrderItemAttachmentTypeId,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductOrderItemAttachmentType, GetAllProductOrderItemAttachmentTypeRequestDto>(model);

    public SearchProductOrderItemAttachmentTypeRequestDto Map(SearchProductOrderItemAttachmentTypeRequest model)
        => new SearchProductOrderItemAttachmentTypeRequestDto
        {
            ProductId = model.ProductId,
            OrderItemAttachmentTypeId = model.OrderItemAttachmentTypeId,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductOrderItemAttachmentType, SearchProductOrderItemAttachmentTypeRequestDto>(model);

    public GetProductOrderItemAttachmentTypeResponse Map(ProductOrderItemAttachmentType model)
    {
        return new GetProductOrderItemAttachmentTypeResponse
        {
            Id = model.Id,
            ProductId = model.ProductId,
            Description = model.Description,
            OrderItemAttachmentTypeId = model.OrderItemAttachmentTypeId,
            Priority = model.Priority
        };
    }

    public PagedResult<GetAllProductOrderItemAttachmentTypeResponse> Map(PagedResult<ProductOrderItemAttachmentType> model)
    {
        var items = model.Items.Select(x => new GetAllProductOrderItemAttachmentTypeResponse
        {
            Id = x.Id,
            ProductId = x.ProductId,
            Description = x.Description,
            OrderItemAttachmentTypeId = x.OrderItemAttachmentTypeId,
            Priority = x.Priority
        }).ToList();

        return PagedResult<GetAllProductOrderItemAttachmentTypeResponse>.Create(items, model);
    }

    public PagedResult<SearchProductOrderItemAttachmentTypeResponse> MapToSearch(PagedResult<ProductOrderItemAttachmentType> model)
    {
        var items = model.Items.Select(x => new SearchProductOrderItemAttachmentTypeResponse
        {
            Id = x.Id,
            ProductId = x.ProductId,
            Description = x.Description,
            OrderItemAttachmentTypeId = x.OrderItemAttachmentTypeId,
            Priority = x.Priority
        }).ToList();

        return PagedResult<SearchProductOrderItemAttachmentTypeResponse>.Create(items, model);
    }
}