import { useEffect, useLayoutEffect, useRef, useState } from 'react'
import { Link, useLocation, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { DashboardSection } from '@/models/dashboard/dashboard.model'
import {
  AddressesIcon,
  CartIcon,
  LogoutIcon,
  OrdersIcon,
  ProfileCompletionIcon,
  WalletIcon,
} from '@/components/dashboard/DashboardIcons'
import { Portal } from '@/components/ui/Portal'
import { UserIcon } from '@/components/ui/HeaderIcons'
import { UserAvatar } from '@/components/ui/UserAvatar'
import { positionAnchorDropdown } from '@/extensions/positionAnchorDropdown'
import { useHydrated } from '@/hooks/useHydrated'
import { useUserStore } from '@/stores/userStore'

interface UserAccountMenuProps {
  accountLabel: string
}

const menuItems: {
  section: DashboardSection | 'cart'
  path: string
  icon: typeof WalletIcon
}[] = [
  { section: 'profile', path: '/account/dashboard/profile', icon: ProfileCompletionIcon },
  { section: 'orders', path: '/account/dashboard/orders', icon: OrdersIcon },
  { section: 'cart', path: '/account/dashboard/cart', icon: CartIcon },
  { section: 'wallet', path: '/account/dashboard/wallet', icon: WalletIcon },
  { section: 'addresses', path: '/account/dashboard/addresses', icon: AddressesIcon },
]

function isMenuPathActive(pathname: string, path: string) {
  return pathname === path || pathname.startsWith(`${path}/`)
}

export function UserAccountMenu({ accountLabel }: UserAccountMenuProps) {
  const { t } = useTranslation()
  const location = useLocation()
  const navigate = useNavigate()
  const hydrated = useHydrated()
  const user = useUserStore((s) => s.user)
  const displayUser = hydrated ? user : null
  const logout = useUserStore((s) => s.logout)
  const [open, setOpen] = useState(false)
  const anchorRef = useRef<HTMLButtonElement>(null)
  const panelRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    setOpen(false)
  }, [location.pathname])

  useEffect(() => {
    if (!open) return

    const handlePointerDown = (event: MouseEvent) => {
      const target = event.target as Node
      if (anchorRef.current?.contains(target) || panelRef.current?.contains(target)) return
      setOpen(false)
    }

    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') setOpen(false)
    }

    document.addEventListener('mousedown', handlePointerDown)
    document.addEventListener('keydown', handleKeyDown)
    return () => {
      document.removeEventListener('mousedown', handlePointerDown)
      document.removeEventListener('keydown', handleKeyDown)
    }
  }, [open])

  useLayoutEffect(() => {
    if (!open) return

    const updatePosition = () => {
      const panel = panelRef.current
      const anchor = anchorRef.current
      if (!panel || !anchor) return
      positionAnchorDropdown(panel, anchor, true)
    }

    updatePosition()

    const panel = panelRef.current
    let observer: ResizeObserver | undefined

    if (panel && typeof ResizeObserver !== 'undefined') {
      observer = new ResizeObserver(updatePosition)
      observer.observe(panel)
    }

    window.addEventListener('resize', updatePosition)
    window.addEventListener('scroll', updatePosition, true)

    return () => {
      observer?.disconnect()
      window.removeEventListener('resize', updatePosition)
      window.removeEventListener('scroll', updatePosition, true)
    }
  }, [open, displayUser])

  const handleLogout = () => {
    void logout().then(() => {
      setOpen(false)
      navigate('/account')
    })
  }

  const displayName = displayUser
    ? displayUser.lastName
      ? `${displayUser.firstName} ${displayUser.lastName}`
      : displayUser.firstName
    : null

  const menuLabel = (section: DashboardSection | 'cart') =>
    section === 'cart' ? t('accountMenu.cart') : t(`dashboard.nav.${section}`)

  return (
    <>
      <button
        ref={anchorRef}
        type="button"
        onClick={() => setOpen((prev) => !prev)}
        aria-label={accountLabel}
        aria-expanded={open}
        aria-haspopup="menu"
        aria-controls="user-account-menu"
        className={`group relative inline-flex items-center rounded-full p-0.5 transition-colors hover:text-warm ${
          open ? 'text-warm' : ''
        }`}
      >
        {displayUser && displayName ? (
          <UserAvatar
            name={displayName}
            imageUrl={displayUser.profileImageUrl}
            size="sm"
            className={`ring-2 transition-shadow ${open ? 'ring-warm/35' : 'ring-transparent group-hover:ring-warm/20'}`}
          />
        ) : (
          <UserIcon size={22} />
        )}
        {displayUser && (
          <span
            aria-hidden
            className="absolute -end-0.5 -top-0.5 size-2 rounded-full border border-surface bg-accent"
          />
        )}
      </button>

      {open && (
        <Portal>
          <div ref={panelRef} className="fixed z-[60]">
            <div
              id="user-account-menu"
              data-dropdown-inner
              role="menu"
              aria-label={t('accountMenu.label')}
              className="w-[17.5rem] overflow-hidden rounded-xl border border-border/80 bg-surface shadow-[0_16px_48px_-16px_rgba(0,0,0,0.22)] ring-1 ring-black/[0.03]"
            >
              {displayUser && displayName ? (
                <>
                  <div className="border-b border-border/80 bg-gradient-to-br from-warm-soft/80 via-surface to-surface px-3 pt-3 pb-2">
                    <Link
                      to="/account/dashboard/profile"
                      role="menuitem"
                      onClick={() => setOpen(false)}
                      className="flex items-center gap-3 rounded-lg border border-border/50 bg-surface/90 p-3 shadow-sm transition-all hover:border-warm/25 hover:bg-surface hover:shadow-md"
                    >
                      <UserAvatar
                        name={displayName}
                        imageUrl={displayUser.profileImageUrl}
                        size="md"
                      />
                      <div className="min-w-0 flex-1">
                        <p className="truncate text-sm font-semibold text-text">
                          {t('accountMenu.greeting', { name: displayUser.firstName })}
                        </p>
                        <p className="mt-0.5 truncate text-xs text-text-muted">{displayUser.email}</p>
                        <p className="mt-1.5 text-[11px] font-medium text-warm">
                          {t('accountMenu.manageAccount')}
                        </p>
                      </div>
                    </Link>
                  </div>

                  <div className="px-3 pt-2">
                    <p className="px-1 pb-1 text-[10px] font-semibold uppercase tracking-[0.14em] text-text-muted">
                      {t('accountMenu.quickLinks')}
                    </p>
                  </div>

                  <ul className="px-2 pb-2">
                    {menuItems.map(({ section, path, icon: Icon }) => {
                      const active = isMenuPathActive(location.pathname, path)
                      return (
                        <li key={section}>
                          <Link
                            to={path}
                            role="menuitem"
                            onClick={() => setOpen(false)}
                            aria-current={active ? 'page' : undefined}
                            className={`flex items-center gap-3 rounded-lg px-3 py-2.5 text-sm font-medium transition-colors ${
                              active
                                ? 'bg-warm-soft text-warm'
                                : 'text-text-muted hover:bg-surface-muted hover:text-text'
                            }`}
                          >
                            <span
                              className={`flex size-8 shrink-0 items-center justify-center rounded-lg ${
                                active ? 'bg-surface text-warm' : 'bg-warm-soft/70 text-warm'
                              }`}
                            >
                              <Icon size={16} />
                            </span>
                            {menuLabel(section)}
                          </Link>
                        </li>
                      )
                    })}
                  </ul>

                  <div className="border-t border-border/80 p-2">
                    <button
                      type="button"
                      role="menuitem"
                      onClick={handleLogout}
                      className="flex w-full items-center gap-3 rounded-lg px-3 py-2.5 text-sm font-medium text-text-muted transition-colors hover:bg-sale/5 hover:text-sale"
                    >
                      <span className="flex size-8 shrink-0 items-center justify-center rounded-lg bg-surface-muted text-text-muted">
                        <LogoutIcon size={16} />
                      </span>
                      {t('dashboard.logout')}
                    </button>
                  </div>
                </>
              ) : (
                <div className="p-2">
                  <div className="rounded-lg border border-border/60 bg-surface-muted/40 px-4 py-4">
                    <p className="text-sm font-semibold text-text">{t('accountMenu.guestTitle')}</p>
                    <p className="mt-1.5 text-xs leading-relaxed text-text-muted">
                      {t('accountMenu.guestDescription')}
                    </p>
                  </div>
                  <ul className="mt-2 space-y-0.5 px-1 pb-1">
                    <li>
                      <Link
                        to="/account"
                        role="menuitem"
                        onClick={() => setOpen(false)}
                        className="flex items-center gap-3 rounded-lg px-3 py-2.5 text-sm font-semibold text-text transition-colors hover:bg-surface-muted"
                      >
                        <span className="flex size-8 shrink-0 items-center justify-center rounded-lg bg-warm text-warm-text">
                          <UserIcon size={16} />
                        </span>
                        {t('auth.signInTab')}
                      </Link>
                    </li>
                    <li>
                      <Link
                        to="/account?mode=signup"
                        role="menuitem"
                        onClick={() => setOpen(false)}
                        className="flex items-center gap-3 rounded-lg px-3 py-2.5 text-sm font-medium text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
                      >
                        <span className="flex size-8 shrink-0 items-center justify-center rounded-lg border border-border bg-surface text-warm">
                          <UserIcon size={16} />
                        </span>
                        {t('auth.signUpTab')}
                      </Link>
                    </li>
                  </ul>
                </div>
              )}
            </div>
          </div>
        </Portal>
      )}
    </>
  )
}
