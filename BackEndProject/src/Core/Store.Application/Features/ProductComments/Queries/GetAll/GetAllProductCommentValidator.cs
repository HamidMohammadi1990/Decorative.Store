using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductComments.Queries;

public class GetAllProductCommentValidator : AbstractValidator<GetAllProductCommentRequest>
{
    public GetAllProductCommentValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.CommentTopicId).MustBeValidOptionalEntityId();
    }
}
