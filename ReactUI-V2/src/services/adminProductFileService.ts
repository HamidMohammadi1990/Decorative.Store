import { API_BASE_URL } from '@/config/api'
import type {
  AdminProductFile,
  CreateProductFileInput,
  ProductFileKind,
} from '@/models/admin/catalog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut, toAcceptLanguage } from '@/services/api/apiClient'
import { normalizeApiEnvelope, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'
import { ApiError } from '@/services/api/apiTypes'

const BASE = '/api/v1/admin/product-file'

export function resolveProductFileUrl(fileName: string): string {
  if (!fileName) return ''
  if (fileName.startsWith('http://') || fileName.startsWith('https://')) return fileName
  const path = fileName.startsWith('/')
    ? fileName
    : `/Uploads/Products/${fileName.replace(/^\/+/, '')}`
  return `${API_BASE_URL}${path}`
}

function normalizeProductFile(data: unknown): AdminProductFile | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const productId = readEncryptedId(record, 'productId', 'ProductId')
  const fileName = readStringField(record, 'fileName', 'FileName')
  if (!id || !productId || !fileName) return null

  const imageUrl =
    readStringField(record, 'imageUrl', 'ImageUrl') || resolveProductFileUrl(fileName)

  const kindRaw = readStringField(record, 'kind', 'Kind')
  const kind: ProductFileKind =
    kindRaw === 'RoomLayout' ? 'RoomLayout' : 'Gallery'

  return {
    id,
    productId,
    productTitle: readStringField(record, 'productTitle', 'ProductTitle'),
    title: readStringField(record, 'title', 'Title'),
    fileName,
    imageUrl,
    isActive: readIsActive(record),
    isMain: Boolean(record.isMain ?? record.IsMain),
    kind,
  }
}

export const adminProductFileService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      productId?: string | null
      pageNumber?: number
      pageSize?: number
    } = {},
  ): Promise<AdminPagedResult<AdminProductFile>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: null,
        productId: options.productId ?? null,
        isActive: null,
        isMain: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeProductFile)
  },

  async createRange(
    accessToken: string,
    locale: Locale,
    files: CreateProductFileInput[],
  ): Promise<AdminProductFile[]> {
    const formData = new FormData()

    files.forEach((file, index) => {
      formData.append(`Files[${index}].ProductId`, file.productId)
      formData.append(`Files[${index}].LanguageId`, String(file.languageId))
      formData.append(`Files[${index}].Title`, file.title)
      formData.append(`Files[${index}].IsIndex`, String(file.isIndex))
      formData.append(`Files[${index}].Kind`, file.kind ?? 'Gallery')
      formData.append(`Files[${index}].Image`, file.image)
    })

    const response = await fetch(`${API_BASE_URL}${BASE}/create-range`, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Accept-Language': toAcceptLanguage(locale),
        Authorization: `Bearer ${accessToken}`,
      },
      body: formData,
    })

    const responseBody = await response.json()
    const { isSuccess, data, messages } = normalizeApiEnvelope<unknown[]>(responseBody)

    if (!response.ok || !isSuccess || !Array.isArray(data)) {
      throw new ApiError(response.status, messages)
    }

    return data.flatMap((item) => {
      const record = readRecord(item)
      if (!record) return []
      const id = readEncryptedId(record, 'id', 'Id')
      const imageUrl = readStringField(record, 'imageUrl', 'ImageUrl')
      const title = readStringField(record, 'title', 'Title')
      if (!id || !imageUrl) return []
      return [
        {
          id,
          productId: files[0]?.productId ?? '',
          productTitle: '',
          title,
          fileName: '',
          imageUrl,
          isActive: true,
          isMain: files.some((f) => f.isIndex),
          kind: files[0]?.kind ?? 'Gallery',
        },
      ]
    })
  },

  async updateStatus(
    accessToken: string,
    locale: Locale,
    id: string,
    status: boolean,
  ): Promise<void> {
    await apiPut(`${BASE}/status`, { id, status }, { locale, accessToken })
  },

  async setMain(accessToken: string, locale: Locale, id: string): Promise<void> {
    await apiPut(`${BASE}/main`, { id }, { locale, accessToken })
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
