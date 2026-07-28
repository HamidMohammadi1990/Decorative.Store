import { useEffect, useLayoutEffect, useRef, useState } from 'react'
import { Link, useLocation, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { DashboardSection } from '@/models/dashboard/dashboard.model'
import {
  AddressesIcon,
  LogoutIcon,
  OrdersIcon,
  TransactionsIcon,
  WalletIcon,
} from '@/components/dashboard/DashboardIcons'
import { Portal } from '@/components/ui/Portal'
import { UserIcon } from '@/components/ui/HeaderIcons'
import { positionAnchorDropdown } from '@/extensions/positionAnchorDropdown'
import { useUserStore } from '@/stores/userStore'

interface UserAccountMenuProps {
  accountLabel: string
}

const menuItems: { section: DashboardSection; path: string; icon: typeof WalletIcon }[] = [
  { section: 'wallet', path: '/account/dashboard/wallet', icon: WalletIcon },
  { section: 'orders', path: '/account/dashboard/orders', icon: OrdersIcon },
  { section: 'transactions', path: '/account/dashboard/transactions', icon: TransactionsIcon },
  { section: 'addresses', path: '/account/dashboard/addresses', icon: AddressesIcon },
]

function getInitials(firstName: string, lastName: string) {
  const first = firstName.charAt(0).toUpperCase()
  const last = lastName ? lastName.charAt(0).toUpperCase() : ''
  return `${first}${last}` || first
}

export function UserAccountMenu({ accountLabel }: UserAccountMenuProps) {
  const { t } = useTranslation()
  const location = useLocation()
  const navigate = useNavigate()
  const user = useUserStore((s) => s.user)
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
  }, [open, user])

  const handleLogout = () => {
    logout()
    setOpen(false)
    navigate('/account')
  }

  const displayName = user
    ? user.lastName
      ? `${user.firstName} ${user.lastName}`
      : user.firstName
    : null

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
        className={`relative inline-flex p-0.5 transition-colors hover:text-warm ${
          open ? 'text-warm' : ''
        }`}
      >
        <UserIcon size={22} />
        {user && (
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
              className="w-64 overflow-hidden rounded-sm border border-border/80 bg-surface shadow-[0_12px_40px_-12px_rgba(0,0,0,0.2)]"
            >
              {user ? (
                <>
                  <div className="border-b border-border bg-gradient-to-br from-warm-soft via-surface to-surface px-4 py-4">
                    <div className="flex items-center gap-3">
                      <span className="flex size-10 shrink-0 items-center justify-center rounded-full bg-gradient-to-br from-warm to-warm-hover text-sm font-semibold text-warm-text shadow-sm">
                        {getInitials(user.firstName, user.lastName)}
                      </span>
                      <div className="min-w-0">
                        <p className="truncate text-sm font-semibold text-text">{displayName}</p>
                        <p className="truncate text-xs text-text-muted">{user.email}</p>
                      </div>
                    </div>
                    <Link
                      to="/account/dashboard/wallet"
                      role="menuitem"
                      onClick={() => setOpen(false)}
                      className="mt-3 inline-flex text-xs font-medium text-warm hover:underline"
                    >
                      {t('accountMenu.viewDashboard')}
                    </Link>
                  </div>

                  <ul className="p-2">
                    {menuItems.map(({ section, path, icon: Icon }) => (
                      <li key={section}>
                        <Link
                          to={path}
                          role="menuitem"
                          onClick={() => setOpen(false)}
                          className="flex items-center gap-3 rounded-sm px-3 py-2.5 text-sm font-medium text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
                        >
                          <span className="flex size-8 shrink-0 items-center justify-center rounded-sm bg-warm-soft text-warm">
                            <Icon size={16} />
                          </span>
                          {t(`dashboard.nav.${section}`)}
                        </Link>
                      </li>
                    ))}
                  </ul>

                  <div className="border-t border-border p-2">
                    <button
                      type="button"
                      role="menuitem"
                      onClick={handleLogout}
                      className="flex w-full items-center gap-3 rounded-sm px-3 py-2.5 text-sm font-medium text-text-muted transition-colors hover:bg-sale/5 hover:text-sale"
                    >
                      <span className="flex size-8 shrink-0 items-center justify-center rounded-sm bg-surface-muted text-text-muted">
                        <LogoutIcon size={16} />
                      </span>
                      {t('dashboard.logout')}
                    </button>
                  </div>
                </>
              ) : (
                <div className="p-2">
                  <div className="border-b border-border px-3 py-3">
                    <p className="text-sm font-semibold text-text">{t('accountMenu.guestTitle')}</p>
                    <p className="mt-1 text-xs leading-relaxed text-text-muted">
                      {t('accountMenu.guestDescription')}
                    </p>
                  </div>
                  <ul className="py-2">
                    <li>
                      <Link
                        to="/account"
                        role="menuitem"
                        onClick={() => setOpen(false)}
                        className="flex items-center gap-3 rounded-sm px-3 py-2.5 text-sm font-medium text-text transition-colors hover:bg-surface-muted"
                      >
                        <span className="flex size-8 shrink-0 items-center justify-center rounded-sm bg-warm text-warm-text">
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
                        className="flex items-center gap-3 rounded-sm px-3 py-2.5 text-sm font-medium text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
                      >
                        <span className="flex size-8 shrink-0 items-center justify-center rounded-sm border border-border bg-surface text-warm">
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
