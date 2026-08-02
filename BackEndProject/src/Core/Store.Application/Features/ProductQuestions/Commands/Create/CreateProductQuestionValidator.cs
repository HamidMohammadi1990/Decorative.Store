using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductQuestions.Commands;

public class CreateProductQuestionValidator : AbstractValidator<CreateProductQuestionRequest>
{
    public CreateProductQuestionValidator()
    {
        RuleFor(x => x.ProductId).MustBeValidEntityId();
        RuleFor(x => x.Question)
            .NotEmpty()
            .MaximumLength(500);
    }
}
