import { Outlet } from 'react-router-dom'
import { useRef } from 'react'
import { useTranslation } from 'react-i18next'
import { PromoAnnouncementBar } from '@/components/home/PromoAnnouncementBar'
import { SiteFooter } from '@/components/home/SiteFooter'
import { SiteHeader } from '@/components/home/SiteHeader'
import { CompareFloatingBar } from '@/components/compare/CompareFloatingBar'
import { CartDrawer } from '@/components/cart/CartDrawer'
import { AddressBookModal } from '@/components/address/AddressBookModal'
import { LanguageSwitcher } from '@/components/layout/LanguageSwitcher'
import { ThemeSwitcher } from '@/components/layout/ThemeSwitcher'
import { StoryViewerModal } from '@/components/stories/StoryViewerModal'
import { StoriesStrip } from '@/components/stories/StoriesStrip'
import { useTheme } from '@/hooks/useTheme'
import { Container } from '@/components/ui/Container'
import { Spinner } from '@/components/ui/Spinner'
import { HomePageProvider, useHomePage } from '@/providers/HomePageProvider'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useShopChromeHeight } from '@/hooks/useShopChromeHeight'
import { AiAssistantWidget } from '@/components/assistant/AiAssistantWidget'

function ShopLayoutContent() {
  const { t } = useTranslation()
  const chromeRef = useRef<HTMLDivElement>(null)
  useLocaleSettings()
  useTheme()
  const { data, loading } = useHomePage()
  useShopChromeHeight(chromeRef, !loading && !!data)

  return (
    <>
      {loading || !data ? (
        <header className="border-b border-border bg-surface">
          <Container className="flex items-center justify-between py-3">
            <span className="font-display text-xl font-semibold tracking-tight md:text-2xl">
              {t('common.brandName')}
            </span>
            <div className="flex items-center gap-2 sm:gap-3">
              <ThemeSwitcher />
              <LanguageSwitcher />
              <Spinner size="sm" />
            </div>
          </Container>
        </header>
      ) : (
        <div ref={chromeRef}>
          <PromoAnnouncementBar data={data.promoAnnouncement} />
          <SiteHeader data={data.header} />
          <StoriesStrip />
        </div>
      )}

      <Outlet />

      {data && <SiteFooter data={data.footer} />}
      <CartDrawer />
      <CompareFloatingBar />
      <AddressBookModal />
      <StoryViewerModal />
      <AiAssistantWidget />
    </>
  )
}

export function ShopLayout() {
  return (
    <HomePageProvider>
      <ShopLayoutContent />
    </HomePageProvider>
  )
}
