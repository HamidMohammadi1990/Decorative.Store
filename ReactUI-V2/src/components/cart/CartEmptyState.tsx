import { useTranslation } from 'react-i18next'
import { Button } from '@/components/ui/Button'

interface CartEmptyStateProps {
  onContinue: () => void
}

export function CartEmptyState({ onContinue }: CartEmptyStateProps) {
  const { t } = useTranslation()

  return (
    <div className="flex flex-1 flex-col items-center justify-center px-6 py-14 text-center">
      <div className="flex size-20 items-center justify-center rounded-full bg-surface-muted">
        <BagIcon />
      </div>
      <h3 className="mt-5 text-base font-semibold text-text">
        {t('common.emptyCartTitle')}
      </h3>
      <p className="mt-2 max-w-[16rem] text-sm leading-relaxed text-text-muted">
        {t('common.emptyCartMessage')}
      </p>
      <a href="/" onClick={onContinue} className="mt-6 w-full max-w-xs">
        <Button variant="secondary" className="w-full">
          {t('common.continueShopping')}
        </Button>
      </a>
    </div>
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
      className="text-text-muted"
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
