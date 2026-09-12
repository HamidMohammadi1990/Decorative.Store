import { create } from 'zustand'
import { permissionService } from '@/services/permissionService'

interface PermissionState {
  permissions: Set<string>
  loaded: boolean
  loading: boolean
  mockAllGranted: boolean
  loadPermissions: (accessToken: string) => Promise<void>
  ensureLoaded: (accessToken: string) => Promise<void>
  reload: () => Promise<void>
  clear: () => void
  hasPermission: (code: string | undefined) => boolean
}

const initialState = {
  permissions: new Set<string>(),
  loaded: false,
  loading: false,
  mockAllGranted: false,
}

export const usePermissionStore = create<PermissionState>()((set, get) => ({
  ...initialState,

  clear: () => {
    set({ ...initialState, permissions: new Set<string>() })
  },

  hasPermission: (code) => {
    if (!code) return true
    const { mockAllGranted, permissions, loaded } = get()
    if (mockAllGranted) return true
    if (!loaded) return false
    return permissions.has(code)
  },

  loadPermissions: async (accessToken) => {
    if (!accessToken) {
      get().clear()
      return
    }

    if (accessToken === 'mock-access-token') {
      set({
        permissions: new Set<string>(),
        loaded: true,
        loading: false,
        mockAllGranted: true,
      })
      return
    }

    set({ loading: true, mockAllGranted: false })

    try {
      const codes = await permissionService.getMyPermissions(accessToken)
      set({
        permissions: new Set(codes),
        loaded: true,
        loading: false,
        mockAllGranted: false,
      })
    } catch {
      set({
        permissions: new Set<string>(),
        loaded: true,
        loading: false,
        mockAllGranted: false,
      })
    }
  },

  ensureLoaded: async (accessToken) => {
    const { loaded, loading } = get()
    if (loaded || loading) return
    await get().loadPermissions(accessToken)
  },

  reload: async () => {
    const { useUserStore } = await import('@/stores/userStore')
    const accessToken = useUserStore.getState().accessToken
    if (!accessToken) {
      get().clear()
      return
    }

    set({ loaded: false })
    await get().loadPermissions(accessToken)
  },
}))
