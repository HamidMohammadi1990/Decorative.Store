import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { SiteFooter as SiteFooterModel } from '@/models/home/siteFooter.model'
import type { SiteHeader as SiteHeaderModel } from '@/models/home/siteHeader.model'
import { CatalogNavLink } from '@/components/routing/CatalogNavLink'
import { TextLink } from '@/components/ui/TextLink'
import { showToast } from '@/stores/toastStore'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { languageService } from '@/services/languageService'
import { newsletterService } from '@/services/newsletterService'
import { useHydrated } from '@/hooks/useHydrated'
import { useIsAuthenticated } from '@/stores/userStore'
import { useCartStore } from '@/stores/cartStore'
import { ThemeSwitcher } from '@/components/layout/ThemeSwitcher'

interface ZaraStoreFooterProps {
  footer: SiteFooterModel
  header: SiteHeaderModel
}

const utilityLink =
  'text-[9px] font-normal uppercase tracking-[0.16em] transition-opacity hover:opacity-55 sm:text-[10px]'

export function ZaraStoreFooter({ footer, header }: ZaraStoreFooterProps) {
  const { t } = useTranslation()
  const { locale } = useLocaleSettings()
  const hydrated = useHydrated()
  const isAuthenticated = useIsAuthenticated()
  const showAuthChrome = hydrated && isAuthenticated
  const openCart = useCartStore((s) => s.openCart)
  const itemCount = useCartStore((s) =>
    showAuthChrome ? s.lines.reduce((sum, line) => sum + line.quantity, 0) : 0,
  )
  const [email, setEmail] = useState('')
  const [submitting, setSubmitting] = useState(false)

  async function handleSubscribe(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const trimmed = email.trim()
    if (!trimmed) {
      showToast(t('footer.newsletter.invalidEmail'), 'error')
      return
    }
    setSubmitting(true)
    try {
      const languageId = await languageService.resolveLanguageId(locale)
      await newsletterService.subscribe(locale, languageId, trimmed)
      setEmail('')
      showToast(t('footer.newsletter.success'), 'success')
    } catch {
      showToast(t('footer.newsletter.error'), 'error')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <footer className="border-t border-border bg-surface text-text">
      <div className="border-b border-border px-4 py-8 md:px-10 lg:px-12">
        <div className="flex flex-col gap-8 lg:grid lg:grid-cols-[1fr_auto_1fr] lg:items-start lg:gap-6">
          <div className="lg:col-start-1">
            <CatalogNavLink
              href="/"
              className="font-display text-[clamp(1.125rem,2vw,1.5rem)] font-medium tracking-[0.1em] uppercase"
            >
              {header.brandLabel}
            </CatalogNavLink>
          </div>

          <div className="col-span-full flex flex-col items-center sm:col-span-1 sm:col-start-2 sm:row-start-1">
            <form className="w-full max-w-md text-center" onSubmit={(e) => void handleSubscribe(e)}>
              <p className="text-[9px] uppercase tracking-[0.14em] text-text-muted">
                {footer.newsletterTitle}
              </p>
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder={footer.newsletterPlaceholder}
                disabled={submitting}
                autoComplete="email"
                className="mt-2 w-full border-0 border-b border-border bg-transparent pb-2 text-center text-[10px] uppercase tracking-[0.12em] text-text outline-none placeholder:text-text-muted focus:border-text"
              />
            </form>
          </div>

          <div className="flex flex-wrap items-center gap-3 sm:justify-end lg:col-start-3 lg:justify-end sm:gap-4">
            <ThemeSwitcher appearance="storefront" className="sm:me-1" />
            <Link to="/account" className={utilityLink}>{header.accountLabel}</Link>
            <Link to="/contact" className={`${utilityLink} hidden sm:inline`}>{t('storefront.help')}</Link>
            {showAuthChrome ? (
              <button type="button" className={utilityLink} onClick={openCart}>
                {header.cartLabel}
                {itemCount > 0 ? ` (${itemCount})` : ''}
              </button>
            ) : (
              <Link to="/account" className={utilityLink}>{header.cartLabel}</Link>
            )}
          </div>
        </div>

        <div className="mt-10 border-t border-border pt-8">
          <p className="text-[10px] uppercase tracking-[0.14em]">{t('storefront.followUs', { defaultValue: 'Follow us on' })}</p>
          <ul className="mt-3 flex flex-wrap gap-4 text-text" aria-label={t('storefront.followUs', { defaultValue: 'Follow us on' })}>
            <SocialPlaceholder label="Instagram" />
            <SocialPlaceholder label="Pinterest" />
            <SocialPlaceholder label="YouTube" />
          </ul>
        </div>
      </div>

      <div className="sf-container py-12">
        <div className="grid gap-10 sm:grid-cols-2 lg:grid-cols-4">
          {footer.columns.map((col) => (
            <div key={col.title}>
              <h3 className="text-[10px] font-medium uppercase tracking-[0.16em]">{col.title}</h3>
              <ul className="mt-4 space-y-2.5 text-[11px] uppercase tracking-[0.08em] text-text-muted">
                {col.links.map((link) => (
                  <li key={link.href}>
                    <TextLink link={link} className="hover:text-text" />
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>

        <div className="mt-12 border-t border-border pt-8 text-[10px] uppercase tracking-[0.1em] text-text-muted">
          <p>{footer.copyright}</p>
          <ul className="mt-4 flex flex-wrap gap-4">
            {footer.legalLinks.map((link) => (
              <li key={link.href}>
                <TextLink link={link} />
              </li>
            ))}
          </ul>
        </div>
      </div>
    </footer>
  )
}

function SocialPlaceholder({ label }: { label: string }) {
  return (
    <li>
      <span className="inline-flex size-5 items-center justify-center rounded-full border border-current text-[9px] font-semibold">
        {label[0]}
      </span>
    </li>
  )
}
