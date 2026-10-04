import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { Button } from '@/components/ui/Button'
import { Container } from '@/components/ui/Container'

export function CheckoutEmptyState() {
  const { t } = useTranslation()

  return (
    <Container className="py-16 sm:py-24">
      <div className="mx-auto max-w-md rounded-sm border border-border bg-surface px-6 py-12 text-center shadow-sm sm:px-10">
        <div className="mx-auto flex size-20 items-center justify-center rounded-full bg-warm-soft">
          <BagIcon />
        </div>
        <h1 className="mt-6 text-xl font-semibold text-text">{t('checkout.emptyTitle')}</h1>
        <p className="mt-2 text-sm leading-relaxed text-text-muted">
          {t('checkout.emptyMessage')}
        </p>
        <Link to="/" className="mt-8 inline-block w-full max-w-xs">
          <Button variant="warm" className="w-full">
            {t('common.continueShopping')}
          </Button>
        </Link>
      </div>
    </Container>
  )
}

function BagIcon() {
  return (
    <svg
      width="36"
      height="36"
      viewBox="0 0 36 36"
      fill="none"
      aria-hidden
      className="text-warm"
    >
      <path
        d="M10 12V10a8 8 0 1 1 16 0v2"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
      <path
        d="M8 12h20l-1.5 18H9.5L8 12Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
    </svg>
  )
}
