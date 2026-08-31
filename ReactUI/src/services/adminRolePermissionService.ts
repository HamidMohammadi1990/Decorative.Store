import type {
  AdminRolePermission,
  CreateAdminRolePermissionInput,
} from '@/models/admin/rolePermission.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost } from '@/services/api/apiClient'
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  fetchAllAdminPages,
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/role-permission'

function normalizeAdminRolePermission(data: unknown): AdminRolePermission | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const roleId = readEncryptedId(record, 'roleId', 'RoleId')
  const permissionId = readEncryptedId(record, 'permissionId', 'PermissionId')
  if (!id || !roleId || !permissionId) return null

  return {
    id,
    roleId,
    roleTitle: readStringField(record, 'roleTitle', 'RoleTitle'),
    permissionId,
    permissionTitle: readStringField(record, 'permissionTitle', 'PermissionTitle'),
  }
}

export const adminRolePermissionService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      roleId?: string | null
      permissionId?: string | null
    } = {},
  ): Promise<AdminPagedResult<AdminRolePermission>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        roleId: options.roleId ?? null,
        permissionId: options.permissionId ?? null,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 200),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeAdminRolePermission)
  },

  async getAllPages(
    accessToken: string,
    locale: Locale,
    options: {
      pageSize?: number
      roleId?: string | null
      permissionId?: string | null
    } = {},
  ): Promise<AdminRolePermission[]> {
    const pageSize = options.pageSize ?? 100
    return fetchAllAdminPages(
      (pageNumber, size) =>
        this.getAll(accessToken, locale, {
          ...options,
          pageNumber,
          pageSize: size,
        }),
      pageSize,
    )
  },

  async create(
    accessToken: string,
    locale: Locale,
    input: CreateAdminRolePermissionInput,
  ): Promise<string> {
    const data = await apiPost<unknown>(
      `${BASE}/create`,
      { roleId: input.roleId, permissionId: input.permissionId },
      { locale, accessToken },
    )
    const record = readRecord(data)
    return readEncryptedId(record ?? {}, 'id', 'Id')
  },

  async delete(accessToken: string, id: string): Promise<void> {
    await apiDelete(`${BASE}/delete`, accessToken, { id })
  },
}
