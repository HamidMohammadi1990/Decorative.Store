using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductComments.Commands;

public class CreateProductCommentValidator : AbstractValidator<CreateProductCommentRequest>
{
    public CreateProductCommentValidator()
    {
        RuleFor(x => x.ProductId)
          .Equal(0)
          .WithMessage(MessageKeys.InvalidProductId);

        RuleFor(x => x.AffordableRating)
         .Equal(0)
         .WithMessage(MessageKeys.ValueRatingRequired);

        RuleFor(x => x.CommentRate)
          .Equal(0)
          .WithMessage(MessageKeys.ReviewRateRequired);

        RuleFor(x => x.QualityRating)
          .Equal(0)
          .WithMessage(MessageKeys.QualityRatingRequired);

        RuleFor(x => x.CommentTopicId)
          .Equal(0)
          .WithMessage(MessageKeys.TopicRequired);
    }
}