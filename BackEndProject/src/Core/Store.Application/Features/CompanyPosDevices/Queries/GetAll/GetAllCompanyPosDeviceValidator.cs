using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.CompanyPosDevices.Queries;

public class GetAllCompanyPosDeviceValidator : AbstractValidator<GetAllCompanyPosDeviceRequest>
{
    public GetAllCompanyPosDeviceValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CompanyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.BankId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Name).MaximumLengthWhenNotEmpty(EntityFieldLengths.CompanyPosDevice.Title);
    }
}
