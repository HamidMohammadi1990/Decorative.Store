import { API_BASE_URL } from '@/config/api'
import type { AdminBlogPostFile, CreateBlogPostFileInput } from '@/models/admin/blog.model'
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

const BASE = '/api/v1/admin/blog-post-file'

export function resolveBlogPostFileUrl(fileName: string): string {
  if (!fileName) return ''
  if (fileName.startsWith('http://') || fileName.startsWith('https://')) return fileName
  const path = fileName.startsWith('/')
    ? fileName
    : `/Uploads/BlogPosts/${fileName.replace(/^\/+/, '')}`
  return `${API_BASE_URL}${path}`
}

function normalizeBlogPostFile(data: unknown): AdminBlogPostFile | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const blogPostId = readEncryptedId(record, 'blogPostId', 'BlogPostId')
  const fileName = readStringField(record, 'fileName', 'FileName')
  if (!id || !blogPostId) return null

  const imageUrl =
    readStringField(record, 'imageUrl', 'ImageUrl') || resolveBlogPostFileUrl(fileName)

  return {
    id,
    blogPostId,
    blogPostTitle: readStringField(record, 'blogPostTitle', 'BlogPostTitle'),
    title: readStringField(record, 'title', 'Title'),
    fileName,
    imageUrl,
    isActive: readIsActive(record),
    isMain: Boolean(record.isMain ?? record.IsMain),
  }
}

export const adminBlogPostFileService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      blogPostId?: string | null
      pageNumber?: number
      pageSize?: number
    } = {},
  ): Promise<AdminPagedResult<AdminBlogPostFile>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: null,
        blogPostId: options.blogPostId ?? null,
        isActive: null,
        isMain: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeBlogPostFile)
  },

  async createRange(
    accessToken: string,
    locale: Locale,
    files: CreateBlogPostFileInput[],
  ): Promise<AdminBlogPostFile[]> {
    const formData = new FormData()

    files.forEach((file, index) => {
      formData.append(`Files[${index}].BlogPostId`, file.blogPostId)
      formData.append(`Files[${index}].LanguageId`, String(file.languageId))
      formData.append(`Files[${index}].Title`, file.title)
      formData.append(`Files[${index}].IsIndex`, String(file.isIndex))
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

    return data
      .map((item) => {
        const record = readRecord(item)
        if (!record) return null
        const id = readEncryptedId(record, 'id', 'Id')
        const imageUrl = readStringField(record, 'imageUrl', 'ImageUrl')
        const title = readStringField(record, 'title', 'Title')
        if (!id || !imageUrl) return null
        return {
          id,
          blogPostId: files[0]?.blogPostId ?? '',
          blogPostTitle: '',
          title,
          fileName: '',
          imageUrl,
          isActive: true,
          isMain: files.some((f) => f.isIndex),
        } satisfies AdminBlogPostFile
      })
      .filter((item): item is AdminBlogPostFile => item !== null)
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
