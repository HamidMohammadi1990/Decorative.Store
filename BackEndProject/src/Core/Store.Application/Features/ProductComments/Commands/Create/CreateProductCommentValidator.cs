using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductComments.Commands;

public class CreateProductCommentValidator : AbstractValidator<CreateProductCommentRequest>
{
    public CreateProductCommentValidator()
    {
        RuleFor(x => x.ProductId).MustBeValidEntityId();

        RuleFor(x => x.CommentTopicId).MustBeValidEntityId();

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(x => x.CommentRate).InclusiveBetween(1, 5);
        RuleFor(x => x.QualityRating).InclusiveBetween(1, 5);
        RuleFor(x => x.AffordableRating).InclusiveBetween(1, 5);
    }
}
