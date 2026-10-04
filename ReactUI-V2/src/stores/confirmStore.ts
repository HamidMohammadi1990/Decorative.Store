export type ConfirmTone = 'default' | 'danger'

export interface ConfirmOptions {
  title?: string
  message: string
  confirmLabel?: string
  cancelLabel?: string
  tone?: ConfirmTone
}

interface ConfirmState {
  open: boolean
  options: ConfirmOptions | null
  resolve: ((value: boolean) => void) | null
}

let state: ConfirmState = {
  open: false,
  options: null,
  resolve: null,
}

const listeners = new Set<() => void>()

function emit() {
  listeners.forEach((listener) => listener())
}

export function subscribeConfirm(listener: () => void) {
  listeners.add(listener)
  return () => listeners.delete(listener)
}

export function getConfirmState() {
  return state
}

export function askConfirm(options: ConfirmOptions): Promise<boolean> {
  return new Promise((resolve) => {
    state = {
      open: true,
      options,
      resolve,
    }
    emit()
  })
}

export function resolveConfirm(confirmed: boolean) {
  const { resolve } = state
  state = {
    open: false,
    options: null,
    resolve: null,
  }
  emit()
  resolve?.(confirmed)
}
