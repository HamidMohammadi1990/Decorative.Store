import { create } from 'zustand'

export type AuthModalMode = 'signin' | 'signup'

interface AuthModalState {
  isOpen: boolean
  mode: AuthModalMode
  onSuccess: (() => void) | null
  openModal: (opts?: { mode?: AuthModalMode; onSuccess?: () => void }) => void
  closeModal: () => void
  setMode: (mode: AuthModalMode) => void
  consumeSuccessAction: () => (() => void) | null
}

export const useAuthModalStore = create<AuthModalState>((set, get) => ({
  isOpen: false,
  mode: 'signin',
  onSuccess: null,
  openModal: (opts) =>
    set({
      isOpen: true,
      mode: opts?.mode ?? 'signin',
      onSuccess: opts?.onSuccess ?? null,
    }),
  closeModal: () => set({ isOpen: false, onSuccess: null }),
  setMode: (mode) => set({ mode }),
  consumeSuccessAction: () => {
    const action = get().onSuccess
    set({ onSuccess: null })
    return action
  },
}))

export function openLoginModal(opts?: { mode?: AuthModalMode; onSuccess?: () => void }) {
  useAuthModalStore.getState().openModal(opts)
}
