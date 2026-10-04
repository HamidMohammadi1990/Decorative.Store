import type {
  AdminRole,
  CreateAdminRoleInput,
  UpdateAdminRoleInput,
} from '@/models/admin/role.model'
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

const BASE = '/api/v1/admin/role'

function normalizeAdminRole(data: unknown): AdminRole | null {
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

export const adminRoleService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: { pageNumber?: number; pageSize?: number; title?: string | null; isActive?: boolean | null } = {},
  ): Promise<AdminPagedResult<AdminRole>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: options.title ?? null,
        isActive: options.isActive ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 20),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeAdminRole)
  },

  async create(accessToken: string, locale: Locale, input: CreateAdminRoleInput): Promise<string> {
    const data = await apiPost<unknown>(`${BASE}/create`, input, { locale, accessToken })
    const record = readRecord(data)
    return readEncryptedId(record ?? {}, 'id', 'Id')
  },

  async update(accessToken: string, locale: Locale, input: UpdateAdminRoleInput): Promise<void> {
    await apiPut(`${BASE}/update`, input, { locale, accessToken })
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
