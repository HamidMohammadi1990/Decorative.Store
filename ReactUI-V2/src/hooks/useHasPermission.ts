import { usePermissionStore } from '@/stores/permissionStore'

export function useHasPermission(code: string | undefined): boolean {
  const mockAllGranted = usePermissionStore((s) => s.mockAllGranted)
  const loaded = usePermissionStore((s) => s.loaded)
  const permissions = usePermissionStore((s) => s.permissions)

  if (!code) return true
  if (mockAllGranted) return true
  if (!loaded) return false
  return permissions.has(code)
}

export function usePermissionsReady(): { loaded: boolean; loading: boolean } {
  const loaded = usePermissionStore((s) => s.loaded)
  const loading = usePermissionStore((s) => s.loading)
  return { loaded, loading }
}
