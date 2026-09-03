import { API_BASE_URL } from '@/config/api'
import type {
  AdminMarketingPromo,
  CreateAdminMarketingPromoInput,
  MarketingPromoStrip,
  MarketingPromoType,
  UpdateAdminMarketingPromoInput,
  UpdateMarketingStripDisclaimerInput,
} from '@/models/admin/marketingPromo.model'
import type { PromoTileStrip } from '@/models/home/promoTiles.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut, toAcceptLanguage } from '@/services/api/apiClient'
import { normalizeApiEnvelope, readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'
import { ApiError } from '@/services/api/apiTypes'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const ADMIN_BASE = '/api/v1/admin/marketing-promo'
const PUBLIC_BASE = '/api/v1/marketing-promo'

export function resolveMarketingImageSrc(imageUrl: string): string {
  if (!imageUrl) return ''
  if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) return imageUrl
  if (imageUrl.startsWith('/images/')) return imageUrl
  if (imageUrl.startsWith('/Uploads/MarketingPromos/')) return `${API_BASE_URL}${imageUrl}`
  if (imageUrl.startsWith('/')) return `${API_BASE_URL}${imageUrl}`
  return `${API_BASE_URL}/Uploads/MarketingPromos/${imageUrl.replace(/^\/+/, '')}`
}

function readPromoType(record: Record<string, unknown>): MarketingPromoType {
  const raw = readNumberField(record, 'promoType', 'PromoType')
  return raw === 2 ? 2 : 1
}

function normalizeAdminMarketingPromo(data: unknown): AdminMarketingPromo | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const title = readStringField(record, 'title', 'Title')
  const linkLabel = readStringField(record, 'linkLabel', 'LinkLabel')
  const linkHref = readStringField(record, 'linkHref', 'LinkHref')
  const imageFileName = readStringField(record, 'imageFileName', 'ImageFileName')
  if (!id || !title || !linkLabel || !linkHref || !imageFileName) return null

  return {
    id,
    languageId: readNumberField(record, 'languageId', 'LanguageId'),
    promoType: readPromoType(record),
    title,
    subtitle: readStringField(record, 'subtitle', 'Subtitle') || null,
    linkLabel,
    linkHref,
    imageFileName,
    priority: readNumberField(record, 'priority', 'Priority'),
    isActive: readIsActive(record),
  }
}

function normalizeMarketingStrip(data: unknown): MarketingPromoStrip | null {
  const record = readRecord(data)
  if (!record) return null

  const tilesRaw = record.tiles ?? record.Tiles
  const tiles = Array.isArray(tilesRaw)
    ? tilesRaw
        .map((item) => {
          const tile = readRecord(item)
          if (!tile) return null
          const id = readEncryptedId(tile, 'id', 'Id')
          const title = readStringField(tile, 'title', 'Title')
          const imageUrl = readStringField(tile, 'imageUrl', 'ImageUrl')
          const linkLabel = readStringField(tile, 'linkLabel', 'LinkLabel')
          const linkHref = readStringField(tile, 'linkHref', 'LinkHref')
          if (!id || !title || !imageUrl || !linkLabel || !linkHref) return null
          return {
            id,
            promoType: readPromoType(tile),
            title,
            subtitle: readStringField(tile, 'subtitle', 'Subtitle') || null,
            imageUrl,
            linkLabel,
            linkHref,
          }
        })
        .filter((item): item is NonNullable<typeof item> => item != null)
    : []

  const disclaimerLinkRecord = readRecord(record.disclaimerLink ?? record.DisclaimerLink)
  const disclaimerLinkLabel = disclaimerLinkRecord
    ? readStringField(disclaimerLinkRecord, 'label', 'Label')
    : ''
  const disclaimerLinkHref = disclaimerLinkRecord
    ? readStringField(disclaimerLinkRecord, 'href', 'Href')
    : ''

  return {
    tiles,
    disclaimer: readStringField(record, 'disclaimer', 'Disclaimer') || null,
    disclaimerLink:
      disclaimerLinkLabel && disclaimerLinkHref
        ? { label: disclaimerLinkLabel, href: disclaimerLinkHref }
        : null,
  }
}

export function mapMarketingStripToPromoTiles(strip: MarketingPromoStrip): PromoTileStrip {
  return {
    tiles: strip.tiles.map((tile) => ({
      id: tile.id,
      title: tile.title,
      subtitle: tile.subtitle ?? undefined,
      image: {
        src: resolveMarketingImageSrc(tile.imageUrl),
        alt: tile.title,
      },
      link: {
        label: tile.linkLabel,
        href: tile.linkHref,
      },
    })),
    disclaimer: strip.disclaimer ?? undefined,
    disclaimerLink: strip.disclaimerLink ?? undefined,
  }
}

export const adminMarketingPromoService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      languageId?: number
      title?: string | null
      promoType?: MarketingPromoType | null
      isActive?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminMarketingPromo>> {
    const data = await apiPost<unknown>(
      `${ADMIN_BASE}/get-all`,
      {
        languageId: options.languageId ?? null,
        title: options.title ?? null,
        promoType: options.promoType ?? null,
        isActive: options.isActive ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 50),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeAdminMarketingPromo)
  },

  async create(accessToken: string, locale: Locale, input: CreateAdminMarketingPromoInput) {
    return apiPost<{ id: string }>(`${ADMIN_BASE}/create`, input, { locale, accessToken })
  },

  async update(accessToken: string, locale: Locale, input: UpdateAdminMarketingPromoInput) {
    return apiPut(`${ADMIN_BASE}/update`, input, accessToken, { locale })
  },

  async delete(accessToken: string, locale: Locale, id: string) {
    return apiDelete(`${ADMIN_BASE}/delete`, accessToken, { id }, { locale })
  },

  async updateDisclaimer(
    accessToken: string,
    locale: Locale,
    input: UpdateMarketingStripDisclaimerInput,
  ) {
    return apiPut(`${ADMIN_BASE}/update-disclaimer`, input, accessToken, { locale })
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
      imageUrl: resolveMarketingImageSrc(imageUrl),
    }
  },
}

export const marketingPromoService = {
  async getPromoStrip(locale: Locale, languageId: number): Promise<MarketingPromoStrip | null> {
    const data = await apiPost<unknown>(`${PUBLIC_BASE}/promo-strip`, { languageId }, { locale })
    return normalizeMarketingStrip(data)
  },
}
