import { useTranslation } from 'react-i18next'
import type { ProductSummary } from '@/models/catalog/product.model'
import { RelatedProductCard } from '@/components/product/RelatedProductCard'
import { useHorizontalDragScroll } from '@/hooks/useHorizontalDragScroll'

interface RelatedProductsSliderProps {
  products: ProductSummary[]
}

export function RelatedProductsSlider({ products }: RelatedProductsSliderProps) {
  const { t } = useTranslation()
  const drag = useHorizontalDragScroll<HTMLDivElement>()

  if (products.length === 0) return null

  return (
    <section className="mt-12 border-t border-border/60 pt-8 md:mt-14 md:pt-10">
      <h2 className="text-sm font-semibold text-text sm:text-base">{t('product.relatedTitle')}</h2>

      <div
        ref={drag.ref}
        className={`-mx-4 mt-4 flex gap-2.5 overflow-x-auto px-4 pb-1 touch-pan-y select-none sm:mt-5 sm:gap-3 ${
          drag.isGrabbing ? 'cursor-grabbing snap-none' : 'cursor-grab snap-x snap-mandatory'
        }`}
        onPointerDown={drag.onPointerDown}
        onPointerMove={drag.onPointerMove}
        onPointerUp={drag.onPointerUp}
        onLostPointerCapture={drag.onLostPointerCapture}
        onClickCapture={drag.onClickCapture}
        onDragStart={(event) => event.preventDefault()}
      >
        {products.map((product) => (
          <div
            key={product.id}
            className="w-[9rem] shrink-0 snap-start sm:w-[10rem] md:w-[11rem]"
          >
            <RelatedProductCard product={product} />
          </div>
        ))}
      </div>
    </section>
  )
}
