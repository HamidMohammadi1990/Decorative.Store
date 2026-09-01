using FluentValidation;
using Edition.Application.Common.Validation;
using Edition.Application.Features.MarketingPromos.Commands;
using Edition.Application.Features.MarketingPromos.Queries;
using Store.Common.Localization;
using Store.Domain.Enums;
using Store.Domain.Repositories;

namespace Edition.Application.Features.MarketingPromos;

public class CreateMarketingPromoValidator : AbstractValidator<CreateMarketingPromoRequest>
{
    public CreateMarketingPromoValidator(
        IMarketingPromoRepository repository,
        ILanguageRepository languageRepository)
    {
        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.PromoType)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(200)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Subtitle)
            .MaximumLength(200)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.LinkLabel)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(50)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.LinkHref)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(200)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.ImageFileName)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(35)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await repository.AnyAsync(
                    x => x.LanguageId == request.LanguageId &&
                         x.Title == request.Title.Trim(),
                    cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}

public class UpdateMarketingPromoValidator : AbstractValidator<UpdateMarketingPromoRequest>
{
    public UpdateMarketingPromoValidator(
        IMarketingPromoRepository repository,
        ILanguageRepository languageRepository)
    {
        RuleFor(x => x.Id).MustBeValidEntityId();

        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.PromoType)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(200)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Subtitle)
            .MaximumLength(200)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.LinkLabel)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(50)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.LinkHref)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(200)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.ImageFileName)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(35)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await repository.AnyAsync(
                    x => x.Id != request.Id &&
                         x.LanguageId == request.LanguageId &&
                         x.Title == request.Title.Trim(),
                    cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}

public class DeleteMarketingPromoValidator : AbstractValidator<DeleteMarketingPromoRequest>
{
    public DeleteMarketingPromoValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}

public class UpdateMarketingStripDisclaimerValidator : AbstractValidator<UpdateMarketingStripDisclaimerRequest>
{
    public UpdateMarketingStripDisclaimerValidator(ILanguageRepository languageRepository)
    {
        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Disclaimer)
            .MaximumLength(500)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.DisclaimerLinkLabel)
            .MaximumLength(50)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.DisclaimerLinkHref)
            .MaximumLength(200)
            .WithMessage(MessageKeys.InvalidRequest);
    }
}

public class GetAllMarketingPromoValidator : AbstractValidator<GetAllMarketingPromoRequest>
{
    public GetAllMarketingPromoValidator()
    {
        RuleFor(x => x.Pagination).NotNull();
    }
}

public class GetMarketingPromoValidator : AbstractValidator<GetMarketingPromoRequest>
{
    public GetMarketingPromoValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}

public class GetMarketingPromoStripValidator : AbstractValidator<GetMarketingPromoStripRequest>
{
    public GetMarketingPromoStripValidator()
    {
        RuleFor(x => x.LanguageId).GreaterThan(0).WithMessage(MessageKeys.InvalidRequest);
    }
}
