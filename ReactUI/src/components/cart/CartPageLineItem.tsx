import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { LocalImage } from '@/components/ui/LocalImage'
import type { CartLine } from '@/models/cart/cartLine.model'
import type { CurrencyConfig } from '@/models/shared/currency.model'

interface CartPageLineItemProps {
  line: CartLine
  currency: CurrencyConfig
  disabled?: boolean
  onRemove: () => void
  onUpdateQuantity: (quantity: number) => void
}

export function CartPageLineItem({
  line,
  currency,
  disabled = false,
  onRemove,
  onUpdateQuantity,
}: CartPageLineItemProps) {
  const { t } = useTranslation()
  const lineTotal = line.unitPrice.amount * line.quantity
  const productHref = line.slug ? `/product/${line.slug}` : undefined

  const decrement = () => onUpdateQuantity(Math.max(1, line.quantity - 1))
  const increment = () => onUpdateQuantity(Math.min(99, line.quantity + 1))

  return (
    <article className="border-b border-border px-4 py-5 last:border-b-0 sm:px-6">
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
        <div className="flex min-w-0 flex-1 gap-4">
          <div className="size-24 shrink-0 overflow-hidden rounded-lg border border-border bg-surface-muted sm:size-28">
            {productHref ? (
              <Link to={productHref} className="block size-full">
                <LocalImage image={line.image} className="size-full object-cover transition-transform hover:scale-105" />
              </Link>
            ) : (
              <LocalImage image={line.image} className="size-full object-cover" />
            )}
          </div>

          <div className="min-w-0 flex-1">
            {productHref ? (
              <Link
                to={productHref}
                className="line-clamp-2 text-base font-semibold leading-snug text-text transition-colors hover:text-warm"
              >
                {line.title}
              </Link>
            ) : (
              <p className="line-clamp-2 text-base font-semibold leading-snug text-text">{line.title}</p>
            )}

            <p className="mt-1.5 text-xs text-text-muted" dir="ltr">
              {t('cartPage.productRef', { id: line.sku })}
            </p>

            <div className="mt-3 flex flex-wrap items-center gap-x-4 gap-y-2 sm:hidden">
              <PriceDisplay
                money={line.unitPrice}
                currency={currency}
                className="text-sm text-text-muted"
                iconSize={12}
              />
              <PriceDisplay
                money={{ amount: lineTotal, currencyCode: line.unitPrice.currencyCode }}
                currency={currency}
                className="text-base font-bold text-text"
              />
            </div>
          </div>
        </div>

        <div className="hidden w-28 shrink-0 text-end sm:block">
          <PriceDisplay
            money={line.unitPrice}
            currency={currency}
            className="text-sm text-text"
            iconSize={12}
          />
        </div>

        <div className="flex items-center justify-between gap-4 sm:w-36 sm:justify-center">
          <div className="inline-flex h-10 items-stretch rounded-lg border border-border bg-surface">
            <button
              type="button"
              onClick={decrement}
              disabled={disabled || line.quantity <= 1}
              aria-label={t('product.decreaseQty')}
              className="flex w-10 items-center justify-center text-base text-text-muted transition-colors hover:bg-surface-muted disabled:opacity-30"
            >
              −
            </button>
            <span
              className="flex min-w-[2.5rem] items-center justify-center border-x border-border px-1 text-sm font-semibold tabular-nums text-text"
              aria-label={t('common.quantity')}
            >
              {line.quantity}
            </span>
            <button
              type="button"
              onClick={increment}
              disabled={disabled || line.quantity >= 99}
              aria-label={t('product.increaseQty')}
              className="flex w-10 items-center justify-center text-base text-text-muted transition-colors hover:bg-surface-muted disabled:opacity-30"
            >
              +
            </button>
          </div>

          <button
            type="button"
            onClick={onRemove}
            disabled={disabled}
            aria-label={t('common.removeItem', { title: line.title })}
            className="flex size-9 shrink-0 items-center justify-center rounded-full border border-border text-text-muted transition-colors hover:border-sale/40 hover:bg-sale/10 hover:text-sale disabled:opacity-40 sm:hidden"
          >
            <CloseIcon size={14} />
          </button>
        </div>

        <div className="hidden w-32 shrink-0 text-end sm:block">
          <PriceDisplay
            money={{ amount: lineTotal, currencyCode: line.unitPrice.currencyCode }}
            currency={currency}
            className="text-base font-bold text-text"
          />
        </div>

        <div className="hidden w-12 shrink-0 sm:flex sm:justify-end">
          <button
            type="button"
            onClick={onRemove}
            disabled={disabled}
            aria-label={t('common.removeItem', { title: line.title })}
            className="flex size-9 items-center justify-center rounded-full border border-border text-text-muted transition-colors hover:border-sale/40 hover:bg-sale/10 hover:text-sale disabled:opacity-40"
          >
            <CloseIcon size={14} />
          </button>
        </div>
      </div>
    </article>
  )
}
