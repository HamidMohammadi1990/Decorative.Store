using Edition.Application.Common.Validation;
using FluentValidation;

namespace Edition.Application.Features.ProfileCompletion;

public class GetAllProfileCompletionUserStateValidator
    : AbstractValidator<Queries.GetAllProfileCompletionUserStateRequest>
{
    public GetAllProfileCompletionUserStateValidator()
    {
        RuleFor(x => x.UserSearch).MaximumLengthWhenNotEmpty(EntityFieldLengths.User.UserName);
        RuleFor(x => x.Pagination).NotNull();
    }
}

public class GetProfileCompletionUserStateValidator
    : AbstractValidator<Queries.GetProfileCompletionUserStateRequest>
{
    public GetProfileCompletionUserStateValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}

public class DeleteProfileCompletionUserStateValidator
    : AbstractValidator<Commands.DeleteProfileCompletionUserStateRequest>
{
    public DeleteProfileCompletionUserStateValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}

public class UpdateProfileCompletionUserStateValidator
    : AbstractValidator<Commands.UpdateProfileCompletionUserStateRequest>
{
    public UpdateProfileCompletionUserStateValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
