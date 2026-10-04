import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { readBooleanField, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/user-story-comment'

export interface AdminUserStoryComment {
  id: string
  userStoryId: string
  userStoryTitle: string
  content: string
  authorName: string
  createdOnUtc: string
  approvedOnUtc: string | null
  isApproved: boolean
}

function normalizeAdminUserStoryComment(data: unknown): AdminUserStoryComment | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const userStoryId = readEncryptedId(record, 'userStoryId', 'UserStoryId')
  const content = readStringField(record, 'content', 'Content')
  if (!id || !userStoryId || !content) return null

  const first = readStringField(record, 'userFirstName', 'UserFirstName')
  const last = readStringField(record, 'userLastName', 'UserLastName')

  return {
    id,
    userStoryId,
    userStoryTitle: readStringField(record, 'userStoryTitle', 'UserStoryTitle'),
    content,
    authorName: [first, last].filter(Boolean).join(' ').trim(),
    createdOnUtc: readStringField(record, 'createdOnUtc', 'CreatedOnUtc'),
    approvedOnUtc: readStringField(record, 'approvedOnUtc', 'ApprovedOnUtc') || null,
    isApproved: readBooleanField(record, 'isApproved', 'IsApproved'),
  }
}

export const adminUserStoryCommentService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      userStoryId?: string | null
      isApproved?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminUserStoryComment>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        userStoryId: options.userStoryId ?? null,
        isApproved: options.isApproved ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeAdminUserStoryComment)
  },

  async approve(accessToken: string, locale: Locale, id: string): Promise<void> {
    await apiPost(`${BASE}/approve`, { id }, { locale, accessToken })
  },
}
