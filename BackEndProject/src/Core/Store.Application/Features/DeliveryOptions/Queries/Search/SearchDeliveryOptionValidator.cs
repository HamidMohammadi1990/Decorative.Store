using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.DeliveryOptions.Queries;

public class SearchDeliveryOptionValidator : AbstractValidator<SearchDeliveryOptionRequest>
{
    public SearchDeliveryOptionValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.DeliveryOption.Title);
    }
}
