using Edition.Application.Common.Directories;
using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductFiles.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductFiles;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductFileMapperService : IProductFileMapperService
{
    public GetProductFileResponse Map(ProductFile model, string title)
    {
        return new GetProductFileResponse
        {
            Title = title,
            ImageUrl = ProductDirectory.GetImageUrl(model.FileName),
            FileName = model.FileName,
            ProductId = model.ProductId
        };
    }

    public GetAllProductFileRequestDto Map(GetAllProductFileRequest model)
    {
        return new GetAllProductFileRequestDto
        {
            Title = model.Title,
            IsMain = model.IsMain,
            IsActive = model.IsActive,
            ProductId = model.ProductId,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductFile, GetAllProductFileRequestDto>(model);
    }

    public SearchProductFileRequestDto Map(SearchProductFileRequest model)
    {
        return new SearchProductFileRequestDto
        {
            Title = model.Title,
            IsMain = model.IsMain,
            ProductId = model.ProductId,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductFile, SearchProductFileRequestDto>(model);
    }

    public PagedResult<GetAllProductFileResponse> Map(PagedResult<GetAllProductFileResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllProductFileResponse
            {
                Id = x.Id,
                Title = x.Title,
                IsMain = x.IsMain,
                IsActive = x.IsActive,
                Kind = x.Kind,
                FileName = x.FileName,
                ProductId = x.ProductId,
                ProductTitle = x.ProductTitle
            })
            .ToList();

        return PagedResult<GetAllProductFileResponse>.Create(items, model);
    }

    public PagedResult<SearchProductFileResponse> Map(PagedResult<SearchProductFileResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchProductFileResponse
            {
                Id = x.Id,
                Title = x.Title,
                IsMain = x.IsMain,
                Kind = x.Kind,
                FileName = x.FileName,
                ProductId = x.ProductId
            })
            .ToList();

        return PagedResult<SearchProductFileResponse>.Create(items, model);
    }
}
