using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductPropertyPrices;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductPropertyPriceRepository
    (EditionDbContext context)
    : Repository<ProductPropertyPrice>(context), IProductPropertyPriceRepository
{
    public async Task<PagedResult<GetAllProductPropertyPriceDto>> GetAllAsync(GetAllProductPropertyPriceRequestDto request)
    {
        var productPropertyPriceSource = Context.ProductPropertyPrice
            .ApplyContentPolicyFilter(request.ContentFilter);

        var productPropertyPrices =
            from productPropertyPrice in productPropertyPriceSource
            join company in Context.Company on productPropertyPrice.CompanyId equals company.Id
            join user in Context.User on company.UserId equals user.Id
            join productProperty in Context.ProductProperty on productPropertyPrice.ProductPropertyId equals productProperty.Id
            join product in Context.Product on productProperty.ProductId equals product.Id
            select new { productPropertyPrice, company, user, product };

        productPropertyPrices = productPropertyPrices.ApplyQueryFilters(request);

        var result = await
            productPropertyPrices
            .Select(x => new GetAllProductPropertyPriceDto
            {
                Id = x.productPropertyPrice.Id,
                Price = x.productPropertyPrice.Price,
                UserId = x.company.UserId,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                IsActive = x.productPropertyPrice.IsActive,
                ProductId = x.product.Id,
                CompanyId = x.productPropertyPrice.CompanyId,
                ProductTitle = x.product.Title,
                CreatedOnUtc = x.productPropertyPrice.CreatedOnUtc,
                CooperationPrice = x.productPropertyPrice.CooperationPrice,
                ProductPropertyId = x.productPropertyPrice.ProductPropertyId
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}