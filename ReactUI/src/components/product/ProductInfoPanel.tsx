import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { ProductColorSelector } from '@/components/product/ProductColorSelector'
import { ProductFeaturesPreview } from '@/components/product/ProductFeaturesPreview'

interface ProductInfoPanelProps {
  product: ProductDetail
  primaryCategory: string
}

function reviewSeed(id: string) {
  let hash = 0
  for (let i = 0; i < id.length; i++) hash = (hash + id.charCodeAt(i) * (i + 1)) % 97
  return {
    rating: (4.2 + (hash % 8) / 10).toFixed(1),
    reviews: 12 + (hash % 140),
    buyers: 80 + (hash % 420),
  }
}

export function ProductInfoPanel({ product, primaryCategory }: ProductInfoPanelProps) {
  const { t } = useTranslation()
  const stats = reviewSeed(product.id)
  const colors = product.facets.color ?? []

  return (
    <div className="min-w-0">
      <div className="flex flex-wrap items-center gap-x-3 gap-y-1 text-sm">
        <Link to="/" className="font-medium text-accent hover:underline">
          {t('product.brandName')}
        </Link>
        <span className="text-border-strong" aria-hidden>
          |
        </span>
        <Link
          to={`/${primaryCategory}`}
          className="text-text-muted transition-colors hover:text-text"
        >
          {t(`product.categories.${primaryCategory}`, {
            defaultValue: primaryCategory.replace(/-/g, ' '),
          })}
        </Link>
      </div>

      <h1 className="mt-3 text-xl font-bold leading-snug text-text md:text-2xl">
        {product.title}
      </h1>

      <div className="mt-3 flex flex-wrap items-center gap-x-4 gap-y-2 text-sm text-text-muted">
        <span className="inline-flex items-center gap-1.5">
          <StarRating />
          <span className="font-medium text-text">{stats.rating}</span>
        </span>
        <span>{t('product.reviewCount', { count: stats.reviews })}</span>
        <span className="hidden h-3 w-px bg-border sm:block" aria-hidden />
        <span>{t('product.buyerCount', { count: stats.buyers })}</span>
      </div>

      {(product.onSale || product.isNew || product.badges.includes('bestseller')) && (
        <div className="mt-4 flex flex-wrap gap-2">
          {product.onSale && <InfoBadge>{t('listing.sale')}</InfoBadge>}
          {product.isNew && !product.onSale && <InfoBadge>{t('listing.new')}</InfoBadge>}
          {product.badges.includes('bestseller') && (
            <InfoBadge>{t('product.bestseller')}</InfoBadge>
          )}
        </div>
      )}

      <p className="mt-4 text-sm leading-relaxed text-text-muted">{product.description}</p>

      {colors.length > 0 && <ProductColorSelector colors={colors} />}

      {product.facets.size && product.facets.size.length > 0 && (
        <div className="mt-5">
          <p className="mb-2 text-sm font-medium text-text">{t('product.size')}</p>
          <div className="flex flex-wrap gap-2">
            {product.facets.size.map((value) => (
              <span
                key={value}
                className="rounded-md border border-border bg-surface-muted px-3 py-1.5 text-sm text-text"
              >
                {t(`product.values.${value}`, { defaultValue: value })}
              </span>
            ))}
          </div>
        </div>
      )}

      <ProductFeaturesPreview features={product.features} />
    </div>
  )
}

function InfoBadge({ children }: { children: string }) {
  return (
    <span className="rounded-md bg-surface-muted px-2.5 py-1 text-xs font-medium text-text-muted">
      {children}
    </span>
  )
}

function StarRating() {
  return (
    <span className="inline-flex text-warm" aria-hidden>
      {Array.from({ length: 5 }).map((_, i) => (
        <svg key={i} width="14" height="14" viewBox="0 0 12 12" fill="currentColor">
          <path d="M6 1.2l1.4 2.9 3.1.5-2.2 2.2.5 3.1L6 8.4 3.2 10l.5-3.1-2.2-2.2 3.1-.5L6 1.2z" />
        </svg>
      ))}
    </span>
  )
}
