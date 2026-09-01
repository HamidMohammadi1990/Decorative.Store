import type {
  AdminDiscount,
  CreateAdminDiscountInput,
  UpdateAdminDiscountInput,
} from '@/models/admin/discount.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import {
  readNumberField,
  readRecord,
  readStringField,
} from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/discount'

function normalizeAdminDiscount(data: unknown): AdminDiscount | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const code = readStringField(record, 'code', 'Code')
  if (!id || !code) return null

  const minimumRaw = record.minimumAmount ?? record.MinimumAmount
  const minimumAmount =
    minimumRaw == null || minimumRaw === ''
      ? null
      : readNumberField(record, 'minimumAmount', 'MinimumAmount')

  return {
    id,
    code,
    percentage: readNumberField(record, 'percentage', 'Percentage'),
    amount: readNumberField(record, 'amount', 'Amount'),
    expiryDateOnUtc: readStringField(record, 'expiryDateOnUtc', 'ExpiryDateOnUtc') || null,
    maxDiscountAmount: readNumberField(record, 'maxDiscountAmount', 'MaxDiscountAmount'),
    usageLimit: readNumberField(record, 'usageLimit', 'UsageLimit'),
    remainingUses: readNumberField(record, 'remainingUses', 'RemainingUses'),
    minimumAmount,
    isActive: readIsActive(record),
  }
}

export const adminDiscountService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      code?: string | null
      isActive?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminDiscount>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        code: options.code ?? null,
        productId: null,
        userId: null,
        isActive: options.isActive ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 20),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeAdminDiscount)
  },

  async create(
    accessToken: string,
    locale: Locale,
    input: CreateAdminDiscountInput,
  ): Promise<string> {
    const data = await apiPost<unknown>(
      `${BASE}/create`,
      {
        ...input,
        userId: input.userId ?? null,
        productId: input.productId ?? null,
        subCategoryId: input.subCategoryId ?? null,
        fromCirculationOrMeterOrCount: null,
        toCirculationOrMeterOrCount: null,
        isCooperation: input.isCooperation ?? false,
      },
      { locale, accessToken },
    )
    const record = readRecord(data)
    return readEncryptedId(record ?? {}, 'id', 'Id')
  },

  async update(
    accessToken: string,
    locale: Locale,
    input: UpdateAdminDiscountInput,
  ): Promise<void> {
    await apiPut(
      `${BASE}/update`,
      {
        ...input,
        userId: input.userId ?? null,
        productId: input.productId ?? null,
        subCategoryId: input.subCategoryId ?? null,
        fromCirculationOrMeterOrCount: null,
        toCirculationOrMeterOrCount: null,
        isCooperation: input.isCooperation ?? false,
      },
      { locale, accessToken },
    )
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
