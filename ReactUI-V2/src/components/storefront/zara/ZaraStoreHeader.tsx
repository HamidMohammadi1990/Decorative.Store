import { Link, useLocation } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { SiteHeader as SiteHeaderModel } from '@/models/home/siteHeader.model'
import { CatalogNavLink } from '@/components/routing/CatalogNavLink'
import { BlogMainNav } from '@/components/header/BlogNav'
import { ZaraHeaderSearch } from '@/components/storefront/zara/ZaraHeaderSearch'
import { UserAccountMenu } from '@/components/header/UserAccountMenu'
import { isBlogRoute } from '@/extensions/blogRoute'
import { useBlogNav } from '@/hooks/useBlogNav'
import { useCartStore } from '@/stores/cartStore'
import { useHydrated } from '@/hooks/useHydrated'
import { useIsAuthenticated } from '@/stores/userStore'
import { useStorefrontNavStore } from '@/stores/storefrontNavStore'
import { ZaraNavDrawer } from '@/components/storefront/zara/ZaraNavDrawer'
import { ThemeSwitcher } from '@/components/layout/ThemeSwitcher'
import { LanguageSwitcher } from '@/components/layout/LanguageSwitcher'

export type ZaraHeaderVariant = 'overlay' | 'solid'

interface ZaraStoreHeaderProps {
  data: SiteHeaderModel
  variant: ZaraHeaderVariant
}

const utilityLink =
  'text-[9px] font-normal uppercase tracking-[0.16em] transition-opacity hover:opacity-55 sm:text-[10px]'

export function ZaraStoreHeader({ data, variant }: ZaraStoreHeaderProps) {
  const { t } = useTranslation()
  const location = useLocation()
  const onBlogRoute = isBlogRoute(location.pathname)
  const { categories: blogCategories, loading: blogNavLoading } = useBlogNav({
    enabled: onBlogRoute,
  })
  const drawerOpen = useStorefrontNavStore((s) => s.menuOpen)
  const openMenu = useStorefrontNavStore((s) => s.openMenu)
  const closeMenu = useStorefrontNavStore((s) => s.closeMenu)
  const hydrated = useHydrated()
  const isAuthenticated = useIsAuthenticated()
  const showAuthChrome = hydrated && isAuthenticated
  const itemCount = useCartStore((s) =>
    showAuthChrome ? s.lines.reduce((sum, line) => sum + line.quantity, 0) : 0,
  )
  const openCart = useCartStore((s) => s.openCart)

  const overlay = variant === 'overlay'
  const chrome = overlay
    ? 'text-white'
    : 'sticky top-0 z-40 border-b border-border bg-surface text-text'

  const searchTone = overlay ? 'light' : 'dark'
  const themeAppearance = overlay ? 'storefront-overlay' : 'storefront'

  return (
    <>
      <header className={chrome}>
        <div className="px-4 py-4 md:px-10 md:py-5 lg:px-12">
          <div className="grid grid-cols-[1fr_auto_1fr] items-start gap-4">
            <div className="flex items-center gap-3 md:gap-4">
              <button
                type="button"
                className="sf-icon-btn relative z-10 -ms-0.5"
                aria-label={t('common.menu')}
                aria-expanded={drawerOpen}
                onClick={openMenu}
              >
                <MenuIcon />
              </button>
              <CatalogNavLink
                href="/"
                className={`font-display text-[clamp(1.125rem,2.2vw,1.625rem)] font-medium leading-none tracking-[0.1em] uppercase ${
                  overlay ? 'text-white' : 'text-text'
                }`}
              >
                {data.brandLabel}
              </CatalogNavLink>
            </div>

            <div className="hidden justify-self-center pt-0.5 sm:block">
              <ZaraHeaderSearch placeholder={data.searchPlaceholder} tone={searchTone} />
            </div>

            <div className="flex flex-wrap items-center justify-end gap-2 sm:gap-4 md:gap-5">
              <ThemeSwitcher appearance={themeAppearance} className="hidden sm:inline-flex" />
              <LanguageSwitcher
                className={
                  overlay
                    ? 'hidden border-white/30 bg-black/15 sm:inline-flex [&_button]:text-white/80 [&_button[aria-pressed=true]]:bg-white/25 [&_button[aria-pressed=true]]:text-white'
                    : 'hidden border-border bg-surface-muted sm:inline-flex'
                }
              />
              {showAuthChrome ? (
                <UserAccountMenu accountLabel={data.accountLabel} />
              ) : (
                <Link to="/account" className={utilityLink}>
                  {data.accountLabel}
                </Link>
              )}
              <Link to="/contact" className={`${utilityLink} hidden sm:inline`}>
                {t('storefront.help')}
              </Link>
              {showAuthChrome ? (
                <button
                  type="button"
                  onClick={openCart}
                  className={utilityLink}
                  aria-label={`${data.cartLabel} (${itemCount})`}
                >
                  {data.cartLabel}
                  {itemCount > 0 ? ` (${itemCount > 99 ? '99+' : itemCount})` : ''}
                </button>
              ) : (
                <Link to="/account" className={utilityLink}>
                  {data.cartLabel}
                </Link>
              )}
            </div>
          </div>

          <div className="mt-3 sm:hidden">
            <ZaraHeaderSearch placeholder={data.searchPlaceholder} tone={searchTone} className="max-w-none" />
          </div>

          {onBlogRoute && (
            <div className="mt-3 border-t border-border/30 pt-3">
              <BlogMainNav categories={blogCategories} loading={blogNavLoading} />
            </div>
          )}
        </div>
      </header>

      <ZaraNavDrawer
        open={drawerOpen}
        onClose={closeMenu}
        brandLabel={data.brandLabel}
        searchPlaceholder={data.searchPlaceholder}
        nav={data.primaryNav ?? []}
      />
    </>
  )
}

function MenuIcon() {
  return (
    <svg width="22" height="22" viewBox="0 0 24 24" fill="none" aria-hidden>
      <path d="M4 7h16M4 12h16M4 17h16" stroke="currentColor" strokeWidth="1.15" strokeLinecap="round" />
    </svg>
  )
}
