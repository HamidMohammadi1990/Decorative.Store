import type {
  AdminProductDescription,
  CreateProductDescriptionInput,
  UpdateProductDescriptionInput,
} from '@/models/admin/catalog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import { readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/product-description'

function normalizeProductDescription(data: unknown): AdminProductDescription | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const productId = readEncryptedId(record, 'productId', 'ProductId')
  const description = readStringField(record, 'description', 'Description')
  if (!id || !productId || !description) return null

  return {
    id,
    productId,
    languageId: readNumberField(record, 'languageId', 'LanguageId'),
    description,
    productTitle: readStringField(record, 'productTitle', 'ProductTitle'),
  }
}

export const adminProductDescriptionService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: { pageNumber?: number; pageSize?: number; productId: string },
  ): Promise<AdminPagedResult<AdminProductDescription>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        productId: options.productId,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeProductDescription)
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminProductDescription | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    return normalizeProductDescription(data)
  },

  async create(
    accessToken: string,
    locale: Locale,
    input: CreateProductDescriptionInput,
  ): Promise<string> {
    const data = await apiPost<unknown>(
      `${BASE}/create`,
      {
        productId: input.productId,
        languageId: input.languageId,
        description: input.description,
      },
      { locale, accessToken },
    )
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(
    accessToken: string,
    locale: Locale,
    input: UpdateProductDescriptionInput,
  ): Promise<void> {
    await apiPut(
      `${BASE}/update`,
      {
        id: input.id,
        productId: input.productId,
        languageId: input.languageId,
        description: input.description,
      },
      { locale, accessToken },
    )
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
