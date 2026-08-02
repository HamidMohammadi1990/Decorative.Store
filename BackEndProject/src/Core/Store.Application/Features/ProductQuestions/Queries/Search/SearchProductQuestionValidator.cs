using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductQuestions.Queries;

public class SearchProductQuestionValidator : AbstractValidator<SearchProductQuestionRequest>
{
    public SearchProductQuestionValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
    }
}
