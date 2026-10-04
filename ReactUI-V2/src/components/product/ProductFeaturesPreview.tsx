import { useTranslation } from 'react-i18next'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import type { ProductFeature } from '@/models/catalog/productDetail.model'

const PREVIEW_COUNT = 6

interface ProductFeaturesPreviewProps {
  features: ProductFeature[]
}

export function ProductFeaturesPreview({ features }: ProductFeaturesPreviewProps) {
  const { t } = useTranslation()

  if (features.length === 0) return null

  const preview = features.slice(0, PREVIEW_COUNT)

  const scrollToSpecs = () => {
    window.location.hash = 'specs'
    document.getElementById('product-detail-sections')?.scrollIntoView({
      behavior: 'smooth',
      block: 'start',
    })
  }

  return (
    <div className="mt-8">
      <h2 className="mb-3 text-sm font-semibold text-text">{t('product.featuresTitle')}</h2>

      <div className="grid grid-cols-2 gap-2.5 sm:grid-cols-3">
        {preview.map((feature) => (
          <div
            key={feature.label}
            className="rounded-lg bg-surface-muted px-3 py-3 text-center"
          >
            <p className="text-xs text-text-muted">{feature.label}</p>
            <p className="mt-1 truncate text-sm font-medium text-text">{feature.value}</p>
          </div>
        ))}
      </div>

      {features.length > PREVIEW_COUNT && (
        <button
          type="button"
          onClick={scrollToSpecs}
          className="mt-3 flex w-full items-center justify-center gap-2 rounded-lg border border-border py-2.5 text-sm font-medium text-text transition-colors hover:bg-surface-muted"
        >
          {t('product.viewAllFeatures')}
          <ChevronIcon expanded={false} className="-rotate-90 rtl:rotate-90" />
        </button>
      )}
    </div>
  )
}
