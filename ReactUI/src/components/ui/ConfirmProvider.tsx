import type { ReactNode } from 'react'
import { ConfirmDialog } from '@/components/ui/ConfirmDialog'

export function ConfirmProvider({ children }: { children: ReactNode }) {
  return (
    <>
      {children}
      <ConfirmDialog />
    </>
  )
}
