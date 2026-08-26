import { useEffect, useState } from 'react'
import { Link, NavLink, useLocation } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import {
  DASHBOARD_FLAT_NAV,
  DASHBOARD_NAV_GROUPS,
  DASHBOARD_PROFILE_NAV_ITEM,
  findActiveNavGroup,
  isNavItemActive,
  type DashboardNavGroup,
  type DashboardNavItem,
} from '@/config/dashboardNav'
import { LogoutIcon } from '@/components/dashboard/DashboardIcons'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
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

function expandedGroupsForPath(pathname: string): Set<string> {
  const active = findActiveNavGroup(pathname)
  return active ? new Set([active]) : new Set()
}

function getInitials(firstName: string, lastName: string) {
  const first = firstName.charAt(0).toUpperCase()
  const last = lastName ? lastName.charAt(0).toUpperCase() : ''
  return `${first}${last}` || first
}

function navItemLabelKey(section: DashboardNavItem['section']) {
  return `dashboard.nav.${section}`
}

export function DashboardSidebar() {
  const { t } = useTranslation()
  const location = useLocation()
  const user = useUserStore((s) => s.user)
  const logout = useUserStore((s) => s.logout)
  const { progress, rewardClaimed } = useProfileCompletion()
  const [collapsed, setCollapsed] = useState(readSidebarCollapsed)
  const [expandedGroups, setExpandedGroups] = useState(() =>
    expandedGroupsForPath(location.pathname),
  )

  useEffect(() => {
    setExpandedGroups(expandedGroupsForPath(location.pathname))
  }, [location.pathname])

  if (!user) return null

  const initials = getInitials(user.firstName, user.lastName)
  const displayName = user.lastName ? `${user.firstName} ${user.lastName}` : user.firstName

  const toggleCollapsed = () => {
    setCollapsed((prev) => {
      const next = !prev
      try {
        localStorage.setItem(SIDEBAR_COLLAPSED_KEY, String(next))
      } catch {
        // ignore
      }
      return next
    })
  }

  const toggleGroup = (groupId: string) => {
    setExpandedGroups((prev) => {
      const next = new Set(prev)
      if (next.has(groupId)) next.delete(groupId)
      else next.add(groupId)
      return next
    })
  }

  return (
    <aside
      className={`hidden shrink-0 transition-[width] duration-200 ease-out lg:block ${
        collapsed ? 'w-[4.75rem]' : 'w-[17.5rem] xl:w-72'
      }`}
    >
      <div className="sticky top-24 flex flex-col rounded-sm border border-border bg-surface shadow-md ring-1 ring-black/[0.03]">
        {/* User card */}
        <div
          className={`relative overflow-hidden border-b border-border bg-gradient-to-br from-warm-soft/90 via-surface to-surface ${
            collapsed ? 'px-3 py-4' : 'px-4 py-5'
          }`}
        >
          <div
            aria-hidden
            className="pointer-events-none absolute -end-10 -top-10 size-36 rounded-full bg-warm/8"
          />
          <div className={`relative flex items-center ${collapsed ? 'justify-center' : 'gap-3'}`}>
            <span
              className={`flex shrink-0 items-center justify-center rounded-full bg-gradient-to-br from-warm to-warm-hover font-semibold text-warm-text shadow-sm ring-1 ring-warm/30 ${
                collapsed ? 'size-10 text-sm' : 'size-11 text-base'
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
                className="flex size-7 shrink-0 items-center justify-center rounded-sm border border-border/70 bg-surface-muted/50 text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
              >
                <ChevronIcon expanded={false} className="ltr:rotate-90 rtl:-rotate-90" />
              </button>
            )}
          </div>
          {collapsed && (
            <div className="mt-3 flex justify-center">
              <button
                type="button"
                onClick={toggleCollapsed}
                aria-label={t('dashboard.expandSidebar')}
                className="flex size-7 items-center justify-center rounded-sm border border-border/70 bg-surface-muted/50 text-text-muted hover:bg-surface-muted hover:text-text"
              >
                <ChevronIcon expanded={false} className="ltr:-rotate-90 rtl:rotate-90" />
              </button>
            </div>
          )}
        </div>

        {/* Back to shop + profile completion */}
        {!collapsed && (
          <div className="space-y-0.5 border-b border-border px-3 py-2.5">
            <Link
              to="/"
              className="flex items-center gap-2 rounded-sm px-2 py-2 text-xs font-medium text-text-muted transition-colors hover:bg-surface-muted hover:text-warm"
            >
              <ShopIcon />
              {t('dashboard.backToShop')}
            </Link>
            <ul>
              <SidebarNavItem
                item={DASHBOARD_PROFILE_NAV_ITEM}
                showProfileBadge={!rewardClaimed && progress.percent < 100}
                profilePercent={progress.percent}
              />
            </ul>
          </div>
        )}

        <nav
          aria-label={t('dashboard.navLabel')}
          className={`overflow-y-auto overscroll-contain ${collapsed ? 'px-2 py-3' : 'px-2 py-3'} max-h-[calc(100vh-14rem)]`}
        >
          {collapsed ? (
            <ul className="space-y-1">
              {DASHBOARD_FLAT_NAV.map((item) => (
                <CollapsedNavItem key={item.path} item={item} />
              ))}
            </ul>
          ) : (
            <div className="space-y-1">
              {DASHBOARD_NAV_GROUPS.map((group) => {
                const isExpanded = expandedGroups.has(group.id)
                const groupActive = group.items.some((item) =>
                  isNavItemActive(location.pathname, item),
                )

                return (
                  <div key={group.id} className="rounded-sm">
                    <NavGroupToggle
                      group={group}
                      isExpanded={isExpanded}
                      groupActive={groupActive}
                      onToggle={() => toggleGroup(group.id)}
                    />
                    {isExpanded && (
                      <ul className="mb-2 mt-0.5 space-y-0.5 border-s-2 border-warm/15 ms-2 ps-1">
                        {group.items.map((item) => (
                          <SidebarNavItem key={item.path} item={item} />
                        ))}
                      </ul>
                    )}
                  </div>
                )
              })}
            </div>
          )}
        </nav>

        <div className={`border-t border-border ${collapsed ? 'p-2' : 'p-2.5'}`}>
          <button
            type="button"
            onClick={logout}
            title={collapsed ? t('dashboard.logout') : undefined}
            className={`group flex w-full items-center rounded-sm text-sm font-medium text-text-muted transition-colors hover:bg-sale/5 hover:text-sale ${
              collapsed ? 'justify-center px-2 py-2.5' : 'gap-3 px-3 py-2.5'
            }`}
          >
            <span className="flex size-8 shrink-0 items-center justify-center rounded-sm bg-surface-muted text-text-muted group-hover:bg-sale/10 group-hover:text-sale">
              <LogoutIcon size={17} />
            </span>
            {!collapsed && <span>{t('dashboard.logout')}</span>}
            {collapsed && <span className="sr-only">{t('dashboard.logout')}</span>}
          </button>
        </div>
      </div>
    </aside>
  )
}

function NavGroupToggle({
  group,
  isExpanded,
  groupActive,
  onToggle,
  className = '',
}: {
  group: DashboardNavGroup
  isExpanded: boolean
  groupActive: boolean
  onToggle: () => void
  className?: string
}) {
  const { t } = useTranslation()
  const Icon = group.icon

  return (
    <button
      type="button"
      onClick={onToggle}
      className={`group flex w-full items-center gap-2 rounded-sm px-2 py-2 text-start text-sm font-semibold transition-colors ${
        groupActive ? 'text-warm' : 'text-text hover:bg-surface-muted'
      } ${className}`}
    >
      <span
        className={`flex size-6 shrink-0 items-center justify-center overflow-hidden rounded-sm border transition-colors ${
          groupActive
            ? 'border-warm/25 bg-warm-soft/60 text-warm'
            : 'border-border/60 bg-surface-muted/40 text-text-muted group-hover:border-warm/20 group-hover:bg-warm-soft/40 group-hover:text-warm'
        }`}
      >
        <ChevronIcon expanded={isExpanded} />
      </span>
      <span
        className={`flex size-7 shrink-0 items-center justify-center overflow-hidden rounded-sm transition-colors ${
          groupActive
            ? 'bg-warm-soft text-warm'
            : 'bg-surface-muted/80 text-text-muted group-hover:bg-warm-soft/60 group-hover:text-warm'
        }`}
      >
        <Icon size={14} className="shrink-0" />
      </span>
      <span className="min-w-0 flex-1 truncate text-start">{t(`dashboard.navGroups.${group.id}`)}</span>
    </button>
  )
}

function SidebarNavItem({
  item,
  showProfileBadge = false,
  profilePercent = 0,
}: {
  item: DashboardNavItem
  showProfileBadge?: boolean
  profilePercent?: number
}) {
  const { t } = useTranslation()
  const label = t(navItemLabelKey(item.section))
  const Icon = item.icon

  return (
    <li>
      <NavLink
        to={item.path}
        end={!item.matchPrefix}
        className={({ isActive }) =>
          `group flex w-full items-center gap-2.5 rounded-sm px-2.5 py-2 text-start text-sm font-medium transition-all duration-150 ${
            isActive
              ? 'bg-warm text-warm-text shadow-sm'
              : 'text-text-muted hover:bg-surface-muted hover:text-text'
          }`
        }
      >
        {({ isActive }) => (
          <>
            <span
              className={`flex size-7 shrink-0 items-center justify-center overflow-hidden rounded-sm transition-colors ${
                isActive
                  ? 'bg-warm-text/15 text-warm-text'
                  : 'bg-warm-soft/80 text-warm group-hover:bg-warm-muted'
              }`}
            >
              <Icon size={14} className="shrink-0" />
            </span>
            <span className="min-w-0 flex-1 truncate text-start">{label}</span>
            {showProfileBadge && (
              <span className="ms-auto rounded-full bg-warm-text/20 px-2 py-0.5 text-[10px] font-bold text-warm-text">
                {profilePercent}%
              </span>
            )}
          </>
        )}
      </NavLink>
    </li>
  )
}

function CollapsedNavItem({ item }: { item: DashboardNavItem }) {
  const { t } = useTranslation()
  const label = t(navItemLabelKey(item.section))
  const Icon = item.icon

  return (
    <li>
      <NavLink
        to={item.path}
        end={!item.matchPrefix}
        title={label}
        className={({ isActive }) =>
          `flex items-center justify-center rounded-sm p-2.5 transition-colors ${
            isActive ? 'bg-warm text-warm-text shadow-sm' : 'text-text-muted hover:bg-surface-muted hover:text-warm'
          }`
        }
      >
        <Icon size={18} />
        <span className="sr-only">{label}</span>
      </NavLink>
    </li>
  )
}

export function DashboardMobileNav() {
  const { t } = useTranslation()
  const location = useLocation()
  const { progress, rewardClaimed } = useProfileCompletion()
  const [open, setOpen] = useState(false)
  const [expandedGroups, setExpandedGroups] = useState(() =>
    expandedGroupsForPath(location.pathname),
  )

  useEffect(() => {
    setOpen(false)
    setExpandedGroups(expandedGroupsForPath(location.pathname))
  }, [location.pathname])

  const toggleGroup = (groupId: string) => {
    setExpandedGroups((prev) => {
      const next = new Set(prev)
      if (next.has(groupId)) next.delete(groupId)
      else next.add(groupId)
      return next
    })
  }

  return (
    <div className="lg:hidden">
      <div className="flex items-center gap-2">
        <button
          type="button"
          onClick={() => setOpen(true)}
          className="inline-flex items-center gap-2 rounded-sm border border-border bg-surface px-3.5 py-2.5 text-sm font-semibold text-text shadow-sm transition-colors hover:border-warm/40 hover:bg-warm-soft/50"
        >
          <MenuIcon />
          {t('dashboard.openMenu')}
        </button>
        <Link
          to="/"
          className="inline-flex items-center gap-1.5 rounded-sm border border-border bg-surface px-3 py-2.5 text-xs font-medium text-text-muted shadow-sm hover:text-warm"
        >
          <ShopIcon className="size-4" />
          {t('dashboard.backToShop')}
        </Link>
      </div>

      {open && (
        <div className="fixed inset-0 z-50 flex">
          <button
            type="button"
            aria-label={t('dashboard.closeMenu')}
            className="absolute inset-0 bg-black/40 backdrop-blur-[1px]"
            onClick={() => setOpen(false)}
          />
          <aside
            className="relative flex h-full w-[min(100%,20rem)] flex-col bg-surface shadow-2xl"
            aria-label={t('dashboard.navLabel')}
          >
            <div className="flex items-center justify-between border-b border-border px-4 py-4">
              <p className="text-sm font-semibold text-text">{t('dashboard.navLabel')}</p>
              <button
                type="button"
                onClick={() => setOpen(false)}
                aria-label={t('dashboard.closeMenu')}
                className="flex size-9 items-center justify-center rounded-sm text-text-muted hover:bg-surface-muted"
              >
                <CloseIcon />
              </button>
            </div>
            <nav className="flex-1 overflow-y-auto px-3 py-3">
              <div className="space-y-1">
                <ul className="mb-3 space-y-0.5 border-b border-border pb-3">
                  <MobileNavItem
                    item={DASHBOARD_PROFILE_NAV_ITEM}
                    showProfileBadge={!rewardClaimed && progress.percent < 100}
                    profilePercent={progress.percent}
                  />
                </ul>
                {DASHBOARD_NAV_GROUPS.map((group) => {
                  const isExpanded = expandedGroups.has(group.id)
                  const groupActive = group.items.some((item) =>
                    isNavItemActive(location.pathname, item),
                  )

                  return (
                    <div key={group.id}>
                      <NavGroupToggle
                        group={group}
                        isExpanded={isExpanded}
                        groupActive={groupActive}
                        onToggle={() => toggleGroup(group.id)}
                        className="px-3"
                      />
                      {isExpanded && (
                        <ul className="mb-2 space-y-0.5 ps-2">
                          {group.items.map((item) => (
                            <MobileNavItem key={item.path} item={item} />
                          ))}
                        </ul>
                      )}
                    </div>
                  )
                })}
              </div>
            </nav>
          </aside>
        </div>
      )}
    </div>
  )
}

function MobileNavItem({
  item,
  showProfileBadge = false,
  profilePercent = 0,
}: {
  item: DashboardNavItem
  showProfileBadge?: boolean
  profilePercent?: number
}) {
  const { t } = useTranslation()
  const Icon = item.icon

  return (
    <li>
      <NavLink
        to={item.path}
        end={!item.matchPrefix}
        className={({ isActive }) =>
          `flex w-full items-center gap-3 rounded-sm px-3 py-2.5 text-start text-sm font-medium transition-colors ${
            isActive
              ? 'bg-warm text-warm-text'
              : 'text-text-muted hover:bg-surface-muted hover:text-text'
          }`
        }
      >
        <span className="flex size-8 shrink-0 items-center justify-center overflow-hidden rounded-sm bg-warm-soft/60 text-warm">
          <Icon size={16} className="shrink-0" />
        </span>
        <span className="min-w-0 flex-1 truncate text-start">{t(navItemLabelKey(item.section))}</span>
        {showProfileBadge && (
          <span className="ms-auto rounded-full bg-warm/15 px-2 py-0.5 text-[10px] font-bold text-warm">
            {profilePercent}%
          </span>
        )}
      </NavLink>
    </li>
  )
}

function ShopIcon({ className = '' }: { className?: string }) {
  return (
    <svg
      width="16"
      height="16"
      viewBox="0 0 16 16"
      fill="none"
      aria-hidden
      className={className}
    >
      <path
        d="M2.5 6.5 3 3h10l.5 3.5M2.5 6.5h11v7.5a1 1 0 0 1-1 1h-9a1 1 0 0 1-1-1V6.5Z"
        stroke="currentColor"
        strokeWidth="1.2"
        strokeLinejoin="round"
      />
      <path d="M6 10.5h4" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
    </svg>
  )
}

function MenuIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 18 18" fill="none" aria-hidden>
      <path
        d="M3 5h12M3 9h12M3 13h12"
        stroke="currentColor"
        strokeWidth="1.4"
        strokeLinecap="round"
      />
    </svg>
  )
}

function CloseIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 18 18" fill="none" aria-hidden>
      <path
        d="M4 4l10 10M14 4 4 14"
        stroke="currentColor"
        strokeWidth="1.4"
        strokeLinecap="round"
      />
    </svg>
  )
}
