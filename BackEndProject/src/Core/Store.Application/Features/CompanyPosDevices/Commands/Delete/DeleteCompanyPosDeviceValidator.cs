using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyPosDevices.Commands;

public class DeleteCompanyPosDeviceValidator : AbstractValidator<DeleteCompanyPosDeviceRequest>
{
    public DeleteCompanyPosDeviceValidator()
    {
        RuleFor(x => x.Id)
         .GreaterThan(0)
         .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
