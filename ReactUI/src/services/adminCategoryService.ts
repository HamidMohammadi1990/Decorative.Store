import type {
  AdminCategory,
  CreateCategoryInput,
  UpdateCategoryInput,
} from '@/models/admin/catalog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  normalizeTranslations,
  paginationBody,
  pickTranslation,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/category'

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
    const probe = await this.getAll(accessToken, locale, {
      pageNumber: 1,
      pageSize: 1,
      languageId: options.languageId,
    })
    const pageSize = Math.max(probe.totalCount, 1)
    if (pageSize <= probe.items.length) return probe

    return this.getAll(accessToken, locale, {
      pageNumber: 1,
      pageSize,
      languageId: options.languageId,
    })
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminCategory | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    return normalizeCategoryFromGet(data)
  },

  async create(accessToken: string, locale: Locale, input: CreateCategoryInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateCategoryInput): Promise<void> {
    await apiPut(`${BASE}/update`, input, { locale, accessToken })
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
