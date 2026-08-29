import type { AdminProductQuestion } from '@/models/admin/catalog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { readOptionalStringField, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/product-question'

function readPersonName(record: Record<string, unknown>, prefix = ''): string {
  const pascalPrefix = prefix ? `${prefix}` : ''
  const camelPrefix = prefix
    ? `${prefix.charAt(0).toLowerCase()}${prefix.slice(1)}`
    : ''

  const userName = readStringField(
    record,
    `${camelPrefix}UserName`,
    `${pascalPrefix}UserName`,
  )
  if (userName) return userName

  const first = readStringField(
    record,
    `${camelPrefix}FirstName`,
    `${pascalPrefix}FirstName`,
  )
  const last = readStringField(record, `${camelPrefix}LastName`, `${pascalPrefix}LastName`)
  return [first, last].filter(Boolean).join(' ').trim()
}

function normalizeProductQuestion(data: unknown): AdminProductQuestion | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const productId = readEncryptedId(record, 'productId', 'ProductId')
  const userId = readEncryptedId(record, 'userId', 'UserId')
  const question = readStringField(record, 'question', 'Question')
  if (!id || !productId || !userId || !question) return null

  const answer = readOptionalStringField(record, 'answer', 'Answer')

  return {
    id,
    productId,
    productTitle: readStringField(record, 'productTitle', 'ProductTitle'),
    userId,
    askerName: readPersonName(record),
    question,
    answer: answer || null,
    createdOnUtc: readOptionalStringField(record, 'createdOnUtc', 'CreatedOnUtc') ?? undefined,
    isActive: readIsActive(record),
    answeredByName: readPersonName(record, 'answeredBy') || null,
  }
}

export const adminProductQuestionService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      productId?: string | null
      isActive?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminProductQuestion>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        productId: options.productId ?? null,
        userId: null,
        isActive: options.isActive ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeProductQuestion)
  },

  async changeStatus(
    accessToken: string,
    locale: Locale,
    id: string,
    isActive: boolean,
  ): Promise<void> {
    await apiPost(`${BASE}/change-status`, { id, isActive }, { locale, accessToken })
  },

  async answer(
    accessToken: string,
    locale: Locale,
    id: string,
    answer: string,
  ): Promise<void> {
    await apiPost(`${BASE}/answer`, { id, answer }, { locale, accessToken })
  },
}
