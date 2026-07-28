using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductDescriptions;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductDescriptionRepository
    (EditionDbContext context)
    : Repository<ProductDescription>(context), IProductDescriptionRepository
{
    public async Task<PagedResult<GetAllProductDescriptionResponseDto>> GetAllAsync(GetAllProductDescriptionRequestDto request)
    {
        var descriptions = Context
               .ProductDescription
               .ApplyContentPolicyFilter(request.ContentFilter)
               .Include(x => x.Product)
               .ApplyQueryFilters(request);

        var result =
            await descriptions
            .AsNoTracking()
            .Select(x => new GetAllProductDescriptionResponseDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Description = x.Description,
                ProductTitle = x.Product.Title
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchProductDescriptionResponseDto>> SearchAsync(SearchProductDescriptionRequestDto request)
    {
        var descriptions = Context
               .ProductDescription
               .ApplyContentPolicyFilter(request.ContentFilter)
               .ApplyQueryFilters(request);

        var result =
            await descriptions
            .AsNoTracking()
            .Select(x => new SearchProductDescriptionResponseDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Description = x.Description
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }
}