import { useEffect, useId } from 'react'
import { useTranslation } from 'react-i18next'
import { ProductSpecTable } from '@/components/product/ProductSpecTable'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'
import type { ProductDetail } from '@/models/catalog/productDetail.model'

interface ProductSpecsSheetProps {
  product: ProductDetail
  isOpen: boolean
  onClose: () => void
}

export function ProductSpecsSheet({ product, isOpen, onClose }: ProductSpecsSheetProps) {
  const { t } = useTranslation()
  const titleId = useId()

  useEffect(() => {
    if (!isOpen) return

    const prev = document.body.style.overflow
    document.body.style.overflow = 'hidden'

    const onKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose()
    }

    window.addEventListener('keydown', onKeyDown)
    return () => {
      document.body.style.overflow = prev
      window.removeEventListener('keydown', onKeyDown)
    }
  }, [isOpen, onClose])

  if (!isOpen) return null

  return (
    <Portal>
      <div className="fixed inset-0 z-[110] flex items-end justify-center">
        <button
          type="button"
          aria-label={t('common.close')}
          className="absolute inset-0 bg-black/40"
          onClick={onClose}
        />

        <div
          role="dialog"
          aria-modal="true"
          aria-labelledby={titleId}
          className="relative z-10 flex max-h-[min(88vh,40rem)] w-full max-w-lg flex-col overflow-hidden rounded-t-2xl bg-surface shadow-2xl"
        >
          <div className="flex shrink-0 flex-col items-center px-4 pt-3">
            <span className="h-1 w-10 rounded-full bg-border" aria-hidden />
          </div>

          <header className="relative flex shrink-0 items-center justify-center border-b border-border px-4 py-3">
            <h2 id={titleId} className="text-sm font-bold text-text">
              {t('product.specsSheetTitle')}
            </h2>
            <button
              type="button"
              onClick={onClose}
              aria-label={t('common.close')}
              className="absolute end-4 flex size-9 items-center justify-center rounded-full text-text-muted transition-colors hover:bg-surface-muted"
            >
              <CloseIcon size={18} />
            </button>
          </header>

          <div
            role="tablist"
            className="flex shrink-0 border-b border-border px-4"
            aria-label={t('product.specsSheetTitle')}
          >
            <button
              type="button"
              role="tab"
              aria-selected
              className="relative pb-3 pt-3 text-sm font-semibold text-text"
            >
              {t('product.specsTableTitle')}
              <span className="absolute inset-x-0 bottom-0 h-0.5 bg-text" aria-hidden />
            </button>
          </div>

          <div className="flex-1 overflow-y-auto px-4 py-5">
            <div className="space-y-6">
              {product.featureGroups.map((group) => (
                <section key={group.id}>
                  <h3 className="mb-3 text-sm font-bold text-text">{group.title}</h3>
                  <ProductSpecTable features={group.features} variant="plain" />
                </section>
              ))}
            </div>
          </div>
        </div>
      </div>
    </Portal>
  )
}
