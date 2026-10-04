import type { AdminTag, CreateTagInput, UpdateTagInput } from '@/models/admin/blog.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readIsActive,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/tag'

function normalizeTag(data: unknown): AdminTag | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const title = readStringField(record, 'title', 'Title')
  if (!id || !title) return null

  return {
    id,
    title,
    isActive: readIsActive(record),
  }
}

export const adminTagService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: { pageNumber?: number; pageSize?: number } = {},
  ): Promise<AdminPagedResult<AdminTag>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: null,
        isActive: null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeTag)
  },

  async create(accessToken: string, locale: Locale, input: CreateTagInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readStringField(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateTagInput): Promise<void> {
    await apiPut(
      `${BASE}/update`,
      {
        id: input.id,
        title: input.title,
        isActive: input.isActive,
      },
      { locale, accessToken },
    )
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
