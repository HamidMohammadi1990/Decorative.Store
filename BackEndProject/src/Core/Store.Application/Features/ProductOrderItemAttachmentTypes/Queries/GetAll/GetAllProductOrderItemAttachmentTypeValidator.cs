using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;

public class GetAllProductOrderItemAttachmentTypeValidator : AbstractValidator<GetAllProductOrderItemAttachmentTypeRequest>
{
    public GetAllProductOrderItemAttachmentTypeValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
        RuleFor(x => x.OrderItemAttachmentTypeId).MustBeValidOptionalEntityId();
    }
}
