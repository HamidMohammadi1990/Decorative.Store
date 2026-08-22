using Store.Common.Extensions;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Features.Orders.Common;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Edition.Application.Contracts.Orders;

namespace Edition.Application.Features.Orders.Commands;

public class AddCartItemHandler
    : IRequestHandler<AddCartItemRequest, OperationResult<GetCartResponse>>
{
    private readonly IUnitOfWork uow;
    private readonly IOrderRepository orderRepository;
    private readonly IProductRepository productRepository;
    private readonly IPropertyRepository propertyRepository;
    private readonly IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository;
    private readonly IDiscountRepository discountRepository;
    private readonly IOrderTrackingCodeGenerator trackingCodeGenerator;
    private readonly ICurrentUserContext currentUser;

    public AddCartItemHandler(
        IUnitOfWork uow,
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IPropertyRepository propertyRepository,
        IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository,
        IDiscountRepository discountRepository,
        IOrderTrackingCodeGenerator trackingCodeGenerator,
        ICurrentUserContext currentUser)
    {
        this.uow = uow;
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
        this.propertyRepository = propertyRepository;
        this.productOrderItemAttachmentTypeRepository = productOrderItemAttachmentTypeRepository;
        this.discountRepository = discountRepository;
        this.trackingCodeGenerator = trackingCodeGenerator;
        this.currentUser = currentUser;
    }

    public async Task<OperationResult<GetCartResponse>> Handle(
        AddCartItemRequest request,
        CancellationToken cancellationToken)
    {
        var productProperties = await propertyRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        var catalog = PurchaseOrderPropertyCatalog.Create(productProperties);
        if (catalog.MandatoryPropertyIds.Count > 0)
            return ErrorModel.Create("ProductRequiresConfiguration");

        var attachmentRestrictions =
            await productOrderItemAttachmentTypeRepository.GetRestrictionByProductIdAsync(request.ProductId);
        if (attachmentRestrictions.Any(x => x.IsRequired))
            return ErrorModel.Create("ProductRequiresConfiguration");

        var product = await productRepository.GetAsNoTrackingAsync(request.ProductId, cancellationToken);
        if (product is null)
            return ErrorModel.Create("InvalidId");

        var productSummary = await productRepository.GetProductSummaryByIdAsync(request.ProductId);
        var orderTitle = productSummary?.Title ?? "Cart";

        var userId = currentUser.UserId;
        var userIsCooperation = currentUser.IsCooperation;
        var pendingUserOrder = await orderRepository.GetPendingOrderByUserIdAsync(userId);
        var order = pendingUserOrder ?? Order.Create(orderTitle, userId);

        if (pendingUserOrder is not null &&
            await orderRepository.HasPendingBankPaymentAsync(order.Id, cancellationToken))
            return ErrorModel.Create("PaymentInProgress");

        var orderItem = OrderItem.CreateForQuickAdd(
            request.ProductId,
            request.Quantity,
            product.Price);

        order.AddOrderItem(orderItem);

        var cartResult = await OrderCartService.RefreshCartAsync(
            order, discountRepository, userIsCooperation, cancellationToken);

        if (pendingUserOrder is null)
        {
            var trackingCode = await trackingCodeGenerator.GenerateUniqueAsync(cancellationToken);
            order.AssignTrackingCode(trackingCode);
            orderRepository.Add(order);
        }

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<GetCartResponse>();

        return await CartResponseMapper.MapAsync(productRepository, order, cartResult, cancellationToken);
    }
}
