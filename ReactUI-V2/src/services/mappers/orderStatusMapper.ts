import type { OrderStatusOption } from '@/models/dashboard/dashboard.model'
import { readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'

export function mapOrderStatusOption(data: unknown): OrderStatusOption | null {
  const record = readRecord(data) ?? {}
  const id = readNumberField(record, 'id', 'Id')
  const title = readStringField(record, 'title', 'Title')

  if (!id || !title) return null

  return { id, title }
}
