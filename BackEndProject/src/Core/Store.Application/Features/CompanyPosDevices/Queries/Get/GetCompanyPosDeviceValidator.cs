using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyPosDevices.Queries;

public class GetCompanyPosDeviceValidator : AbstractValidator<GetCompanyPosDeviceRequest>
{
    public GetCompanyPosDeviceValidator()
    {
        RuleFor(x => x.Id)
        .GreaterThan(0)
        .WithMessage(MessageKeys.InvalidIdValidator);
    }
}