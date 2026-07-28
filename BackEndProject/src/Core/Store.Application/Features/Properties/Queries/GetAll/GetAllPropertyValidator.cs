using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Properties.Queries;

public class GetAllPropertyValidator : AbstractValidator<GetAllPropertyRequest>
{
    public GetAllPropertyValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ParentId).MustBeValidOptionalEntityId();
        RuleFor(x => x.PropertyCategoryId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Property.Title);
    }
}
