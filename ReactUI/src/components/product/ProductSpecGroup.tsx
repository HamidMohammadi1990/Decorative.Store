import { ProductSpecRow } from '@/components/product/ProductSpecRow'
import type { ProductFeatureGroup } from '@/models/catalog/productDetail.model'

interface ProductSpecGroupProps {
  group: ProductFeatureGroup
}

export function ProductSpecGroup({ group }: ProductSpecGroupProps) {
  return (
    <section className="grid grid-cols-1 gap-4 border-b border-border py-6 last:border-b-0 sm:grid-cols-[7.5rem_minmax(0,28rem)] sm:gap-x-10 md:grid-cols-[8.5rem_minmax(0,32rem)]">
      <h4 className="text-sm font-bold leading-relaxed text-text sm:pt-1">{group.title}</h4>

      <dl className="min-w-0">
        {group.features.map((feature) => (
          <ProductSpecRow key={`${group.id}-${feature.label}`} label={feature.label} value={feature.value} />
        ))}
      </dl>
    </section>
  )
}
