import { useCallback, useEffect, useRef, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { NavLinkGroup } from '@/models/shared/link.model'
import { navGroupHasPanel, splitJournalNav } from '@/extensions/flattenNavLinks'
import { MegaMenuDropdown } from '@/components/header/MegaMenuDropdown'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { JournalIcon } from '@/components/ui/NavCategoryIcons'
import { ScrollArrowButton } from '@/components/ui/ScrollArrowButton'
import { useHorizontalScrollArrows } from '@/hooks/useHorizontalScrollArrows'

interface MainNavProps {
  items: NavLinkGroup[]
}

const CLOSE_DELAY_MS = 120

export function MainNav({ items }: MainNavProps) {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const [openId, setOpenId] = useState<string | null>(null)
  const navRef = useRef<HTMLElement>(null)
  const itemRefs = useRef<Record<string, HTMLLIElement | null>>({})
  const closeTimer = useRef<ReturnType<typeof setTimeout> | null>(null)
  const {
    scrollRef,
    canScrollPrev,
    canScrollNext,
    hasOverflow,
    scrollPrev,
    scrollNext,
  } = useHorizontalScrollArrows()

  const clearCloseTimer = useCallback(() => {
    if (closeTimer.current) {
      clearTimeout(closeTimer.current)
      closeTimer.current = null
    }
  }, [])

  const scheduleClose = useCallback(() => {
    clearCloseTimer()
    closeTimer.current = setTimeout(() => setOpenId(null), CLOSE_DELAY_MS)
  }, [clearCloseTimer])

  const openItem = useCallback(
    (id: string) => {
      clearCloseTimer()
      setOpenId(id)
    },
    [clearCloseTimer],
  )

  useEffect(() => {
    const onDocClick = (e: MouseEvent) => {
      const target = e.target as Node
      if (navRef.current?.contains(target)) return
      if (target instanceof Element && target.closest('[data-mega-menu]')) return
      setOpenId(null)
    }
    document.addEventListener('click', onDocClick)
    return () => document.removeEventListener('click', onDocClick)
  }, [])

  useEffect(() => () => clearCloseTimer(), [clearCloseTimer])

  const { journal, categories } = splitJournalNav(items)

  if (!journal && !categories.length) {
    return null
  }

  const alignEndFromIndex = Math.ceil(categories.length * 0.55)
  const openItemData = openId ? categories.find((i) => i.id === openId) : null
  const openIndex = openId ? categories.findIndex((i) => i.id === openId) : -1
  const openAnchor = openId ? itemRefs.current[openId] : null

  return (
    <nav
      ref={navRef}
      className="relative hidden min-w-0 items-center gap-2 lg:flex"
      aria-label="Primary"
    >
      {hasOverflow && (
        <ScrollArrowButton
          direction="prev"
          disabled={!canScrollPrev}
          label={t('common.scrollNavPrev')}
          onClick={scrollPrev}
        />
      )}

      <div
        ref={scrollRef}
        className="main-nav-scroll min-w-0 flex-1 overflow-x-auto"
      >
        <ul className="flex w-max flex-nowrap items-center gap-x-4 xl:gap-x-5">
          {journal?.href && (
            <li className="relative shrink-0">
              <Link
                to={journal.href}
                className="inline-flex items-center gap-1.5 rounded-full bg-warm-soft/70 px-3 py-1.5 text-sm font-semibold tracking-wide whitespace-nowrap text-warm transition-colors hover:bg-warm-soft hover:text-warm"
              >
                <JournalIcon className="text-warm" />
                {journal.label}
              </Link>
            </li>
          )}

          {journal && categories.length > 0 && (
            <li
              className="h-5 w-px shrink-0 bg-border"
              aria-hidden
            />
          )}

          {categories.map((item) => {
            const hasPanel = navGroupHasPanel(item)
            const isOpen = openId === item.id

            return (
              <li
                key={item.id}
                ref={(el) => {
                  itemRefs.current[item.id] = el
                }}
                className="relative shrink-0"
                onMouseEnter={() => hasPanel && openItem(item.id)}
                onMouseLeave={() => hasPanel && scheduleClose()}
              >
                {item.href && !hasPanel ? (
                  <Link
                    to={item.href}
                    className="inline-block py-2 text-sm font-medium tracking-wide whitespace-nowrap transition-colors hover:text-accent"
                  >
                    {item.label}
                  </Link>
                ) : (
                  <button
                    type="button"
                    aria-expanded={isOpen}
                    aria-haspopup={hasPanel ? 'true' : undefined}
                    onClick={(e) => {
                      e.stopPropagation()
                      if (!hasPanel && item.href) {
                        navigate(item.href)
                        return
                      }
                      setOpenId(isOpen ? null : item.id)
                    }}
                    className={`inline-flex items-center gap-1 py-2 text-sm font-medium tracking-wide whitespace-nowrap transition-colors hover:text-accent ${
                      isOpen ? 'text-accent' : ''
                    }`}
                  >
                    {item.label}
                    {hasPanel && <ChevronIcon expanded={isOpen} />}
                  </button>
                )}
              </li>
            )
          })}
        </ul>
      </div>

      {hasOverflow && (
        <ScrollArrowButton
          direction="next"
          disabled={!canScrollNext}
          label={t('common.scrollNavNext')}
          onClick={scrollNext}
        />
      )}

      {openItemData && openAnchor && navGroupHasPanel(openItemData) && (
        <MegaMenuDropdown
          group={openItemData}
          anchorEl={openAnchor}
          alignEnd={openIndex >= alignEndFromIndex}
          scrollEl={scrollRef}
          onClose={scheduleClose}
          onCancelClose={clearCloseTimer}
        />
      )}
    </nav>
  )
}
