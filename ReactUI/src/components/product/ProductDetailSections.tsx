import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { ProductQuestionsPanel } from '@/components/product/ProductQuestionsPanel'
import { ProductReviewsPanel } from '@/components/product/ProductReviewsPanel'
import { ProductSpecsPanel } from '@/components/product/ProductSpecsPanel'
import type { ProductDetail } from '@/models/catalog/productDetail.model'

type SectionTab = 'specs' | 'reviews' | 'questions'

interface ProductDetailSectionsProps {
  product: ProductDetail
}

const TABS: SectionTab[] = ['specs', 'reviews', 'questions']

function tabFromHash(): SectionTab | null {
  const hash = window.location.hash.replace('#', '')
  return TABS.includes(hash as SectionTab) ? (hash as SectionTab) : null
}

export function ProductDetailSections({ product }: ProductDetailSectionsProps) {
  const { t } = useTranslation()
  const [activeTab, setActiveTab] = useState<SectionTab>('specs')

  useEffect(() => {
    const sync = () => {
      const tab = tabFromHash()
      if (tab) setActiveTab(tab)
    }

    sync()
    window.addEventListener('hashchange', sync)
    return () => window.removeEventListener('hashchange', sync)
  }, [])

  const selectTab = (tab: SectionTab) => {
    setActiveTab(tab)
    window.history.replaceState(null, '', `#${tab}`)
  }

  const tabLabels: Record<SectionTab, string> = {
    specs: t('product.tabSpecs'),
    reviews: t('product.tabReviews'),
    questions: t('product.tabQuestions'),
  }

  return (
    <section
      id="product-detail-sections"
      className="scroll-mt-24 mt-10 border-t border-border pt-6 lg:mt-14"
    >
      <div
        role="tablist"
        aria-label={t('product.sectionsLabel')}
        className="flex gap-6 overflow-x-auto border-b border-border"
      >
        {TABS.map((tab) => {
          const isActive = activeTab === tab

          return (
            <button
              key={tab}
              type="button"
              role="tab"
              aria-selected={isActive}
              aria-controls={`panel-${tab}`}
              id={`tab-${tab}`}
              onClick={() => selectTab(tab)}
              className={`relative shrink-0 pb-3 text-sm font-medium transition-colors ${
                isActive
                  ? 'text-warm'
                  : 'text-text-muted hover:text-text'
              }`}
            >
              {tabLabels[tab]}
              {isActive && (
                <span className="absolute inset-x-0 bottom-0 h-0.5 bg-warm" aria-hidden />
              )}
            </button>
          )
        })}
      </div>

      <div className="py-6">
        {activeTab === 'specs' && (
          <div role="tabpanel" id="panel-specs" aria-labelledby="tab-specs">
            <ProductSpecsPanel product={product} />
          </div>
        )}

        {activeTab === 'reviews' && (
          <div role="tabpanel" id="panel-reviews" aria-labelledby="tab-reviews">
            <ProductReviewsPanel product={product} />
          </div>
        )}

        {activeTab === 'questions' && (
          <div role="tabpanel" id="panel-questions" aria-labelledby="tab-questions">
            <ProductQuestionsPanel product={product} />
          </div>
        )}
      </div>
    </section>
  )
}
