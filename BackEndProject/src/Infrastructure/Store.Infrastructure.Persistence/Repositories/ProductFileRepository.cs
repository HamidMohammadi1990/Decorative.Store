using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductFiles;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductFileRepository
    (EditionDbContext context)
    : Repository<ProductFile>(context), IProductFileRepository
{
    public async Task<PagedResult<GetAllProductFileResponseDto>> GetAllAsync(GetAllProductFileRequestDto request)
    {
        var productFileSource = Context.ProductFile
            .ApplyContentPolicyFilter(request.ContentFilter);

        var productFiles =
            from productFile in productFileSource
            join product in Context.Product on productFile.ProductId equals product.Id
            select new { productFile, product };

        productFiles = productFiles.ApplyQueryFilters(request);

        var result = await
            productFiles
            .Select(x => new GetAllProductFileResponseDto
            {
                Id = x.productFile.Id,
                Title = x.productFile.Title,
                IsMain = x.productFile.IsMain,
                IsActive = x.productFile.IsActive,
                FileName = x.productFile.FileName,
                ProductId = x.productFile.ProductId,
                ProductTitle = x.product.Title
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchProductFileResponseDto>> SearchAsync(SearchProductFileRequestDto request)
    {
        var productFileSource = Context.ProductFile
            .ApplyContentPolicyFilter(request.ContentFilter);

        var productFiles =
            from productFile in productFileSource
            where productFile.IsActive
            select productFile;

        productFiles = productFiles.ApplyQueryFilters(request);

        var result = await
            productFiles
            .Select(x => new SearchProductFileResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                IsMain = x.IsMain,
                FileName = x.FileName,
                ProductId = x.ProductId
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}