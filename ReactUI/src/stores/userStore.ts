import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { DashboardUser } from '@/models/dashboard/dashboard.model'
import {
  authService,
  mapCurrentUserToDashboardUser,
} from '@/services/authService'
import { useCartStore } from '@/stores/cartStore'

interface LoginInput {
  userName: string
  password: string
  firstName?: string
  lastName?: string
}

interface UserState {
  user: DashboardUser | null
  accessToken: string | null
  refreshToken: string | null
  tokenExpiresAt: number | null
  authLoading: boolean
  authError: string | null
  login: (input: LoginInput) => Promise<void>
  loginDemo: (input: Omit<LoginInput, 'password'>) => void
  logout: () => Promise<void>
  restoreSession: () => Promise<boolean>
  clearAuthError: () => void
}

function createDemoUser(input: Omit<LoginInput, 'password'>): DashboardUser {
  const userName = input.userName.trim()
  const firstName = input.firstName?.trim() || userName.split('@')[0] || userName
  const lastName = input.lastName?.trim() || ''

  return {
    id: `user-${Date.now().toString(36)}`,
    firstName,
    lastName,
    email: userName,
    memberSince: new Date().toISOString().slice(0, 10),
  }
}

const useMockAuth = import.meta.env.VITE_AUTH_USE_MOCK === 'true'

export const useUserStore = create<UserState>()(
  persist(
    (set, get) => ({
      user: null,
      accessToken: null,
      refreshToken: null,
      tokenExpiresAt: null,
      authLoading: false,
      authError: null,

      clearAuthError: () => set({ authError: null }),

      loginDemo: (input) => {
        set({
          user: createDemoUser(input),
          accessToken: 'mock-access-token',
          refreshToken: null,
          tokenExpiresAt: null,
          authError: null,
        })
      },

      login: async (input) => {
        set({ authLoading: true, authError: null })

        if (useMockAuth) {
          set({
            user: createDemoUser(input),
            accessToken: 'mock-access-token',
            refreshToken: null,
            tokenExpiresAt: null,
            authLoading: false,
          })
          return
        }

        try {
          const tokens = await authService.signIn(input.userName.trim(), input.password)
          const profile = await authService.getCurrentUser(tokens.accessToken)

          set({
            user: mapCurrentUserToDashboardUser(profile),
            accessToken: tokens.accessToken,
            refreshToken: tokens.refreshToken,
            tokenExpiresAt: Date.now() + tokens.expiresIn * 1000,
            authLoading: false,
            authError: null,
          })
        } catch {
          set({
            authLoading: false,
            authError: 'auth.signInFailed',
          })
          throw new Error('Sign in failed')
        }
      },

      logout: async () => {
        const { accessToken } = get()

        if (accessToken && accessToken !== 'mock-access-token') {
          try {
            await authService.signOut(accessToken)
          } catch {
            // Ignore sign-out API errors and clear local session anyway.
          }
        }

        useCartStore.getState().clearCart()
        set({
          user: null,
          accessToken: null,
          refreshToken: null,
          tokenExpiresAt: null,
          authError: null,
        })
      },

      restoreSession: async () => {
        const { accessToken, refreshToken, tokenExpiresAt } = get()
        if (!accessToken) return false

        if (accessToken === 'mock-access-token') {
          return get().user !== null
        }

        const shouldRefresh =
          refreshToken &&
          tokenExpiresAt !== null &&
          Date.now() > tokenExpiresAt - 60_000

        try {
          let activeToken = accessToken

          if (shouldRefresh && refreshToken) {
            const refreshed = await authService.refreshToken(accessToken, refreshToken)
            activeToken = refreshed.accessToken
            set({
              accessToken: refreshed.accessToken,
              refreshToken: refreshed.refreshToken,
              tokenExpiresAt: Date.now() + refreshed.expiresIn * 1000,
            })
          }

          const profile = await authService.getCurrentUser(activeToken)
          set({ user: mapCurrentUserToDashboardUser(profile) })
          return true
        } catch {
          useCartStore.getState().clearCart()
          set({
            user: null,
            accessToken: null,
            refreshToken: null,
            tokenExpiresAt: null,
          })
          return false
        }
      },
    }),
    {
      name: 'westelm-user',
      partialize: (state) => ({
        user: state.user,
        accessToken: state.accessToken,
        refreshToken: state.refreshToken,
        tokenExpiresAt: state.tokenExpiresAt,
      }),
    },
  ),
)

export function useIsAuthenticated() {
  return useUserStore((s) => s.user !== null && s.accessToken !== null)
}

export function useAccessToken() {
  return useUserStore((s) => s.accessToken)
}
