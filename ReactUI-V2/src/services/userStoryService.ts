import { API_BASE_URL } from '@/config/api'
import type { Locale } from '@/models/shared/locale.model'
import type { StoryMediaType, UserStoryDraft, UserStoryInput } from '@/models/stories/story.model'
import { ApiError } from '@/services/api/apiTypes'
import { normalizeApiEnvelope, readRecord, readStringField } from '@/services/api/apiNormalize'
import { apiDelete, apiGet, apiPost, apiPut, toAcceptLanguage } from '@/services/api/apiClient'

const USER_STORY_MY_PATH = '/api/v1/user-story/my'
const USER_STORY_CREATE_PATH = '/api/v1/user-story/create'
const USER_STORY_UPDATE_PATH = '/api/v1/user-story/update'
const USER_STORY_DELETE_PATH = '/api/v1/user-story/delete'
const USER_STORY_UPLOAD_PATH = '/api/v1/user-story/upload-media'
const USER_STORY_SEARCH_ACTIVE_PATH = '/api/v1/user-story/search-active'

function resolveMediaUrl(path: string): string {
  if (!path) return ''
  if (path.startsWith('http://') || path.startsWith('https://') || path.startsWith('data:'))
    return path
  return `${API_BASE_URL}${path.startsWith('/') ? path : `/${path}`}`
}

function readMediaType(value: unknown): StoryMediaType {
  if (value === 2 || value === 'Video' || value === 'video') return 'video'
  return 'image'
}

export function normalizeUserStoryItem(data: unknown): UserStoryDraft {
  const record = readRecord(data) ?? {}

  const mediaPath = readStringField(record, 'mediaUrl', 'MediaUrl', 'mediaPath', 'MediaPath')
  const posterUrl = readStringField(record, 'posterUrl', 'PosterUrl', 'posterPath', 'PosterPath')
  const productSlug = readStringField(record, 'productSlug', 'ProductSlug')
  const createdOnUtc = readStringField(record, 'createdOnUtc', 'CreatedOnUtc')
  const ownerFirstName = readStringField(record, 'ownerFirstName', 'OwnerFirstName')
  const ownerLastName = readStringField(record, 'ownerLastName', 'OwnerLastName')
  const ownerName = `${ownerFirstName} ${ownerLastName}`.trim()

  return {
    id: readStringField(record, 'id', 'Id'),
    userId: readStringField(record, 'userId', 'UserId') || undefined,
    title: readStringField(record, 'title', 'Title'),
    caption: readStringField(record, 'caption', 'Caption'),
    mediaType: readMediaType(record.mediaType ?? record.MediaType),
    mediaPath,
    mediaSrc: resolveMediaUrl(mediaPath),
    mediaAlt: readStringField(record, 'mediaAlt', 'MediaAlt'),
    posterSrc: posterUrl ? resolveMediaUrl(posterUrl) : undefined,
    productSlugs: productSlug ? [productSlug] : [],
    createdAt: createdOnUtc || new Date().toISOString(),
    isActive: Boolean(record.isActive ?? record.IsActive ?? true),
    ownerName: ownerName || undefined,
    likeCount: Number(record.likeCount ?? record.LikeCount ?? 0),
    commentCount: Number(record.commentCount ?? record.CommentCount ?? 0),
    isLikedByCurrentUser: Boolean(
      record.isLikedByCurrentUser ?? record.IsLikedByCurrentUser ?? false,
    ),
  }
}

export const userStoryService = {
  async getMy(accessToken: string, locale: Locale): Promise<UserStoryDraft[]> {
    const result = await apiGet<{ items?: unknown[]; Items?: unknown[] }>(
      USER_STORY_MY_PATH,
      locale,
      accessToken,
    )
    const items = result.items ?? result.Items ?? []
    return items.map(normalizeUserStoryItem).filter((story) => story.id)
  },

  async searchActive(locale: Locale, limit = 40, accessToken?: string | null): Promise<UserStoryDraft[]> {
    const result = await apiPost<{ items?: unknown[]; Items?: unknown[] }>(
      USER_STORY_SEARCH_ACTIVE_PATH,
      { limit },
      { locale, accessToken: accessToken ?? undefined },
    )
    const items = result.items ?? result.Items ?? []
    return items.map(normalizeUserStoryItem).filter((story) => story.id)
  },

  async uploadMedia(accessToken: string, locale: Locale, file: File) {
    const headers: Record<string, string> = {
      Accept: 'application/json',
      'Accept-Language': toAcceptLanguage(locale),
      Authorization: `Bearer ${accessToken}`,
    }

    const formData = new FormData()
    formData.append('file', file)

    const response = await fetch(`${API_BASE_URL}${USER_STORY_UPLOAD_PATH}`, {
      method: 'POST',
      headers,
      body: formData,
    })

    const responseBody = await response.json()
    const { isSuccess, data, messages } = normalizeApiEnvelope<{
      mediaPath?: string
      MediaPath?: string
      mediaType?: number | string
      MediaType?: number | string
    }>(responseBody)

    if (!response.ok || !isSuccess || !data) {
      throw new ApiError(response.status, messages)
    }

    const record = readRecord(data) ?? data
    const mediaPath = readStringField(record as Record<string, unknown>, 'mediaPath', 'MediaPath')

    return {
      mediaPath,
      mediaType: readMediaType(
        (record as Record<string, unknown>).mediaType ??
          (record as Record<string, unknown>).MediaType,
      ),
      mediaSrc: resolveMediaUrl(mediaPath),
    }
  },

  async create(accessToken: string, locale: Locale, input: UserStoryInput) {
    const payload = {
      title: input.title.trim(),
      caption: input.caption.trim() || null,
      mediaType: input.mediaType === 'video' ? 2 : 1,
      mediaPath: input.mediaPath,
      mediaAlt: input.mediaAlt.trim(),
      posterPath: input.posterPath ?? null,
      productSlug: input.productSlug?.trim() || null,
    }

    await apiPost(USER_STORY_CREATE_PATH, payload, { locale, accessToken })
  },

  async update(accessToken: string, locale: Locale, id: string, input: UserStoryInput) {
    const payload = {
      id,
      title: input.title.trim(),
      caption: input.caption.trim() || null,
      isActive: input.isActive ?? true,
      productSlug: input.productSlug?.trim() || null,
      mediaType: input.mediaType === 'video' ? 2 : 1,
      mediaPath: input.mediaPath,
      mediaAlt: input.mediaAlt.trim(),
      posterPath: input.posterPath ?? null,
    }

    await apiPut(USER_STORY_UPDATE_PATH, payload, { locale, accessToken })
  },

  async delete(accessToken: string, id: string) {
    await apiDelete(USER_STORY_DELETE_PATH, accessToken, { id })
  },
}
