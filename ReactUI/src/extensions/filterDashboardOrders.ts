import type {
  DashboardOrder,
  OrderSortOption,
  OrderStatus,
  OrderStatusFilter,
} from '@/models/dashboard/dashboard.model'

export interface OrderListFilters {
  status: OrderStatusFilter
  query: string
  sort: OrderSortOption
}

export const ORDER_STATUS_FILTERS: OrderStatusFilter[] = [
  'all',
  'pending',
  'processing',
  'shipped',
  'delivered',
  'cancelled',
]

export function filterDashboardOrders(
  orders: DashboardOrder[],
  { status, query, sort }: OrderListFilters,
): DashboardOrder[] {
  const normalizedQuery = query.trim().toLowerCase()

  let result = orders.filter((order) => {
    if (status !== 'all' && order.status !== status) return false
    if (!normalizedQuery) return true
    return (
      order.id.toLowerCase().includes(normalizedQuery) ||
      order.items.some((item) => item.title.toLowerCase().includes(normalizedQuery))
    )
  })

  result = [...result].sort((a, b) => {
    switch (sort) {
      case 'oldest':
        return a.date.localeCompare(b.date)
      case 'amountHigh':
        return b.total - a.total
      case 'amountLow':
        return a.total - b.total
      case 'newest':
      default:
        return b.date.localeCompare(a.date)
    }
  })

  return result
}

export function countOrdersByStatus(orders: DashboardOrder[]): Record<OrderStatusFilter, number> {
  const counts: Record<OrderStatus, number> = {
    pending: 0,
    processing: 0,
    shipped: 0,
    delivered: 0,
    cancelled: 0,
  }

  for (const order of orders) {
    counts[order.status] += 1
  }

  return {
    all: orders.length,
    ...counts,
  }
}
