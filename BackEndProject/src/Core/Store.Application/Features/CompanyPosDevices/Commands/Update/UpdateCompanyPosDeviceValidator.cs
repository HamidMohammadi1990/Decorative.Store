using FluentValidation;
using Edition.Application.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyPosDevices.Commands;

public class UpdateCompanyPosDeviceValidator : AbstractValidator<UpdateCompanyPosDeviceRequest>
{
    public UpdateCompanyPosDeviceValidator()
    {
        RuleFor(x => x.Description)
         .MaximumLength(200)
         .WithMessage(MessageKeys.MaxLength200Characters)
         .MinimumLength(50)
         .WithMessage(MessageKeys.MinLength50Characters);

        RuleFor(x => x.BankId)
         .Equal(0)
         .WithMessage(MessageKeys.InvalidIdValidator);

        RuleFor(x => x.CompanyId)
         .Equal(0)
         .WithMessage(MessageKeys.InvalidIdValidator);

        RuleFor(x => x.IP)
         .IsValidIP()
         .NotNull()
         .WithMessage(MessageKeys.IpRequired);

        RuleFor(x => x.Name)
         .NotNull()
         .WithMessage(MessageKeys.NameRequired)
         .MaximumLength(50)
         .WithMessage(MessageKeys.MaxLength50Characters)
         .MinimumLength(10)
         .WithMessage(MessageKeys.MinLength10Characters);
    }
}