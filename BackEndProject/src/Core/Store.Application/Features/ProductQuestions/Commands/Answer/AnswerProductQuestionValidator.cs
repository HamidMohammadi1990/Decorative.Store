using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductQuestions.Commands;

public class AnswerProductQuestionValidator : AbstractValidator<AnswerProductQuestionRequest>
{
    public AnswerProductQuestionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
        RuleFor(x => x.Answer).NotEmpty().MaximumLength(2500);
    }
}
