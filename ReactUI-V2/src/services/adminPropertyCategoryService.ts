import type {
  AdminPropertyCategory,
  CreatePropertyCategoryInput,
  UpdatePropertyCategoryInput,
} from '@/models/admin/property.model'
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

const BASE = '/api/v1/admin/property-category'

function normalizePropertyCategory(data: unknown, languageId?: number): AdminPropertyCategory | null {
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
    translations,
  }
}

export const adminPropertyCategoryService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: { pageNumber?: number; pageSize?: number; languageId?: number } = {},
  ): Promise<AdminPagedResult<AdminPropertyCategory>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: null,
        code: null,
        isActive: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizePropertyCategory(item, options.languageId))
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminPropertyCategory | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    return normalizePropertyCategory(data)
  },

  async create(accessToken: string, locale: Locale, input: CreatePropertyCategoryInput): Promise<string> {
    const data = await apiPost<unknown>(
      `${BASE}/create`,
      {
        languageId: input.languageId,
        code: input.code,
        title: input.title,
      },
      { locale, accessToken },
    )
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdatePropertyCategoryInput): Promise<void> {
    await apiPut(
      `${BASE}/update`,
      {
        id: input.id,
        languageId: input.languageId,
        code: input.code,
        title: input.title,
        isActive: input.isActive,
      },
      { locale, accessToken },
    )
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
