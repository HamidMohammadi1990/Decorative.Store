import { create } from 'zustand'

export type AuthModalMode = 'signin' | 'signup'

interface AuthModalState {
  isOpen: boolean
  mode: AuthModalMode
  openModal: (opts?: { mode?: AuthModalMode }) => void
  closeModal: () => void
  setMode: (mode: AuthModalMode) => void
}

export const useAuthModalStore = create<AuthModalState>((set) => ({
  isOpen: false,
  mode: 'signin',
  openModal: (opts) =>
    set({
      isOpen: true,
      mode: opts?.mode ?? 'signin',
    }),
  closeModal: () => set({ isOpen: false }),
  setMode: (mode) => set({ mode }),
}))

export function openLoginModal(opts?: { mode?: AuthModalMode }) {
  useAuthModalStore.getState().openModal(opts)
}
