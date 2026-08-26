import type {
  AdminProductDetail,
  AdminProductListItem,
  CreateProductInput,
  UpdateProductInput,
} from '@/models/admin/catalog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import {
  readBooleanField,
  readNumberField,
  readOptionalNumberField,
  readRecord,
  readStringField,
} from '@/services/api/apiNormalize'
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

const BASE = '/api/v1/admin/product'

function normalizeProductListItem(data: unknown, languageId?: number): AdminProductListItem | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const productCode = readStringField(record, 'productCode', 'ProductCode')
  const subCategoryId = readOptionalEncryptedId(record, 'subCategoryId', 'SubCategoryId')
  if (!id || !productCode) return null

  const translations = normalizeTranslations(record.translations ?? record.Translations)
  const translation = pickTranslation(translations, languageId)

  return {
    id,
    productCode,
    isActive: readIsActive(record),
    creationDate: readStringField(record, 'creationDate', 'CreationDate'),
    title: translation?.title ?? productCode,
    slug: translation?.slug ?? '',
    subCategoryId: subCategoryId ?? '',
    translations,
  }
}

function normalizeProductDetail(data: unknown): AdminProductDetail | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const subCategoryId = readOptionalEncryptedId(record, 'subCategoryId', 'SubCategoryId')
  const title = readStringField(record, 'title', 'Title')
  if (!id || !title) return null

  const compareAt = readOptionalNumberField(record, 'compareAtPrice', 'CompareAtPrice')

  return {
    id,
    subCategoryId: subCategoryId ?? '',
    subCategoryTitle: readStringField(record, 'subCategoryTitle', 'SubCategoryTitle'),
    categoryTitle: readStringField(record, 'categoryTitle', 'CategoryTitle'),
    title,
    slug: readStringField(record, 'slug', 'Slug'),
    description: readStringField(record, 'description', 'Description'),
    productCode: readStringField(record, 'productCode', 'ProductCode'),
    price: readNumberField(record, 'price', 'Price'),
    compareAtPrice: compareAt ?? null,
    isActive: readBooleanField(record, 'isActive', 'IsActive'),
    creationDate: readStringField(record, 'creationDate', 'CreationDate'),
  }
}

export const adminProductService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      languageId?: number
      title?: string | null
      productCode?: string | null
      subCategoryId?: string | null
    } = {},
  ): Promise<AdminPagedResult<AdminProductListItem>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        categoryId: null,
        categorySlug: null,
        subCategoryId: options.subCategoryId ?? null,
        subCategorySlug: null,
        slug: null,
        title: options.title ?? null,
        productCode: options.productCode ?? null,
        isActive: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 50),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, (item) => normalizeProductListItem(item, options.languageId))
  },

  async get(accessToken: string, locale: Locale, id: string): Promise<AdminProductDetail | null> {
    const data = await apiPost<unknown>(`${BASE}/get`, { id }, { locale, accessToken })
    return normalizeProductDetail(data)
  },

  async create(accessToken: string, locale: Locale, input: CreateProductInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateProductInput): Promise<void> {
    await apiPut(`${BASE}/update`, input, { locale, accessToken })
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
