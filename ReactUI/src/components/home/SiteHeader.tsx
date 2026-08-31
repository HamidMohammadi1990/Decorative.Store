import { useState } from 'react'
import { Link, useLocation } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { SiteHeader as SiteHeaderModel } from '@/models/home/siteHeader.model'
import { BlogMainNav } from '@/components/header/BlogNav'
import { HeaderSearch } from '@/components/header/HeaderSearch'
import { MainNav } from '@/components/header/MainNav'
import { MobileNavDrawer } from '@/components/header/MobileNavDrawer'
import { isBlogRoute } from '@/extensions/blogRoute'
import { useBlogNav } from '@/hooks/useBlogNav'
import { LanguageSwitcher } from '@/components/layout/LanguageSwitcher'
import { ThemeSwitcher } from '@/components/layout/ThemeSwitcher'
import { Container } from '@/components/ui/Container'
import { UserAccountMenu } from '@/components/header/UserAccountMenu'
import { CompareIcon } from '@/components/compare/CompareIcon'
import { CartIcon, LocationIcon } from '@/components/ui/HeaderIcons'
import { useCompareStore } from '@/stores/compareStore'
import { useCartStore } from '@/stores/cartStore'
import { useAddressStore } from '@/stores/addressStore'
import { useIsAuthenticated } from '@/stores/userStore'

interface SiteHeaderProps {
  data: SiteHeaderModel
}

export function SiteHeader({ data }: SiteHeaderProps) {
  const { t } = useTranslation()
  const location = useLocation()
  const onBlogRoute = isBlogRoute(location.pathname)
  const { categories: blogCategories, loading: blogNavLoading } = useBlogNav({
    enabled: onBlogRoute,
  })
  const [mobileOpen, setMobileOpen] = useState(false)
  const isAuthenticated = useIsAuthenticated()
  const itemCount = useCartStore((s) =>
    isAuthenticated ? s.lines.reduce((sum, line) => sum + line.quantity, 0) : 0,
  )
  const openCart = useCartStore((s) => s.openCart)
  const openAddressModal = useAddressStore((s) => s.openModal)
  const compareCount = useCompareStore((s) => s.slugs.length)
  return (
    <header className="sticky top-0 z-30 overflow-visible border-b border-border bg-surface">
      <Container className="flex flex-col gap-1 overflow-visible py-2">
        <div className="flex flex-wrap items-center gap-3">
          <button
            type="button"
            className="lg:hidden"
            aria-label={t('common.menu')}
            onClick={() => setMobileOpen(true)}
          >
            <MenuIcon />
          </button>

          <Link
            to="/"
            className="font-display text-xl font-semibold tracking-tight md:text-2xl"
          >
            {data.brandLabel}
          </Link>

          <HeaderSearch placeholder={data.searchPlaceholder} className="min-w-0 flex-1 max-lg:order-last max-lg:w-full" />

          <div className="ms-auto flex shrink-0 items-center gap-2 text-sm sm:gap-3">
            <ThemeSwitcher />
            <LanguageSwitcher />
            {isAuthenticated && (
              <button
                type="button"
                onClick={openAddressModal}
                className="inline-flex p-0.5 hover:text-warm"
                aria-label={t('address.headerLabel')}
              >
                <LocationIcon size={22} />
              </button>
            )}
            <UserAccountMenu accountLabel={data.accountLabel} />
            <Link
              to="/compare"
              className="relative inline-flex p-0.5 hover:text-warm"
              aria-label={`${t('compare.title')} (${compareCount})`}
            >
              <CompareIcon size={22} />
              <span
                aria-hidden
                className={`pointer-events-none absolute -top-0.5 -end-0.5 flex h-3.5 min-w-3.5 items-center justify-center rounded-full border-2 border-surface px-0.5 text-[9px] leading-none font-semibold tabular-nums shadow-sm ${
                  compareCount > 0
                    ? 'bg-warm text-warm-text'
                    : 'border-border bg-surface text-text-muted'
                }`}
              >
                {compareCount > 9 ? '9+' : compareCount}
              </span>
            </Link>
            {isAuthenticated && (
              <button
                type="button"
                onClick={openCart}
                className="relative inline-flex p-0.5 hover:text-accent"
                aria-label={`${data.cartLabel} (${itemCount})`}
              >
                <CartIcon size={22} />
                <span
                  aria-hidden
                  className={`pointer-events-none absolute -top-0.5 -end-0.5 flex h-3.5 min-w-3.5 items-center justify-center rounded-full border-2 border-surface px-0.5 text-[9px] leading-none font-semibold tabular-nums shadow-sm ${
                    itemCount > 0
                      ? 'bg-accent text-text-inverse'
                      : 'border-border bg-surface text-text-muted'
                  }`}
                >
                  {itemCount > 99 ? '99+' : itemCount}
                </span>
              </button>
            )}
          </div>
        </div>

        {onBlogRoute ? (
          <BlogMainNav categories={blogCategories} loading={blogNavLoading} />
        ) : (
          <MainNav items={data.primaryNav ?? []} />
        )}
      </Container>

      <MobileNavDrawer
        open={mobileOpen}
        onClose={() => setMobileOpen(false)}
        nav={onBlogRoute ? undefined : data.primaryNav ?? []}
        blogCategories={onBlogRoute ? blogCategories : undefined}
        blogNavLoading={onBlogRoute ? blogNavLoading : false}
      />
    </header>
  )
}

function MenuIcon() {
  return (
    <svg width="22" height="22" viewBox="0 0 24 24" fill="none" aria-hidden>
      <path
        d="M4 7h16M4 12h16M4 17h16"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </svg>
  )
}
