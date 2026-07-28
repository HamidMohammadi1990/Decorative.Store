using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserAddresses.Commands;

public class CreateUserAddressValidator : AbstractValidator<CreateUserAddressRequest>
{
    public CreateUserAddressValidator(IUserAddressRepository userAddressRepository)
    {
        RuleFor(x => new { x.Title, x.PostalCode })
        .NotNull()
        .WithMessage(MessageKeys.TitleRequired)
        .MustAsync(async (x, CancellationToken)
               => !await userAddressRepository.AnyAsync(c => c.PostalCode == x.PostalCode || c.Title == x.Title.Trim()))
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
    }
}
