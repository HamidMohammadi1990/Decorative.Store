using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public class SearchCompanyStoryCommentValidator : AbstractValidator<SearchCompanyStoryCommentRequest>
{
    public SearchCompanyStoryCommentValidator()
    {
        RuleFor(x => x.CompanyStoryId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidCompanyStoryId);

        RuleFor(x => x.Pagination).NotNull();
    }
}
