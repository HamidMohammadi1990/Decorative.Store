using FluentValidation;
using Edition.Application.Common.Validation;
using Store.Common.Localization;

namespace Edition.Application.Features.Orders.Queries;

public class GetOrderByStatusValidator : AbstractValidator<GetOrderByStatusRequest>
{
    public GetOrderByStatusValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(MessageKeys.StatusTypeRequired);

        RuleFor(x => x.Pagination)
            .MustBeValidPagination();
    }
}
