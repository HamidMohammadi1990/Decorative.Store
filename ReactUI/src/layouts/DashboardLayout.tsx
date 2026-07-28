import { Navigate, Outlet, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { Container } from '@/components/ui/Container'
import { InlineLoading } from '@/components/ui/Spinner'
import {
  DashboardMobileNav,
  DashboardSidebar,
} from '@/components/dashboard/DashboardSidebar'
import { ProfileCompletionBanner } from '@/components/dashboard/ProfileCompletionBanner'
import { useDashboard } from '@/hooks/useDashboard'
import { useUserStore } from '@/stores/userStore'

export function DashboardLayout() {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const user = useUserStore((s) => s.user)
  const logout = useUserStore((s) => s.logout)
  const { data, loading, error } = useDashboard()

  if (!user) {
    return <Navigate to="/account" replace />
  }

  const handleLogout = () => {
    logout()
    navigate('/account')
  }

  const initials = user.firstName.charAt(0).toUpperCase()

  return (
    <div className="flex-1 bg-gradient-to-b from-surface-muted/60 via-surface to-surface py-6 md:py-10">
      <Container>
        <div className="mb-5 overflow-hidden rounded-sm border border-border bg-gradient-to-br from-warm-soft/80 via-surface to-surface p-5 shadow-sm lg:hidden">
          <div className="flex items-center justify-between gap-4">
            <div className="flex min-w-0 items-center gap-3">
              <span className="flex size-11 shrink-0 items-center justify-center rounded-full bg-gradient-to-br from-warm to-warm-hover text-sm font-semibold text-warm-text shadow-sm ring-2 ring-warm/15 ring-offset-2 ring-offset-surface">
                {initials}
              </span>
              <div className="min-w-0">
                <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-warm">
                  {t('dashboard.breadcrumb')}
                </p>
                <h1 className="truncate text-lg font-semibold text-text">
                  {t('dashboard.greeting', { name: user.firstName })}
                </h1>
              </div>
            </div>
            <button
              type="button"
              onClick={handleLogout}
              className="shrink-0 text-xs font-medium text-text-muted hover:text-sale"
            >
              {t('dashboard.logout')}
            </button>
          </div>
        </div>

        <DashboardMobileNav />

        {loading ? (
          <div className="mt-8 flex justify-center rounded-sm border border-border bg-surface py-20 shadow-sm lg:mt-0">
            <InlineLoading label={t('common.loading')} />
          </div>
        ) : error || !data ? (
          <div className="mt-8 rounded-sm border border-border bg-surface px-6 py-16 text-center shadow-sm lg:mt-0">
            <p className="text-sm text-text-muted">{t('common.error')}</p>
          </div>
        ) : (
          <div className="mt-5 flex gap-6 lg:mt-0 lg:gap-8">
            <DashboardSidebar />
            <div className="min-w-0 flex-1">
              <div className="rounded-sm border border-border bg-surface p-5 shadow-sm sm:p-6 lg:p-8 lg:shadow-md">
                <ProfileCompletionBanner />
                <Outlet context={data} />
              </div>
            </div>
          </div>
        )}
      </Container>
    </div>
  )
}
