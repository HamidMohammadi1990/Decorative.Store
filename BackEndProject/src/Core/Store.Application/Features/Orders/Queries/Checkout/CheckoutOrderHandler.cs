using Store.Common.Extensions;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.UserAddresses;

namespace Edition.Application.Features.Orders.Queries;

public class CheckoutOrderHandler
    : IRequestHandler<CheckoutOrderRequest, OperationResult<CheckoutOrderResponse>>
{
    private readonly IOrderMapperService mapper;
    private readonly IProductRepository productRepository;
    private readonly IPropertyRepository propertyRepository;
    private readonly IPostTypeRepository postTypeRepository;
    private readonly ICurrentUserContext currentUser;
    private readonly IUserAddressRepository userAddressRepository;
    private readonly IDeliveryTypeRepository deliveryTypeRepository;
    private readonly IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository;

    public CheckoutOrderHandler(
        IOrderMapperService mapper,
        IProductRepository productRepository,
        IPropertyRepository propertyRepository,
        IPostTypeRepository postTypeRepository,
        ICurrentUserContext currentUser,
        IUserAddressRepository userAddressRepository,
        IDeliveryTypeRepository deliveryTypeRepository,
        IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository)
    {
        this.mapper = mapper;
        this.productRepository = productRepository;
        this.propertyRepository = propertyRepository;
        this.postTypeRepository = postTypeRepository;
        this.currentUser = currentUser;
        this.userAddressRepository = userAddressRepository;
        this.deliveryTypeRepository = deliveryTypeRepository;
        this.productOrderItemAttachmentTypeRepository = productOrderItemAttachmentTypeRepository;
    }

    public async Task<OperationResult<CheckoutOrderResponse>> Handle(CheckoutOrderRequest request, CancellationToken cancellationToken)
    {
        var productDetail = await productRepository.GetProductSummaryByIdAsync(request.ProductId);
        if (productDetail is null)
            return ErrorModel.Create("InvalidId");

        var properties = await propertyRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        var deliveryTypes = await deliveryTypeRepository.GetAllAsync();
        var postTypes = await postTypeRepository.GetAllAsync();

        var userAddresses = new List<UserAddressSummaryDto>();
        if (currentUser.IsAuthenticated)
            userAddresses = await userAddressRepository.GetSummariesAsync(currentUser.UserId);

        var attachments = await productOrderItemAttachmentTypeRepository.GetCheckoutProductAttachmentsAsync(request.ProductId);

        var userIsCooperation = currentUser.IsCooperation;
        return mapper.Map(properties, deliveryTypes, postTypes, userAddresses, productDetail, attachments, userIsCooperation);
    }
}