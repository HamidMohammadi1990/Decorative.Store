import { useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { SiteFooter as SiteFooterModel } from '@/models/home/siteFooter.model'
import { Container } from '@/components/ui/Container'
import { TextLink } from '@/components/ui/TextLink'
import { Button } from '@/components/ui/Button'
import { BackToTop } from '@/components/ui/BackToTop'
import { showToast } from '@/stores/toastStore'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { newsletterService } from '@/services/newsletterService'

interface SiteFooterProps {
  data: SiteFooterModel
}

export function SiteFooter({ data }: SiteFooterProps) {
  const { t } = useTranslation()
  const { languageId, locale } = useCurrentLanguageId()
  const [email, setEmail] = useState('')
  const [submitting, setSubmitting] = useState(false)

  async function handleSubscribe(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault()

    const trimmed = email.trim()
    if (!trimmed) {
      showToast(t('footer.newsletter.invalidEmail'), 'error')
      return
    }

    if (languageId == null) {
      showToast(t('footer.newsletter.error'), 'error')
      return
    }

    setSubmitting(true)
    try {
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
    <footer className="border-t border-border bg-surface-muted">
      <Container className="py-12">
        <div className="mb-10 max-w-md">
          <h2 className="text-sm font-semibold">{data.newsletterTitle}</h2>
          <form className="mt-3 flex gap-2" onSubmit={(event) => void handleSubscribe(event)}>
            <input
              type="email"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
              placeholder={data.newsletterPlaceholder}
              className="flex-1 rounded-sm border border-border bg-surface px-3 py-2 text-sm"
              disabled={submitting}
              autoComplete="email"
            />
            <Button type="submit" variant="warm" disabled={submitting}>
              {data.newsletterButton}
            </Button>
          </form>
        </div>

        <div className="grid gap-8 sm:grid-cols-2 lg:grid-cols-4">
          {data.columns.map((col) => (
            <div key={col.title}>
              <h3 className="text-xs font-semibold uppercase tracking-wider">
                {col.title}
              </h3>
              <ul className="mt-3 space-y-2 text-sm text-text-muted">
                {col.links.map((link) => (
                  <li key={link.href}>
                    <TextLink link={link} className="hover:text-text" />
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>

        <div className="relative mt-10 border-t border-border pt-8 text-xs text-text-muted">
          <BackToTop />
          <div className="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
            <p>{data.copyright}</p>
            <ul className="flex flex-wrap gap-4">
              {data.legalLinks.map((link) => (
                <li key={link.href}>
                  <TextLink link={link} />
                </li>
              ))}
            </ul>
          </div>
        </div>
      </Container>
    </footer>
  )
}
