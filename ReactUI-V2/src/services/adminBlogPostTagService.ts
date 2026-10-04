import type {
  AdminBlogPostTag,
  CreateBlogPostTagInput,
  UpdateBlogPostTagInput,
} from '@/models/admin/blog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/blog-post-tag'

function normalizeBlogPostTag(data: unknown): AdminBlogPostTag | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const tagId = readEncryptedId(record, 'tagId', 'TagId')
  const blogPostId = readEncryptedId(record, 'blogPostId', 'BlogPostId')
  if (!id || !tagId || !blogPostId) return null

  return {
    id,
    tagId,
    tagTitle: readStringField(record, 'tagTitle', 'TagTitle'),
    blogPostId,
    blogPostTitle: readStringField(record, 'blogPostTitle', 'BlogPostTitle'),
  }
}

export const adminBlogPostTagService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      blogPostId?: string | null
      tagId?: string | null
    } = {},
  ): Promise<AdminPagedResult<AdminBlogPostTag>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        blogPostId: options.blogPostId ?? null,
        tagId: options.tagId ?? null,
        tagTitle: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeBlogPostTag)
  },

  async create(accessToken: string, locale: Locale, input: CreateBlogPostTagInput): Promise<string> {
    const data = await apiPost<unknown>(
      `${BASE}/create`,
      {
        tagId: input.tagId,
        blogPostId: input.blogPostId,
      },
      { locale, accessToken },
    )
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateBlogPostTagInput): Promise<void> {
    await apiPut(
      `${BASE}/update`,
      {
        id: input.id,
        tagId: input.tagId,
        blogPostId: input.blogPostId,
      },
      { locale, accessToken },
    )
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
