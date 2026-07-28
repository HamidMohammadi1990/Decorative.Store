using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PropertyItems.Queries;

public class GetAllPropertyItemValidator : AbstractValidator<GetAllPropertyItemRequest>
{
    public GetAllPropertyItemValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.PropertyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.PropertyCategoryId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.PropertyItem.Title);
        RuleFor(x => x.PropertyTitle).MaximumLengthWhenNotEmpty(EntityFieldLengths.Property.Title);
    }
}
