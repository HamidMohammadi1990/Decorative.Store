import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { WishlistIcon } from '@/components/wishlist/WishlistIcon'
import { ProductGrid } from '@/components/listing/ProductGrid'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useWishlistProducts } from '@/hooks/useWishlistProducts'
import { useWishlistStore } from '@/stores/wishlistStore'

export function WishlistPanel() {
  const { t } = useTranslation()
  const slugs = useWishlistStore((s) => s.slugs)
  const clear = useWishlistStore((s) => s.clear)
  const { products, loading, error } = useWishlistProducts()

  if (slugs.length === 0 && !loading) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.wishlist.title')}
          description={t('dashboard.wishlist.description')}
          icon={<WishlistIcon size={22} />}
        />
        <DashboardEmptyState
          icon={<WishlistIcon size={28} />}
          title={t('dashboard.wishlist.emptyTitle')}
          message={t('dashboard.wishlist.emptyMessage')}
          action={
            <Link to="/shop">
              <Button variant="warm">{t('dashboard.wishlist.browseProducts')}</Button>
            </Link>
          }
        />
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.wishlist.title')}
        description={t('dashboard.wishlist.description')}
        icon={<WishlistIcon size={22} />}
        action={
          slugs.length > 0 ? (
            <div className="flex flex-wrap items-center gap-3">
              <span className="rounded-sm bg-warm-soft px-2.5 py-1 text-xs font-semibold text-warm">
                {t('dashboard.wishlist.itemCount', { count: slugs.length })}
              </span>
              <button
                type="button"
                onClick={clear}
                className="text-sm font-medium text-text-muted transition-colors hover:text-text"
              >
                {t('dashboard.wishlist.clearAll')}
              </button>
            </div>
          ) : undefined
        }
      />

      {loading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('common.loading')} />
        </div>
      ) : error ? (
        <p className="py-12 text-center text-sm text-text-muted">{t('common.error')}</p>
      ) : (
        <ProductGrid products={products} />
      )}
    </div>
  )
}
