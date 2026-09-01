import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { ProductSummary } from '@/models/catalog/product.model'
import { AddToBagButton } from '@/components/cart/AddToBagButton'
import { CompareButton } from '@/components/compare/CompareButton'
import { WishlistButton } from '@/components/wishlist/WishlistButton'
import { LocalImage } from '@/components/ui/LocalImage'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'

interface ProductCardProps {
  product: ProductSummary
}

function StarRating({ rating, count }: { rating: number; count: number }) {
  return (
    <div className="flex items-center gap-0.5 text-[10px] text-text-muted">
      <span className="text-warm" aria-hidden>
        ★
      </span>
      <span className="font-medium text-text">{rating.toFixed(1)}</span>
      <span className="text-text-muted/80">({count})</span>
    </div>
  )
}

export function ProductCard({ product }: ProductCardProps) {
  const { t } = useTranslation()
  const { currency } = useLocaleSettings()
  const productPath = `/product/${product.slug}`
  const discountPercent =
    product.compareAtPrice && product.compareAtPrice.amount > product.price.amount
      ? Math.round(
          ((product.compareAtPrice.amount - product.price.amount) /
            product.compareAtPrice.amount) *
            100,
        )
      : null

  return (
    <article className="group flex h-full flex-col overflow-hidden rounded-xl border border-border/70 bg-surface shadow-sm ring-1 ring-black/[0.02] transition-all duration-300 hover:-translate-y-0.5 hover:border-warm/20 hover:shadow-md">
      <div className="relative aspect-[3/4] overflow-hidden bg-surface-muted">
        <Link to={productPath} className="block size-full">
          <LocalImage
            image={product.image}
            className="size-full object-cover transition-transform duration-500 ease-out group-hover:scale-[1.04]"
          />
        </Link>

        <div
          aria-hidden
          className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/20 via-transparent to-transparent opacity-0 transition-opacity duration-300 group-hover:opacity-100"
        />

        <div className="absolute start-2 top-2 flex flex-col gap-1">
          {product.onSale && discountPercent != null && (
            <span className="rounded bg-sale px-1.5 py-0.5 text-[9px] font-bold uppercase tracking-wide text-text-inverse shadow-sm">
              -{discountPercent}%
            </span>
          )}
          {product.onSale && discountPercent == null && (
            <span className="rounded bg-sale px-1.5 py-0.5 text-[9px] font-bold uppercase tracking-wide text-text-inverse shadow-sm">
              {t('listing.sale')}
            </span>
          )}
          {product.isNew && !product.onSale && (
            <span className="rounded bg-warm px-1.5 py-0.5 text-[9px] font-bold uppercase tracking-wide text-warm-text shadow-sm">
              {t('listing.new')}
            </span>
          )}
        </div>

        <div className="absolute end-2 top-2 z-10">
          <WishlistButton
            slug={product.slug}
            variant="icon"
            className="rounded-full bg-surface/90 shadow-sm backdrop-blur-sm transition-transform hover:scale-105"
          />
        </div>
      </div>

      <div className="flex flex-1 flex-col p-3">
        <Link
          to={productPath}
          className="line-clamp-2 text-sm font-semibold leading-snug text-text transition-colors group-hover:text-warm"
        >
          {product.title}
        </Link>

        <div className="mt-1.5 flex flex-wrap items-center justify-between gap-x-2 gap-y-1">
          <div className="flex flex-wrap items-baseline gap-x-1.5 gap-y-0.5">
            {currency && (
              <>
                <PriceDisplay
                  money={product.price}
                  currency={currency}
                  className="text-sm font-bold text-text"
                />
                {product.compareAtPrice && (
                  <PriceDisplay
                    money={product.compareAtPrice}
                    currency={currency}
                    className="text-xs text-text-muted"
                    strike
                  />
                )}
              </>
            )}
          </div>
          {product.averageRating != null && product.reviewCount > 0 && (
            <StarRating rating={product.averageRating} count={product.reviewCount} />
          )}
        </div>

        <p className="mt-1 flex items-center gap-1 text-[11px]">
          <span
            aria-hidden
            className={`size-1.5 rounded-full ${product.inStock ? 'bg-emerald-500' : 'bg-amber-400'}`}
          />
          <span className={product.inStock ? 'text-emerald-700 dark:text-emerald-400' : 'text-text-muted'}>
            {product.inStock ? t('listing.inStock') : t('listing.madeToOrder')}
          </span>
        </p>

        <div className="mt-auto flex items-stretch gap-1.5 pt-2.5">
          <AddToBagButton
            item={{
              sku: product.id,
              title: product.title,
              image: product.image,
              unitPrice: product.price,
              quantity: 1,
              inStock: product.inStock,
              productSlug: product.slug,
            }}
            className="min-w-0 flex-1 py-2 text-xs shadow-sm"
          />
          <CompareButton
            slug={product.slug}
            variant="icon"
            className="size-9 shrink-0 rounded-lg border border-border/70 bg-surface shadow-sm"
          />
        </div>
      </div>
    </article>
  )
}
