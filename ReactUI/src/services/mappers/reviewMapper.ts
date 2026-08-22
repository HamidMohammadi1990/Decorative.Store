import type { DashboardReview } from '@/models/dashboard/dashboard.model'
import { readBooleanField, readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'

function normalizeMyComments(data: unknown): unknown[] {
  const record = readRecord(data) ?? {}
  const raw = record.items ?? record.Items
  return Array.isArray(raw) ? raw : []
}

export function mapMyProductCommentToDashboardReview(data: unknown): DashboardReview | null {
  const record = readRecord(data) ?? {}
  const id = readStringField(record, 'id', 'Id')
  const productTitle = readStringField(record, 'productTitle', 'ProductTitle')
  const description = readStringField(record, 'description', 'Description')
  const productSlug = readStringField(record, 'productSlug', 'ProductSlug')
  const rating = readNumberField(record, 'commentRate', 'CommentRate')
  const isActive = readBooleanField(record, 'isActive', 'IsActive')

  if (!id || !productTitle) return null

  return {
    id,
    productTitle,
    productSlug,
    rating: rating || 0,
    comment: description,
    date: '',
    status: isActive ? 'published' : 'pending',
  }
}

export function mapMyProductCommentsResponse(data: unknown): DashboardReview[] {
  return normalizeMyComments(data)
    .map(mapMyProductCommentToDashboardReview)
    .filter((review): review is DashboardReview => review !== null)
}
