using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Discounts.Queries;
using Store.Domain.Dtos.Discounts;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class DiscountMapperService : IDiscountMapperService
{
    public GetDiscountResponse Map(Discount model)
    {
        return new GetDiscountResponse
        {
            Id = model.Id,
            Code = model.Code,
            UserId = model.UserId,
            ProductId = model.ProductId,
            SubCategoryId = model.SubCategoryId,
            Percentage = model.Percentage,
            Amount = model.Amount,
            ExpiryDateOnUtc = model.ExpiryDateOnUtc,
            MaxDiscountAmount = model.MaxDiscountAmount,
            FromCirculationOrMeterOrCount = model.FromCirculationOrMeterOrCount,
            ToCirculationOrMeterOrCount = model.ToCirculationOrMeterOrCount,
            UsageLimit = model.UsageLimit,
            RemainingUses = model.RemainingUses,
            IsCooperation = model.IsCooperation,
            MinimumAmount = model.MinimumAmount,
            IsActive = model.IsActive
        };
    }

    public PagedResult<GetAllDiscountResponse> Map(PagedResult<GetAllDiscountResponseDto> model)
    {
        var items = model.Items.Select(x => new GetAllDiscountResponse
        {
            Id = x.Id,
            Code = x.Code,
            UserId = x.UserId,
            ProductId = x.ProductId,
            SubCategoryId = x.SubCategoryId,
            Percentage = x.Percentage,
            Amount = x.Amount,
            ExpiryDateOnUtc = x.ExpiryDateOnUtc,
            MaxDiscountAmount = x.MaxDiscountAmount,
            UsageLimit = x.UsageLimit,
            RemainingUses = x.RemainingUses,
            IsCooperation = x.IsCooperation,
            MinimumAmount = x.MinimumAmount,
            IsActive = x.IsActive
        }).ToList();

        return PagedResult<GetAllDiscountResponse>.Create(items, model);
    }

    public GetAllDiscountRequestDto Map(GetAllDiscountRequest model)
    {
        return new GetAllDiscountRequestDto
        {
            Code = model.Code,
            UserId = model.UserId,
            IsActive = model.IsActive,
            ProductId = model.ProductId,
            Pagination = model.Pagination
        }.WithContentPolicy<Discount, GetAllDiscountRequestDto>(model);
    }
}