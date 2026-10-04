import type { AdminOrderListItem, AdminOrderStatus } from '@/models/admin/order.model'
import type { DashboardOrder } from '@/models/dashboard/dashboard.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { readBooleanField, readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'
import { applyOrderDetail, mapBackendStatusToUi } from '@/services/mappers/orderMapper'

const BASE = '/api/v1/admin/order'

function readAdminOrderStatus(record: Record<string, unknown>): AdminOrderStatus {
  const raw = record.status ?? record.Status
  if (typeof raw === 'number' && raw >= 1 && raw <= 4) return raw as AdminOrderStatus
  if (typeof raw === 'string') {
    const parsed = Number(raw)
    if (parsed >= 1 && parsed <= 4) return parsed as AdminOrderStatus
  }
  return 1
}

export function normalizeAdminOrderListItem(data: unknown): AdminOrderListItem | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  if (!id) return null

  return {
    id,
    trackingCode: readNumberField(record, 'trackingCode', 'TrackingCode'),
    title: readStringField(record, 'title', 'Title'),
    userId: readEncryptedId(record, 'userId', 'UserId'),
    userFirstName: readStringField(record, 'userFirstName', 'UserFirstName'),
    userLastName: readStringField(record, 'userLastName', 'UserLastName'),
    status: readAdminOrderStatus(record),
    isFinaly: readBooleanField(record, 'isFinaly', 'IsFinaly'),
    createdOnUtc: readStringField(record, 'createdOnUtc', 'CreatedOnUtc'),
    totalPrice: readNumberField(record, 'totalPrice', 'TotalPrice'),
    finalPrice: readNumberField(record, 'finalPrice', 'FinalPrice'),
    vatPrice: readNumberField(record, 'vatPrice', 'VatPrice'),
  }
}

export function mapAdminOrderToDashboardOrder(item: AdminOrderListItem): DashboardOrder {
  const status = mapBackendStatusToUi(item.status, item.trackingCode)
  const date = item.createdOnUtc.slice(0, 10) || new Date().toISOString().slice(0, 10)
  const customerName = [item.userFirstName, item.userLastName].filter(Boolean).join(' ')

  return {
    id: item.id,
    date,
    status,
    total: item.finalPrice || item.totalPrice,
    subtotal: item.totalPrice,
    shippingCost: Math.max(0, item.finalPrice - item.totalPrice),
    paymentMethod: 'card',
    items: [],
    shippingAddress: {
      label: customerName,
      recipient: customerName,
      lines: [],
    },
    timeline: [],
    trackingNumber: item.trackingCode > 0 ? String(item.trackingCode) : undefined,
    displayId: item.trackingCode > 0 ? String(item.trackingCode) : item.id,
  }
}

export const adminOrderService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      status?: AdminOrderStatus | null
      title?: string | null
      trackingCode?: number | null
      isFinaly?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminOrderListItem>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: options.title?.trim() || null,
        trackingCode: options.trackingCode ?? null,
        userId: null,
        status: options.status ?? null,
        isFinaly: options.isFinaly ?? null,
        pagination: paginationBody(options.pageNumber, options.pageSize),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeAdminOrderListItem)
  },

  async getDetail(
    accessToken: string,
    locale: Locale,
    order: AdminOrderListItem,
  ): Promise<DashboardOrder> {
    const detail = await apiPost<unknown>(
      `${BASE}/detail`,
      { orderId: order.id },
      { locale, accessToken },
    )

    return applyOrderDetail(mapAdminOrderToDashboardOrder(order), detail)
  },
}
