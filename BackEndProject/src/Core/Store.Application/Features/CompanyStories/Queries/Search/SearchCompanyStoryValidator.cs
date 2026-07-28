using FluentValidation;

namespace Edition.Application.Features.CompanyStories.Queries;

public class SearchCompanyStoryValidator : AbstractValidator<SearchCompanyStoryRequest>
{
    public SearchCompanyStoryValidator()
    {
        RuleFor(x => x.Pagination).NotNull();
    }
}
