import type { ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import { AccessDeniedPanel } from '@/components/dashboard/AccessDeniedPanel'
import { InlineLoading } from '@/components/ui/Spinner'
import { useHasPermission, usePermissionsReady } from '@/hooks/useHasPermission'

interface DashboardAdminGateProps {
  permission: string
  children: ReactNode
}

export function DashboardAdminGate({ permission, children }: DashboardAdminGateProps) {
  const { t } = useTranslation()
  const { loaded, loading } = usePermissionsReady()
  const hasPermission = useHasPermission(permission)

  if (!loaded && loading) {
    return (
      <div className="flex justify-center py-16">
        <InlineLoading label={t('common.loading')} />
      </div>
    )
  }

  if (!hasPermission) {
    return <AccessDeniedPanel />
  }

  return children
}
