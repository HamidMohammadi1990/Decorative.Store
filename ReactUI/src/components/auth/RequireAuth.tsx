import { useEffect, type ReactNode } from 'react'
import { AuthGate } from '@/components/auth/LoginModal'
import { useIsAuthenticated } from '@/stores/userStore'
import { useAuthModalStore } from '@/stores/authModalStore'

interface RequireAuthProps {
  children: ReactNode
}

export function RequireAuth({ children }: RequireAuthProps) {
  const isAuthenticated = useIsAuthenticated()
  const openModal = useAuthModalStore((s) => s.openModal)

  useEffect(() => {
    if (!isAuthenticated) {
      openModal({ mode: 'signin' })
    }
  }, [isAuthenticated, openModal])

  if (!isAuthenticated) {
    return <AuthGate />
  }

  return children
}
