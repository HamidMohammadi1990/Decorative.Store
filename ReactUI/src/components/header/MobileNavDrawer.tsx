import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { AppLink } from '@/models/shared/link.model'
import type { MegaMenuColumn } from '@/models/shared/megaMenu.model'
import type { BlogCategory } from '@/models/blog/blog.model'
import type { NavLinkGroup } from '@/models/shared/link.model'
import { BlogMobileNav } from '@/components/header/BlogNav'
import { navGroupHasPanel, splitJournalNav } from '@/extensions/flattenNavLinks'
import { MegaMenuLink, navShopAllLinkClass } from '@/components/header/MegaMenuLink'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { JournalIcon, NavCategoryIcon } from '@/components/ui/NavCategoryIcons'
import { NavGroupIcon, NavLinkIcon } from '@/components/ui/NavTreeIcons'
import { InlineLoading } from '@/components/ui/Spinner'
import { Portal } from '@/components/ui/Portal'

interface MobileNavDrawerProps {
  open: boolean
  onClose: () => void
  nav?: NavLinkGroup[]
  blogCategories?: BlogCategory[]
  blogNavLoading?: boolean
}

export function MobileNavDrawer({
  open,
  onClose,
  nav = [],
  blogCategories,
  blogNavLoading = false,
}: MobileNavDrawerProps) {
  const { t } = useTranslation()
  const [expanded, setExpanded] = useState<Record<string, boolean>>({})

  useEffect(() => {
    if (open) {
      setExpanded({})
    }
  }, [open])

  useEffect(() => {
    if (!open) return
    const prev = document.body.style.overflow
    document.body.style.overflow = 'hidden'
    return () => {
      document.body.style.overflow = prev
    }
  }, [open])

  if (!open) return null

  const isBlogNav = blogCategories != null
  const { journal, categories } = splitJournalNav(nav)

  const toggle = (id: string) => {
    setExpanded((prev) => ({ ...prev, [id]: !prev[id] }))
  }

  return (
    <Portal>
      <button
        type="button"
        className="fixed inset-0 z-[100] bg-black/50 lg:hidden"
        aria-label={t('common.close')}
        onClick={onClose}
      />
      <nav
        className="fixed inset-y-0 start-0 z-[110] flex w-[min(100%,20rem)] flex-col bg-surface text-text shadow-2xl lg:hidden"
        aria-label={isBlogNav ? t('blog.navLabel') : t('common.menu')}
      >
        <div className="flex shrink-0 items-center justify-between border-b border-border px-4 py-4">
          <span className="text-base font-semibold">
            {isBlogNav ? t('blog.title') : t('common.menu')}
          </span>
          <button
            type="button"
            onClick={onClose}
            aria-label={t('common.close')}
            className="flex size-9 items-center justify-center rounded-full text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
          >
            <CloseIcon />
          </button>
        </div>

        <div className="min-h-0 flex-1 overflow-y-auto overscroll-contain py-3">
          {isBlogNav ? (
            <BlogMobileNav
              categories={blogCategories}
              loading={blogNavLoading}
              onClose={onClose}
            />
          ) : !journal && categories.length === 0 ? (
            <InlineLoading className="px-4 py-8" />
          ) : (
            <ul className="px-2">
              {journal?.href && (
                <li className="border-b border-border">
                  <Link
                    to={journal.href}
                    className="flex items-center gap-2 px-2 py-2.5 text-base font-semibold text-text transition-colors hover:text-warm"
                    onClick={onClose}
                  >
                    <JournalIcon className="text-warm" />
                    {journal.label}
                  </Link>
                </li>
              )}
              {categories.map((group) => (
                <MobileNavCategory
                  key={group.id}
                  group={group}
                  isExpanded={expanded[group.id] ?? false}
                  onToggle={() => toggle(group.id)}
                  onClose={onClose}
                  shopAllLabel={t('common.shopAll', { category: group.label })}
                />
              ))}
            </ul>
          )}
        </div>
      </nav>
    </Portal>
  )
}

interface MobileNavCategoryProps {
  group: NavLinkGroup
  isExpanded: boolean
  onToggle: () => void
  onClose: () => void
  shopAllLabel: string
}

function MobileNavCategory({
  group,
  isExpanded,
  onToggle,
  onClose,
  shopAllLabel,
}: MobileNavCategoryProps) {
  const hasPanel = navGroupHasPanel(group)

  return (
    <li className="border-b border-border last:border-b-0">
      <div className="flex items-center gap-2 px-2 py-2.5">
        <NavCategoryIcon categoryId={group.id} />
        {hasPanel ? (
          <button
            type="button"
            className="flex flex-1 items-center gap-2 text-start text-base font-semibold text-text"
            aria-expanded={isExpanded}
            onClick={onToggle}
          >
            <span className="flex-1">{group.label}</span>
            <ChevronIcon expanded={isExpanded} className="text-text-muted" />
          </button>
        ) : (
          <Link
            to={group.href!}
            className="flex-1 text-base font-semibold text-text"
            onClick={onClose}
          >
            {group.label}
          </Link>
        )}
      </div>

      {hasPanel && isExpanded && (
        <div className="mb-3 ms-4 border-s-2 border-warm-muted ps-3">
          {group.href && (
            <Link
              to={group.href}
              className={`${navShopAllLinkClass} mb-3 w-full bg-warm-soft/80`}
              onClick={onClose}
            >
              <NavLinkIcon className="text-warm" />
              {shopAllLabel}
            </Link>
          )}

          {group.columns?.map((col) => (
            <MobileNavColumn
              key={col.title}
              column={col}
              onClose={onClose}
            />
          ))}

          {group.children && !group.columns && (
            <ul className="space-y-0.5">
              {group.children.map((link) => (
                <MobileNavLinkItem key={link.href} link={link} onClose={onClose} />
              ))}
            </ul>
          )}
        </div>
      )}
    </li>
  )
}

function MobileNavColumn({
  column,
  onClose,
}: {
  column: MegaMenuColumn
  onClose: () => void
}) {
  return (
    <div className="mb-4 last:mb-1">
      <div className="mb-1.5 flex items-center gap-2 py-1 ps-2">
        <NavGroupIcon />
        <span className="text-sm font-semibold text-text">
          {column.title}
        </span>
      </div>
      <ul className="ms-2 space-y-1 border-s border-warm-muted/70 ps-3">
        {column.links.map((link) => (
          <MobileNavLinkItem key={link.href} link={link} onClose={onClose} />
        ))}
      </ul>
    </div>
  )
}

function MobileNavLinkItem({
  link,
  onClose,
}: {
  link: AppLink
  onClose: () => void
}) {
  return (
    <li>
      <MegaMenuLink link={link} className="text-sm" onNavigate={onClose} />
    </li>
  )
}
