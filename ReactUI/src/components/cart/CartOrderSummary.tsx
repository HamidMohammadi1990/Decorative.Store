import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { ReactNode } from 'react'
import type { CurrencyConfig } from '@/models/shared/currency.model'
import type { CheckoutTotals } from '@/extensions/calculateCheckoutTotals'
import { FREE_SHIPPING_THRESHOLD } from '@/extensions/calculateCheckoutTotals'
import { formatMoney } from '@/extensions/formatMoney'
import type { ServerCartSummary } from '@/services/cartService'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { Button } from '@/components/ui/Button'

interface CartOrderSummaryProps {
  totals: CheckoutTotals
  currency: CurrencyConfig
  summary: ServerCartSummary | null
  trackingCode: number | null
  checkoutDisabled?: boolean
}

export function CartOrderSummary({
  totals,
  currency,
  summary,
  trackingCode,
  checkoutDisabled = false,
}: CartOrderSummaryProps) {
  const { t } = useTranslation()
  const freeShippingRemaining = Math.max(0, FREE_SHIPPING_THRESHOLD - totals.subtotal)
  const freeShippingProgress = Math.min(100, (totals.subtotal / FREE_SHIPPING_THRESHOLD) * 100)

  const discountAmount = summary?.isDiscountApplied ? summary.discountAmount : 0
  const adjustedSubtotal = summary?.isDiscountApplied ? summary.finalPrice : totals.subtotal
  const taxableBase = adjustedSubtotal + totals.shipping
  const estimatedTax = Math.round(taxableBase * 0.2 * 100) / 100
  const estimatedTotal = Math.round((taxableBase + estimatedTax) * 100) / 100
  const freeShippingRemainingLabel = formatMoney(
    { amount: freeShippingRemaining, currencyCode: currency.code },
    currency,
  )

  return (
    <div className="rounded-sm border border-border bg-surface shadow-sm lg:sticky lg:top-24">
      <div className="border-b border-border px-5 py-4">
        <h2 className="text-lg font-semibold text-text">{t('cartPage.orderSummary')}</h2>
        <p className="mt-1 text-sm text-text-muted">
          {t('checkout.itemCount', { count: totals.itemCount })}
        </p>
        {trackingCode ? (
          <p className="mt-2 font-mono text-xs text-text-muted" dir="ltr">
            {t('cartPage.orderRef', { code: trackingCode })}
          </p>
        ) : null}
      </div>

      {totals.fulfillment === 'delivery' && freeShippingRemaining > 0 && (
        <div className="border-b border-border bg-warm-soft/30 px-5 py-4">
          <p className="text-sm text-text">
            {t('cartPage.freeShippingRemaining', {
              amount: freeShippingRemainingLabel,
            })}
          </p>
          <div className="mt-2 h-2 overflow-hidden rounded-full bg-surface-muted">
            <div
              className="h-full rounded-full bg-warm transition-all duration-300"
              style={{ width: `${freeShippingProgress}%` }}
            />
          </div>
        </div>
      )}

      {totals.shippingIsFree && (
        <div className="border-b border-border bg-accent/5 px-5 py-3 text-sm font-medium text-accent">
          {t('cartPage.freeShippingUnlocked')}
        </div>
      )}

      <div className="space-y-2.5 px-5 py-4 text-sm">
        <SummaryRow
          label={t('common.subtotal')}
          value={
            <PriceDisplay
              money={{ amount: totals.subtotal, currencyCode: currency.code }}
              currency={currency}
            />
          }
        />

        {discountAmount > 0 && (
          <SummaryRow
            label={t('cartPage.discount')}
            value={
              <span className="font-medium text-accent">
                −{' '}
                <PriceDisplay
                  money={{ amount: discountAmount, currencyCode: currency.code }}
                  currency={currency}
                />
              </span>
            }
          />
        )}

        {summary?.isDiscountInvalidated && summary.discountInvalidationMessage && (
          <p className="rounded-sm bg-sale/10 px-3 py-2 text-xs text-sale">
            {summary.discountInvalidationMessage}
          </p>
        )}

        <SummaryRow
          label={t('cartPage.estimatedShipping')}
          value={
            totals.shipping === 0 ? (
              <span className="font-medium text-accent">{t('checkout.shippingFree')}</span>
            ) : (
              <PriceDisplay
                money={{ amount: totals.shipping, currencyCode: currency.code }}
                currency={currency}
              />
            )
          }
        />

        <SummaryRow
          label={t('checkout.estimatedTax')}
          value={
            <PriceDisplay
              money={{ amount: estimatedTax, currencyCode: currency.code }}
              currency={currency}
            />
          }
        />

        <div className="flex items-center justify-between border-t border-border pt-3 text-base font-semibold text-text">
          <span>{t('cartPage.estimatedTotal')}</span>
          <PriceDisplay
            money={{ amount: estimatedTotal, currencyCode: currency.code }}
            currency={currency}
          />
        </div>

        <p className="text-xs leading-relaxed text-text-muted">{t('cartPage.totalsNote')}</p>
      </div>

      <div className="space-y-2 border-t border-border px-5 py-4">
        <Link to="/checkout" className="block">
          <Button
            variant="warm"
            className="w-full py-3 text-sm font-semibold"
            disabled={checkoutDisabled}
          >
            {t('common.checkout')}
          </Button>
        </Link>

        <Link to="/room-layout" className="block">
          <Button variant="secondary" className="w-full">
            {t('roomLayout.openFromCart')}
          </Button>
        </Link>

        <Link to="/" className="block pt-1">
          <Button variant="ghost" className="w-full text-text-muted">
            {t('common.continueShopping')}
          </Button>
        </Link>
      </div>

      <div className="border-t border-border px-5 py-4">
        <ul className="space-y-2 text-xs text-text-muted">
          <TrustRow icon="lock">{t('cartPage.secureCheckout')}</TrustRow>
          <TrustRow icon="truck">{t('cartPage.deliveryEstimate')}</TrustRow>
          <TrustRow icon="return">{t('cartPage.easyReturns')}</TrustRow>
        </ul>
      </div>
    </div>
  )
}

function SummaryRow({ label, value }: { label: string; value: ReactNode }) {
  return (
    <div className="flex items-center justify-between gap-4 text-text-muted">
      <span>{label}</span>
      <span className="text-text">{value}</span>
    </div>
  )
}

function TrustRow({ icon, children }: { icon: 'lock' | 'truck' | 'return'; children: ReactNode }) {
  return (
    <li className="flex items-start gap-2.5">
      <span className="mt-0.5 flex size-5 shrink-0 items-center justify-center rounded-full bg-surface-muted text-warm">
        {icon === 'lock' && (
          <svg viewBox="0 0 16 16" className="size-3" aria-hidden>
            <path
              d="M5 7V5a3 3 0 1 1 6 0v2"
              fill="none"
              stroke="currentColor"
              strokeWidth="1.2"
            />
            <rect x="3.5" y="7" width="9" height="6" rx="1" fill="none" stroke="currentColor" strokeWidth="1.2" />
          </svg>
        )}
        {icon === 'truck' && (
          <svg viewBox="0 0 16 16" className="size-3" aria-hidden>
            <path
              d="M2 11h1.5M11.5 11H14M2 4h7v4H2zM9 6h2l2 2v2h-4z"
              fill="none"
              stroke="currentColor"
              strokeWidth="1.2"
              strokeLinejoin="round"
            />
          </svg>
        )}
        {icon === 'return' && (
          <svg viewBox="0 0 16 16" className="size-3" aria-hidden>
            <path
              d="M3 5h6a3 3 0 0 1 0 6H7M3 5l2-2M3 5l2 2"
              fill="none"
              stroke="currentColor"
              strokeWidth="1.2"
              strokeLinecap="round"
              strokeLinejoin="round"
            />
          </svg>
        )}
      </span>
      <span>{children}</span>
    </li>
  )
}
