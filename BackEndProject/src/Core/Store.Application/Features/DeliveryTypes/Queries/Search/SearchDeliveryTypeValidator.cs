using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.DeliveryTypes.Queries;

public class SearchDeliveryTypeValidator : AbstractValidator<SearchDeliveryTypeRequest>
{
    public SearchDeliveryTypeValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.DeliveryType.Title);
    }
}
