using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Orders.Queries;

public class GetAllOrderValidator : AbstractValidator<GetAllOrderRequest>
{
    public GetAllOrderValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Order.Title);
    }
}
