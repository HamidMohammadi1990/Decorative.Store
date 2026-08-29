import type { ProductQuestion } from '@/models/catalog/productQuestion.model'
import type { PagedResult } from '@/models/shared/paged.model'
import {
  readNumberField,
  readOptionalStringField,
  readRecord,
  readStringField,
} from '@/services/api/apiNormalize'
import { readEncryptedId } from '@/services/admin/adminCatalogNormalize'

function normalizeProductQuestion(data: unknown): ProductQuestion | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const productId = readEncryptedId(record, 'productId', 'ProductId')
  const userId = readEncryptedId(record, 'userId', 'UserId')
  if (!id || !productId || !userId) return null

  const question = readStringField(record, 'question', 'Question')
  if (!question) return null

  return {
    id,
    productId,
    userId,
    question,
    answer: readOptionalStringField(record, 'answer', 'Answer') ?? undefined,
    createdOnUtc:
      readOptionalStringField(record, 'createdOnUtc', 'CreatedOnUtc') ??
      readOptionalStringField(record, 'createdOn', 'CreatedOn') ??
      '',
    userName: readStringField(record, 'userName', 'UserName'),
    userFirstName: readOptionalStringField(record, 'userFirstName', 'UserFirstName') ?? undefined,
    userLastName: readOptionalStringField(record, 'userLastName', 'UserLastName') ?? undefined,
    answeredByFirstName:
      readOptionalStringField(record, 'answeredByFirstName', 'AnsweredByFirstName') ?? undefined,
    answeredByLastName:
      readOptionalStringField(record, 'answeredByLastName', 'AnsweredByLastName') ?? undefined,
    answeredByUserName:
      readOptionalStringField(record, 'answeredByUserName', 'AnsweredByUserName') ?? undefined,
  }
}

export function normalizeProductQuestionPaged(data: unknown): PagedResult<ProductQuestion> {
  const record = readRecord(data) ?? {}
  const rawItems = record.items ?? record.Items
  const items = Array.isArray(rawItems)
    ? rawItems.map(normalizeProductQuestion).filter((item): item is ProductQuestion => item !== null)
    : []

  return {
    items,
    pageNumber: readNumberField(record, 'pageNumber', 'PageNumber') || 1,
    pageSize: readNumberField(record, 'pageSize', 'PageSize') || 20,
    totalCount: readNumberField(record, 'totalCount', 'TotalCount'),
    totalPages: readNumberField(record, 'totalPages', 'TotalPages') || 1,
  }
}
