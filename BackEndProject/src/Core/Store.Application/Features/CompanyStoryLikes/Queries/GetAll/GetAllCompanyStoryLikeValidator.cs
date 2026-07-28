using FluentValidation;

namespace Edition.Application.Features.CompanyStoryLikes.Queries;

public class GetAllCompanyStoryLikeValidator : AbstractValidator<GetAllCompanyStoryLikeRequest>
{
    public GetAllCompanyStoryLikeValidator()
    {
        RuleFor(x => x.Pagination).NotNull();
    }
}
