using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;

public class SearchProductOrderItemAttachmentTypeValidator : AbstractValidator<SearchProductOrderItemAttachmentTypeRequest>
{
    public SearchProductOrderItemAttachmentTypeValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
        RuleFor(x => x.OrderItemAttachmentTypeId).MustBeValidOptionalEntityId();
    }
}
