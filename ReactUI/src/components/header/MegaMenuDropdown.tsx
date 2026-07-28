import { useLayoutEffect, useRef, type RefObject } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { NavLinkGroup } from '@/models/shared/link.model'
import { MegaMenuPanel } from '@/components/header/MegaMenuPanel'
import { navShopAllLinkClass } from '@/components/header/MegaMenuLink'
import { Portal } from '@/components/ui/Portal'
import { positionMegaMenuDropdown } from '@/extensions/positionMegaMenu'

interface MegaMenuDropdownProps {
  group: NavLinkGroup
  anchorEl: HTMLElement
  alignEnd: boolean
  scrollEl?: RefObject<HTMLElement | null>
  onClose: () => void
  onCancelClose: () => void
}

export function MegaMenuDropdown({
  group,
  anchorEl,
  alignEnd,
  scrollEl,
  onClose,
  onCancelClose,
}: MegaMenuDropdownProps) {
  const { t } = useTranslation()
  const panelRef = useRef<HTMLDivElement>(null)

  useLayoutEffect(() => {
    const updatePosition = () => {
      const panel = panelRef.current
      if (!panel) return
      positionMegaMenuDropdown(panel, anchorEl, alignEnd)
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

    const scrollContainer = scrollEl?.current
    scrollContainer?.addEventListener('scroll', updatePosition, { passive: true })

    return () => {
      observer?.disconnect()
      window.removeEventListener('resize', updatePosition)
      window.removeEventListener('scroll', updatePosition, true)
      scrollContainer?.removeEventListener('scroll', updatePosition)
    }
  }, [anchorEl, alignEnd, scrollEl, group])

  return (
    <Portal>
      <div
        ref={panelRef}
        data-mega-menu
        className="fixed z-[60]"
        style={{ paddingTop: 8, marginTop: -8 }}
        onMouseEnter={onCancelClose}
        onMouseLeave={onClose}
      >
        <div
          data-mega-menu-inner
          className="w-max max-w-[calc(100vw-2rem)] overflow-hidden rounded-lg border border-border/80 bg-surface shadow-[0_12px_40px_-12px_rgba(0,0,0,0.18)]"
        >
          {group.href && (
            <div className="border-b border-warm-muted/60 bg-warm-soft/70 px-8 py-3.5">
              <Link to={group.href} className={`group ${navShopAllLinkClass}`}>
                {t('common.shopAll', { category: group.label })}
                <span aria-hidden className="transition-transform duration-200 group-hover:translate-x-0.5 rtl:group-hover:-translate-x-0.5">
                  →
                </span>
              </Link>
            </div>
          )}
          <MegaMenuPanel group={group} />
        </div>
      </div>
    </Portal>
  )
}
