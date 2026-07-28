using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.ProductPrices;
using Store.Domain.Dtos.Others;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductPriceRepository
    (EditionDbContext context)
    : Repository<ProductPrice>(context), IProductPriceRepository
{
    public async Task<PagedResult<GetAllProductPriceDto>> GetAllAsync(GetAllProductPriceRequestDto request)
    {
        var productPriceSource = Context.ProductPrice
            .ApplyContentPolicyFilter(request.ContentFilter);

        var productprices =
            from productPrice in productPriceSource
            join product in Context.Product on productPrice.ProductId equals product.Id
            join company in Context.Company on productPrice.CompanyId equals company.Id
            join user in Context.User on company.UserId equals user.Id
            select new { productPrice, product, company, user };

        productprices = productprices.ApplyQueryFilters(request);

        var result = await productprices
            .Select(x => new GetAllProductPriceDto
            {
                Id = x.productPrice.Id,
                Price = x.productPrice.Price,
                IsActive = x.productPrice.IsActive,
                ProductId = x.productPrice.ProductId,
                CompanyId = x.productPrice.CompanyId,
                CompanyName = x.company.Name,
                ProductTitle = x.product.Title,
                UserFirstName = x.user.FirstName!,
                UserLastName = x.user.LastName!,
                UserId = x.user.Id,
                CreatedOnUtc = x.productPrice.CreatedOnUtc,
                CooperationPrice = x.productPrice.CooperationPrice,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PurchaseProductPriceDto?> GetPriceByProductId(int productId, int companyId)
    {
        return await
            Context.ProductPrice
            .Where(x => x.ProductId == productId && x.CompanyId == companyId && x.IsActive)
            .Select(x => PurchaseProductPriceDto.Create(PriceField.Create("", x.Price), PriceField.Create("", x.CooperationPrice)))
            .FirstOrDefaultAsync();
    }
}