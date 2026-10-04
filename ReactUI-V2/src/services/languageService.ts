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

function searchCacheKey(locale: Locale, code?: string) {
  return `${locale}:${code ?? ''}`
}

const searchResultsCache = new Map<string, StoreLanguage[]>()
const languageIdCache = new Map<Locale, number>()
const searchRequests = new Map<string, Promise<StoreLanguage[]>>()
const languageIdRequests = new Map<Locale, Promise<number>>()

export const languageService = {
  /** Seed client cache from SSR loader data to avoid duplicate /language/search calls. */
  seedResolvedLanguage(locale: Locale, languageId: number) {
    languageIdCache.set(locale, languageId)
  },

  async search(locale: Locale, code?: string): Promise<StoreLanguage[]> {
    const cacheKey = searchCacheKey(locale, code)
    const cached = searchResultsCache.get(cacheKey)
    if (cached) return cached

    const inFlight = searchRequests.get(cacheKey)
    if (inFlight) return inFlight

    const request = (async () => {
      const data = await apiPost<unknown>(
        LANGUAGE_SEARCH_PATH,
        {
          code: code ?? null,
          name: null,
          pagination: paginationBody(1, 50),
        },
        { locale },
      )

      const items = normalizeAdminPaged(data, normalizeLanguage).items
      searchResultsCache.set(cacheKey, items)
      return items
    })()

    searchRequests.set(cacheKey, request)

    try {
      return await request
    } catch (error) {
      searchRequests.delete(cacheKey)
      throw error
    }
  },

  async resolveLanguageId(locale: Locale): Promise<number> {
    const cachedId = languageIdCache.get(locale)
    if (cachedId !== undefined) return cachedId

    const inFlight = languageIdRequests.get(locale)
    if (inFlight) return inFlight

    const request = (async () => {
      const languages = await this.search(locale)
      const match = languages.find((item) => matchLocale(item.code, locale))
      if (match) return match.id

      const fallback = languages.find((item) => item.isDefault) ?? languages[0]
      if (!fallback) {
        throw new Error('No languages available')
      }
      return fallback.id
    })()

    languageIdRequests.set(locale, request)

    try {
      const id = await request
      languageIdCache.set(locale, id)
      return id
    } catch (error) {
      languageIdRequests.delete(locale)
      throw error
    }
  },
}
