using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductComments.Queries;

public class SearchProductCommentValidator : AbstractValidator<SearchProductCommentRequest>
{
    public SearchProductCommentValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.CompanyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.CommentTopicId).MustBeValidOptionalEntityId();
    }
}
