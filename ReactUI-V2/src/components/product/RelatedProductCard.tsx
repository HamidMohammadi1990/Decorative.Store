import { CatalogNavLink } from '@/components/routing/CatalogNavLink'
import { useTranslation } from 'react-i18next'
import type { ProductSummary } from '@/models/catalog/product.model'
import { LocalImage } from '@/components/ui/LocalImage'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'

interface RelatedProductCardProps {
  product: ProductSummary
}

export function RelatedProductCard({ product }: RelatedProductCardProps) {
  const { t } = useTranslation()
  const { currency } = useLocaleSettings()
  const productPath = `/product/${product.slug}`

  return (
    <article className="group flex h-full flex-col overflow-hidden rounded-lg border border-border/55 bg-surface transition-all hover:border-warm/25 hover:shadow-sm">
      <CatalogNavLink href={productPath} className="relative block aspect-square overflow-hidden bg-surface-muted">
        <LocalImage
          image={product.image}
          sizes="160px"
          className="size-full object-cover transition-transform duration-500 group-hover:scale-[1.04]"
        />

        {(product.onSale || product.isNew) && (
          <div className="absolute start-1.5 top-1.5 flex flex-col gap-0.5">
            {product.onSale && (
              <span className="rounded bg-sale px-1 py-0.5 text-[8px] font-bold uppercase text-text-inverse">
                {t('listing.sale')}
              </span>
            )}
            {product.isNew && !product.onSale && (
              <span className="rounded bg-warm px-1 py-0.5 text-[8px] font-bold uppercase text-warm-text">
                {t('listing.new')}
              </span>
            )}
          </div>
        )}
      </CatalogNavLink>

      <div className="flex flex-1 flex-col gap-1 p-2">
        <CatalogNavLink
          href={productPath}
          className="line-clamp-2 text-[11px] font-medium leading-snug text-text transition-colors group-hover:text-warm sm:text-xs"
        >
          {product.title}
        </CatalogNavLink>

        {currency && (
          <div className="mt-auto flex flex-wrap items-baseline gap-1">
            <PriceDisplay
              money={product.price}
              currency={currency}
              iconSize={10}
              className="text-[11px] font-bold text-text sm:text-xs"
            />
            {product.compareAtPrice && (
              <PriceDisplay
                money={product.compareAtPrice}
                currency={currency}
                iconSize={9}
                className="text-[10px] text-text-muted"
                strike
              />
            )}
          </div>
        )}
      </div>
    </article>
  )
}
