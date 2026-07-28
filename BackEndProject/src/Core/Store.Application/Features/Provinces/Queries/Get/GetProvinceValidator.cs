using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Provinces.Queries;

public class GetProvinceValidator : AbstractValidator<GetProvinceRequest>
{
    public GetProvinceValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
