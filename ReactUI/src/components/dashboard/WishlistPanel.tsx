import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { WishlistIcon } from '@/components/wishlist/WishlistIcon'
import { ProductGrid } from '@/components/listing/ProductGrid'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useWishlistPage } from '@/hooks/useWishlistPage'

export function WishlistPanel() {
  const { t } = useTranslation()
  const {
    products,
    slugs,
    loading,
    error,
    reload,
    clearAllRemote,
    accessToken,
  } = useWishlistPage()
  const [isClearing, setIsClearing] = useState(false)
  const [actionError, setActionError] = useState<string | null>(null)

  const handleClearAll = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || isClearing || slugs.length === 0) {
      return
    }

    setIsClearing(true)
    setActionError(null)

    try {
      await clearAllRemote(accessToken)
    } catch {
      setActionError(t('dashboard.wishlist.clearFailed'))
    } finally {
      setIsClearing(false)
    }
  }

  const isEmpty = !loading && !error && slugs.length === 0

  if (loading) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.wishlist.title')}
          description={t('dashboard.wishlist.description')}
          icon={<WishlistIcon size={22} />}
        />
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.wishlist.loading')} />
        </div>
      </div>
    )
  }

  if (error) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.wishlist.title')}
          description={t('dashboard.wishlist.description')}
          icon={<WishlistIcon size={22} />}
        />
        <div className="rounded-sm border border-dashed border-border bg-surface-muted/30 px-4 py-12 text-center">
          <p className="text-sm text-text-muted">{t('dashboard.wishlist.loadFailed')}</p>
          <Button variant="secondary" className="mt-4" onClick={() => reload()}>
            {t('dashboard.wishlist.retry')}
          </Button>
        </div>
      </div>
    )
  }

  if (isEmpty) {
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
                onClick={() => void handleClearAll()}
                disabled={isClearing}
                className="text-sm font-medium text-text-muted transition-colors hover:text-text disabled:opacity-50"
              >
                {isClearing ? t('dashboard.wishlist.clearing') : t('dashboard.wishlist.clearAll')}
              </button>
            </div>
          ) : undefined
        }
      />

      {actionError && <p className="mb-4 text-sm text-sale">{actionError}</p>}

      {products.length === 0 ? (
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
      ) : (
        <ProductGrid products={products} />
      )}
    </div>
  )
}
