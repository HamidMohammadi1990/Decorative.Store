import type { PropertyTypeValue } from '@/models/checkout/checkout.model'
import type {
  AdminPropertyItem,
  CreatePropertyItemInput,
  UpdatePropertyItemInput,
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
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/property-item'

function normalizePropertyItem(data: unknown, languageId?: number): AdminPropertyItem | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  const propertyId = readEncryptedId(record, 'propertyId', 'PropertyId')
  if (!id || !code || !propertyId) return null

  const translations = normalizeTranslations(record.translations ?? record.Translations)
  const translation = pickTranslation(translations, languageId)
  const propertyType = readNumberField(record, 'propertyType', 'PropertyType') as PropertyTypeValue

  return {
    id,
    code,
    propertyId,
    propertyCode: readStringField(record, 'propertyCode', 'PropertyCode'),
    propertyCategoryId: readEncryptedId(record, 'propertyCategoryId', 'PropertyCategoryId'),
    propertyType,
    priority: readNumberField(record, 'priority', 'Priority'),
    isActive: readIsActive(record),
    title: translation?.title ?? code,
    translations,
  }
}

export const adminPropertyItemService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      languageId?: number
      propertyId?: string | null
      propertyCategoryId?: string | null
    } = {},
  ): Promise<AdminPagedResult<AdminPropertyItem>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: null,
        propertyId: options.propertyId ?? null,
        isActive: null,
        propertyTitle: null,
        propertyCategoryId: options.propertyCategoryId ?? null,
        propertyType: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizePropertyItem(item, options.languageId))
  },

  async create(accessToken: string, locale: Locale, input: CreatePropertyItemInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdatePropertyItemInput): Promise<void> {
    await apiPut(
      `${BASE}/update`,
      {
        id: input.id,
        languageId: input.languageId,
        code: input.code,
        title: input.title,
        propertyId: input.propertyId,
        priority: input.priority,
        status: input.status,
      },
      { locale, accessToken },
    )
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
