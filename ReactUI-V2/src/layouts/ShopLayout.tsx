import { useRef } from 'react'
import { Outlet, useLocation } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { PromoAnnouncementBar } from '@/components/home/PromoAnnouncementBar'
import { CompareFloatingBar } from '@/components/compare/CompareFloatingBar'
import { CartDrawer } from '@/components/cart/CartDrawer'
import { AddressBookModal } from '@/components/address/AddressBookModal'
import { LoginModal } from '@/components/auth/LoginModal'
import { StoryViewerModal } from '@/components/stories/StoryViewerModal'
import { useTheme } from '@/hooks/useTheme'
import { Spinner } from '@/components/ui/Spinner'
import { HomePageProvider, useHomePage } from '@/providers/HomePageProvider'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useShopChromeHeight } from '@/hooks/useShopChromeHeight'
import { useResumePendingCartAdd } from '@/hooks/useResumePendingCartAdd'
import { useCartSync } from '@/hooks/useCartSync'
import { useAddressSync } from '@/hooks/useAddressSync'
import { useWishlistSync } from '@/hooks/useWishlistSync'
import { useHydrated } from '@/hooks/useHydrated'
import { useIsAuthenticated } from '@/stores/userStore'
import { AiAssistantWidget } from '@/components/assistant/AiAssistantWidget'
import { useShopPageMetaDefaults } from '@/hooks/useShopPageMeta'
import { useStorefrontFonts } from '@/hooks/useStorefrontFonts'
import { useSettingsStore } from '@/stores/settingsStore'
import { ZaraStoreHeader } from '@/components/storefront/zara/ZaraStoreHeader'
import { ZaraStoreFooter } from '@/components/storefront/zara/ZaraStoreFooter'
import { FloatingBackToTop } from '@/components/ui/BackToTop'
import '@/styles/storefront-zara.css'

function isHomePath(pathname: string) {
  return pathname === '/' || pathname === ''
}

function ShopLayoutContent() {
  const { t } = useTranslation()
  const location = useLocation()
  const isHome = isHomePath(location.pathname)
  const chromeRef = useRef<HTMLDivElement>(null)
  const hydrated = useHydrated()
  const isAuthenticated = useIsAuthenticated()
  const showAuthChrome = hydrated && isAuthenticated
  const locale = useSettingsStore((s) => s.locale)
  useLocaleSettings()
  useStorefrontFonts(locale)
  useTheme()
  useResumePendingCartAdd()
  useCartSync()
  useAddressSync()
  useWishlistSync()
  const { data, loading } = useHomePage()
  useShopPageMetaDefaults()
  useShopChromeHeight(chromeRef, !loading && !!data && !isHome)

  return (
    <div className="storefront-zara flex min-h-svh flex-col bg-surface text-text">
      {!data ? (
        <header className="border-b border-border bg-surface">
          <div className="sf-container flex items-center justify-between py-3">
            <span className="font-display text-xl tracking-[0.12em] uppercase md:text-2xl">
              {t('common.brandName')}
            </span>
            {loading && <Spinner size="sm" />}
          </div>
        </header>
      ) : (
        !isHome && (
          <div ref={chromeRef}>
            <PromoAnnouncementBar data={data.promoAnnouncement} />
            <ZaraStoreHeader data={data.header} variant="solid" />
          </div>
        )
      )}

      <main className="flex-1">
        <Outlet />
      </main>

      {data && <ZaraStoreFooter footer={data.footer} header={data.header} />}
      {showAuthChrome && <CartDrawer />}
      <CompareFloatingBar />
      {showAuthChrome && <AddressBookModal />}
      <LoginModal />
      <StoryViewerModal />
      <AiAssistantWidget />
      <FloatingBackToTop />
    </div>
  )
}

export function ShopLayout() {
  return (
    <HomePageProvider>
      <ShopLayoutContent />
    </HomePageProvider>
  )
}
