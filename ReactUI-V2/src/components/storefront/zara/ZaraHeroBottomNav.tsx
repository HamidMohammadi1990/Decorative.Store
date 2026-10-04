import { useLocation } from 'react-router-dom'
import { CatalogNavLink } from '@/components/routing/CatalogNavLink'
import type { AppLink } from '@/models/shared/link.model'
interface ZaraHeroBottomNavProps {
  items: AppLink[]
}

function navItemActive(pathname: string, href: string) {
  if (href === '/') return pathname === '/'
  return pathname === href || pathname.startsWith(`${href}/`)
}

export function ZaraHeroBottomNav({ items }: ZaraHeroBottomNavProps) {
  const { pathname } = useLocation()

  if (items.length === 0) return null

  return (
    <nav
      className="pointer-events-auto absolute inset-x-0 bottom-0 z-[3] px-4 pb-5 pt-3 md:px-8 md:pb-6"
      aria-label="Collections"
    >
      <ul className="zara-hero-bottom-nav flex items-center justify-center gap-x-5 gap-y-2 overflow-x-auto pb-1 md:gap-x-8">
        {items.map((item) => {
          const active = navItemActive(pathname, item.href)
          return (
            <li key={item.href} className="shrink-0">
              <CatalogNavLink
                href={item.href}
                className={`text-[9px] uppercase tracking-[0.16em] text-white transition-opacity hover:opacity-100 sm:text-[10px] md:text-[11px] ${
                  active ? 'font-semibold opacity-100' : 'font-normal opacity-90'
                }`}
              >
                {item.label}
              </CatalogNavLink>
            </li>
          )
        })}
      </ul>
    </nav>
  )
}
