import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { normalizeAdminPaged, paginationBody } from '@/services/admin/adminCatalogNormalize'
import { readBooleanField, readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'

const LANGUAGE_SEARCH_PATH = '/api/v1/language/search'

export interface StoreLanguage {
  id: number
  code: string
  name: string
  isDefault: boolean
}

function normalizeLanguage(data: unknown): StoreLanguage | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readNumberField(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  if (!id || !code) return null

  return {
    id,
    code,
    name: readStringField(record, 'name', 'Name') || code,
    isDefault: readBooleanField(record, 'isDefault', 'IsDefault'),
  }
}

function matchLocale(code: string, locale: Locale): boolean {
  const normalized = code.trim().toLowerCase()
  if (locale === 'fa') {
    return normalized === 'fa' || normalized.startsWith('fa-')
  }
  return normalized === 'en' || normalized.startsWith('en-')
}

export const languageService = {
  async search(locale: Locale, code?: string): Promise<StoreLanguage[]> {
    const data = await apiPost<unknown>(
      LANGUAGE_SEARCH_PATH,
      {
        code: code ?? null,
        name: null,
        pagination: paginationBody(1, 50),
      },
      { locale },
    )

    return normalizeAdminPaged(data, normalizeLanguage).items
  },

  async resolveLanguageId(locale: Locale): Promise<number> {
    const languages = await this.search(locale)
    const match = languages.find((item) => matchLocale(item.code, locale))
    if (match) return match.id

    const fallback = languages.find((item) => item.isDefault) ?? languages[0]
    if (!fallback) {
      throw new Error('No languages available')
    }
    return fallback.id
  },
}
