using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Provinces.Commands;

public class DeleteProvinceValidator : AbstractValidator<DeleteProvinceRequest>
{
    public DeleteProvinceValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
