import type { ReactNode } from 'react'
import { ConfirmProvider } from '@/components/ui/ConfirmProvider'

/** Shared shell for SSR and CSR so hydration trees match. */
export function AppProviders({ children }: { children: ReactNode }) {
  return <ConfirmProvider>{children}</ConfirmProvider>
}
