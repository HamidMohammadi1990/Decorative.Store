using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.DeliveryTypes.Queries;

public class GetAllDeliveryTypeValidator : AbstractValidator<GetAllDeliveryTypeRequest>
{
    public GetAllDeliveryTypeValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.DeliveryType.Title);
    }
}
