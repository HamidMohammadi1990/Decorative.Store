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
