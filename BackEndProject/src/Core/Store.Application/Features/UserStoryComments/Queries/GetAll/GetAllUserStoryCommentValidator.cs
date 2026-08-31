using FluentValidation;

namespace Edition.Application.Features.UserStoryComments.Queries;

public class GetAllUserStoryCommentValidator : AbstractValidator<GetAllUserStoryCommentRequest>
{
    public GetAllUserStoryCommentValidator()
    {
        RuleFor(x => x.Pagination).NotNull();
    }
}
