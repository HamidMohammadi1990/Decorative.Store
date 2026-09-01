using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.MarketingPromos.Queries;
using Store.Domain.Dtos.MarketingPromos;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class MarketingPromoMapperService : IMarketingPromoMapperService
{
    public GetAllMarketingPromoRequestDto Map(GetAllMarketingPromoRequest model)
        => new GetAllMarketingPromoRequestDto
        {
            LanguageId = model.LanguageId,
            Title = model.Title,
            PromoType = model.PromoType,
            IsActive = model.IsActive,
            Pagination = model.Pagination,
        }.WithContentPolicy<MarketingPromo, GetAllMarketingPromoRequestDto>(model);

    public PagedResult<GetAllMarketingPromoResponse> Map(PagedResult<GetAllMarketingPromoResponseDto> model)
    {
        var items = model.Items
            .Select(x => new GetAllMarketingPromoResponse
            {
                Id = x.Id,
                LanguageId = x.LanguageId,
                PromoType = x.PromoType,
                Title = x.Title,
                Subtitle = x.Subtitle,
                LinkLabel = x.LinkLabel,
                LinkHref = x.LinkHref,
                ImageFileName = x.ImageFileName,
                Priority = x.Priority,
                IsActive = x.IsActive,
            })
            .ToList();

        return PagedResult<GetAllMarketingPromoResponse>.Create(items, model);
    }

    public GetMarketingPromoResponse Map(MarketingPromo model)
        => new()
        {
            Id = model.Id,
            LanguageId = model.LanguageId,
            PromoType = model.PromoType,
            Title = model.Title,
            Subtitle = model.Subtitle,
            LinkLabel = model.LinkLabel,
            LinkHref = model.LinkHref,
            ImageFileName = model.ImageFileName,
            Priority = model.Priority,
            IsActive = model.IsActive,
        };
}
