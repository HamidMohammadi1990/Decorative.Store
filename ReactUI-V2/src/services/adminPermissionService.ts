import type { AdminPermission } from '@/models/admin/permission.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { readBooleanField, readNumberField, readRecord, readStringField } from '@/services/api/apiNormalize'
import {
  fetchAllAdminPages,
  normalizeAdminPaged,
  paginationBody,
  readEncryptedId,
  readOptionalEncryptedId,
  type AdminPagedResult,
} from '@/services/admin/adminCatalogNormalize'

const BASE = '/api/v1/admin/permission'

function normalizeAdminPermission(data: unknown): AdminPermission | null {
  const record = readRecord(data)
  if (!record) return null

  const id = readEncryptedId(record, 'id', 'Id')
  const title = readStringField(record, 'title', 'Title')
  if (!id || !title) return null

  return {
    id,
    title,
    url: readStringField(record, 'url', 'Url'),
    nameSpace: readStringField(record, 'nameSpace', 'NameSpace') || null,
    parentId: readOptionalEncryptedId(record, 'parentId', 'ParentId'),
    levelTypeId: readNumberField(record, 'levelTypeId', 'LevelTypeId'),
    levelTypeTitle: readStringField(record, 'levelTypeTitle', 'LevelTypeTitle'),
    priority: readNumberField(record, 'priority', 'Priority'),
    isActive: readBooleanField(record, 'isActive', 'IsActive'),
  }
}

export const adminPermissionService = {
  async getAll(
    accessToken: string,
    locale: Locale,
    options: {
      pageNumber?: number
      pageSize?: number
      title?: string | null
      isActive?: boolean | null
    } = {},
  ): Promise<AdminPagedResult<AdminPermission>> {
    const data = await apiPost<unknown>(
      `${BASE}/get-all`,
      {
        title: options.title ?? null,
        url: null,
        nameSpace: null,
        parentId: null,
        levelTypeId: null,
        isActive: options.isActive ?? true,
        pagination: paginationBody(options.pageNumber ?? 1, options.pageSize ?? 200),
      },
      { locale, accessToken },
    )

    return normalizeAdminPaged(data, normalizeAdminPermission)
  },

  async getAllPages(
    accessToken: string,
    locale: Locale,
    options: {
      pageSize?: number
      title?: string | null
      isActive?: boolean | null
    } = {},
  ): Promise<AdminPermission[]> {
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
}
