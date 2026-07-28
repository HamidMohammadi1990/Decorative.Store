import type { ProductFeature } from '@/models/catalog/productDetail.model'

interface ProductSpecTableProps {
  features: ProductFeature[]
  variant?: 'card' | 'plain'
}

export function ProductSpecTable({ features, variant = 'card' }: ProductSpecTableProps) {
  if (features.length === 0) return null

  const showColumnBorder = variant === 'card'

  return (
    <div
      className={
        variant === 'card'
          ? 'overflow-hidden rounded-lg border border-border'
          : 'overflow-hidden'
      }
    >
      {features.map((feature, index) => (
        <div
          key={`${feature.label}-${index}`}
          className={`grid grid-cols-[minmax(0,38%)_minmax(0,1fr)] ${
            index < features.length - 1 ? 'border-b border-border' : ''
          }`}
        >
          <div
            className={`bg-surface-muted px-3 py-3.5 text-sm text-text-muted ${
              showColumnBorder ? 'border-s border-border' : ''
            }`}
          >
            {feature.label}
          </div>
          <div className="bg-surface px-3 py-3.5 text-sm leading-relaxed text-text">
            {feature.value}
          </div>
        </div>
      ))}
    </div>
  )
}
