using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.MarketingPromos.Commands;

public class CreateMarketingPromoHandler
    (IUnitOfWork uow, IMarketingPromoRepository repository)
    : IRequestHandler<CreateMarketingPromoRequest, OperationResult<CreateMarketingPromoResponse>>
{
    public async Task<OperationResult<CreateMarketingPromoResponse>> Handle(
        CreateMarketingPromoRequest request,
        CancellationToken cancellationToken)
    {
        var entity = MarketingPromo.Create(
            request.LanguageId,
            request.PromoType,
            request.Title.Trim(),
            string.IsNullOrWhiteSpace(request.Subtitle) ? null : request.Subtitle.Trim(),
            request.LinkLabel.Trim(),
            request.LinkHref.Trim(),
            request.ImageFileName.Trim(),
            request.Priority);

        repository.Add(entity);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateMarketingPromoResponse>();

        return new CreateMarketingPromoResponse { Id = entity.Id };
    }
}

public class UpdateMarketingPromoHandler
    (IUnitOfWork uow, IMarketingPromoRepository repository)
    : IRequestHandler<UpdateMarketingPromoRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateMarketingPromoRequest request, CancellationToken cancellationToken)
    {
        var entity = await repository.FindAsync(request.Id, cancellationToken);
        if (entity is null)
            return ErrorModel.Create("InvalidId");

        entity.Update(
            request.LanguageId,
            request.PromoType,
            request.Title.Trim(),
            string.IsNullOrWhiteSpace(request.Subtitle) ? null : request.Subtitle.Trim(),
            request.LinkLabel.Trim(),
            request.LinkHref.Trim(),
            request.ImageFileName.Trim(),
            request.Priority,
            request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}

public class DeleteMarketingPromoHandler
    (IUnitOfWork uow, IMarketingPromoRepository repository)
    : IRequestHandler<DeleteMarketingPromoRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteMarketingPromoRequest request, CancellationToken cancellationToken)
    {
        var entity = await repository.FindAsync(request.Id, cancellationToken);
        if (entity is null)
            return ErrorModel.Create("InvalidId");

        repository.Remove(entity);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}

public class UpdateMarketingStripDisclaimerHandler
    (IUnitOfWork uow, IMarketingPromoRepository repository)
    : IRequestHandler<UpdateMarketingStripDisclaimerRequest, OperationResult>
{
    public async Task<OperationResult> Handle(
        UpdateMarketingStripDisclaimerRequest request,
        CancellationToken cancellationToken)
    {
        var disclaimer = await repository.GetDisclaimerForUpdateAsync(request.LanguageId, cancellationToken);
        var trimmedDisclaimer = string.IsNullOrWhiteSpace(request.Disclaimer) ? null : request.Disclaimer.Trim();
        var trimmedLinkLabel = string.IsNullOrWhiteSpace(request.DisclaimerLinkLabel)
            ? null
            : request.DisclaimerLinkLabel.Trim();
        var trimmedLinkHref = string.IsNullOrWhiteSpace(request.DisclaimerLinkHref)
            ? null
            : request.DisclaimerLinkHref.Trim();

        if (disclaimer is null)
        {
            repository.AddDisclaimer(MarketingStripDisclaimer.Create(
                request.LanguageId,
                trimmedDisclaimer,
                trimmedLinkLabel,
                trimmedLinkHref));
        }
        else
        {
            disclaimer.Update(trimmedDisclaimer, trimmedLinkLabel, trimmedLinkHref);
        }

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
