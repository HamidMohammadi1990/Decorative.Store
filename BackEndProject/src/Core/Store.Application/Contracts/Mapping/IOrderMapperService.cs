using Edition.Application.Features.Orders.Queries;
using Edition.Application.Features.Orders.Commands;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.UserAddresses;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Orders;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IOrderMapperService : IMapper
{
	GetAllOrderRequestDto Map(GetAllOrderRequest model);

    PagedResult<GetAllOrderResponse> Map(PagedResult<GetAllOrderDto> model);

    CheckoutOrderResponse Map(List<ProductPropertyDto> properties,
         List<PostType> postTypes, List<UserAddressSummaryDto> userAddresses,
         ProductSummaryDto productDetail, List<ProductOrderAttachmentDto> orderAttachments, bool userIsCooperation);

	GetOrderResponse Map(Order model);


    List<OrderItemProperty> ToOrderItemProperties(List<BaseOrderProperty> properties, List<ProductPropertyDto> productProperties, bool userIsCooperation);
	GetOrderDetailResponse? MapToOrderDetail(OrderDetailDto? order);
	List<GetOrderByStatusResponse> MapToUserOrdersByStatus(List<GetUserOrdersByStatusDto> orders);
	List<GetStatusSummaryOrderResponse> MapToStatusSummary(List<GetStatusSummaryPropertiesDto> orders);
}