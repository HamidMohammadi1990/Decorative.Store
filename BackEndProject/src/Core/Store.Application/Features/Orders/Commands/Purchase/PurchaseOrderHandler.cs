using Store.Common.Extensions;
using Edition.Application.Contracts;
using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.Infrastructure;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.Orders;
using Edition.Application.Features.Orders.Common;
using Store.Common.Models;
using Store.Common.Localization;
using Store.Domain.Dtos.Products;
using Store.Domain.Repositories;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Commands;

public class PurchaseOrderHandler
    : IRequestHandler<PurchaseOrderRequest, OperationResult<PurchaseOrderResponse>>
{
    private readonly IUnitOfWork uow;
    private readonly IImageService imageService;
    private readonly IOrderRepository orderRepository;
    private readonly ILocalFileService localFileService;
    private readonly IPropertyRepository propertyRepository;
    private readonly IOrderMapperService orderMapperService;
    private readonly ICurrentUserContext currentUser;
    private readonly IProductRepository productRepository;
    private readonly IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository;
    private readonly IDiscountRepository discountRepository;
    private readonly IOrderTrackingCodeGenerator trackingCodeGenerator;

    public PurchaseOrderHandler(
        IUnitOfWork uow,
        IImageService imageService,
        IOrderRepository orderRepository,
        ILocalFileService localFileService,
        IPropertyRepository propertyRepository,
        IOrderMapperService orderMapperService,
        ICurrentUserContext currentUser,
        IProductRepository productRepository,
        IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository,
        IDiscountRepository discountRepository,
        IOrderTrackingCodeGenerator trackingCodeGenerator)
    {
        this.uow = uow;
        this.imageService = imageService;
        this.orderRepository = orderRepository;
        this.localFileService = localFileService;
        this.propertyRepository = propertyRepository;
        this.orderMapperService = orderMapperService;
        this.currentUser = currentUser;
        this.productRepository = productRepository;
        this.productOrderItemAttachmentTypeRepository = productOrderItemAttachmentTypeRepository;
        this.discountRepository = discountRepository;
        this.trackingCodeGenerator = trackingCodeGenerator;
    }

    public async Task<OperationResult<PurchaseOrderResponse>> Handle(PurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var userIsCooperation = currentUser.IsCooperation;
        var productProperties = await propertyRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        var orderProperties = orderMapperService.ToOrderItemProperties(request.Properties, productProperties, userIsCooperation);

        var validateProperties = PurchaseOrderPropertyValidator.Validate(productProperties, orderProperties);
        if (validateProperties.Length != 0)
            return validateProperties;

        var productAttachmentRestrictions = await productOrderItemAttachmentTypeRepository.GetRestrictionByProductIdAsync(request.ProductId);
        var validationFiles = await ValidationAttachmentsAsync(request.Attachments, productAttachmentRestrictions);
        if (validationFiles.Length != 0)
            return validationFiles;

        var product = await productRepository.GetAsNoTrackingAsync(request.ProductId, cancellationToken);
        if (product is null)
            return ErrorModel.Create("InvalidId");

        var orderProductPrice = product.Price;
        var orderItem = OrderItem.Create(request.ProductId, request.Quantity, request.PostTypeId,
                                         request.DeliveryTypeId, request.UserAddressId,
                                         request.Description, request.EmergencyPhoneNumber,
                                         orderProductPrice, request.IsNeedToDesign);
        orderItem.AddProperties(orderProperties);

        if (request.Attachments?.Count > 0)
        {
            var orderAttachments = await SaveAttachmentsAsync(request.Attachments);
            if (!orderAttachments.IsSuccess)
                return orderAttachments.Messages;

            orderItem.AddAttachments(orderAttachments.Result!);
        }

        var userId = currentUser.UserId;
        var pendingUserOrder = await orderRepository.GetPendingOrderByUserIdAsync(userId);
        var order = pendingUserOrder ?? Order.Create(request.Title, userId);

        if (pendingUserOrder is not null &&
            await orderRepository.HasPendingBankPaymentAsync(order.Id, cancellationToken))
            return ErrorModel.Create("PaymentInProgress");

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
            return saveChangesResult.ToGenericFailure<PurchaseOrderResponse>();

        return new PurchaseOrderResponse
        {
            OrderId = order.Id,
            TrackingCode = order.TrackingCode,
            Cart = OrderCartSummaryMapper.Map(cartResult)
        };
    }

    private async Task<ErrorModel[]> ValidationAttachmentsAsync(List<OrderAttachmentRequest>? attachments, List<ProductAttachmentTypeRestrictionDto> productOrderItemAttachments)
    {
        var errors = new List<ErrorModel>();
        void AddError(string code, string messageKey) => errors.Add(ErrorModel.CreateLiteral(code, messageKey));

        if (attachments?.Count == 0 && productOrderItemAttachments.Count == 0)
            return [];

        if (productOrderItemAttachments.Count == 0 && attachments?.Count > 0)
            return [ErrorModel.Create("InvalidAttachments")];

        if (productOrderItemAttachments.Count > 0 && attachments?.Count == 0)
            return [ErrorModel.Create("RequiredAttachments")];

        if (attachments!.Count(x => x.File.Length > 0) < productOrderItemAttachments.Count(x => x.IsRequired))
            return [ErrorModel.Create("RequiredAttachments")];

        foreach (var attachment in attachments!)
        {
            var attachmentRestriction = productOrderItemAttachments.FirstOrDefault(x => x.ProductOrderItemAttachmentTypeId == attachment.Id);
            if (attachmentRestriction is null)
            {
                AddError($"File_{attachment.Id}", MessageKeys.InvalidUploadedAttachmentFile);
                continue;
            }

            if (attachment.File.Length == 0)
            {
                AddError($"File_{attachment.Id}", MessageKeys.InvalidUploadedAttachmentFile);
                continue;
            }

            if (attachment.File.Length > attachmentRestriction.MaxFileSizeInBytes)
            {
                AddError($"File_{attachment.Id}", MessageKeys.InvalidAttachmentFileSize);
            }

            var imageMetaData = await imageService.GetImageMetaDataAsync(attachment.File);
            if (!imageMetaData.IsSuccess)
            {
                AddError($"File_{attachment.Id}", MessageKeys.InvalidUploadedAttachmentFile);
                continue;
            }

            var metaDataResult = imageMetaData.Result!;
            if (metaDataResult.Width < attachmentRestriction.MinWidth)
            {
                AddError($"File_{attachment.Id}", MessageKeys.ImageWidthBelowMinimum);
            }
            if (metaDataResult.Width > attachmentRestriction.MaxWidth)
            {
                AddError($"File_{attachment.Id}", MessageKeys.ImageWidthAboveMaximum);
            }


            if (metaDataResult.Height < attachmentRestriction.MinHeight)
            {
                AddError($"File_{attachment.Id}", MessageKeys.ImageHeightBelowMinimum);
            }
            if (metaDataResult.Height > attachmentRestriction.MaxHeight)
            {
                AddError($"File_{attachment.Id}", MessageKeys.ImageHeightAboveMaximum);
            }


            if (metaDataResult.HorizontalResolution < attachmentRestriction.MinHorizontalResolution)
            {
                AddError($"File_{attachment.Id}", MessageKeys.ImageHorizontalResolutionBelowMinimum);
            }
            if (metaDataResult.HorizontalResolution > attachmentRestriction.MaxHorizontalResolution)
            {
                AddError($"File_{attachment.Id}", MessageKeys.ImageHorizontalResolutionAboveMaximum);
            }


            if (metaDataResult.VerticalResolution < attachmentRestriction.MinVerticalResolution)
            {
                AddError($"File_{attachment.Id}", MessageKeys.ImageVerticalResolutionBelowMinimum);
            }
            if (metaDataResult.VerticalResolution > attachmentRestriction.MaxVerticalResolution)
            {
                AddError($"File_{attachment.Id}", MessageKeys.ImageVerticalResolutionAboveMaximum);
            }

            if (attachmentRestriction.ColorMode is PictureColorModeType.All)
                continue;

            if (metaDataResult.ColorMode != attachmentRestriction.ColorMode)
            {
                AddError($"File_{attachment.Id}", MessageKeys.InvalidImageColorMode);
            }
        }

        return [.. errors];
    }

    private async Task<OperationResult<List<OrderItemAttachment>>> SaveAttachmentsAsync(List<OrderAttachmentRequest> attachments)
    {
        var orderAttachments = new List<OrderItemAttachment>();

        for (int i = 0; i < attachments.Count; i++)
        {
            var fileName = await localFileService.SaveFileAsync(attachments[i].File, OrderItemDirectory.OrderImage);
            if (!fileName.IsSuccess)
                continue;

            var orderAttachment = OrderItemAttachment.Create(fileName.Result!, attachments[i].Id);
            orderAttachments.Add(orderAttachment);
        }

        if (orderAttachments.Count < attachments.Count)
            return ErrorModel.Create("AttachmentsProcessingFailed");

        return orderAttachments;
    }
}