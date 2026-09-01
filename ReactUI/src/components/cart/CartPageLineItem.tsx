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
  embedded?: boolean
  disabled?: boolean
  onRemove: () => void
  onUpdateQuantity: (quantity: number) => void
}

function shouldShowProductRef(sku: string) {
  return sku.length > 0 && !sku.includes(':')
}

export function CartPageLineItem({
  line,
  currency,
  embedded = false,
  disabled = false,
  onRemove,
  onUpdateQuantity,
}: CartPageLineItemProps) {
  const { t } = useTranslation()
  const lineTotal = line.unitPrice.amount * line.quantity
  const productHref = line.slug ? `/product/${line.slug}` : undefined

  const decrement = () => onUpdateQuantity(Math.max(1, line.quantity - 1))
  const increment = () => onUpdateQuantity(Math.min(99, line.quantity + 1))

  const productImage = (
    <div className="size-[5.5rem] shrink-0 overflow-hidden rounded-lg bg-surface-muted ring-1 ring-border/60 sm:size-24">
      {productHref ? (
        <Link to={productHref} className="block size-full">
          <LocalImage
            image={line.image}
            className="size-full object-cover transition-transform duration-300 hover:scale-105"
          />
        </Link>
      ) : (
        <LocalImage image={line.image} className="size-full object-cover" />
      )}
    </div>
  )

  const productTitle = (compact = false) =>
    productHref ? (
      <Link
        to={productHref}
        className={`line-clamp-2 text-sm font-semibold leading-snug text-text transition-colors hover:text-warm sm:text-base${compact ? ' pe-8' : ''}`}
      >
        {line.title}
      </Link>
    ) : (
      <p
        className={`line-clamp-2 text-sm font-semibold leading-snug text-text sm:text-base${compact ? ' pe-8' : ''}`}
      >
        {line.title}
      </p>
    )

  const quantityControl = (
    <div className="inline-flex h-9 items-stretch rounded-lg border border-border bg-surface">
      <button
        type="button"
        onClick={decrement}
        disabled={disabled || line.quantity <= 1}
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
        disabled={disabled || line.quantity >= 99}
        aria-label={t('product.increaseQty')}
        className="flex w-9 items-center justify-center text-base text-text-muted transition-colors hover:bg-surface-muted disabled:opacity-30"
      >
        +
      </button>
    </div>
  )

  const removeButton = (
    <button
      type="button"
      onClick={onRemove}
      disabled={disabled}
      aria-label={t('common.removeItem', { title: line.title })}
      className="absolute end-3 top-3 flex size-8 items-center justify-center rounded-full border border-border/80 bg-surface text-text-muted transition-colors hover:border-sale/40 hover:bg-sale/10 hover:text-sale disabled:opacity-40 sm:end-4 sm:top-4"
    >
      <CloseIcon size={14} />
    </button>
  )

  const compactCardLayout = (
    <article className="relative overflow-hidden rounded-xl border border-border bg-surface p-3 shadow-sm transition-shadow hover:shadow-md sm:p-4">
      {removeButton}

      <div className="flex gap-3 sm:gap-4">
        {productImage}

        <div className="flex min-w-0 flex-1 flex-col">
          {productTitle(true)}

          <div className="mt-1.5 flex flex-wrap items-center gap-x-2 gap-y-1 text-xs text-text-muted">
            <span>{t('common.unitPrice')}:</span>
            <PriceDisplay money={line.unitPrice} currency={currency} iconSize={12} />
          </div>

          {shouldShowProductRef(line.sku) && (
            <p className="mt-1 text-[11px] text-text-muted/80" dir="ltr">
              {t('cartPage.productRef', { id: line.sku })}
            </p>
          )}

          <div className="mt-auto flex flex-wrap items-center justify-between gap-3 pt-3">
            {quantityControl}
            <div className="text-end">
              <p className="text-[10px] font-medium uppercase tracking-wide text-text-muted">
                {t('cartPage.colTotal')}
              </p>
              <PriceDisplay
                money={{ amount: lineTotal, currencyCode: line.unitPrice.currencyCode }}
                currency={currency}
                className="text-base font-bold text-text"
              />
            </div>
          </div>
        </div>
      </div>
    </article>
  )

  const tableLayout = (
    <article className="border-b border-border px-4 py-5 last:border-b-0 sm:px-6">
      <div className="grid grid-cols-[minmax(0,1fr)_7rem_9rem_8rem_3rem] items-center gap-4">
        <div className="flex min-w-0 gap-4">
          {productImage}
          <div className="min-w-0 flex-1">
            {productTitle()}
            {shouldShowProductRef(line.sku) && (
              <p className="mt-1.5 text-xs text-text-muted" dir="ltr">
                {t('cartPage.productRef', { id: line.sku })}
              </p>
            )}
          </div>
        </div>

        <div className="text-end">
          <PriceDisplay money={line.unitPrice} currency={currency} className="text-sm text-text" iconSize={12} />
        </div>

        <div className="flex justify-center">{quantityControl}</div>

        <div className="text-end">
          <PriceDisplay
            money={{ amount: lineTotal, currencyCode: line.unitPrice.currencyCode }}
            currency={currency}
            className="text-base font-bold text-text"
          />
        </div>

        <div className="flex justify-end">
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

  if (embedded) {
    return compactCardLayout
  }

  return (
    <>
      <div className="lg:hidden">{compactCardLayout}</div>
      <div className="hidden lg:block">{tableLayout}</div>
    </>
  )
}
