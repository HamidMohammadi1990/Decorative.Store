using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductComments.Commands;

public class UpdateProductCommentValidator : AbstractValidator<UpdateProductCommentRequest>
{
    public UpdateProductCommentValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidId);

        RuleFor(x => x.CommentTopicId)
            .NotEqual(0)
            .WithMessage(MessageKeys.TopicRequired);

        RuleFor(x => x.AffordableRating)
            .NotEqual(0)
            .WithMessage(MessageKeys.ValueRatingRequired);

        RuleFor(x => x.CommentRate)
            .NotEqual(0)
            .WithMessage(MessageKeys.ReviewRateRequired);

        RuleFor(x => x.QualityRating)
            .NotEqual(0)
            .WithMessage(MessageKeys.QualityRatingRequired);
    }
}
