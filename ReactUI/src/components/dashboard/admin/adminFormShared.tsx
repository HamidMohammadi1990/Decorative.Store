import type { ReactNode } from 'react'
import { ApiError } from '@/services/api/apiTypes'

export function resolveAdminMutationError(error: unknown, fallback: string) {
  if (error instanceof ApiError && error.messages.length > 0) {
    return error.messages[0].message || fallback
  }
  return fallback
}

export function AdminField({ label, children }: { label: string; children: ReactNode }) {
  return (
    <label className="block">
      <span className="text-sm font-medium text-text">{label}</span>
      <div className="mt-2">{children}</div>
    </label>
  )
}

export const adminInputClass =
  'w-full rounded-lg border border-border bg-surface px-3 py-2.5 text-sm text-text outline-none transition-colors placeholder:text-text-muted focus:border-warm'
