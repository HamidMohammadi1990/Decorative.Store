import { Outlet } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { PromoAnnouncementBar } from '@/components/home/PromoAnnouncementBar'
import { SiteHeader } from '@/components/home/SiteHeader'
import { CartDrawer } from '@/components/cart/CartDrawer'
import { AddressBookModal } from '@/components/address/AddressBookModal'
import { LanguageSwitcher } from '@/components/layout/LanguageSwitcher'
import { ThemeSwitcher } from '@/components/layout/ThemeSwitcher'
import { useTheme } from '@/hooks/useTheme'
import { Container } from '@/components/ui/Container'
import { Spinner } from '@/components/ui/Spinner'
import { HomePageProvider, useHomePage } from '@/providers/HomePageProvider'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'

function AuthLayoutContent() {
  const { t } = useTranslation()
  useLocaleSettings()
  useTheme()
  const { data, loading } = useHomePage()

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
        <>
          <PromoAnnouncementBar data={data.promoAnnouncement} />
          <SiteHeader data={data.header} />
        </>
      )}

      <main className="flex min-h-[calc(100svh-4rem)] flex-col bg-surface-muted">
        <Outlet />
      </main>

      <CartDrawer />
      <AddressBookModal />
    </>
  )
}

export function AuthLayout() {
  return (
    <HomePageProvider>
      <AuthLayoutContent />
    </HomePageProvider>
  )
}
