using FluentValidation;

namespace Edition.Application.Features.UserStories.Queries;

public class SearchActiveUserStoriesValidator : AbstractValidator<SearchActiveUserStoriesRequest>
{
    public SearchActiveUserStoriesValidator()
    {
        RuleFor(x => x.Limit).InclusiveBetween(1, 50);
    }
}
