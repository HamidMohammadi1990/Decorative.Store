using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Discounts;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class DiscountRepository
    (EditionDbContext context)
    : Repository<Discount>(context), IDiscountRepository
{
    public async Task<PagedResult<GetAllDiscountResponseDto>> GetAllAsync(GetAllDiscountRequestDto request)
    {
        var discounts = Context.Discount
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        var result =
            await discounts
            .Select(x => new GetAllDiscountResponseDto
            {
                Id = x.Id,
                Code = x.Code,
                UserId = x.UserId,
                Amount = x.Amount,
                IsActive = x.IsActive,
                ProductId = x.ProductId,
                UsageLimit = x.UsageLimit,
                Percentage = x.Percentage,
                RemainingUses = x.RemainingUses,
                MinimumAmount = x.MinimumAmount,
                SubCategoryId = x.SubCategoryId,
                IsCooperation = x.IsCooperation,
                ExpiryDateOnUtc = x.ExpiryDateOnUtc,
                MaxDiscountAmount = x.MaxDiscountAmount,
                ToCirculationOrMeterOrCount = x.ToCirculationOrMeterOrCount,
                FromCirculationOrMeterOrCount = x.FromCirculationOrMeterOrCount
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public Task<Discount?> GetByCodeAsync(string code)
    {
        return Context.Discount.SingleOrDefaultAsync(x => x.Code == code);
    }

    public Task<Discount?> GetByIdAsync(int id)
    {
        return Context.Discount.SingleOrDefaultAsync(x => x.Id == id);
    }
}
