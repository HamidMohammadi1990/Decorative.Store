using Edition.Application.Common.Validation;
using FluentValidation;

namespace Edition.Application.Features.ProductFiles.Commands;

public class UpdateSetMainProductFileValidator : AbstractValidator<UpdateSetMainProductFileRequest>
{
    public UpdateSetMainProductFileValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
