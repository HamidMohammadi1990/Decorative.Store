using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductRepository
    (EditionDbContext context)
    : Repository<Product>(context), IProductRepository
{
    public async Task<ProductSummaryDto?> GetProductSummaryByIdAsync(int id)
    {
        return await Context
            .Product
            .Include(x => x.ProductFiles)
            .Include(x => x.SubCategory)
            .AsNoTracking()
            .Select(x => new ProductSummaryDto
            {
                Id = id,
                Slug = x.Slug,
                Title = x.Title,
                Description = x.Description,
                ProductCode = x.ProductCode,
                SubCategoryTitle = x.SubCategory!.Title,
                Images = x.ProductFiles!.Select(file => new CheckoutProductImageDto
                {
                    Url = file.FileName,
                    Title = file.Title
                }).ToList()
            }).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PagedResult<Product>> GetAllAsync(GetAllProductRequestDto request, CancellationToken cancellationToken = default)
    {
        var productSource = Context.Product
            .ApplyContentPolicyFilter(request.ContentFilter);

        var products =
            from product in productSource
            join subCategory in Context.SubCategory on product.SubCategoryId equals subCategory.Id
            join category in Context.Category on subCategory.CategoryId equals category.Id
            join companyProduct in Context.CompanyProduct on product.Id equals companyProduct.ProductId
            where product.IsActive
            select new { product, subCategory, category, companyProduct };

        products = products.ApplyQueryFilters(request);

        var result =
            await products
            .Select(x => x.product)
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}