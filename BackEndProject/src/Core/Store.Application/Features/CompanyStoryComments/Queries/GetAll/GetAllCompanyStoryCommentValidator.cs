using FluentValidation;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public class GetAllCompanyStoryCommentValidator : AbstractValidator<GetAllCompanyStoryCommentRequest>
{
    public GetAllCompanyStoryCommentValidator()
    {
        RuleFor(x => x.Pagination).NotNull();
    }
}
