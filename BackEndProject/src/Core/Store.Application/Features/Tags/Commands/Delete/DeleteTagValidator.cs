using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Tags.Commands;

public class DeleteTagValidator : AbstractValidator<DeleteTagRequest>
{
    public DeleteTagValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
