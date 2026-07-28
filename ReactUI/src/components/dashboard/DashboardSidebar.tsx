import { useState } from 'react'
import { NavLink } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { DashboardSection } from '@/models/dashboard/dashboard.model'
import {
  AddressesIcon,
  CouponsIcon,
  LogoutIcon,
  OrdersIcon,
  ProfileCompletionIcon,
  ReviewsIcon,
  StoriesIcon,
  TransactionsIcon,
  WalletIcon,
} from '@/components/dashboard/DashboardIcons'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { WishlistIcon } from '@/components/wishlist/WishlistIcon'
import { useUserStore } from '@/stores/userStore'
import { useProfileCompletion } from '@/hooks/useProfileCompletion'

const SIDEBAR_COLLAPSED_KEY = 'westelm-dashboard-sidebar-collapsed'

function readSidebarCollapsed() {
  try {
    return localStorage.getItem(SIDEBAR_COLLAPSED_KEY) === 'true'
  } catch {
    return false
  }
}

const navItems: { section: DashboardSection; path: string; icon: typeof WalletIcon }[] = [
  { section: 'wallet', path: '/account/dashboard/wallet', icon: WalletIcon },
  { section: 'profile', path: '/account/dashboard/profile', icon: ProfileCompletionIcon },
  { section: 'orders', path: '/account/dashboard/orders', icon: OrdersIcon },
  { section: 'transactions', path: '/account/dashboard/transactions', icon: TransactionsIcon },
  { section: 'reviews', path: '/account/dashboard/reviews', icon: ReviewsIcon },
  { section: 'wishlist', path: '/account/dashboard/wishlist', icon: WishlistIcon },
  { section: 'stories', path: '/account/dashboard/stories', icon: StoriesIcon },
  { section: 'addresses', path: '/account/dashboard/addresses', icon: AddressesIcon },
  { section: 'coupons', path: '/account/dashboard/coupons', icon: CouponsIcon },
]

function getInitials(firstName: string, lastName: string) {
  const first = firstName.charAt(0).toUpperCase()
  const last = lastName ? lastName.charAt(0).toUpperCase() : ''
  return `${first}${last}` || first
}

export function DashboardSidebar() {
  const { t } = useTranslation()
  const user = useUserStore((s) => s.user)
  const logout = useUserStore((s) => s.logout)
  const { progress, rewardClaimed } = useProfileCompletion()
  const [collapsed, setCollapsed] = useState(readSidebarCollapsed)

  if (!user) return null

  const initials = getInitials(user.firstName, user.lastName)
  const displayName = user.lastName
    ? `${user.firstName} ${user.lastName}`
    : user.firstName

  const toggleCollapsed = () => {
    setCollapsed((prev) => {
      const next = !prev
      try {
        localStorage.setItem(SIDEBAR_COLLAPSED_KEY, String(next))
      } catch {
        // ignore storage errors
      }
      return next
    })
  }

  return (
    <aside
      className={`hidden shrink-0 transition-[width] duration-200 ease-out lg:block ${
        collapsed ? 'w-[4.75rem]' : 'w-64 xl:w-72'
      }`}
    >
      <div className="sticky top-24">
        <div className="overflow-hidden rounded-sm border border-border bg-surface shadow-md">
          <div
            className={`relative bg-gradient-to-br from-warm-soft via-surface to-surface ${
              collapsed ? 'px-3 py-4' : 'px-5 py-5'
            }`}
          >
            <div
              aria-hidden
              className="pointer-events-none absolute -end-8 -top-8 size-32 rounded-full bg-warm/10"
            />
            <div className={`relative flex items-center ${collapsed ? 'justify-center' : 'gap-3.5'}`}>
              <span
                className={`relative flex shrink-0 items-center justify-center rounded-full bg-gradient-to-br from-warm to-warm-hover font-semibold text-warm-text shadow-sm ring-2 ring-warm/20 ring-offset-2 ring-offset-surface ${
                  collapsed ? 'size-10 text-sm' : 'size-12 text-base'
                }`}
                title={collapsed ? displayName : undefined}
              >
                {initials}
              </span>
              {!collapsed && (
                <div className="min-w-0 flex-1">
                  <p className="truncate text-sm font-semibold text-text">{displayName}</p>
                  <p className="truncate text-xs text-text-muted">{user.email}</p>
                </div>
              )}
              {!collapsed && (
                <button
                  type="button"
                  onClick={toggleCollapsed}
                  aria-label={t('dashboard.collapseSidebar')}
                  aria-expanded
                  title={t('dashboard.collapseSidebar')}
                  className="flex size-8 shrink-0 items-center justify-center rounded-sm text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
                >
                  <ChevronIcon expanded={false} className="ltr:rotate-90 rtl:-rotate-90" />
                </button>
              )}
            </div>
            {collapsed && (
              <div className="relative mt-3 flex justify-center">
                <button
                  type="button"
                  onClick={toggleCollapsed}
                  aria-label={t('dashboard.expandSidebar')}
                  aria-expanded={false}
                  title={t('dashboard.expandSidebar')}
                  className="flex size-8 items-center justify-center rounded-sm text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
                >
                  <ChevronIcon expanded={false} className="ltr:-rotate-90 rtl:rotate-90" />
                </button>
              </div>
            )}
          </div>

          {!collapsed && (
            <div className="border-b border-border px-5 py-3">
              <p className="text-[10px] font-semibold uppercase tracking-[0.16em] text-text-muted">
                {t('dashboard.navLabel')}
              </p>
            </div>
          )}

          <nav aria-label={t('dashboard.navLabel')} className={collapsed ? 'px-2 py-3' : 'px-3 py-4'}>
            <ul className="space-y-1">
              {navItems.map((item) => (
                <SidebarNavItem
                  key={item.section}
                  item={item}
                  collapsed={collapsed}
                  showProfileBadge={item.section === 'profile' && !rewardClaimed && progress.percent < 100}
                  profilePercent={progress.percent}
                />
              ))}
            </ul>
          </nav>

          <div className={`border-t border-border ${collapsed ? 'p-2' : 'p-3'}`}>
            <button
              type="button"
              onClick={logout}
              title={collapsed ? t('dashboard.logout') : undefined}
              className={`group flex w-full items-center rounded-sm text-sm font-medium text-text-muted transition-colors hover:bg-sale/5 hover:text-sale ${
                collapsed ? 'justify-center px-2 py-2.5' : 'gap-3 px-3 py-2.5'
              }`}
            >
              <span className="flex size-8 shrink-0 items-center justify-center rounded-sm bg-surface-muted text-text-muted transition-colors group-hover:bg-sale/10 group-hover:text-sale">
                <LogoutIcon size={17} />
              </span>
              {!collapsed && <span>{t('dashboard.logout')}</span>}
              {collapsed && <span className="sr-only">{t('dashboard.logout')}</span>}
            </button>
          </div>
        </div>
      </div>
    </aside>
  )
}

type NavItem = { section: DashboardSection; path: string; icon: typeof WalletIcon }

function SidebarNavItem({
  item,
  collapsed,
  showProfileBadge = false,
  profilePercent = 0,
}: {
  item: NavItem
  collapsed: boolean
  showProfileBadge?: boolean
  profilePercent?: number
}) {
  const { t } = useTranslation()
  const label = t(`dashboard.nav.${item.section}`)
  const Icon = item.icon

  return (
    <li>
      <NavLink
        to={item.path}
        title={collapsed ? label : undefined}
        className={({ isActive }) =>
          `group relative flex items-center rounded-sm text-sm font-medium transition-all duration-150 ${
            collapsed ? 'justify-center px-2 py-2.5' : 'gap-3 px-3 py-2.5'
          } ${
            isActive
              ? collapsed
                ? 'text-warm'
                : 'bg-warm text-warm-text shadow-sm'
              : 'text-text-muted hover:bg-surface-muted hover:text-text'
          }`
        }
      >
        {({ isActive }) => (
          <>
            <span
              className={`flex size-8 shrink-0 items-center justify-center rounded-sm transition-colors ${
                isActive
                  ? collapsed
                    ? 'bg-warm text-warm-text shadow-sm'
                    : 'bg-warm-text/15 text-warm-text'
                  : 'bg-warm-soft text-warm group-hover:bg-warm-muted'
              }`}
            >
              <Icon size={17} />
            </span>
            {!collapsed && <span className="truncate">{label}</span>}
            {!collapsed && showProfileBadge && (
              <span className="ms-auto rounded-full bg-warm px-2 py-0.5 text-[10px] font-bold text-warm-text">
                {profilePercent}%
              </span>
            )}
            {collapsed && showProfileBadge && (
              <span className="absolute -end-0.5 -top-0.5 size-2.5 rounded-full bg-warm ring-2 ring-surface" />
            )}
            {collapsed && <span className="sr-only">{label}</span>}
          </>
        )}
      </NavLink>
    </li>
  )
}

export function DashboardMobileNav() {
  const { t } = useTranslation()

  return (
    <nav
      aria-label={t('dashboard.navLabel')}
      className="scrollbar-none -mx-1 flex gap-2 overflow-x-auto px-1 pb-2 lg:hidden"
    >
      {navItems.map((item) => {
        const Icon = item.icon
        return (
          <NavLink
            key={item.section}
            to={item.path}
            className={({ isActive }) =>
              `inline-flex shrink-0 items-center gap-2 rounded-sm border px-3.5 py-2.5 text-xs font-semibold transition-all duration-150 ${
                isActive
                  ? 'border-warm bg-warm text-warm-text shadow-sm'
                  : 'border-border bg-surface text-text-muted shadow-sm hover:border-warm/40 hover:bg-warm-soft hover:text-warm'
              }`
            }
          >
            {({ isActive }) => (
              <>
                <span
                  className={`flex size-6 items-center justify-center rounded-sm ${
                    isActive ? 'bg-warm-text/15' : 'bg-warm-soft text-warm'
                  }`}
                >
                  <Icon size={13} />
                </span>
                {t(`dashboard.nav.${item.section}`)}
              </>
            )}
          </NavLink>
        )
      })}
    </nav>
  )
}
