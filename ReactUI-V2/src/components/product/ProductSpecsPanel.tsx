import { useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { ProductSpecGroup } from '@/components/product/ProductSpecGroup'
import { ProductSpecsSheet } from '@/components/product/ProductSpecsSheet'
import { ProductSpecTable } from '@/components/product/ProductSpecTable'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import type { ProductDetail } from '@/models/catalog/productDetail.model'

interface ProductSpecsPanelProps {
  product: ProductDetail
}

const MOBILE_PREVIEW_COUNT = 5

export function ProductSpecsPanel({ product }: ProductSpecsPanelProps) {
  const { t } = useTranslation()
  const [expanded, setExpanded] = useState(false)
  const [sheetOpen, setSheetOpen] = useState(false)

  const allFeatures = useMemo(
    () => product.featureGroups.flatMap((group) => group.features),
    [product.featureGroups],
  )

  const previewFeatures = allFeatures.slice(0, MOBILE_PREVIEW_COUNT)
  const hasMoreSpecs = allFeatures.length > MOBILE_PREVIEW_COUNT

  const hasExtra =
    product.longDescription.length > 0 ||
    product.highlights.length > 0 ||
    Boolean(product.deliveryNote)

  return (
    <div>
      <ProductSpecsSheet
        product={product}
        isOpen={sheetOpen}
        onClose={() => setSheetOpen(false)}
      />

      {/* ── Mobile ── */}
      <div className="lg:hidden">
        <h3 className="text-base font-bold text-text">{t('product.specsTableTitle')}</h3>

        <div className="mt-4">
          <ProductSpecTable features={previewFeatures} variant="card" />
        </div>

        {hasMoreSpecs && (
          <div className="mt-4 flex justify-center">
            <button
              type="button"
              onClick={() => setSheetOpen(true)}
              className="inline-flex items-center gap-2 rounded-full bg-surface-muted px-5 py-2.5 text-sm font-medium text-text transition-colors hover:bg-border/60"
            >
              {t('product.seeMoreSpecs')}
              <ChevronIcon expanded={false} className="text-text rtl:rotate-90 ltr:-rotate-90" />
            </button>
          </div>
        )}
      </div>

      {/* ── Desktop ── */}
      <div className="hidden lg:block">
        <h3 className="text-base font-bold text-text">
          {t('product.tabSpecs')}
          <span className="mt-2 block h-0.5 w-10 bg-warm" aria-hidden />
        </h3>

        <div className="mt-6">
          {product.featureGroups.map((group) => (
            <ProductSpecGroup key={group.id} group={group} />
          ))}
        </div>

        {hasExtra && (
          <button
            type="button"
            onClick={() => setExpanded((v) => !v)}
            className="mt-2 inline-flex items-center gap-1 text-sm font-medium text-accent hover:underline"
          >
            {expanded ? t('product.seeLess') : t('product.seeMore')}
            <ChevronIcon
              expanded={expanded}
              className="text-accent rtl:rotate-90 ltr:-rotate-90"
            />
          </button>
        )}

        {expanded && (
          <div className="mt-8 max-w-3xl space-y-8 border-t border-border pt-8">
            {product.longDescription.length > 0 && (
              <div>
                <h4 className="text-sm font-bold text-text">{t('product.aboutTitle')}</h4>
                <div className="mt-3 space-y-3">
                  {product.longDescription.map((paragraph) => (
                    <p key={paragraph} className="text-sm leading-relaxed text-text-muted">
                      {paragraph}
                    </p>
                  ))}
                </div>
              </div>
            )}

            {product.deliveryNote && (
              <div>
                <h4 className="text-sm font-bold text-text">{t('product.tabDelivery')}</h4>
                <p className="mt-3 text-sm leading-relaxed text-text-muted">
                  {product.deliveryNote}
                </p>
              </div>
            )}

            {product.highlights.length > 0 && (
              <div>
                <h4 className="text-sm font-bold text-text">{t('product.highlightsTitle')}</h4>
                <ul className="mt-3 space-y-2 text-sm text-text-muted">
                  {product.highlights.map((item) => (
                    <li key={item}>• {item}</li>
                  ))}
                </ul>
              </div>
            )}
          </div>
        )}
      </div>
    </div>
  )
}
