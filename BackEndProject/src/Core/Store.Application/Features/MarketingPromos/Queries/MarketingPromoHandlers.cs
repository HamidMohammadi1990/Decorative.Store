using Edition.Application.Common.Directories;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.MarketingPromos.Queries;

public class GetAllMarketingPromoHandler
    (IMarketingPromoRepository repository, IMarketingPromoMapperService mapper)
    : IRequestHandler<GetAllMarketingPromoRequest, OperationResult<PagedResult<GetAllMarketingPromoResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllMarketingPromoResponse>>> Handle(
        GetAllMarketingPromoRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var result = await repository.GetAllAsync(requestModel);
        return mapper.Map(result);
    }
}

public class GetMarketingPromoHandler
    (IMarketingPromoRepository repository, IMarketingPromoMapperService mapper)
    : IRequestHandler<GetMarketingPromoRequest, OperationResult<GetMarketingPromoResponse?>>
{
    public async Task<OperationResult<GetMarketingPromoResponse?>> Handle(
        GetMarketingPromoRequest request,
        CancellationToken cancellationToken)
    {
        var entity = await repository.GetAsNoTrackingAsync(request.Id, cancellationToken);
        if (entity is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(entity);
    }
}

public class GetMarketingPromoStripHandler
    (IMarketingPromoRepository repository)
    : IRequestHandler<GetMarketingPromoStripRequest, OperationResult<GetMarketingPromoStripResponse>>
{
    public async Task<OperationResult<GetMarketingPromoStripResponse>> Handle(
        GetMarketingPromoStripRequest request,
        CancellationToken cancellationToken)
    {
        if (request.LanguageId <= 0)
            return ErrorModel.Create("InvalidRequest");

        var promos = await repository.GetActiveStripAsync(request.LanguageId, cancellationToken);
        var disclaimer = await repository.GetDisclaimerAsync(request.LanguageId, cancellationToken);

        var tiles = promos
            .Select(x => new MarketingPromoStripTileResponse
            {
                Id = x.Id,
                PromoType = x.PromoType,
                Title = x.Title,
                Subtitle = x.Subtitle,
                ImageUrl = ResolveImageUrl(x.ImageFileName),
                LinkLabel = x.LinkLabel,
                LinkHref = x.LinkHref,
            })
            .ToList();

        MarketingPromoStripLinkResponse? disclaimerLink = null;
        if (!string.IsNullOrWhiteSpace(disclaimer?.DisclaimerLinkLabel)
            && !string.IsNullOrWhiteSpace(disclaimer.DisclaimerLinkHref))
        {
            disclaimerLink = new MarketingPromoStripLinkResponse
            {
                Label = disclaimer.DisclaimerLinkLabel,
                Href = disclaimer.DisclaimerLinkHref,
            };
        }

        return new GetMarketingPromoStripResponse
        {
            Tiles = tiles,
            Disclaimer = disclaimer?.Disclaimer,
            DisclaimerLink = disclaimerLink,
        };
    }

    private static string ResolveImageUrl(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return string.Empty;

        if (fileName.StartsWith('/') || fileName.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return fileName;

        return ProductDirectory.GetImageUrl(fileName);
    }
}
