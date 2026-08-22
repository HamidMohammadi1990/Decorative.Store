import { useState, type ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { getColorSwatch } from '@/extensions/colorSwatches'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { CompareButton } from '@/components/compare/CompareButton'
import { useAddToBag } from '@/hooks/useAddToBag'

interface ProductPurchasePanelProps {
  product: ProductDetail
}

export function ProductPurchasePanel({ product }: ProductPurchasePanelProps) {
  const { t } = useTranslation()
  const { currency } = useLocaleSettings()
  const { addToBag, status } = useAddToBag()
  const [quantity, setQuantity] = useState(1)

  const handleAdd = () => {
    void addToBag({
      sku: product.id,
      title: product.title,
      image: product.image,
      unitPrice: product.price,
      quantity,
      inStock: product.inStock,
      productSlug: product.slug,
    })
  }

  const addLabel =
    status === 'adding'
      ? t('cart.adding')
      : status === 'added'
        ? t('cart.added')
        : t('listing.addToBag')

  const decrement = () => setQuantity((q) => Math.max(1, q - 1))
  const increment = () => setQuantity((q) => Math.min(99, q + 1))

  return (
    <div className="flex flex-col lg:sticky lg:top-24 lg:self-start">
      {(product.onSale || product.isNew || product.badges.includes('bestseller')) && (
        <div className="flex flex-wrap gap-2">
          {product.onSale && <ProductBadge>{t('listing.sale')}</ProductBadge>}
          {product.isNew && !product.onSale && <ProductBadge>{t('listing.new')}</ProductBadge>}
          {product.badges.includes('bestseller') && (
            <ProductBadge>{t('product.bestseller')}</ProductBadge>
          )}
        </div>
      )}

      <h1 className="mt-4 text-2xl font-normal leading-snug tracking-tight text-text md:text-[1.75rem]">
        {product.title}
      </h1>

      <p className="mt-3 max-w-md text-sm leading-relaxed text-text-muted">
        {product.description}
      </p>

      {currency && (
        <div className="mt-6 flex items-baseline gap-3 border-t border-border pt-6">
          <PriceDisplay
            money={product.price}
            currency={currency}
            className="text-xl font-medium text-text"
            iconSize={16}
          />
          {product.compareAtPrice && (
            <PriceDisplay
              money={product.compareAtPrice}
              currency={currency}
              className="text-sm text-text-muted"
              strike
            />
          )}
        </div>
      )}

      <p className="mt-2 text-xs uppercase tracking-wider text-text-muted">
        {product.inStock ? t('listing.inStock') : t('listing.madeToOrder')}
      </p>

      {product.facets.color && product.facets.color.length > 0 && (
        <OptionGroup label={t('product.color')}>
          {product.facets.color.map((value) => (
            <OptionPill
              key={value}
              label={t(`product.values.${value}`, { defaultValue: value })}
              swatch={getColorSwatch(value)}
            />
          ))}
        </OptionGroup>
      )}

      {product.facets.size && product.facets.size.length > 0 && (
        <OptionGroup label={t('product.size')}>
          {product.facets.size.map((value) => (
            <OptionPill
              key={value}
              label={t(`product.values.${value}`, { defaultValue: value })}
            />
          ))}
        </OptionGroup>
      )}

      {product.facets.material && product.facets.material.length > 0 && (
        <OptionGroup label={t('product.material')}>
          {product.facets.material.map((value) => (
            <OptionPill
              key={value}
              label={t(`product.values.${value}`, { defaultValue: value })}
            />
          ))}
        </OptionGroup>
      )}

      <div className="mt-8 flex flex-col gap-3 sm:flex-row sm:items-stretch">
        <div className="inline-flex h-11 shrink-0 items-stretch border border-border">
          <button
            type="button"
            onClick={decrement}
            disabled={quantity <= 1}
            className="px-3.5 text-lg text-text-muted transition-colors hover:bg-surface-muted disabled:opacity-30"
            aria-label={t('product.decreaseQty')}
          >
            −
          </button>
          <span className="flex min-w-[3rem] items-center justify-center border-x border-border text-sm font-medium tabular-nums">
            {quantity}
          </span>
          <button
            type="button"
            onClick={increment}
            disabled={quantity >= 99}
            className="px-3.5 text-lg text-text-muted transition-colors hover:bg-surface-muted disabled:opacity-30"
            aria-label={t('product.increaseQty')}
          >
            +
          </button>
        </div>

        <div className="flex min-w-0 flex-1 flex-col gap-2 sm:flex-row">
          <button
            type="button"
            onClick={handleAdd}
            disabled={status === 'adding'}
            aria-live="polite"
            className="h-11 flex-1 bg-text px-8 text-sm font-medium tracking-wide text-text-inverse transition-opacity hover:opacity-90 disabled:opacity-60"
          >
            {addLabel}
          </button>
          <CompareButton
            slug={product.slug}
            variant="compact"
            className="h-11 shrink-0 justify-center px-4 sm:min-w-[11rem]"
          />
        </div>
      </div>

      {product.deliveryNote && (
        <p className="mt-6 border-s-2 border-border-strong ps-4 text-sm leading-relaxed text-text-muted">
          {product.deliveryNote}
        </p>
      )}
    </div>
  )
}

function ProductBadge({ children }: { children: string }) {
  return (
    <span className="border border-border px-2.5 py-1 text-[10px] font-medium uppercase tracking-[0.15em] text-text-muted">
      {children}
    </span>
  )
}

function OptionGroup({
  label,
  children,
}: {
  label: string
  children: ReactNode
}) {
  return (
    <div className="mt-6">
      <p className="mb-2.5 text-xs font-medium uppercase tracking-[0.15em] text-text-muted">
        {label}
      </p>
      <div className="flex flex-wrap gap-2">{children}</div>
    </div>
  )
}

function OptionPill({ label, swatch }: { label: string; swatch?: string }) {
  return (
    <span className="inline-flex items-center gap-2 border border-border px-3 py-2 text-sm text-text">
      {swatch && (
        <span
          aria-hidden
          className="size-3.5 rounded-full border border-border-strong"
          style={{ backgroundColor: swatch }}
        />
      )}
      {label}
    </span>
  )
}
