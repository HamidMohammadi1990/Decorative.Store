import type { ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import type { CartLine } from '@/models/cart/cartLine.model'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import type { CurrencyConfig } from '@/models/shared/currency.model'
import type { CheckoutTotals, DeliveryMethod, FulfillmentType } from '@/extensions/calculateCheckoutTotals'
import { formatSavedAddressLines } from '@/extensions/formatSavedAddress'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { LocalImage } from '@/components/ui/LocalImage'

interface CheckoutReviewPanelProps {
  email: string
  fulfillment: FulfillmentType
  delivery: DeliveryMethod
  selectedAddress: SavedAddress | null
  lines: CartLine[]
  totals: CheckoutTotals
  currency: CurrencyConfig
  orderRef: string
  onEditDetails: () => void
}

export function CheckoutReviewPanel({
  email,
  fulfillment,
  delivery,
  selectedAddress,
  lines,
  totals,
  currency,
  orderRef,
  onEditDetails,
}: CheckoutReviewPanelProps) {
  const { t } = useTranslation()

  return (
    <div className="space-y-5">
      <div className="rounded-sm border border-border bg-surface p-5 shadow-sm sm:p-6">
        <div className="flex flex-wrap items-start justify-between gap-3 border-b border-border pb-4">
          <div>
            <h2 className="text-lg font-semibold text-text">{t('checkout.reviewTitle')}</h2>
            <p className="mt-1 text-sm text-text-muted">{t('checkout.reviewSubtitle')}</p>
          </div>
          <button
            type="button"
            onClick={onEditDetails}
            className="text-sm font-medium text-warm hover:underline"
          >
            {t('checkout.editDetails')}
          </button>
        </div>

        <p className="mt-4 inline-flex rounded-sm bg-warm-soft px-3 py-1.5 text-xs font-semibold text-warm">
          {t('checkout.orderRef', { id: orderRef })}
        </p>

        <div className="mt-6 grid gap-4 sm:grid-cols-2">
          <ReviewBlock title={t('checkout.contactTitle')}>
            <p className="text-sm text-text">{email}</p>
          </ReviewBlock>

          <ReviewBlock title={t('checkout.fulfillmentTitle')}>
            <p className="text-sm font-medium text-text">
              {fulfillment === 'pickup'
                ? t('checkout.fulfillmentPickup')
                : t('checkout.fulfillmentDelivery')}
            </p>
            {fulfillment === 'delivery' && (
              <p className="mt-1 text-sm text-text-muted">
                {delivery === 'express'
                  ? t('checkout.deliveryExpress')
                  : t('checkout.deliveryStandard')}
              </p>
            )}
          </ReviewBlock>
        </div>

        <div className="mt-4">
          <ReviewBlock
            title={
              fulfillment === 'pickup'
                ? t('checkout.pickupLocation')
                : t('checkout.shipToSelected')
            }
          >
            {fulfillment === 'pickup' ? (
              <div className="text-sm text-text-muted">
                <p className="font-medium text-text">{t('checkout.pickupStoreName')}</p>
                <p className="mt-1">{t('checkout.pickupStoreAddress')}</p>
                <p className="mt-1 text-xs">{t('checkout.pickupStoreHours')}</p>
              </div>
            ) : selectedAddress ? (
              <div className="text-sm text-text-muted">
                <p className="font-medium text-text">{selectedAddress.label}</p>
                <ul className="mt-1 space-y-0.5">
                  {formatSavedAddressLines(selectedAddress).map((line) => (
                    <li key={line}>{line}</li>
                  ))}
                </ul>
              </div>
            ) : null}
          </ReviewBlock>
        </div>
      </div>

      <div className="rounded-sm border border-border bg-surface shadow-sm">
        <div className="border-b border-border px-5 py-4">
          <h3 className="text-base font-semibold text-text">{t('checkout.orderSummary')}</h3>
          <p className="mt-1 text-sm text-text-muted">
            {t('checkout.itemCount', { count: totals.itemCount })}
          </p>
        </div>

        <ul className="divide-y divide-border">
          {lines.map((line) => {
            const lineTotal = line.unitPrice.amount * line.quantity
            return (
              <li key={line.lineId} className="flex gap-3 px-5 py-4">
                <LocalImage
                  image={line.image}
                  className="size-16 shrink-0 rounded-sm object-cover"
                />
                <div className="min-w-0 flex-1">
                  <p className="text-sm font-medium text-text">{line.title}</p>
                  <p className="mt-1 flex flex-wrap items-center gap-1 text-xs text-text-muted">
                    <PriceDisplay money={line.unitPrice} currency={currency} iconSize={12} />
                    <span>× {line.quantity}</span>
                  </p>
                </div>
                <PriceDisplay
                  money={{ amount: lineTotal, currencyCode: line.unitPrice.currencyCode }}
                  currency={currency}
                  className="shrink-0 text-sm font-semibold"
                />
              </li>
            )
          })}
        </ul>

        <div className="space-y-2 border-t border-border px-5 py-4 text-sm">
          <ReviewRow
            label={t('common.subtotal')}
            value={
              <PriceDisplay
                money={{ amount: totals.subtotal, currencyCode: currency.code }}
                currency={currency}
              />
            }
          />
          <ReviewRow
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
          />
          <ReviewRow
            label={t('checkout.estimatedTax')}
            value={
              <PriceDisplay
                money={{ amount: totals.tax, currencyCode: currency.code }}
                currency={currency}
              />
            }
          />
          <div className="flex justify-between border-t border-border pt-3 text-base font-semibold text-text">
            <span>{t('checkout.total')}</span>
            <PriceDisplay
              money={{ amount: totals.total, currencyCode: currency.code }}
              currency={currency}
            />
          </div>
        </div>
      </div>
    </div>
  )
}

function ReviewBlock({
  title,
  children,
}: {
  title: string
  children: ReactNode
}) {
  return (
    <div className="rounded-sm border border-border bg-surface-muted/50 p-4">
      <p className="text-xs font-semibold uppercase tracking-wider text-text-muted">{title}</p>
      <div className="mt-2">{children}</div>
    </div>
  )
}

function ReviewRow({ label, value }: { label: string; value: ReactNode }) {
  return (
    <div className="flex justify-between gap-4 text-text-muted">
      <span>{label}</span>
      <span className="text-text">{value}</span>
    </div>
  )
}
