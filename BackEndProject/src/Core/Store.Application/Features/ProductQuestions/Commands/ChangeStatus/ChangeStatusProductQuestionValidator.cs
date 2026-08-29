using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductQuestions.Commands;

public class ChangeStatusProductQuestionValidator : AbstractValidator<ChangeStatusProductQuestionRequest>
{
    public ChangeStatusProductQuestionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
