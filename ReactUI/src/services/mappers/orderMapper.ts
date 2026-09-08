import { API_BASE_URL } from '@/config/api'
import type {
  DashboardOrder,
  DashboardOrderItem,
  DashboardOrderTimelineStep,
  OrderPaymentMethod,
  OrderStatus,
} from '@/models/dashboard/dashboard.model'
import { readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'

function resolveProductImage(fileName?: string): string | undefined {
  if (!fileName?.trim()) return undefined
  if (fileName.startsWith('http://') || fileName.startsWith('https://') || fileName.startsWith('/')) {
    if (fileName.startsWith('/')) {
      return `${API_BASE_URL}${fileName}`
    }
    return fileName
  }
  return `${API_BASE_URL}/Uploads/Products/${fileName}`
}

function readBackendOrderStatus(record: Record<string, unknown>): number {
  const raw = record.status ?? record.Status
  if (typeof raw === 'number' && Number.isFinite(raw)) return raw
  if (typeof raw === 'string') {
    const normalized = raw.toLowerCase()
    if (normalized === 'pending') return 1
    if (normalized === 'rejected') return 2
    if (normalized === 'inprogress') return 3
    if (normalized === 'completed') return 4
    const parsed = Number(raw)
    if (Number.isFinite(parsed)) return parsed
  }
  return 1
}

export function mapBackendStatusToUi(
  backendStatus: number,
  trackingCode?: number,
): OrderStatus {
  switch (backendStatus) {
    case 4:
      return 'delivered'
    case 2:
      return 'cancelled'
    case 3:
      return trackingCode && trackingCode > 0 ? 'shipped' : 'processing'
    case 1:
    default:
      return 'pending'
  }
}

function buildTimeline(status: OrderStatus, createdOnUtc: string): DashboardOrderTimelineStep[] {
  const date = createdOnUtc.slice(0, 10)

  if (status === 'cancelled') {
    return [
      { key: 'placed', date, done: true },
      { key: 'cancelled', date, done: true },
    ]
  }

  const progress: Record<OrderStatus, number> = {
    pending: 2,
    processing: 3,
    shipped: 4,
    delivered: 5,
    cancelled: 1,
  }

  const level = progress[status] ?? 1
  const steps: DashboardOrderTimelineStep['key'][] = [
    'placed',
    'confirmed',
    'processing',
    'shipped',
    'delivered',
  ]

  return steps.map((key, index) => ({
    key,
    date: index < level ? date : undefined,
    done: index < level,
  }))
}

function mapListItem(data: unknown): DashboardOrderItem {
  const record = readRecord(data) ?? {}

  return {
    title: readStringField(record, 'productTitle', 'ProductTitle') || '—',
    imageUrl: resolveProductImage(readStringField(record, 'productImage', 'ProductImage')),
    quantity: readNumberField(record, 'quantity', 'Quantity') || 1,
    price: readNumberField(record, 'productPrice', 'ProductPrice'),
  }
}

export function mapOrderListItem(data: unknown): DashboardOrder | null {
  const record = readRecord(data) ?? {}
  const id = readStringField(record, 'id', 'Id')
  if (!id) return null

  const trackingCode = readNumberField(record, 'trackingCode', 'TrackingCode')
  const createdOnUtc = readStringField(record, 'createdOnUtc', 'CreatedOnUtc')
  const backendStatus = readBackendOrderStatus(record)
  const status = mapBackendStatusToUi(backendStatus, trackingCode)
  const itemsRaw = record.items ?? record.Items
  const items = Array.isArray(itemsRaw) ? itemsRaw.map(mapListItem) : []

  const subtotal = items.reduce((sum, item) => sum + item.price * item.quantity, 0)
  const total = readNumberField(record, 'finalPrice', 'FinalPrice') || subtotal

  return {
    id,
    date: createdOnUtc.slice(0, 10) || new Date().toISOString().slice(0, 10),
    status,
    total,
    subtotal: subtotal || total,
    shippingCost: Math.max(0, total - (subtotal || total)),
    paymentMethod: 'card' satisfies OrderPaymentMethod,
    items,
    shippingAddress: {
      label: '',
      recipient: '',
      lines: [],
    },
    timeline: buildTimeline(status, createdOnUtc || new Date().toISOString()),
    trackingNumber: trackingCode > 0 ? String(trackingCode) : undefined,
    displayId: trackingCode > 0 ? String(trackingCode) : id,
  }
}

function formatPropertyValue(data: Record<string, unknown>): string {
  const propertyType = readStringField(data, 'propertyType', 'PropertyType').toLowerCase()

  if (propertyType.includes('boolean')) {
    const selected = data.isSelected ?? data.IsSelected
    return selected ? '✓' : '—'
  }

  if (propertyType.includes('numeric')) {
    const quantity = readNumberField(data, 'quantity', 'Quantity')
    const itemTitle = readStringField(data, 'itemTitle', 'ItemTitle')
    return itemTitle ? `${itemTitle} (${quantity})` : String(quantity || '—')
  }

  if (propertyType.includes('select')) {
    return readStringField(data, 'itemTitle', 'ItemTitle') || '—'
  }

  if (propertyType.includes('dimensions')) {
    const width = readNumberField(data, 'width', 'Width')
    const height = readNumberField(data, 'height', 'Height')
    return `${width} × ${height}`
  }

  if (propertyType.includes('text')) {
    return readStringField(data, 'value', 'Value') || '—'
  }

  return readStringField(data, 'title', 'Title') || '—'
}

export function applyOrderDetail(base: DashboardOrder, detailData: unknown): DashboardOrder {
  const record = readRecord(detailData) ?? {}
  const backendStatus = readBackendOrderStatus(record)
  const trackingCode = readNumberField(record, 'trackingCode', 'TrackingCode')
  const status = mapBackendStatusToUi(backendStatus, trackingCode)
  const createdOnUtc = readStringField(record, 'createdOnUtc', 'CreatedOnUtc')
  const totalPrice = readNumberField(record, 'totalPrice', 'TotalPrice')
  const finalPrice = readNumberField(record, 'finalPrice', 'FinalPrice') || base.total

  const itemsRaw = record.items ?? record.Items
  const items: DashboardOrderItem[] = Array.isArray(itemsRaw)
    ? itemsRaw.map((item) => {
        const itemRecord = readRecord(item) ?? {}
        const product = readRecord(itemRecord.product ?? itemRecord.Product) ?? {}
        const title =
          readStringField(product, 'title', 'Title') ||
          readStringField(itemRecord, 'description', 'Description') ||
          '—'

        return {
          title,
          imageUrl: base.items.find((existing) => existing.title === title)?.imageUrl,
          quantity: readNumberField(itemRecord, 'quantity', 'Quantity') || 1,
          price: readNumberField(itemRecord, 'productPrice', 'ProductPrice'),
          description: readStringField(itemRecord, 'description', 'Description') || undefined,
          postType: readStringField(itemRecord, 'postType', 'PostType') || undefined,
          deliveryType: readStringField(itemRecord, 'deliveryType', 'DeliveryType') || undefined,
          properties: (() => {
            const raw = itemRecord.properties ?? itemRecord.Properties
            return Array.isArray(raw)
              ? raw.map((property: unknown) => {
                const propertyRecord = readRecord(property) ?? {}
                return {
                  title: readStringField(propertyRecord, 'title', 'Title'),
                  value: formatPropertyValue(propertyRecord),
                }
              })
              : undefined
          })(),
          attachments: (() => {
            const raw = itemRecord.attachments ?? itemRecord.Attachments
            return Array.isArray(raw)
              ? raw.map((attachment: unknown) => {
                const attachmentRecord = readRecord(attachment) ?? {}
                return {
                  title: readStringField(attachmentRecord, 'typeTitle', 'TypeTitle'),
                  fileName: readStringField(attachmentRecord, 'fileName', 'FileName'),
                }
              })
              : undefined
          })(),
        }
      })
    : base.items

  const firstItem = Array.isArray(itemsRaw) ? readRecord(itemsRaw[0]) : null
  const address = firstItem ? readRecord(firstItem.userAddress ?? firstItem.UserAddress) : null

  const recipient = address
    ? [readStringField(address, 'recipientFirstName', 'RecipientFirstName'), readStringField(address, 'recipientLastName', 'RecipientLastName')]
        .filter(Boolean)
        .join(' ')
    : base.shippingAddress.recipient

  const addressLines = address
    ? [
        readStringField(address, 'address', 'Address'),
        readStringField(address, 'cityTitle', 'CityTitle'),
        readStringField(address, 'postalCode', 'PostalCode'),
        readStringField(address, 'phoneNumber', 'PhoneNumber'),
      ].filter(Boolean)
    : base.shippingAddress.lines

  const subtotal = items.reduce((sum, item) => sum + item.price * item.quantity, 0) || totalPrice || finalPrice

  return {
    ...base,
    status,
    total: finalPrice,
    subtotal,
    shippingCost: Math.max(0, finalPrice - subtotal),
    items: items.length > 0 ? items : base.items,
    shippingAddress: address
      ? {
          label: readStringField(address, 'title', 'Title'),
          recipient,
          lines: addressLines,
        }
      : base.shippingAddress,
    timeline: buildTimeline(status, createdOnUtc || base.date),
    trackingNumber: trackingCode > 0 ? String(trackingCode) : base.trackingNumber,
    displayId: trackingCode > 0 ? String(trackingCode) : base.displayId ?? base.id,
    detailsLoaded: true,
  }
}
