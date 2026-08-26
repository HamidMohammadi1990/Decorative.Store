import type { PropertyTypeValue } from '@/models/checkout/checkout.model'
import type {
  AdminProperty,
  CreatePropertyInput,
  UpdatePropertyInput,
} from '@/models/admin/property.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import { readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  normalizeTranslations,
  paginationBody,
  pickTranslation,
  readEncryptedId,
  readIsActive,
  readOptionalEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/property'

function normalizeProperty(data: unknown, languageId?: number): AdminProperty | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  if (!id || !code) return null

  const propertyCategoryId = readEncryptedId(record, 'propertyCategoryId', 'PropertyCategoryId')
  if (!propertyCategoryId) return null

  const translations = normalizeTranslations(record.translations ?? record.Translations)
  const translation = pickTranslation(translations, languageId)
  const propertyType = readNumberField(record, 'propertyType', 'PropertyType') as PropertyTypeValue

  return {
    id,
    code,
    parentId: readOptionalEncryptedId(record, 'parentId', 'ParentId'),
    propertyCategoryId,
    propertyCategoryCode: readStringField(record, 'propertyCategoryCode', 'PropertyCategoryCode'),
    priority: readNumberField(record, 'priority', 'Priority'),
    propertyType,
    isActive: readIsActive(record),
    title: translation?.title ?? readStringField(record, 'title', 'Title') ?? code,
    description: translation?.description,
    translations,
  }
}

export const adminPropertyService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      languageId?: number
      propertyCategoryId?: string | null
      propertyType?: PropertyTypeValue | null
    } = {},
  ): Promise<AdminPagedResult<AdminProperty>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        parentId: null,
        title: null,
        propertyCategoryId: options.propertyCategoryId ?? null,
        propertyType: options.propertyType ?? null,
        isActive: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizeProperty(item, options.languageId))
  },

  async create(accessToken: string, locale: Locale, input: CreatePropertyInput): Promise<string> {
    const data = await apiPost<unknown>(
      `${BASE}/create`,
      {
        languageId: input.languageId,
        code: input.code,
        title: input.title,
        description: input.description ?? null,
        parentId: input.parentId ?? null,
        priority: input.priority,
        propertyCategoryId: input.propertyCategoryId,
        propertyType: input.propertyType,
      },
      { locale, accessToken },
    )
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdatePropertyInput): Promise<void> {
    await apiPut(
      `${BASE}/update`,
      {
        id: input.id,
        languageId: input.languageId,
        code: input.code,
        title: input.title,
        description: input.description ?? null,
        parentId: input.parentId ?? null,
        priority: input.priority,
        propertyCategoryId: input.propertyCategoryId,
        propertyType: input.propertyType,
        status: input.status,
      },
      { locale, accessToken },
    )
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
