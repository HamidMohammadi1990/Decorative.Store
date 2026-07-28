import { useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { CurrencyConfig } from '@/models/shared/currency.model'
import type { CheckoutTotals } from '@/extensions/calculateCheckoutTotals'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { Button } from '@/components/ui/Button'

interface CheckoutPaymentPanelProps {
  orderRef: string
  totals: CheckoutTotals
  currency: CurrencyConfig
  onPay: () => void
  paying: boolean
}

export function CheckoutPaymentPanel({
  orderRef,
  totals,
  currency,
  onPay,
  paying,
}: CheckoutPaymentPanelProps) {
  const { t } = useTranslation()
  const [gatewayNotice, setGatewayNotice] = useState(false)

  const handlePay = () => {
    setGatewayNotice(true)
    onPay()
  }

  return (
    <div className="mx-auto max-w-lg">
      <div className="rounded-sm border border-border bg-surface p-6 shadow-sm sm:p-8">
        <div className="text-center">
          <div className="mx-auto flex size-14 items-center justify-center rounded-full bg-warm-soft">
            <BankIcon />
          </div>
          <h2 className="mt-4 text-xl font-semibold text-text">{t('checkout.paymentPanelTitle')}</h2>
          <p className="mt-2 text-sm text-text-muted">{t('checkout.paymentPanelSubtitle')}</p>
        </div>

        <div className="mt-6 rounded-sm border border-border bg-surface-muted/50 p-4">
          <div className="flex justify-between text-sm">
            <span className="text-text-muted">{t('checkout.orderRef', { id: orderRef })}</span>
            <PriceDisplay
              money={{ amount: totals.total, currencyCode: currency.code }}
              currency={currency}
              className="font-semibold text-text"
            />
          </div>
        </div>

        <ul className="mt-5 space-y-2 text-xs text-text-muted">
          <li className="flex items-center gap-2">
            <span className="text-warm">✓</span>
            {t('checkout.paymentSecureTransfer')}
          </li>
          <li className="flex items-center gap-2">
            <span className="text-warm">✓</span>
            {t('checkout.paymentBankRedirect')}
          </li>
        </ul>

        {gatewayNotice && (
          <div className="mt-5 rounded-sm border border-warm-muted bg-warm-soft px-4 py-3 text-sm text-warm">
            {t('checkout.paymentGatewaySoon')}
          </div>
        )}

        <Button
          type="button"
          variant="warm"
          className="mt-6 w-full py-3 text-sm font-semibold"
          disabled={paying}
          onClick={handlePay}
        >
          {paying ? t('checkout.redirectingToGateway') : t('checkout.payNow')}
        </Button>

        <p className="mt-4 text-center text-xs text-text-muted">
          {t('checkout.paymentDisclaimer')}
        </p>
      </div>
    </div>
  )
}

function BankIcon() {
  return (
    <svg width="28" height="28" viewBox="0 0 24 24" fill="none" aria-hidden className="text-warm">
      <path
        d="M3 10h18M5 10V18M9 10V18M15 10V18M19 10V18M2 18h20M12 3l9 5H3l9-5Z"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinejoin="round"
      />
    </svg>
  )
}
