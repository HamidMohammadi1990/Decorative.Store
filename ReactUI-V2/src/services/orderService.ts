import type { Locale } from '@/models/shared/locale.model'
import type { DashboardOrder, OrderStatusOption } from '@/models/dashboard/dashboard.model'
import { apiGet, apiPost } from '@/services/api/apiClient'
import { mapOrderListItem, applyOrderDetail } from '@/services/mappers/orderMapper'
import { mapOrderStatusOption } from '@/services/mappers/orderStatusMapper'

const ORDER_STATUSES_PATH = '/api/v1/order/statuses'
const ORDER_BY_STATUS_PATH = '/api/v1/order/by-status'
const ORDER_DETAIL_PATH = '/api/v1/order/detail'

const DEFAULT_PAGINATION = { pageNumber: 1, pageSize: 50 }

export const orderService = {
  async getOrderStatuses(accessToken: string, locale: Locale): Promise<OrderStatusOption[]> {
    const data = await apiGet<unknown[]>(ORDER_STATUSES_PATH, locale, accessToken)
    return data
      .map(mapOrderStatusOption)
      .filter((status): status is OrderStatusOption => status !== null)
  },

  async getOrdersByStatus(
    accessToken: string,
    locale: Locale,
    statusId: number,
  ): Promise<DashboardOrder[]> {
    const data = await apiPost<unknown[]>(
      ORDER_BY_STATUS_PATH,
      { status: statusId, pagination: DEFAULT_PAGINATION },
      { locale, accessToken },
    )

    return data
      .map(mapOrderListItem)
      .filter((order): order is DashboardOrder => order !== null)
      .sort((a, b) => b.date.localeCompare(a.date))
  },

  async getOrderDetail(
    accessToken: string,
    locale: Locale,
    order: DashboardOrder,
  ): Promise<DashboardOrder> {
    const detail = await apiPost<unknown>(
      ORDER_DETAIL_PATH,
      { orderId: order.id },
      { locale, accessToken },
    )

    return applyOrderDetail(order, detail)
  },
}
