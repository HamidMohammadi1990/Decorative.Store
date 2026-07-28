import { Link, useLocation } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { CompareIcon } from '@/components/compare/CompareIcon'
import { Container } from '@/components/ui/Container'
import { Button } from '@/components/ui/Button'

const QUICK_LINKS = [
  { to: '/', labelKey: 'notFound.links.home', descKey: 'notFound.links.homeDesc' },
  { to: '/shop', labelKey: 'notFound.links.shop', descKey: 'notFound.links.shopDesc' },
  { to: '/blog', labelKey: 'notFound.links.blog', descKey: 'notFound.links.blogDesc' },
  { to: '/compare', labelKey: 'notFound.links.compare', descKey: 'notFound.links.compareDesc' },
] as const

export function NotFoundPage() {
  const { t } = useTranslation()
  const location = useLocation()

  return (
    <div className="relative flex-1 overflow-hidden bg-gradient-to-b from-warm-soft/50 via-surface to-surface">
      <div
        aria-hidden
        className="pointer-events-none absolute -end-24 -top-24 size-72 rounded-full bg-warm/10 blur-3xl"
      />
      <div
        aria-hidden
        className="pointer-events-none absolute -start-16 bottom-20 size-56 rounded-full bg-accent/10 blur-3xl"
      />

      <Container className="relative py-16 md:py-24 lg:py-28">
        <div className="mx-auto max-w-3xl text-center">
          <div className="relative inline-block">
            <p
              aria-hidden
              className="select-none text-[clamp(5.5rem,22vw,11rem)] font-semibold leading-none tracking-tighter text-warm/12"
            >
              404
            </p>
            <div className="absolute inset-0 flex items-center justify-center">
              <span className="flex size-16 items-center justify-center rounded-full bg-gradient-to-br from-warm to-warm-hover text-warm-text shadow-lg ring-4 ring-warm/15 md:size-20">
                <LostIcon className="size-8 md:size-10" />
              </span>
            </div>
          </div>

          <p className="mt-6 text-xs font-semibold uppercase tracking-[0.18em] text-warm">
            {t('notFound.eyebrow')}
          </p>
          <h1 className="mt-3 text-2xl font-semibold tracking-tight text-text md:text-4xl">
            {t('notFound.title')}
          </h1>
          <p className="mx-auto mt-4 max-w-lg text-sm leading-relaxed text-text-muted md:text-base">
            {t('notFound.message')}
          </p>

          {location.pathname !== '/' && (
            <p className="mx-auto mt-4 max-w-md truncate rounded-sm border border-border bg-surface-muted/50 px-4 py-2 font-mono text-xs text-text-muted">
              {t('notFound.pathLabel')}: {location.pathname}
            </p>
          )}

          <div className="mt-8 flex flex-wrap items-center justify-center gap-3">
            <Link to="/">
              <Button variant="warm">{t('notFound.backHome')}</Button>
            </Link>
            <Link to="/shop">
              <Button variant="secondary">{t('common.continueShopping')}</Button>
            </Link>
          </div>
        </div>

        <div className="mx-auto mt-14 grid max-w-4xl gap-4 sm:grid-cols-2 lg:grid-cols-4">
          {QUICK_LINKS.map(({ to, labelKey, descKey }) => (
            <Link
              key={to}
              to={to}
              className="group rounded-sm border border-border bg-surface p-5 text-start shadow-sm transition-all duration-200 hover:-translate-y-0.5 hover:border-warm/40 hover:shadow-md"
            >
              <span className="flex size-10 items-center justify-center rounded-sm bg-warm-soft text-warm transition-colors group-hover:bg-warm group-hover:text-warm-text">
                {to === '/compare' ? <CompareIcon size={18} /> : <ArrowIcon />}
              </span>
              <p className="mt-4 text-sm font-semibold text-text group-hover:text-warm">
                {t(labelKey)}
              </p>
              <p className="mt-1 text-xs leading-relaxed text-text-muted">{t(descKey)}</p>
            </Link>
          ))}
        </div>

        <p className="mt-12 text-center text-xs text-text-muted">{t('notFound.hint')}</p>
      </Container>
    </div>
  )
}

function LostIcon({ className = '' }: { className?: string }) {
  return (
    <svg
      viewBox="0 0 24 24"
      fill="none"
      aria-hidden
      className={className}
    >
      <path
        d="M12 21s6-5.2 6-10a6 6 0 1 0-12 0c0 4.8 6 10 6 10Z"
        stroke="currentColor"
        strokeWidth="1.5"
      />
      <circle cx="12" cy="11" r="2" stroke="currentColor" strokeWidth="1.5" />
      <path
        d="M12 13v2"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </svg>
  )
}

function ArrowIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 18 18" fill="none" aria-hidden>
      <path
        d="M4 9h10M10 5l4 4-4 4"
        stroke="currentColor"
        strokeWidth="1.4"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  )
}
