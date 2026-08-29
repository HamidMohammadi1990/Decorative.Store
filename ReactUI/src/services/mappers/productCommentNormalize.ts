import type { ProductComment } from '@/models/catalog/productComment.model'
import type { PagedResult } from '@/models/shared/paged.model'
import {
  readBooleanField,
  readNumberField,
  readOptionalStringField,
  readRecord,
  readStringField,
} from '@/services/api/apiNormalize'
import {
  readEncryptedId,
  readOptionalEncryptedId,
} from '@/services/admin/adminCatalogNormalize'

function normalizeProductComment(data: unknown): ProductComment | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const productId = readEncryptedId(record, 'productId', 'ProductId')
  const userId = readEncryptedId(record, 'userId', 'UserId')
  const commentTopicId = readEncryptedId(record, 'commentTopicId', 'CommentTopicId')
  if (!id || !productId || !userId || !commentTopicId) return null

  const parentId = readOptionalEncryptedId(record, 'parentId', 'ParentId')
  const description = readStringField(record, 'description', 'Description')

  return {
    id,
    parentId,
    productId,
    userId,
    description,
    commentRate: readNumberField(record, 'commentRate', 'CommentRate'),
    qualityRating: readNumberField(record, 'qualityRating', 'QualityRating'),
    affordableRating: readNumberField(record, 'affordableRating', 'AffordableRating'),
    commentTopicId,
    commentTopicTitle: readStringField(record, 'commentTopicTitle', 'CommentTopicTitle'),
    userName: readStringField(record, 'userName', 'UserName'),
    userFirstName: readOptionalStringField(record, 'userFirstName', 'UserFirstName') ?? undefined,
    userLastName: readOptionalStringField(record, 'userLastName', 'UserLastName') ?? undefined,
    createdOnUtc:
      readOptionalStringField(record, 'createdOnUtc', 'CreatedOnUtc') ??
      readOptionalStringField(record, 'createdOn', 'CreatedOn') ??
      undefined,
    isBuyer: readBooleanField(record, 'isBuyer', 'IsBuyer'),
    helpfulCount: readNumberField(record, 'helpfulCount', 'HelpfulCount'),
    notHelpfulCount: readNumberField(record, 'notHelpfulCount', 'NotHelpfulCount'),
  }
}

export function normalizeProductCommentPaged(data: unknown): PagedResult<ProductComment> {
  const record = readRecord(data) ?? {}
  const rawItems = record.items ?? record.Items
  const items = Array.isArray(rawItems)
    ? rawItems.map(normalizeProductComment).filter((item): item is ProductComment => item !== null)
    : []

  return {
    items,
    pageNumber: readNumberField(record, 'pageNumber', 'PageNumber') || 1,
    pageSize: readNumberField(record, 'pageSize', 'PageSize') || 20,
    totalCount: readNumberField(record, 'totalCount', 'TotalCount'),
    totalPages: readNumberField(record, 'totalPages', 'TotalPages') || 1,
  }
}

export function normalizeVoteProductCommentResult(data: unknown) {
  const record = readRecord(data) ?? {}
  const helpfulCount = readNumberField(record, 'helpfulCount', 'HelpfulCount')
  const notHelpfulCount = readNumberField(record, 'notHelpfulCount', 'NotHelpfulCount')
  const userVoteHelpfulRaw = record.userVoteHelpful ?? record.UserVoteHelpful

  return {
    helpfulCount,
    notHelpfulCount,
    userVoteHelpful:
      typeof userVoteHelpfulRaw === 'boolean' ? userVoteHelpfulRaw : null,
  }
}
