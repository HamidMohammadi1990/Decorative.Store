import { apiGetAuth } from '@/services/api/apiClient'

const MY_PERMISSIONS_PATH = '/api/v1/account/permissions'

export interface MyPermissionsResponse {
  permissions: string[]
}

function normalizePermissionsResponse(data: unknown): string[] {
  if (!data || typeof data !== 'object') return []

  const record = data as Record<string, unknown>
  const raw = record.permissions ?? record.Permissions

  if (!Array.isArray(raw)) return []

  return raw.filter((item): item is string => typeof item === 'string' && item.length > 0)
}

export const permissionService = {
  async getMyPermissions(accessToken: string): Promise<string[]> {
    const data = await apiGetAuth<MyPermissionsResponse>(MY_PERMISSIONS_PATH, accessToken)
    return normalizePermissionsResponse(data)
  },
}
