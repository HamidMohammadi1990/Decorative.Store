import { API_BASE_URL } from '@/config/api'
import type { Locale } from '@/models/shared/locale.model'
import { toAcceptLanguage } from '@/services/api/apiClient'
import { normalizeApiEnvelope, readRecord, readStringField } from '@/services/api/apiNormalize'
import { ApiError } from '@/services/api/apiTypes'

const SECTION_UPLOAD = '/api/v1/admin/section/upload-image'
const SECTION_ITEM_UPLOAD = '/api/v1/admin/section-item/upload-image'

/** Stored value may be a filename, `/Uploads/Cms/...`, or static `/images/...` path. */
export function normalizeCmsImageStorageValue(imageUrl: string | null | undefined): string {
  const trimmed = imageUrl?.trim() ?? ''
  if (!trimmed) return ''
  if (trimmed.startsWith('/Uploads/Cms/')) {
    return trimmed.slice('/Uploads/Cms/'.length)
  }
  return trimmed
}

export function resolveCmsImageSrc(imageUrl: string): string {
  if (!imageUrl) return ''
  if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) return imageUrl
  if (imageUrl.startsWith('/images/')) return imageUrl
  if (imageUrl.startsWith('/Uploads/')) return imageUrl
  return `/Uploads/Cms/${imageUrl.replace(/^\/+/, '')}`
}

async function uploadCmsImage(
  uploadPath: string,
  accessToken: string,
  locale: Locale,
  file: File,
): Promise<{ imageFileName: string; imageUrl: string; storageValue: string }> {
  const formData = new FormData()
  formData.append('image', file)

  const response = await fetch(`${API_BASE_URL}${uploadPath}`, {
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
    throw new ApiError(response.status, normalizeApiEnvelope(responseBody).messages)
  }

  const envelope = normalizeApiEnvelope(responseBody)
  const record = readRecord(envelope.data)
  const imageFileName = readStringField(record ?? {}, 'imageFileName', 'ImageFileName')
  const imageUrl = readStringField(record ?? {}, 'imageUrl', 'ImageUrl')
  if (!imageFileName || !imageUrl) throw new ApiError(response.status, envelope.messages)

  return {
    imageFileName,
    imageUrl: resolveCmsImageSrc(imageFileName),
    storageValue: normalizeCmsImageStorageValue(imageUrl),
  }
}

export const adminCmsMediaService = {
  uploadSectionImage(accessToken: string, locale: Locale, file: File) {
    return uploadCmsImage(SECTION_UPLOAD, accessToken, locale, file)
  },

  uploadSectionItemImage(accessToken: string, locale: Locale, file: File) {
    return uploadCmsImage(SECTION_ITEM_UPLOAD, accessToken, locale, file)
  },
}
