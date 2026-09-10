import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { ReactNode } from 'react'
import type { CartLine } from '@/models/cart/cartLine.model'
import type { CurrencyConfig } from '@/models/shared/currency.model'
import type { CheckoutTotals } from '@/extensions/calculateCheckoutTotals'
import type { ServerCartSummary } from '@/services/cartService'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { Button } from '@/components/ui/Button'
import { LocalImage } from '@/components/ui/LocalImage'

interface CheckoutOrderSummaryProps {
  lines: CartLine[]
  totals: CheckoutTotals
  currency: CurrencyConfig
  summary?: ServerCartSummary | null
  discountSlot?: ReactNode
  showAction?: boolean
  actionLabel?: string
  actionHint?: string
  actionLoading?: boolean
  onAction?: () => void
  compact?: boolean
}

export function CheckoutOrderSummary({
  lines,
  totals,
  currency,
  summary = null,
  discountSlot,
  showAction = false,
  actionLabel,
  actionHint,
  actionLoading = false,
  onAction,
  compact = false,
}: CheckoutOrderSummaryProps) {
  const { t } = useTranslation()

  return (
    <div className="rounded-sm border border-border bg-surface shadow-sm">
      <div className="border-b border-border px-5 py-4">
        <h2 className="text-base font-semibold text-text">{t('checkout.orderSummary')}</h2>
        <p className="mt-1 text-sm text-text-muted">
          {t('checkout.itemCount', { count: totals.itemCount })}
        </p>
      </div>

      <ul className={`divide-y divide-border ${compact ? 'max-h-64 overflow-y-auto' : ''}`}>
        {lines.map((line) => {
          const lineTotal = line.unitPrice.amount * line.quantity
          return (
            <li key={line.lineId} className="flex gap-3 px-5 py-4">
              <div className="relative shrink-0">
                <LocalImage
                  image={line.image}
                  className="size-16 rounded-sm object-cover sm:size-20"
                />
                <span className="absolute -end-2 -top-2 flex size-5 items-center justify-center rounded-full bg-warm text-[10px] font-semibold text-warm-text">
                  {line.quantity}
                </span>
              </div>
              <div className="min-w-0 flex-1">
                <p className="text-sm font-medium leading-snug text-text">{line.title}</p>
                <p className="mt-1 flex flex-wrap items-center gap-1 text-xs text-text-muted">
                  <PriceDisplay money={line.unitPrice} currency={currency} iconSize={12} />
                  <span>× {line.quantity}</span>
                </p>
              </div>
              <PriceDisplay
                money={{ amount: lineTotal, currencyCode: line.unitPrice.currencyCode }}
                currency={currency}
                className="shrink-0 text-sm font-semibold text-text"
              />
            </li>
          )
        })}
      </ul>

      {discountSlot ? (
        <div className="border-t border-border">{discountSlot}</div>
      ) : null}

      <div className="space-y-2.5 border-t border-border px-5 py-4 text-sm">
        <SummaryRow
          label={t('common.subtotal')}
          value={
            <PriceDisplay
              money={{ amount: totals.subtotal, currencyCode: currency.code }}
              currency={currency}
            />
          }
        />

        {totals.discountAmount > 0 && (
          <SummaryRow
            label={t('cartPage.discount')}
            value={
              <span className="font-medium text-accent">
                −{' '}
                <PriceDisplay
                  money={{ amount: totals.discountAmount, currencyCode: currency.code }}
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
          label={
            totals.fulfillment === 'pickup'
              ? t('checkout.pickup')
              : t('checkout.shipping')
          }
          value={
            totals.shipping === 0 ? (
              t('checkout.shippingFree')
            ) : (
              <PriceDisplay
                money={{ amount: totals.shipping, currencyCode: currency.code }}
                currency={currency}
              />
            )
          }
          muted={totals.shipping === 0}
        />
        <SummaryRow
          label={t('checkout.estimatedTax')}
          value={
            <PriceDisplay
              money={{ amount: totals.tax, currencyCode: currency.code }}
              currency={currency}
            />
          }
        />
        <div className="flex items-center justify-between border-t border-border pt-3 text-base font-semibold text-text">
          <span>{t('checkout.total')}</span>
          <PriceDisplay
            money={{ amount: totals.total, currencyCode: currency.code }}
            currency={currency}
          />
        </div>
      </div>

      {showAction && onAction && (
        <div className="border-t border-border px-5 py-4">
          <Button
            type="button"
            variant="warm"
            className="w-full py-3 text-sm font-semibold"
            disabled={actionLoading}
            onClick={onAction}
          >
            {actionLoading
              ? t('checkout.placingOrder')
              : actionLabel ?? t('checkout.placeOrder')}
          </Button>
          {actionHint && (
            <p className="mt-3 text-center text-xs text-text-muted">{actionHint}</p>
          )}
        </div>
      )}

      {!showAction && (
        <div className="border-t border-border px-5 py-4">
          <Link to="/">
            <Button variant="secondary" className="w-full">
              {t('common.continueShopping')}
            </Button>
          </Link>
        </div>
      )}
    </div>
  )
}

function SummaryRow({
  label,
  value,
  muted,
}: {
  label: string
  value: ReactNode
  muted?: boolean
}) {
  return (
    <div className="flex items-center justify-between gap-4 text-text-muted">
      <span>{label}</span>
      <span className={muted ? 'font-medium text-warm' : 'text-text'}>{value}</span>
    </div>
  )
}
