using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Users.Queries;

public class GetAllUserValidator : AbstractValidator<GetAllUserRequest>
{
    public GetAllUserValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CityId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserName).MaximumLengthWhenNotEmpty(EntityFieldLengths.User.UserName);
        RuleFor(x => x.FirstName).MaximumLengthWhenNotEmpty(EntityFieldLengths.User.FirstName);
        RuleFor(x => x.LastName).MaximumLengthWhenNotEmpty(EntityFieldLengths.User.LastName);
        RuleFor(x => x.Email).MaximumLengthWhenNotEmpty(EntityFieldLengths.User.Email);
        RuleFor(x => x.PhoneNumber).MaximumLengthWhenNotEmpty(EntityFieldLengths.User.Mobile);
    }
}
