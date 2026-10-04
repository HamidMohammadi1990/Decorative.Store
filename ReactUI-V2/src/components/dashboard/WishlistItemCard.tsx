import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import { CompareIcon } from '@/components/compare/CompareIcon'
import { CartIcon, EyeIcon } from '@/components/dashboard/DashboardIcons'
import { AdminGridIconButton, AdminGridIconLink } from '@/components/dashboard/admin/AdminGridActions'
import { LocalImage } from '@/components/ui/LocalImage'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { WishlistIcon } from '@/components/wishlist/WishlistIcon'
import { useAddToBag } from '@/hooks/useAddToBag'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { useCompareStore } from '@/stores/compareStore'
import { useWishlistStore } from '@/stores/wishlistStore'
import { useUserStore } from '@/stores/userStore'

interface WishlistItemCardProps {
  product: ProductDetail
}

export function WishlistItemCard({ product }: WishlistItemCardProps) {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const { locale, currency } = useLocaleSettings()
  const accessToken = useUserStore((s) => s.accessToken)
  const toggleRemote = useWishlistStore((s) => s.toggleRemote)
  const toggleCompare = useCompareStore((s) => s.toggle)
  const isInCompare = useCompareStore((s) => s.isInCompare(product.slug))
  const { addToBag, status } = useAddToBag()
  const [removing, setRemoving] = useState(false)

  const productHref = `/product/${product.slug}`
  const busy = removing || status === 'adding'

  const handleRemove = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || removing) return
    if (!(await confirm({ message: t('dashboard.wishlist.removeConfirm', { title: product.title }) }))) {
      return
    }

    setRemoving(true)
    try {
      await toggleRemote(accessToken, locale, product.slug)
    } finally {
      setRemoving(false)
    }
  }

  const handleAddToCart = () => {
    void addToBag({
      sku: product.id,
      title: product.title,
      image: product.image,
      unitPrice: product.price,
      quantity: 1,
      inStock: product.inStock,
      productSlug: product.slug,
    })
  }

  const cartLabel =
    status === 'adding'
      ? t('cart.adding')
      : status === 'added'
        ? t('cart.added')
        : product.inStock
          ? t('listing.addToBag')
          : t('cart.viewProductToOrder')

  return (
    <article className="flex gap-3 overflow-hidden rounded-xl border border-border bg-surface p-3 shadow-sm transition-shadow hover:shadow-md sm:gap-4 sm:p-3.5">
      <Link
        to={productHref}
        className="relative h-[6.75rem] w-[6.75rem] shrink-0 overflow-hidden rounded-lg bg-surface-muted ring-1 ring-border/60 sm:h-28 sm:w-28"
      >
        <LocalImage
          image={product.image}
          className="size-full object-cover transition-transform duration-300 hover:scale-105"
        />
        {product.onSale && (
          <span className="pointer-events-none absolute start-1.5 top-1.5 rounded bg-sale px-1.5 py-0.5 text-[9px] font-semibold uppercase tracking-wide text-text-inverse">
            {t('listing.sale')}
          </span>
        )}
        {product.isNew && !product.onSale && (
          <span className="pointer-events-none absolute start-1.5 top-1.5 rounded bg-warm px-1.5 py-0.5 text-[9px] font-semibold uppercase tracking-wide text-warm-text">
            {t('listing.new')}
          </span>
        )}
      </Link>

      <div className="flex min-w-0 flex-1 flex-col justify-between gap-2">
        <div className="min-w-0 space-y-1">
          <div className="flex items-start gap-2">
            <Link
              to={productHref}
              className="min-w-0 flex-1 truncate text-sm font-semibold text-text transition-colors hover:text-warm"
            >
              {product.title}
            </Link>
            <span
              className={`shrink-0 rounded-full px-2 py-0.5 text-[9px] font-semibold uppercase tracking-wide ${
                product.inStock ? 'bg-accent/15 text-accent' : 'bg-surface-muted text-text-muted'
              }`}
            >
              {product.inStock ? t('listing.inStock') : t('listing.madeToOrder')}
            </span>
          </div>

          {currency && (
            <div className="flex flex-wrap items-baseline gap-2">
              <PriceDisplay
                money={product.price}
                currency={currency}
                className="text-sm font-semibold text-text"
                iconSize={12}
              />
              {product.compareAtPrice && (
                <PriceDisplay
                  money={product.compareAtPrice}
                  currency={currency}
                  className="text-xs text-text-muted"
                  iconSize={10}
                  strike
                />
              )}
            </div>
          )}

          {product.averageRating != null && product.reviewCount > 0 && (
            <p className="text-xs text-text-muted">
              {t('dashboard.wishlist.ratingSummary', {
                rating: product.averageRating.toFixed(1),
                count: product.reviewCount,
              })}
            </p>
          )}
        </div>

        <div className="flex flex-wrap items-center gap-1.5">
          <AdminGridIconLink
            label={t('dashboard.wishlist.viewProduct')}
            icon={<EyeIcon size={15} />}
            to={productHref}
          />
          <AdminGridIconButton
            label={cartLabel}
            icon={<CartIcon size={15} />}
            onClick={handleAddToCart}
            disabled={busy}
          />
          <AdminGridIconButton
            label={isInCompare ? t('compare.inCompare') : t('compare.add')}
            icon={<CompareIcon size={15} className="block" />}
            onClick={() => toggleCompare(product.slug)}
            disabled={busy}
          />
          <AdminGridIconButton
            label={t('dashboard.wishlist.remove')}
            icon={<WishlistIcon size={15} filled />}
            onClick={() => void handleRemove()}
            disabled={busy}
            tone="danger"
          />
        </div>
      </div>
    </article>
  )
}
