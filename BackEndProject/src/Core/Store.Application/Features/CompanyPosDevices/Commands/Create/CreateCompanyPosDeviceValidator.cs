using FluentValidation;
using Edition.Application.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyPosDevices.Commands;

public class CreateCompanyPosDeviceValidator : AbstractValidator<CreateCompanyPosDeviceRequest>
{
    public CreateCompanyPosDeviceValidator()
    {
        RuleFor(x => x.Description)
         .MaximumLength(200)
         .WithMessage(MessageKeys.MaxLength200Characters)
         .MinimumLength(50)
         .WithMessage(MessageKeys.MinLength50Characters);

        RuleFor(x => x.BankId)
         .GreaterThan(0)
         .WithMessage(MessageKeys.InvalidBankId);

        RuleFor(x => x.CompanyId)
         .GreaterThan(0)
         .WithMessage(MessageKeys.InvalidStoreId);

        RuleFor(x => x.IP)
         .IsValidIP()
         .NotEmpty()
         .WithMessage(MessageKeys.IpRequired);

        RuleFor(x => x.Name)
         .NotEmpty()
         .WithMessage(MessageKeys.NameRequired)
         .MaximumLength(50)
         .WithMessage(MessageKeys.MaxLength50Characters)
         .MinimumLength(10)
         .WithMessage(MessageKeys.MinLength10Characters);
    }
}
