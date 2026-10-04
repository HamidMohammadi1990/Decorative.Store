import type { AdminBlogPostComment } from '@/models/admin/blog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { readBooleanField, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readOptionalEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/blog-post-comment'

function normalizeBlogPostComment(data: unknown): AdminBlogPostComment | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const blogPostId = readEncryptedId(record, 'blogPostId', 'BlogPostId')
  const content = readStringField(record, 'content', 'Content')
  if (!id || !blogPostId || !content) return null

  const first = readStringField(record, 'createdByUserFirstName', 'CreatedByUserFirstName')
  const last = readStringField(record, 'createdByUserLastName', 'CreatedByUserLastName')

  return {
    id,
    parentId: readOptionalEncryptedId(record, 'parentId', 'ParentId'),
    content,
    blogPostId,
    blogPostTitle: readStringField(record, 'blogPostTitle', 'BlogPostTitle'),
    authorName: [first, last].filter(Boolean).join(' ').trim(),
    createdOnUtc: readStringField(record, 'createdOnUtc', 'CreatedOnUtc'),
    approvedOnUtc: readStringField(record, 'approvedOnUtc', 'ApprovedOnUtc') || null,
    isApproved: readBooleanField(record, 'isApproved', 'IsApproved'),
  }
}

export const adminBlogPostCommentService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      blogPostId?: string | null
      isApproved?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminBlogPostComment>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        blogPostId: options.blogPostId ?? null,
        isApproved: options.isApproved ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeBlogPostComment)
  },

  async approve(accessToken: string, locale: Locale, id: string): Promise<void> {
    await apiPost(`${BASE}/approve`, { id }, { locale, accessToken })
  },
}
