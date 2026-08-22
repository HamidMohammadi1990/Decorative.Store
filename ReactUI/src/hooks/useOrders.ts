import { useCallback, useEffect, useState } from 'react'
import type { DashboardOrder, OrderStatusOption } from '@/models/dashboard/dashboard.model'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { orderService } from '@/services/orderService'
import { useUserStore } from '@/stores/userStore'

interface UseOrdersResult {
  statuses: OrderStatusOption[]
  selectedStatusId: number | null
  orders: DashboardOrder[]
  statusesLoading: boolean
  ordersLoading: boolean
  error: string | null
  detailLoadingId: string | null
  setSelectedStatusId: (statusId: number) => void
  reload: () => Promise<void>
  loadOrderDetail: (order: DashboardOrder) => Promise<DashboardOrder | null>
}

export function useOrders(): UseOrdersResult {
  const { locale } = useLocaleSettings()
  const accessToken = useUserStore((state) => state.accessToken)
  const [statuses, setStatuses] = useState<OrderStatusOption[]>([])
  const [selectedStatusId, setSelectedStatusId] = useState<number | null>(null)
  const [orders, setOrders] = useState<DashboardOrder[]>([])
  const [statusesLoading, setStatusesLoading] = useState(true)
  const [ordersLoading, setOrdersLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [detailLoadingId, setDetailLoadingId] = useState<string | null>(null)

  const loadStatuses = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setStatuses([])
      setSelectedStatusId(null)
      setStatusesLoading(false)
      return
    }

    setStatusesLoading(true)
    setError(null)

    try {
      const data = await orderService.getOrderStatuses(accessToken, locale)
      setStatuses(data)
      setSelectedStatusId((current) => {
        if (current && data.some((status) => status.id === current)) {
          return current
        }
        return data[0]?.id ?? null
      })
    } catch {
      setError('failed')
      setStatuses([])
      setSelectedStatusId(null)
    } finally {
      setStatusesLoading(false)
    }
  }, [accessToken, locale])

  const loadOrders = useCallback(
    async (statusId: number) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        setOrders([])
        return
      }

      setOrdersLoading(true)
      setError(null)

      try {
        const data = await orderService.getOrdersByStatus(accessToken, locale, statusId)
        setOrders(data)
      } catch {
        setError('failed')
        setOrders([])
      } finally {
        setOrdersLoading(false)
      }
    },
    [accessToken, locale],
  )

  const reload = useCallback(async () => {
    await loadStatuses()
    if (selectedStatusId) {
      await loadOrders(selectedStatusId)
    }
  }, [loadOrders, loadStatuses, selectedStatusId])

  const loadOrderDetail = useCallback(
    async (order: DashboardOrder) => {
      if (!accessToken || accessToken === 'mock-access-token') return null
      if (order.detailsLoaded) return order

      setDetailLoadingId(order.id)

      try {
        const detailed = await orderService.getOrderDetail(accessToken, locale, order)
        setOrders((current) =>
          current.map((item) => (item.id === detailed.id ? detailed : item)),
        )
        return detailed
      } catch {
        return null
      } finally {
        setDetailLoadingId(null)
      }
    },
    [accessToken, locale],
  )

  useEffect(() => {
    void loadStatuses()
  }, [loadStatuses])

  useEffect(() => {
    if (!selectedStatusId) {
      setOrders([])
      return
    }

    void loadOrders(selectedStatusId)
  }, [loadOrders, selectedStatusId])

  return {
    statuses,
    selectedStatusId,
    orders,
    statusesLoading,
    ordersLoading,
    error,
    detailLoadingId,
    setSelectedStatusId,
    reload,
    loadOrderDetail,
  }
}
