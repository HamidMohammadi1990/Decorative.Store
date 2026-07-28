import { useEffect, useRef, useState } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { LocalImage } from '@/components/ui/LocalImage'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'

interface StoryProductStripProps {
  products: ProductDetail[]
}

export function StoryProductStrip({ products }: StoryProductStripProps) {
  const { t } = useTranslation()
  const { currency } = useLocaleSettings()

  if (products.length === 0) return null

  return (
    <div className="absolute inset-x-0 bottom-0 z-20 bg-gradient-to-t from-black/80 via-black/50 to-transparent px-3 pb-4 pt-10">
      <p className="mb-2 text-[10px] font-semibold uppercase tracking-wider text-white/70">
        {t('stories.linkedProducts')}
      </p>
      <div className="flex gap-2 overflow-x-auto pb-1">
        {products.map((product) => (
          <Link
            key={product.slug}
            to={`/product/${product.slug}`}
            className="flex min-w-[11rem] max-w-[13rem] shrink-0 items-center gap-2.5 rounded-lg border border-white/15 bg-black/40 p-2 backdrop-blur-sm transition-colors hover:border-warm/50 hover:bg-black/55"
          >
            <div className="size-12 shrink-0 overflow-hidden rounded-md bg-white/10">
              <LocalImage image={product.image} className="size-full object-cover" />
            </div>
            <div className="min-w-0 flex-1">
              <p className="line-clamp-2 text-xs font-medium leading-snug text-white">
                {product.title}
              </p>
              {currency && (
                <PriceDisplay
                  money={product.price}
                  currency={currency}
                  className="mt-1 text-xs font-semibold text-warm"
                />
              )}
            </div>
          </Link>
        ))}
      </div>
    </div>
  )
}
