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
  totalPages: number
}

export function computeTotalPages(totalCount: number, pageSize: number): number {
  if (pageSize <= 0 || totalCount <= 0) return 1
  return Math.max(1, Math.ceil(totalCount / pageSize))
}

export function normalizeAdminPaged<T>(
  data: unknown,
  mapItem: (item: unknown) => T | null,
): AdminPagedResult<T> {
  const record = readRecord(data)
  if (!record) {
    return { items: [], totalCount: 0, pageNumber: 1, pageSize: 20, totalPages: 1 }
  }

  const rawItems = record.items ?? record.Items
  const items = Array.isArray(rawItems)
    ? rawItems.map(mapItem).filter((item): item is T => item !== null)
    : []

  const totalCount = readNumberField(record, 'totalCount', 'TotalCount')
  const pageNumber = readNumberField(record, 'pageNumber', 'PageNumber') || 1
  const pageSize = readNumberField(record, 'pageSize', 'PageSize') || 20
  const totalPages =
    readNumberField(record, 'totalPages', 'TotalPages') || computeTotalPages(totalCount, pageSize)

  return {
    items,
    totalCount,
    pageNumber,
    pageSize,
    totalPages,
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

export function readOptionalEncryptedId(
  record: Record<string, unknown>,
  ...keys: string[]
): string | null {
  const value = readStringField(record, ...keys)
  return value || null
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

export function translationLanguageIds(
  translations: { languageId: number; title?: string; slug?: string; description?: string }[],
): number[] {
  return translations
    .filter((item) => {
      const title = item.title?.trim()
      const slug = item.slug?.trim()
      const description = item.description?.trim()
      return Boolean(title || slug || description)
    })
    .map((item) => item.languageId)
}

export function languageNameById(
  languages: { id: number; name: string; code: string }[],
  languageId: number,
): string {
  const match = languages.find((item) => item.id === languageId)
  return match?.name ?? String(languageId)
}
