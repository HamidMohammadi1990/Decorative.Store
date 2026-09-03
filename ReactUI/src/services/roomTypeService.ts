import { API_BASE_URL } from '@/config/api'
import type {
  AdminRoomType,
  CreateAdminRoomTypeInput,
  RoomTypeItem,
  UpdateAdminRoomTypeInput,
} from '@/models/admin/roomType.model'
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

const ADMIN_BASE = '/api/v1/admin/room-type'
const PUBLIC_BASE = '/api/v1/room-type'

export function resolveRoomTypeImageUrl(imageUrlOrFileName: string): string {
  if (!imageUrlOrFileName) return ''
  if (
    imageUrlOrFileName.startsWith('http://') ||
    imageUrlOrFileName.startsWith('https://') ||
    imageUrlOrFileName.startsWith('/images/')
  ) {
    return imageUrlOrFileName
  }
  if (imageUrlOrFileName.startsWith('/')) {
    return `${API_BASE_URL}${imageUrlOrFileName}`
  }
  return `${API_BASE_URL}/Uploads/RoomTypes/${imageUrlOrFileName.replace(/^\/+/, '')}`
}

function normalizeAdminRoomType(data: unknown): AdminRoomType | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  const imageFileName = readStringField(record, 'imageFileName', 'ImageFileName')
  if (!id || !code || !imageFileName) return null

  const imageUrl =
    readStringField(record, 'imageUrl', 'ImageUrl') || resolveRoomTypeImageUrl(imageFileName)

  return {
    id,
    code,
    title: readStringField(record, 'title', 'Title') || null,
    imageFileName,
    imageUrl,
    priority: Number(record.priority ?? record.Priority ?? 0),
    isActive: readIsActive(record),
    languageId: record.languageId != null || record.LanguageId != null
      ? Number(record.languageId ?? record.LanguageId)
      : null,
  }
}

function normalizeRoomTypeItem(data: unknown): RoomTypeItem | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  const title = readStringField(record, 'title', 'Title')
  const imageFileName = readStringField(record, 'imageFileName', 'ImageFileName')
  const rawImageUrl =
    readStringField(record, 'imageUrl', 'ImageUrl') ||
    (imageFileName ? resolveRoomTypeImageUrl(imageFileName) : '')
  if (!id || !code || !title || !rawImageUrl) return null

  return {
    id,
    code,
    title,
    imageUrl: resolveRoomTypeImageUrl(rawImageUrl),
    priority: Number(record.priority ?? record.Priority ?? 0),
  }
}

export const adminRoomTypeService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      languageId?: number
      title?: string | null
      code?: string | null
      isActive?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminRoomType>> {
    const data = await apiPost<unknown>(
      `${ADMIN_BASE}/get-all`,
      {
        languageId: options.languageId ?? null,
        title: options.title ?? null,
        code: options.code ?? null,
        isActive: options.isActive ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 50),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeAdminRoomType)
  },

  async create(
    accessToken: string,
    locale: Locale,
    input: CreateAdminRoomTypeInput,
  ): Promise<string> {
    const data = await apiPost<unknown>(`${ADMIN_BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readEncryptedId(record ?? {}, 'id', 'Id')
  },

  async update(
    accessToken: string,
    locale: Locale,
    input: UpdateAdminRoomTypeInput,
  ): Promise<void> {
    await apiPut(`${ADMIN_BASE}/update`, input, { locale, accessToken })
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${ADMIN_BASE}/delete`, accessToken, { id })
  },

  async uploadImage(
    accessToken: string,
    locale: Locale,
    file: File,
  ): Promise<{ imageFileName: string; imageUrl: string }> {
    const formData = new FormData()
    formData.append('image', file)

    const response = await fetch(`${API_BASE_URL}${ADMIN_BASE}/upload-image`, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Accept-Language': toAcceptLanguage(locale),
        Authorization: `Bearer ${accessToken}`,
      },
      body: formData,
    })

    const responseBody = await response.json()
    if (!response.ok) {
      throw new ApiError(response.status, normalizeApiEnvelope(responseBody))
    }

    const envelope = normalizeApiEnvelope(responseBody)
    const record = readRecord(envelope.data)
    const imageFileName = readStringField(record ?? {}, 'imageFileName', 'ImageFileName')
    const imageUrl = readStringField(record ?? {}, 'imageUrl', 'ImageUrl')
    if (!imageFileName || !imageUrl) throw new ApiError(response.status, envelope)

    return {
      imageFileName,
      imageUrl: resolveRoomTypeImageUrl(imageUrl),
    }
  },
}

export const roomTypeService = {
  async list(locale: Locale, languageId: number): Promise<RoomTypeItem[]> {
    const data = await apiPost<unknown>(
      `${PUBLIC_BASE}/list`,
      { languageId },
      { locale },
    )

    const list = Array.isArray(data) ? data : []
    return list
      .map(normalizeRoomTypeItem)
      .filter((item): item is RoomTypeItem => item !== null)
      .sort((a, b) => a.priority - b.priority || a.title.localeCompare(b.title))
  },
}
