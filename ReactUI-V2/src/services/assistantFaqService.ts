import type {
  AdminAssistantFaq,
  AssistantFaqItem,
  CreateAdminAssistantFaqInput,
  UpdateAdminAssistantFaqInput,
} from '@/models/admin/assistantFaq.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import { readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const ADMIN_BASE = '/api/v1/admin/assistant-faq'
const PUBLIC_BASE = '/api/v1/assistant-faq'

function normalizeAdminAssistantFaq(data: unknown): AdminAssistantFaq | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const question = readStringField(record, 'question', 'Question')
  const answer = readStringField(record, 'answer', 'Answer')
  if (!id || !question || !answer) return null

  return {
    id,
    languageId: readNumberField(record, 'languageId', 'LanguageId'),
    question,
    answer,
    priority: readNumberField(record, 'priority', 'Priority'),
    isActive: readIsActive(record),
  }
}

function normalizeAssistantFaqItem(data: unknown): AssistantFaqItem | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const question = readStringField(record, 'question', 'Question')
  const answer = readStringField(record, 'answer', 'Answer')
  if (!id || !question || !answer) return null

  return {
    id,
    question,
    answer,
    priority: readNumberField(record, 'priority', 'Priority'),
  }
}

export const adminAssistantFaqService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      languageId?: number
      question?: string | null
      isActive?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminAssistantFaq>> {
    const data = await apiPost<unknown>(
      `${ADMIN_BASE}/get-all`,
      {
        languageId: options.languageId ?? null,
        question: options.question ?? null,
        isActive: options.isActive ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 50),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeAdminAssistantFaq)
  },

  async create(
    accessToken: string,
    locale: Locale,
    input: CreateAdminAssistantFaqInput,
  ): Promise<string> {
    const data = await apiPost<unknown>(`${ADMIN_BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readEncryptedId(record ?? {}, 'id', 'Id')
  },

  async update(
    accessToken: string,
    locale: Locale,
    input: UpdateAdminAssistantFaqInput,
  ): Promise<void> {
    await apiPut(`${ADMIN_BASE}/update`, input, { locale, accessToken })
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${ADMIN_BASE}/delete`, accessToken, { id })
  },
}

export const assistantFaqService = {
  async search(locale: Locale, languageId: number): Promise<AssistantFaqItem[]> {
    const data = await apiPost<unknown>(`${PUBLIC_BASE}/search`, {
      languageId,
      isActive: true,
      pagination: paginationBody(1, 50),
    }, { locale })

    const result = normalizeAdminPaged(data, normalizeAssistantFaqItem)
    return result.items.sort((a, b) => a.priority - b.priority || a.question.localeCompare(b.question))
  },
}
