import type { AdminProductComment } from '@/models/admin/catalog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/product-comment'

function readAuthorName(record: Record<string, unknown>): string {
  const userName = readStringField(record, 'userName', 'UserName')
  if (userName) return userName
  const first = readStringField(record, 'userFirstName', 'UserFirstName')
  const last = readStringField(record, 'userLastName', 'UserLastName')
  return [first, last].filter(Boolean).join(' ').trim()
}

function normalizeProductComment(data: unknown): AdminProductComment | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const productId = readEncryptedId(record, 'productId', 'ProductId')
  const userId = readEncryptedId(record, 'userId', 'UserId')
  const commentTopicId = readEncryptedId(record, 'commentTopicId', 'CommentTopicId')
  if (!id || !productId || !userId || !commentTopicId) return null

  const description = readStringField(record, 'description', 'Description')

  return {
    id,
    productId,
    productTitle: readStringField(record, 'productTitle', 'ProductTitle'),
    userId,
    authorName: readAuthorName(record),
    commentTopicId,
    commentTopicTitle: readStringField(record, 'commentTopicTitle', 'CommentTopicTitle'),
    commentRate: readNumberField(record, 'commentRate', 'CommentRate'),
    qualityRating: readNumberField(record, 'qualityRating', 'QualityRating'),
    affordableRating: readNumberField(record, 'affordableRating', 'AffordableRating'),
    description,
    isActive: readIsActive(record),
  }
}

export const adminProductCommentService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      productId?: string | null
      isActive?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminProductComment>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        productId: options.productId ?? null,
        userId: null,
        commentTopicId: null,
        isActive: options.isActive ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeProductComment)
  },

  async changeStatus(
    accessToken: string,
    locale: Locale,
    id: string,
    isActive: boolean,
  ): Promise<void> {
    await apiPost(`${BASE}/change-status`, { id, isActive }, { locale, accessToken })
  },
}
