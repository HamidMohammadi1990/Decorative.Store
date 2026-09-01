import type { ProductSummary } from '@/models/catalog/product.model'
import { ProductCard } from '@/components/listing/ProductCard'

interface ProductGridProps {
  products: ProductSummary[]
}

export function ProductGrid({ products }: ProductGridProps) {
  return (
    <div className="grid grid-cols-2 gap-3 sm:gap-4 md:grid-cols-2 lg:grid-cols-3 lg:gap-5 xl:gap-6">
      {products.map((product) => (
        <ProductCard key={product.id} product={product} />
      ))}
    </div>
  )
}
