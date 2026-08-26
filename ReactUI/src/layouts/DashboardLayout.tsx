import { useEffect } from 'react'
import { Link, Outlet, useLocation, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { AuthGate } from '@/components/auth/LoginModal'
import { Container } from '@/components/ui/Container'
import { InlineLoading } from '@/components/ui/Spinner'
import {
  DashboardMobileNav,
  DashboardSidebar,
} from '@/components/dashboard/DashboardSidebar'
import { DASHBOARD_FLAT_NAV, isNavItemActive } from '@/config/dashboardNav'
import { useDashboard } from '@/hooks/useDashboard'
import { useWishlistSync } from '@/hooks/useWishlistSync'
import { useAddressSync } from '@/hooks/useAddressSync'
import { useAuthModalStore } from '@/stores/authModalStore'
import { useUserStore } from '@/stores/userStore'

function useDashboardPageTitle() {
  const { t } = useTranslation()
  const location = useLocation()

  const item = DASHBOARD_FLAT_NAV.find((nav) => isNavItemActive(location.pathname, nav))
  if (!item) return t('dashboard.breadcrumb')

  return t(`dashboard.nav.${item.section}`)
}

export function DashboardLayout() {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const location = useLocation()
  const pageTitle = useDashboardPageTitle()
  const user = useUserStore((s) => s.user)
  const logout = useUserStore((s) => s.logout)
  const openModal = useAuthModalStore((s) => s.openModal)
  const { data, loading, error } = useDashboard()
  useWishlistSync()
  useAddressSync()

  useEffect(() => {
    if (!user) {
      openModal({ mode: 'signin' })
    }
  }, [openModal, user])

  const handleLogout = () => {
    void logout().then(() => navigate('/account'))
  }

  if (!user) {
    return (
      <div className="flex-1 bg-surface-muted/40 py-8 md:py-12">
        <Container>
          <AuthGate message={t('auth.dashboardGateMessage')} />
        </Container>
      </div>
    )
  }

  const initials = user.firstName.charAt(0).toUpperCase()

  return (
    <div className="flex-1 bg-surface-muted/35">
      {/* Subtle top accent */}
      <div className="h-1 bg-gradient-to-r from-warm via-warm-hover to-warm/60" aria-hidden />

      <Container className="py-6 md:py-8 lg:py-10">
        {/* Mobile welcome strip */}
        <div className="mb-4 flex items-center justify-between gap-3 lg:hidden">
          <div className="flex min-w-0 items-center gap-3">
            <span className="flex size-10 shrink-0 items-center justify-center rounded-full bg-warm text-sm font-semibold text-warm-text shadow-sm">
              {initials}
            </span>
            <div className="min-w-0">
              <p className="text-[10px] font-semibold uppercase tracking-widest text-warm">
                {t('dashboard.breadcrumb')}
              </p>
              <h1 className="truncate text-base font-semibold text-text">
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

        <DashboardMobileNav />

        <div className="mt-4 flex gap-6 lg:mt-0 lg:gap-8">
          <DashboardSidebar />

          <div className="min-w-0 flex-1">
            {/* Desktop content header */}
            <div className="mb-5 hidden rounded-sm border border-border bg-surface px-5 py-4 shadow-sm lg:block">
              <div className="flex flex-wrap items-center justify-between gap-4">
                <div>
                  <p className="text-[10px] font-semibold uppercase tracking-[0.16em] text-text-muted">
                    {t('dashboard.breadcrumb')}
                  </p>
                  <h1 className="mt-1 text-xl font-semibold tracking-tight text-text md:text-2xl">
                    {pageTitle}
                  </h1>
                  <p className="mt-1 text-sm text-text-muted">
                    {t('dashboard.greeting', { name: user.firstName })}
                  </p>
                </div>
                <div className="flex items-center gap-2">
                  <Link
                    to="/"
                    className="rounded-sm border border-border px-3.5 py-2 text-xs font-semibold text-text-muted transition-colors hover:border-warm/40 hover:bg-warm-soft/50 hover:text-warm"
                  >
                    {t('dashboard.backToShop')}
                  </Link>
                  <button
                    type="button"
                    onClick={handleLogout}
                    className="rounded-sm px-3.5 py-2 text-xs font-semibold text-text-muted transition-colors hover:bg-sale/5 hover:text-sale"
                  >
                    {t('dashboard.logout')}
                  </button>
                </div>
              </div>
            </div>

            <div className="rounded-sm border border-border bg-surface shadow-sm ring-1 ring-black/[0.02]">
              <div className="border-b border-border/60 bg-surface-muted/30 px-4 py-3 sm:px-6 lg:hidden">
                <p className="text-sm font-semibold text-text">{pageTitle}</p>
              </div>

              <div className="p-4 sm:p-6 lg:p-8">
                {loading ? (
                  <div className="flex justify-center py-16">
                    <InlineLoading label={t('common.loading')} />
                  </div>
                ) : error || !data ? (
                  <div className="rounded-sm border border-dashed border-border bg-surface-muted/30 px-6 py-14 text-center">
                    <p className="text-sm text-text-muted">{t('common.error')}</p>
                  </div>
                ) : (
                  <Outlet context={data} />
                )}
              </div>
            </div>
          </div>
        </div>
      </Container>
    </div>
  )
}
