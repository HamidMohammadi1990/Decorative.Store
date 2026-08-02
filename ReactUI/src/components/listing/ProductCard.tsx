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

export function ProductCard({ product }: ProductCardProps) {
  const { t } = useTranslation()
  const { currency } = useLocaleSettings()
  const productPath = `/product/${product.slug}`

  return (
    <article className="group flex flex-col overflow-hidden rounded-sm border border-border bg-surface transition-shadow hover:shadow-md">
      <div className="relative aspect-[4/5] overflow-hidden bg-surface-muted">
        <Link to={productPath} className="block size-full">
          <LocalImage
            image={product.image}
            className="size-full object-cover transition-transform duration-500 group-hover:scale-105"
          />
        </Link>
        {product.onSale && (
          <span className="pointer-events-none absolute start-3 top-3 rounded-sm bg-sale px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide text-text-inverse">
            {t('listing.sale')}
          </span>
        )}
        {product.isNew && !product.onSale && (
          <span className="pointer-events-none absolute start-3 top-3 rounded-sm bg-warm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide text-warm-text">
            {t('listing.new')}
          </span>
        )}
        <div className="absolute end-3 top-3 z-10">
          <WishlistButton
            slug={product.slug}
            variant="icon"
            className="transition-transform hover:scale-105"
          />
        </div>
      </div>

      <div className="flex flex-1 flex-col p-4">
        <Link
          to={productPath}
          className="text-sm font-medium text-text transition-colors hover:text-warm"
        >
          {product.title}
        </Link>

        <div className="mt-2 flex items-baseline gap-2">
          {currency && (
            <>
              <PriceDisplay
                money={product.price}
                currency={currency}
                className="text-base font-semibold text-text"
              />
              {product.compareAtPrice && (
                <PriceDisplay
                  money={product.compareAtPrice}
                  currency={currency}
                  className="text-sm text-text-muted"
                  strike
                />
              )}
            </>
          )}
        </div>

        <p className="mt-1 text-xs text-text-muted">
          {product.inStock ? t('listing.inStock') : t('listing.madeToOrder')}
        </p>

        <div className="mt-auto flex items-stretch gap-2 pt-4">
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
            className="min-w-0 flex-1 py-2.5 text-sm"
          />
          <CompareButton slug={product.slug} variant="icon" className="size-10 shrink-0" />
        </div>
      </div>
    </article>
  )
}
