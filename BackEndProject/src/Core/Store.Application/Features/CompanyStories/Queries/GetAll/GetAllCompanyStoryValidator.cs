using FluentValidation;

namespace Edition.Application.Features.CompanyStories.Queries;

public class GetAllCompanyStoryValidator : AbstractValidator<GetAllCompanyStoryRequest>
{
    public GetAllCompanyStoryValidator()
    {
        RuleFor(x => x.Pagination).NotNull();
    }
}
