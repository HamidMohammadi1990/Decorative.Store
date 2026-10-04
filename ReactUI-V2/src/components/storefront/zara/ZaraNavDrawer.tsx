import { useEffect, useLayoutEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { NavLinkGroup } from '@/models/shared/link.model'
import { CatalogNavLink } from '@/components/routing/CatalogNavLink'
import { ZaraHeaderSearch } from '@/components/storefront/zara/ZaraHeaderSearch'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { Portal } from '@/components/ui/Portal'
import { navGroupHasPanel, splitJournalNav } from '@/extensions/flattenNavLinks'
import { MegaMenuLink } from '@/components/header/MegaMenuLink'
import { LanguageSwitcher } from '@/components/layout/LanguageSwitcher'
import { ThemeSwitcher } from '@/components/layout/ThemeSwitcher'

const DRAWER_MS = 320

interface ZaraNavDrawerProps {
  open: boolean
  onClose: () => void
  brandLabel: string
  searchPlaceholder: string
  nav: NavLinkGroup[]
}

export function ZaraNavDrawer({
  open,
  onClose,
  brandLabel,
  searchPlaceholder,
  nav,
}: ZaraNavDrawerProps) {
  const { t } = useTranslation()
  const [expanded, setExpanded] = useState<Record<string, boolean>>({})
  const [present, setPresent] = useState(false)
  const [visible, setVisible] = useState(false)

  useEffect(() => {
    if (open) setExpanded({})
  }, [open])

  useLayoutEffect(() => {
    if (open) {
      setPresent(true)
      setVisible(false)
      return
    }
    setVisible(false)
  }, [open])

  useEffect(() => {
    if (!open || !present) return
    const timer = window.setTimeout(() => setVisible(true), 16)
    return () => window.clearTimeout(timer)
  }, [open, present])

  useEffect(() => {
    if (open || !present) return
    const timer = window.setTimeout(() => setPresent(false), DRAWER_MS)
    return () => window.clearTimeout(timer)
  }, [open, present])

  useEffect(() => {
    if (!present) return
    const prev = document.body.style.overflow
    document.body.style.overflow = 'hidden'
    return () => {
      document.body.style.overflow = prev
    }
  }, [present])

  useEffect(() => {
    if (!open) return
    const onKey = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose()
    }
    window.addEventListener('keydown', onKey)
    return () => window.removeEventListener('keydown', onKey)
  }, [open, onClose])

  if (!present) return null

  const { journal, categories } = splitJournalNav(nav)

  return (
    <Portal>
      <div className="storefront-zara zara-nav-drawer-root">
        <button
          type="button"
          className={`fixed inset-0 z-[480] cursor-default bg-black/30 transition-opacity duration-300 ease-out ${
            visible ? 'pointer-events-auto opacity-100' : 'pointer-events-none opacity-0'
          }`}
          aria-label={t('common.close')}
          onClick={onClose}
        />
        <aside
          className={`fixed inset-y-0 start-0 z-[490] flex w-[min(100%,20.5rem)] flex-col bg-surface text-text shadow-2xl transition-transform duration-300 ease-[cubic-bezier(0.22,1,0.36,1)] will-change-transform lg:w-[22rem] ${
            visible ? 'translate-x-0' : '-translate-x-full rtl:translate-x-full'
          }`}
          role="dialog"
          aria-modal="true"
          aria-label={t('common.menu')}
        >
          <div className="flex items-center justify-between border-b border-border px-5 py-4">
            <CatalogNavLink href="/" onClick={onClose} className="font-display text-xl tracking-[0.12em] uppercase">
              {brandLabel}
            </CatalogNavLink>
            <button type="button" className="sf-icon-btn" aria-label={t('common.close')} onClick={onClose}>
              <CloseIcon />
            </button>
          </div>
          <div className="border-b border-border px-5 py-4">
            <ZaraHeaderSearch placeholder={searchPlaceholder} tone="dark" className="max-w-none" />
          </div>
          <nav className="min-h-0 flex-1 overflow-y-auto px-3 py-4">
            <ul>
              {journal?.href && (
                <li className="border-b border-border/60">
                  <CatalogNavLink
                    href={journal.href}
                    onClick={onClose}
                    className="block px-2 py-3.5 text-[11px] uppercase tracking-[0.1em]"
                  >
                    {journal.label}
                  </CatalogNavLink>
                </li>
              )}
              {categories.map((group) => {
                const hasPanel = navGroupHasPanel(group)
                const isOpen = expanded[group.id] ?? false
                return (
                  <li key={group.id} className="border-b border-border/60">
                    {hasPanel ? (
                      <button
                        type="button"
                        className="flex w-full items-center justify-between px-2 py-3.5 text-start text-[11px] uppercase tracking-[0.1em]"
                        aria-expanded={isOpen}
                        onClick={() => setExpanded((p) => ({ ...p, [group.id]: !p[group.id] }))}
                      >
                        {group.label}
                        <ChevronIcon
                          expanded={isOpen}
                          className="size-3.5 opacity-50 transition-transform duration-300"
                        />
                      </button>
                    ) : group.href ? (
                      <CatalogNavLink
                        href={group.href}
                        onClick={onClose}
                        className="block px-2 py-3.5 text-[11px] uppercase tracking-[0.1em]"
                      >
                        {group.label}
                      </CatalogNavLink>
                    ) : null}
                    {hasPanel && (
                      <div
                        className={`grid transition-[grid-template-rows] duration-300 ease-out ${
                          isOpen ? 'grid-rows-[1fr]' : 'grid-rows-[0fr]'
                        }`}
                      >
                        <div className="min-h-0 overflow-hidden">
                          <ul className="space-y-2 border-s border-border/80 pb-4 ps-4 pt-1">
                            {group.columns?.flatMap((col) =>
                              col.links.map((link) => (
                                <li key={link.href}>
                                  <MegaMenuLink
                                    link={link}
                                    className="text-[11px] uppercase text-text-muted"
                                    onNavigate={onClose}
                                  />
                                </li>
                              )),
                            )}
                            {group.children?.map((link) => (
                              <li key={link.href}>
                                <MegaMenuLink
                                  link={link}
                                  className="text-[11px] uppercase text-text-muted"
                                  onNavigate={onClose}
                                />
                              </li>
                            ))}
                          </ul>
                        </div>
                      </div>
                    )}
                  </li>
                )
              })}
            </ul>
          </nav>
          <div className="flex flex-col gap-3 border-t border-border px-5 py-4">
            <ThemeSwitcher appearance="storefront" />
            <LanguageSwitcher className="border-border bg-surface-muted" />
          </div>
        </aside>
      </div>
    </Portal>
  )
}
