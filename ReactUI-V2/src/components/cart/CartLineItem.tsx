import { useTranslation } from 'react-i18next'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { LocalImage } from '@/components/ui/LocalImage'
import type { CartLine } from '@/models/cart/cartLine.model'
import type { CurrencyConfig } from '@/models/shared/currency.model'

interface CartLineItemProps {
  line: CartLine
  currency: CurrencyConfig | null
  onRemove: () => void
  onUpdateQuantity: (quantity: number) => void
}

export function CartLineItem({
  line,
  currency,
  onRemove,
  onUpdateQuantity,
}: CartLineItemProps) {
  const { t } = useTranslation()

  const decrement = () => onUpdateQuantity(Math.max(1, line.quantity - 1))
  const increment = () => onUpdateQuantity(Math.min(99, line.quantity + 1))

  return (
    <li className="relative rounded-xl border border-border bg-surface p-3.5 shadow-sm">
      <button
        type="button"
        onClick={onRemove}
        aria-label={t('common.removeItem', { title: line.title })}
        className="absolute end-2.5 top-2.5 flex size-8 items-center justify-center rounded-full border border-border bg-surface text-text-muted transition-colors hover:border-border-strong hover:bg-surface-muted hover:text-text"
      >
        <CloseIcon size={14} />
      </button>

      <div className="flex gap-3.5 pe-7">
        <div className="size-[5.5rem] shrink-0 overflow-hidden rounded-lg border border-border bg-surface-muted">
          <LocalImage image={line.image} className="size-full object-cover" />
        </div>

        <div className="flex min-w-0 flex-1 flex-col">
          <p className="line-clamp-2 text-sm font-semibold leading-snug text-text">
            {line.title}
          </p>

          {currency && (
            <p className="mt-1.5 flex flex-wrap items-center gap-1 text-xs text-text-muted">
              <span>{t('common.unitPrice')}:</span>
              <PriceDisplay money={line.unitPrice} currency={currency} iconSize={12} />
            </p>
          )}

          <div className="mt-3 flex flex-wrap items-center justify-between gap-3">
            <div className="inline-flex h-9 items-stretch rounded-lg border border-border bg-surface">
              <button
                type="button"
                onClick={decrement}
                disabled={line.quantity <= 1}
                aria-label={t('product.decreaseQty')}
                className="flex w-9 items-center justify-center text-base text-text-muted transition-colors hover:bg-surface-muted disabled:opacity-30"
              >
                −
              </button>
              <span
                className="flex min-w-[2.25rem] items-center justify-center border-x border-border px-1 text-sm font-semibold tabular-nums text-text"
                aria-label={t('common.quantity')}
              >
                {line.quantity}
              </span>
              <button
                type="button"
                onClick={increment}
                disabled={line.quantity >= 99}
                aria-label={t('product.increaseQty')}
                className="flex w-9 items-center justify-center text-base text-text-muted transition-colors hover:bg-surface-muted disabled:opacity-30"
              >
                +
              </button>
            </div>

            {currency && (
              <PriceDisplay
                money={{
                  amount: line.unitPrice.amount * line.quantity,
                  currencyCode: line.unitPrice.currencyCode,
                }}
                currency={currency}
                className="text-base font-bold text-text"
              />
            )}
          </div>
        </div>
      </div>
    </li>
  )
}
