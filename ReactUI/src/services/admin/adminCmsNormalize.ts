import type { CmsContentTranslation } from '@/models/admin/cms.model'
import { readRecord, readStringField, readNumberField } from '@/services/api/apiNormalize'

export function normalizeCmsTranslations(raw: unknown): CmsContentTranslation[] {
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
        slug: readStringField(record, 'slug', 'Slug') || undefined,
        description: readStringField(record, 'description', 'Description') || undefined,
        url: readStringField(record, 'url', 'Url') || undefined,
        metaTitle: readStringField(record, 'metaTitle', 'MetaTitle') || undefined,
        metaDescription: readStringField(record, 'metaDescription', 'MetaDescription') || undefined,
      }
    })
    .filter((item): item is CmsContentTranslation => item !== null)
}

export function normalizeSectionTypeTranslations(raw: unknown) {
  if (!Array.isArray(raw)) return []

  return raw
    .map((item) => {
      const record = readRecord(item)
      if (!record) return null

      const name = readStringField(record, 'name', 'Name')
      if (!name) return null

      return {
        languageId: readNumberField(record, 'languageId', 'LanguageId'),
        name,
      }
    })
    .filter((item): item is { languageId: number; name: string } => item !== null)
}

export function pickCmsTranslation(
  translations: CmsContentTranslation[],
  languageId?: number,
): CmsContentTranslation | null {
  if (translations.length === 0) return null
  if (languageId != null) {
    const match = translations.find((item) => item.languageId === languageId)
    if (match) return match
  }
  return translations[0] ?? null
}

export function pickSectionTypeName(
  translations: { languageId: number; name: string }[],
  languageId?: number,
): string {
  if (translations.length === 0) return ''
  if (languageId != null) {
    const match = translations.find((item) => item.languageId === languageId)
    if (match?.name) return match.name
  }
  return translations.find((item) => item.name)?.name ?? translations[0]?.name ?? ''
}

const PAGE_TYPE_BY_NAME: Record<string, number> = {
  General: 1,
  Home: 2,
  CategoryPage: 3,
  ProductPage: 4,
  CheckoutPage: 5,
}

/** Maps API PageType enum names or numeric values to dashboard page-type ids. */
export function readPageType(value: unknown, fallback = 1): number {
  if (typeof value === 'number' && Number.isFinite(value)) return value

  if (typeof value === 'string') {
    const trimmed = value.trim()
    if (!trimmed) return fallback

    const named = Object.entries(PAGE_TYPE_BY_NAME).find(
      ([name]) => name.toLowerCase() === trimmed.toLowerCase(),
    )
    if (named) return named[1]

    const parsed = Number(trimmed)
    if (Number.isFinite(parsed)) return parsed
  }

  return fallback
}
