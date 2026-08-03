using FluentValidation;
using Store.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Commands;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(u => u.UserName)
               .NotNull()
               .WithMessage(MessageKeys.UserNameRequired)
               .Must(u => u.IsMobile())
               .WithMessage(MessageKeys.UserNameMustBeMobile)
               .MaximumLength(11)
               .WithMessage(MessageKeys.UserNameMaxLength11);

        RuleFor(u => u.FirstName)
               .NotNull()
               .WithMessage(MessageKeys.FirstNameRequired)
               .MaximumLength(50)
               .WithMessage(MessageKeys.FirstNameMaxLength50);

        RuleFor(u => u.LastName)
               .NotNull()
               .WithMessage(MessageKeys.LastNameRequired)
               .MaximumLength(50)
               .WithMessage(MessageKeys.LastNameMaxLength50);

        RuleFor(u => u.Email)
            .NotNull()
            .WithMessage(MessageKeys.EmailRequired)
            .Must(x => x.IsEmail())
            .WithMessage(MessageKeys.EmailIsNotValid);

        RuleFor(u => u.PhoneNumber)
            .Must(x => x.IsMobile())
            .WithMessage(MessageKeys.MobileIsNotValid);

        RuleFor(u => u.Password)
           .NotNull()
           .WithMessage(MessageKeys.PasswordRequired)
           .MinimumLength(5)
           .WithMessage(MessageKeys.PasswordIsNotValid);

        RuleFor(u => u.Gender)
          .IsInEnum()
          .NotNull()
          .WithMessage(MessageKeys.GenderRequired);
    }
}
