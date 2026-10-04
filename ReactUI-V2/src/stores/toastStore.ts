import { create } from 'zustand'

export type ToastTone = 'success' | 'error'

interface ToastItem {
  id: number
  message: string
  tone: ToastTone
}

interface ToastState {
  toasts: ToastItem[]
  show: (message: string, tone?: ToastTone) => void
  dismiss: (id: number) => void
}

let nextToastId = 0

export const useToastStore = create<ToastState>((set, get) => ({
  toasts: [],
  show: (message, tone = 'success') => {
    const id = ++nextToastId
    set((state) => ({ toasts: [...state.toasts, { id, message, tone }] }))
    window.setTimeout(() => get().dismiss(id), 2800)
  },
  dismiss: (id) =>
    set((state) => ({
      toasts: state.toasts.filter((toast) => toast.id !== id),
    })),
}))

export function showToast(message: string, tone: ToastTone = 'success') {
  useToastStore.getState().show(message, tone)
}
