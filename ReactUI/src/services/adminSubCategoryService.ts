import type {
  AdminSubCategory,
  CreateSubCategoryInput,
  UpdateSubCategoryInput,
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

const BASE = '/api/v1/admin/sub-category'

function normalizeSubCategory(data: unknown, languageId?: number): AdminSubCategory | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  const categoryId = readEncryptedId(record, 'categoryId', 'CategoryId')
  if (!id || !code || !categoryId) return null

  const translations = normalizeTranslations(record.translations ?? record.Translations)
  const categoryTranslations = normalizeTranslations(
    record.categoryTranslations ?? record.CategoryTranslations,
  )
  const translation = pickTranslation(translations, languageId)
  const categoryTranslation = pickTranslation(categoryTranslations, languageId)

  return {
    id,
    code,
    categoryId,
    categoryCode: readStringField(record, 'categoryCode', 'CategoryCode'),
    categoryTitle: categoryTranslation?.title ?? readStringField(record, 'categoryCode', 'CategoryCode'),
    isActive: readIsActive(record),
    title: translation?.title ?? code,
    slug: translation?.slug ?? '',
    translations,
  }
}

function normalizeSubCategoryFromGet(data: unknown): AdminSubCategory | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  const categoryId = readEncryptedId(record, 'categoryId', 'CategoryId')
  const title = readStringField(record, 'title', 'Title')
  if (!id || !code || !categoryId || !title) return null

  return {
    id,
    code,
    categoryId,
    categoryCode: readStringField(record, 'categoryCode', 'CategoryCode'),
    categoryTitle: readStringField(record, 'categoryTitle', 'CategoryTitle'),
    isActive: readIsActive(record),
    title,
    slug: readStringField(record, 'slug', 'Slug'),
    translations: [],
  }
}

export const adminSubCategoryService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      languageId?: number
      categoryId?: string | null
    } = {},
  ): Promise<AdminPagedResult<AdminSubCategory>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: null,
        slug: null,
        code: null,
        categoryId: options.categoryId ?? null,
        categoryTitle: null,
        categoryCode: null,
        isActive: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 50),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizeSubCategory(item, options.languageId))
  },

  async getAllForSelect(
    accessToken: string,
    locale: Locale,
    options: {
      languageId?: number
      categoryId?: string | null
    } = {},
  ): Promise<AdminPagedResult<AdminSubCategory>> {
    const probe = await this.getAll(accessToken, locale, {
      pageNumber: 1,
      pageSize: 1,
      languageId: options.languageId,
      categoryId: options.categoryId,
    })
    const pageSize = Math.max(probe.totalCount, 1)
    if (pageSize <= probe.items.length) return probe

    return this.getAll(accessToken, locale, {
      pageNumber: 1,
      pageSize,
      languageId: options.languageId,
      categoryId: options.categoryId,
    })
  },

  async get(
    accessToken: string,
    locale: Locale,
    id: string,
  ): Promise<AdminSubCategory | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    return normalizeSubCategoryFromGet(data)
  },

  async create(accessToken: string, locale: Locale, input: CreateSubCategoryInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateSubCategoryInput): Promise<void> {
    await apiPut(`${BASE}/update`, input, { locale, accessToken })
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
