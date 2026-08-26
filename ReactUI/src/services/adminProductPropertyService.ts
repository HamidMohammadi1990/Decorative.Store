import type {
  AdminProductProperty,
  CreateProductPropertyInput,
  UpdateProductPropertyInput,
} from '@/models/admin/property.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  readOptionalEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/product-property'

function normalizeProductProperty(data: unknown): AdminProductProperty | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const productId = readEncryptedId(record, 'productId', 'ProductId')
  const propertyId = readEncryptedId(record, 'propertyId', 'PropertyId')
  if (!id || !productId || !propertyId) return null

  return {
    id,
    productId,
    propertyId,
    propertyItemId: readOptionalEncryptedId(record, 'propertyItemId', 'PropertyItemId'),
    isActive: readIsActive(record),
  }
}

export const adminProductPropertyService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      productId?: string | null
      propertyId?: string | null
    } = {},
  ): Promise<AdminPagedResult<AdminProductProperty>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        productId: options.productId ?? null,
        propertyId: options.propertyId ?? null,
        isActive: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeProductProperty)
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminProductProperty | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    return normalizeProductProperty(data)
  },

  async create(accessToken: string, locale: Locale, input: CreateProductPropertyInput): Promise<string> {
    const data = await apiPost<unknown>(
      `${BASE}/create`,
      {
        productId: input.productId,
        propertyId: input.propertyId,
        propertyItemId: input.propertyItemId ?? null,
        isActive: input.isActive,
      },
      { locale, accessToken },
    )
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateProductPropertyInput): Promise<void> {
    await apiPut(
      `${BASE}/update`,
      {
        id: input.id,
        productId: input.productId,
        propertyId: input.propertyId,
        propertyItemId: input.propertyItemId ?? null,
        isActive: input.isActive,
      },
      { locale, accessToken },
    )
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
