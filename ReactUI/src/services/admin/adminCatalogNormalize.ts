import type { CatalogTranslation } from '@/models/admin/catalog.model'
import {
  readBooleanField,
  readNumberField,
  readRecord,
  readStringField,
} from '@/services/api/apiNormalize'

export interface AdminPagedResult<T> {
  items: T[]
  totalCount: number
  pageNumber: number
  pageSize: number
}

export function normalizeAdminPaged<T>(
  data: unknown,
  mapItem: (item: unknown) => T | null,
): AdminPagedResult<T> {
  const record = readRecord(data)
  if (!record) {
    return { items: [], totalCount: 0, pageNumber: 1, pageSize: 20 }
  }

  const rawItems = record.items ?? record.Items
  const items = Array.isArray(rawItems)
    ? rawItems.map(mapItem).filter((item): item is T => item !== null)
    : []

  return {
    items,
    totalCount: readNumberField(record, 'totalCount', 'TotalCount'),
    pageNumber: readNumberField(record, 'pageNumber', 'PageNumber') || 1,
    pageSize: readNumberField(record, 'pageSize', 'PageSize') || 20,
  }
}

export function normalizeTranslations(raw: unknown): CatalogTranslation[] {
  if (!Array.isArray(raw)) return []

  return raw
    .map((item) => {
      const record = readRecord(item)
      if (!record) return null

      const title = readStringField(record, 'title', 'Title')
      if (!title) return null

      return {
        languageId: readNumberField(record, 'languageId', 'LanguageId'),
        title,
        slug: readStringField(record, 'slug', 'Slug'),
        description: readStringField(record, 'description', 'Description') || undefined,
      }
    })
    .filter((item): item is CatalogTranslation => item !== null)
}

export function pickTranslation(
  translations: CatalogTranslation[],
  languageId?: number,
): CatalogTranslation | null {
  if (translations.length === 0) return null
  if (languageId != null) {
    const match = translations.find((item) => item.languageId === languageId)
    if (match) return match
  }
  return translations[0] ?? null
}

export function readEncryptedId(record: Record<string, unknown>, ...keys: string[]): string {
  return readStringField(record, ...keys)
}

export function readIsActive(record: Record<string, unknown>): boolean {
  return readBooleanField(record, 'isActive', 'IsActive')
}

export function paginationBody(pageNumber = 1, pageSize = 50) {
  return { pageNumber, pageSize }
}

export function slugifyTitle(value: string): string {
  return value
    .trim()
    .toLowerCase()
    .replace(/\s+/g, '-')
    .replace(/[^\w\u0600-\u06FF-]+/g, '')
    .replace(/-+/g, '-')
    .replace(/^-|-$/g, '')
}
