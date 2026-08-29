import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { ProductColorSelector } from '@/components/product/ProductColorSelector'
import { ProductFeaturesPreview } from '@/components/product/ProductFeaturesPreview'
import {
  computeReviewStatsFromItems,
  formatProductRatingDisplay,
  type ProductReviewItem,
} from '@/extensions/productReviews'

interface ProductInfoPanelProps {
  product: ProductDetail
  primaryCategory: string
  reviews: ProductReviewItem[]
  reviewsLoading: boolean
}

export function ProductInfoPanel({
  product,
  primaryCategory,
  reviews,
  reviewsLoading,
}: ProductInfoPanelProps) {
  const { t } = useTranslation()
  const colors = product.facets.color ?? []

  const liveStats = computeReviewStatsFromItems(reviews)
  const hasLiveReviews = liveStats.reviewCount > 0

  const ratingDisplay = reviewsLoading
    ? product.averageRating != null
      ? formatProductRatingDisplay(product.averageRating)
      : '—'
    : hasLiveReviews
      ? liveStats.ratingDisplay
      : product.averageRating != null
        ? formatProductRatingDisplay(product.averageRating)
        : '—'

  const ratingValue = reviewsLoading
    ? product.averageRating ?? 0
    : hasLiveReviews
      ? liveStats.ratingValue
      : product.averageRating ?? 0

  const reviewCount = reviewsLoading
    ? product.reviewCount
    : hasLiveReviews
      ? liveStats.reviewCount
      : product.reviewCount

  const purchaseCount = product.purchaseCount

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
          <StarRating value={ratingValue} />
          <span className="font-medium text-text">{ratingDisplay}</span>
        </span>
        <span>{t('product.reviewCount', { count: reviewCount })}</span>
        <span className="hidden h-3 w-px bg-border sm:block" aria-hidden />
        <span>{t('product.buyerCount', { count: purchaseCount })}</span>
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

function StarRating({ value }: { value: number }) {
  const filledCount = value > 0 ? Math.min(5, Math.round(value)) : 0

  return (
    <span className="inline-flex text-warm" aria-hidden>
      {Array.from({ length: 5 }).map((_, index) => (
        <svg
          key={index}
          width="14"
          height="14"
          viewBox="0 0 12 12"
          fill={index < filledCount ? 'currentColor' : 'none'}
          className={index < filledCount ? 'text-warm' : 'text-border'}
        >
          <path
            d="M6 1.2l1.4 2.9 3.1.5-2.2 2.2.5 3.1L6 8.4 3.2 10l.5-3.1-2.2-2.2 3.1-.5L6 1.2z"
            stroke="currentColor"
            strokeWidth={index < filledCount ? 0 : 0.8}
          />
        </svg>
      ))}
    </span>
  )
}
