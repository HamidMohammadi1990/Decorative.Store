using FluentValidation;
using Edition.Application.Contracts;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserAddresses.Commands;

public class UpdateUserAddressValidator : AbstractValidator<UpdateUserAddressRequest>
{
    public UpdateUserAddressValidator(IUserAddressRepository userAddressRepository, ICurrentUserContext currentUser)
    {
        RuleFor(x => new { x.Id, x.Title, x.PostalCode })
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired)
            .MustAsync(async (x, cancellationToken)
                => !await userAddressRepository.AnyAsync(
                    c => c.UserId == currentUser.UserId
                         && c.Id != x.Id
                         && (c.Title == x.Title.Trim()
                             || (!string.IsNullOrWhiteSpace(x.PostalCode) && c.PostalCode == x.PostalCode)),
                    cancellationToken))
            .WithMessage(MessageKeys.DuplicateAddress)
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(30)
            .WithMessage(MessageKeys.MaxLength30Characters);

        RuleFor(x => x.RecipientFirstName)
            .MaximumLength(20)
            .WithMessage(MessageKeys.MaxLength20Characters);

        RuleFor(x => x.RecipientLastName)
            .MaximumLength(30)
            .WithMessage(MessageKeys.MaxLength30Characters);

        RuleFor(x => x.PostalCode)
            .MaximumLength(10)
            .WithMessage(MessageKeys.MaxLength10Characters);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(11)
            .WithMessage(MessageKeys.MaxLength11Characters);

        RuleFor(x => x.Address)
            .NotNull()
            .WithMessage(MessageKeys.AddressRequired)
            .MaximumLength(150)
            .WithMessage(MessageKeys.MaxLength150Characters);

        RuleFor(x => x.Apartment)
            .MaximumLength(50)
            .WithMessage(MessageKeys.MaxLength50Characters)
            .When(x => !string.IsNullOrEmpty(x.Apartment));

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .WithMessage(MessageKeys.InvalidLatitude)
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .WithMessage(MessageKeys.InvalidLongitude)
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x)
            .Must(x => x.Latitude.HasValue == x.Longitude.HasValue)
            .WithMessage(MessageKeys.InvalidLatitude)
            .When(x => x.Latitude.HasValue || x.Longitude.HasValue);
    }
}
