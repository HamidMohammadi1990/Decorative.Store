import type {
  AdminCategory,
  CreateCategoryInput,
  UpdateCategoryInput,
} from '@/models/admin/catalog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  ADMIN_SELECT_PAGE_SIZE,
  computeTotalPages,
  fetchAllAdminPages,
  normalizeAdminPaged,
  normalizeTranslations,
  paginationBody,
  pickTranslation,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'
import {
  buildAdminSelectCacheKey,
  createAdminSelectCache,
} from '@/services/admin/adminSelectListCache'

const BASE = '/api/v1/admin/category'

const categorySelectCache = createAdminSelectCache<AdminPagedResult<AdminCategory>>()

function normalizeCategory(data: unknown, languageId?: number): AdminCategory | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  if (!id || !code) return null

  const translations = normalizeTranslations(record.translations ?? record.Translations)
  const translation = pickTranslation(translations, languageId)

  return {
    id,
    code,
    isActive: readIsActive(record),
    title: translation?.title ?? code,
    slug: translation?.slug ?? '',
    translations,
  }
}

function normalizeCategoryFromGet(data: unknown): AdminCategory | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  const title = readStringField(record, 'title', 'Title')
  if (!id || !code || !title) return null

  return {
    id,
    code,
    isActive: readIsActive(record),
    title,
    slug: readStringField(record, 'slug', 'Slug'),
    translations: [],
  }
}

export const adminCategoryService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: { pageNumber?: number; pageSize?: number; languageId?: number } = {},
  ): Promise<AdminPagedResult<AdminCategory>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: null,
        slug: null,
        code: null,
        isActive: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 50),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizeCategory(item, options.languageId))
  },

  async getAllForSelect(
    accessToken: string,
    locale: Locale,
    options: { languageId?: number } = {},
  ): Promise<AdminPagedResult<AdminCategory>> {
    const cacheKey = buildAdminSelectCacheKey(accessToken, locale, options.languageId)

    return categorySelectCache.get(cacheKey, async () => {
      const items = await fetchAllAdminPages(
        (pageNumber, pageSize) =>
          this.getAll(accessToken, locale, {
            pageNumber,
            pageSize,
            languageId: options.languageId,
          }),
        ADMIN_SELECT_PAGE_SIZE,
      )

      const pageSize = Math.max(items.length, 1)
      return {
        items,
        totalCount: items.length,
        pageNumber: 1,
        pageSize,
        totalPages: computeTotalPages(items.length, pageSize),
      }
    })
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminCategory | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    return normalizeCategoryFromGet(data)
  },

  async create(accessToken: string, locale: Locale, input: CreateCategoryInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    categorySelectCache.invalidate()
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateCategoryInput): Promise<void> {
    await apiPut(`${BASE}/update`, input, { locale, accessToken })
    categorySelectCache.invalidate()
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
    categorySelectCache.invalidate()
  },
}
