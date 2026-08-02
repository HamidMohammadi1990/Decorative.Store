import type { ReactNode } from 'react'
import { Navigate, useLocation } from 'react-router-dom'
import { useIsAuthenticated } from '@/stores/userStore'

interface RequireAuthProps {
  children: ReactNode
}

export function RequireAuth({ children }: RequireAuthProps) {
  const location = useLocation()
  const isAuthenticated = useIsAuthenticated()

  if (!isAuthenticated) {
    const returnUrl = encodeURIComponent(`${location.pathname}${location.search}`)
    return <Navigate to={`/account?returnUrl=${returnUrl}`} replace />
  }

  return children
}
