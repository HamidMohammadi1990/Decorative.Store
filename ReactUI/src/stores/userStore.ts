import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { DashboardUser } from '@/models/dashboard/dashboard.model'

interface LoginInput {
  email: string
  firstName?: string
  lastName?: string
}

interface UserState {
  user: DashboardUser | null
  login: (input: LoginInput) => void
  logout: () => void
}

function createUserId() {
  return `user-${Date.now().toString(36)}`
}

export const useUserStore = create<UserState>()(
  persist(
    (set) => ({
      user: null,

      login: (input) => {
        const firstName = input.firstName?.trim() || input.email.split('@')[0]
        const lastName = input.lastName?.trim() || ''

        set({
          user: {
            id: createUserId(),
            firstName,
            lastName,
            email: input.email.trim(),
            memberSince: new Date().toISOString().slice(0, 10),
          },
        })
      },

      logout: () => set({ user: null }),
    }),
    {
      name: 'westelm-user',
      partialize: (state) => ({ user: state.user }),
    },
  ),
)

export function useIsAuthenticated() {
  return useUserStore((s) => s.user !== null)
}
