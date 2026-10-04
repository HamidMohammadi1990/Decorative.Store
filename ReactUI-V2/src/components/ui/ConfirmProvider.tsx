import type { ReactNode } from 'react'
import { ConfirmDialog } from '@/components/ui/ConfirmDialog'
import { ToastHost } from '@/components/ui/ToastHost'

export function ConfirmProvider({ children }: { children: ReactNode }) {
  return (
    <>
      {children}
      <ConfirmDialog />
      <ToastHost />
    </>
  )
}
