import type { AdminUserRole, CreateAdminUserRoleInput } from '@/models/admin/userRole.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/user-role'

function normalizeAdminUserRole(data: unknown): AdminUserRole | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const userId = readEncryptedId(record, 'userId', 'UserId')
  const roleId = readEncryptedId(record, 'roleId', 'RoleId')
  if (!id || !userId || !roleId) return null

  return {
    id,
    userId,
    userName: readStringField(record, 'userName', 'UserName'),
    roleId,
    roleTitle: readStringField(record, 'roleTitle', 'RoleTitle'),
  }
}

export const adminUserRoleService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      userId?: string | null
      roleId?: string | null
    } = {},
  ): Promise<AdminPagedResult<AdminUserRole>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        userId: options.userId ?? null,
        roleId: options.roleId ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 100),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeAdminUserRole)
  },

  async create(
    accessToken: string,
    locale: Locale,
    input: CreateAdminUserRoleInput,
  ): Promise<string> {
    const data = await apiPost<unknown>(
      `${BASE}/create`,
      { userId: input.userId, roleId: input.roleId },
      { locale, accessToken },
    )
    const record = readRecord(data)
    return readEncryptedId(record ?? {}, 'id', 'Id')
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
