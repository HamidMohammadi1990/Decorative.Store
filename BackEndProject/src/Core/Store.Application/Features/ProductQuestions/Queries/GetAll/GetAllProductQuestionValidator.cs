using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductQuestions.Queries;

public class GetAllProductQuestionValidator : AbstractValidator<GetAllProductQuestionRequest>
{
    public GetAllProductQuestionValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
    }
}
