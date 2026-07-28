using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.DeliveryOptions.Queries;

public class GetAllDeliveryOptionValidator : AbstractValidator<GetAllDeliveryOptionRequest>
{
    public GetAllDeliveryOptionValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.DeliveryOption.Title);
    }
}
