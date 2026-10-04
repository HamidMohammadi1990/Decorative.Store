import type { DashboardOrder, OrderSortOption } from '@/models/dashboard/dashboard.model'

export interface OrderListFilters {
  query: string
  sort: OrderSortOption
}

export function filterDashboardOrders(
  orders: DashboardOrder[],
  { query, sort }: OrderListFilters,
): DashboardOrder[] {
  const normalizedQuery = query.trim().toLowerCase()

  let result = orders.filter((order) => {
    if (!normalizedQuery) return true
    return (
      order.id.toLowerCase().includes(normalizedQuery) ||
      (order.displayId?.toLowerCase().includes(normalizedQuery) ?? false) ||
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
