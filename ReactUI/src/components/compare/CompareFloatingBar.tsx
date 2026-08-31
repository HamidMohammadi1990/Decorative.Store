import { Link, useLocation } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import { CompareIcon } from '@/components/compare/CompareIcon'
import { Button } from '@/components/ui/Button'
import { LocalImage } from '@/components/ui/LocalImage'
import { useCompareProducts } from '@/hooks/useCompareProducts'
import { useCompareStore } from '@/stores/compareStore'
import { MAX_COMPARE_PRODUCTS } from '@/models/catalog/compare.model'

export function CompareFloatingBar() {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const location = useLocation()
  const slugs = useCompareStore((s) => s.slugs)
  const clear = useCompareStore((s) => s.clear)
  const { products, loading } = useCompareProducts()

  const handleClear = async () => {
    if (!(await confirm({ message: t('compare.clearConfirm') }))) return
    clear()
  }

  if (location.pathname === '/compare' || slugs.length === 0) return null

  return (
    <div className="pointer-events-none fixed inset-x-0 bottom-0 z-40 px-4 pb-4 sm:px-6">
      <div className="pointer-events-auto mx-auto flex max-w-3xl items-center gap-4 overflow-hidden rounded-sm border border-border/80 bg-surface/95 p-3 shadow-[0_16px_48px_-12px_rgba(0,0,0,0.28)] backdrop-blur-md sm:p-4">
        <div className="flex min-w-0 flex-1 items-center gap-3">
          <span className="flex size-10 shrink-0 items-center justify-center rounded-sm bg-warm text-warm-text shadow-sm">
            <CompareIcon size={18} />
          </span>
          <div className="min-w-0">
            <p className="truncate text-sm font-semibold text-text">
              {t('compare.floatingTitle', { count: slugs.length, max: MAX_COMPARE_PRODUCTS })}
            </p>
            <p className="truncate text-xs text-text-muted">{t('compare.floatingHint')}</p>
          </div>
          <div className="hidden items-center -space-x-2 sm:flex">
            {products.slice(0, MAX_COMPARE_PRODUCTS).map((product) => (
              <span
                key={product.slug}
                className="relative size-10 overflow-hidden rounded-sm border-2 border-surface bg-surface-muted shadow-sm ring-1 ring-border"
              >
                <LocalImage image={product.image} className="size-full object-cover" />
              </span>
            ))}
            {loading &&
              slugs.slice(products.length).map((slug) => (
                <span
                  key={slug}
                  className="relative size-10 overflow-hidden rounded-sm border-2 border-surface bg-surface-muted shadow-sm ring-1 ring-border"
                >
                  <span className="flex size-full animate-pulse items-center justify-center bg-surface-muted text-[10px] text-text-muted">
                    …
                  </span>
                </span>
              ))}
          </div>
        </div>
        <div className="flex shrink-0 items-center gap-2">
          <button
            type="button"
            onClick={() => void handleClear()}
            className="hidden text-xs font-medium text-text-muted hover:text-text sm:inline"
          >
            {t('compare.clearAll')}
          </button>
          <Link to="/compare">
            <Button variant="warm" className="px-4 py-2 text-xs sm:text-sm">
              {t('compare.open')}
            </Button>
          </Link>
        </div>
      </div>
    </div>
  )
}
