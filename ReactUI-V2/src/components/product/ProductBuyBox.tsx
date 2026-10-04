import { useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { calcDiscountPercent } from '@/extensions/calcDiscountPercent'
import {
  computeSatisfactionPercent,
  type ProductReviewItem,
} from '@/extensions/productReviews'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { WishlistButton } from '@/components/wishlist/WishlistButton'
import { useAddToBag } from '@/hooks/useAddToBag'

interface ProductBuyBoxProps {
  product: ProductDetail
  reviews: ProductReviewItem[]
  reviewsLoading: boolean
}

export function ProductBuyBox({ product, reviews, reviewsLoading }: ProductBuyBoxProps) {
  const { t, i18n } = useTranslation()
  const { currency } = useLocaleSettings()
  const { addToBag, status } = useAddToBag()
  const [quantity, setQuantity] = useState(1)

  const discount =
    product.compareAtPrice && currency
      ? calcDiscountPercent(product.price, product.compareAtPrice)
      : null

  const handleAdd = () => {
    void addToBag({
      sku: product.id,
      title: product.title,
      image: product.image,
      unitPrice: product.price,
      quantity,
      inStock: product.inStock,
      productSlug: product.slug,
    })
  }

  const addLabel =
    status === 'adding'
      ? t('cart.adding')
      : status === 'added'
        ? t('cart.added')
        : t('listing.addToBag')

  const liveSatisfaction = computeSatisfactionPercent(reviews)
  const hasLiveReviews = liveSatisfaction !== null
  const satisfactionPercent = reviewsLoading
    ? product.satisfactionPercent
    : hasLiveReviews
      ? liveSatisfaction
      : product.satisfactionPercent

  const numberLocale = i18n.language === 'fa' ? 'fa-IR' : 'en-US'
  const satisfactionLabel =
    satisfactionPercent != null
      ? t('product.sellerRating', {
          percent: satisfactionPercent.toLocaleString(numberLocale),
        })
      : t('product.sellerRatingEmpty')

  return (
    <aside className="lg:sticky lg:top-24 lg:self-start">
      <div className="rounded-lg border border-border bg-surface p-5 shadow-sm">
        <div className="flex items-center justify-between gap-3 border-b border-border pb-4">
          <div>
            <p className="text-xs text-text-muted">{t('product.sellerLabel')}</p>
            <p className="mt-0.5 text-sm font-semibold text-text">{t('product.sellerName')}</p>
          </div>
          <span className="shrink-0 rounded-full bg-surface-muted px-2.5 py-1 text-xs font-medium text-accent">
            {satisfactionLabel}
          </span>
        </div>

        {currency && (
          <div className="mt-4 space-y-2">
            {product.compareAtPrice && (
              <div className="flex items-center gap-2">
                <PriceDisplay
                  money={product.compareAtPrice}
                  currency={currency}
                  className="text-sm text-text-muted"
                  strike
                />
                {discount !== null && (
                  <span className="rounded-sm bg-warm px-1.5 py-0.5 text-xs font-bold text-warm-text">
                    {t('product.discountBadge', { percent: discount })}
                  </span>
                )}
              </div>
            )}
            <PriceDisplay
              money={product.price}
              currency={currency}
              className="text-2xl font-bold text-text"
              iconSize={18}
            />
          </div>
        )}

        <p className="mt-2 text-xs text-text-muted">
          {product.inStock ? t('listing.inStock') : t('listing.madeToOrder')}
        </p>

        <div className="mt-5 flex items-center gap-3">
          <div className="inline-flex h-10 items-stretch rounded-md border border-border">
            <button
              type="button"
              onClick={() => setQuantity((q) => Math.max(1, q - 1))}
              disabled={quantity <= 1}
              className="px-3 text-lg text-text-muted transition-colors hover:bg-surface-muted disabled:opacity-30"
              aria-label={t('product.decreaseQty')}
            >
              −
            </button>
            <span className="flex min-w-[2.5rem] items-center justify-center border-x border-border text-sm font-medium tabular-nums">
              {quantity}
            </span>
            <button
              type="button"
              onClick={() => setQuantity((q) => Math.min(99, q + 1))}
              disabled={quantity >= 99}
              className="px-3 text-lg text-text-muted transition-colors hover:bg-surface-muted disabled:opacity-30"
              aria-label={t('product.increaseQty')}
            >
              +
            </button>
          </div>
        </div>

        <button
          type="button"
          onClick={handleAdd}
          disabled={status === 'adding'}
          aria-live="polite"
          className="mt-4 w-full rounded-md bg-warm py-3 text-sm font-semibold text-warm-text transition-colors hover:bg-warm-hover disabled:opacity-60"
        >
          {addLabel}
        </button>

        <WishlistButton slug={product.slug} variant="card" className="mt-3" />

        {(product.warranty || product.deliveryNote) && (
          <ul className="mt-5 space-y-3 border-t border-border pt-4 text-xs leading-relaxed text-text-muted">
            {product.warranty && <TrustRow icon="shield">{product.warranty}</TrustRow>}
            {product.deliveryNote && <TrustRow icon="truck">{product.deliveryNote}</TrustRow>}
          </ul>
        )}
      </div>
    </aside>
  )
}

function TrustRow({
  icon,
  children,
}: {
  icon: 'shield' | 'truck'
  children: string
}) {
  return (
    <li className="flex items-start gap-2.5">
      <TrustIcon type={icon} />
      <span>{children}</span>
    </li>
  )
}

function TrustIcon({ type }: { type: 'shield' | 'truck' }) {
  const paths: Record<typeof type, string> = {
    shield: 'M6 2l4 1.5v3.5c0 2.5-1.7 4.8-4 5.5C3.7 11.8 2 9.5 2 7V3.5L6 2z',
    truck: 'M1 4h8v5H1V4zm9 1h2l2 2v2h-4V5zM3 10a1.5 1.5 0 103 0 1.5 1.5 0 00-3 0zm7 0a1.5 1.5 0 103 0 1.5 1.5 0 00-3 0z',
  }

  return (
    <svg
      width="16"
      height="16"
      viewBox="0 0 12 12"
      aria-hidden
      className="mt-0.5 shrink-0 text-accent"
      fill="currentColor"
    >
      <path d={paths[type]} />
    </svg>
  )
}
