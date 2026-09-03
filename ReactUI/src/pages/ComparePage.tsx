import { Link } from 'react-router-dom'
import { useMemo } from 'react'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import { CompareButton } from '@/components/compare/CompareButton'
import { AddToBagButton } from '@/components/cart/AddToBagButton'
import { CompareIcon } from '@/components/compare/CompareIcon'
import { Container } from '@/components/ui/Container'
import { Button } from '@/components/ui/Button'
import { LocalImage } from '@/components/ui/LocalImage'
import { InlineLoading } from '@/components/ui/Spinner'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { buildCompareRows, findBestPriceIndex } from '@/extensions/buildCompareRows'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { calcDiscountPercent } from '@/extensions/calcDiscountPercent'
import { useCompareProducts } from '@/hooks/useCompareProducts'
import { useShopPageMeta } from '@/hooks/useShopPageMeta'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useCompareStore } from '@/stores/compareStore'
import { MAX_COMPARE_PRODUCTS } from '@/models/catalog/compare.model'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import type { CompareRow } from '@/models/catalog/compare.model'

export function ComparePage() {
  const { t } = useTranslation()
  const confirm = useConfirm()

  useShopPageMeta({
    title: t('compare.title', { defaultValue: 'Compare products' }),
    description: t('seo.compareDescription'),
    path: '/compare',
    noindex: true,
  })
  const { currency } = useLocaleSettings()
  const slugs = useCompareStore((s) => s.slugs)
  const remove = useCompareStore((s) => s.remove)
  const clear = useCompareStore((s) => s.clear)
  const { products, loading, error } = useCompareProducts()

  const rows = useMemo(
    () => (products.length > 0 ? buildCompareRows(products, t) : []),
    [products, t],
  )

  const bestPriceIndex = useMemo(() => findBestPriceIndex(products), [products])

  const handleClear = async () => {
    if (!(await confirm({ message: t('compare.clearConfirm') }))) return
    clear()
  }

  const handleRemove = async (slug: string, title: string) => {
    if (!(await confirm({ message: t('compare.removeConfirm', { title }) }))) return
    remove(slug)
  }

  const emptySlots = Math.max(0, MAX_COMPARE_PRODUCTS - products.length)

  if (slugs.length === 0 && !loading) {
    return (
      <div className="flex-1 bg-gradient-to-b from-surface-muted/60 to-surface py-16 md:py-24">
        <Container className="max-w-lg text-center">
          <div className="mx-auto flex size-20 items-center justify-center rounded-full bg-warm-soft text-warm shadow-sm ring-8 ring-warm-soft/40">
            <CompareIcon size={32} />
          </div>
          <h1 className="mt-8 text-2xl font-semibold text-text">{t('compare.emptyTitle')}</h1>
          <p className="mt-3 text-sm leading-relaxed text-text-muted">
            {t('compare.emptyMessage')}
          </p>
          <Link to="/shop" className="mt-8 inline-block">
            <Button variant="warm">{t('compare.browseProducts')}</Button>
          </Link>
        </Container>
      </div>
    )
  }

  return (
    <div className="flex-1 bg-gradient-to-b from-warm-soft/30 via-surface to-surface pb-24">
      <div className="border-b border-border bg-gradient-to-br from-warm-soft/80 via-surface to-surface">
        <Container className="py-8 md:py-10">
          <div className="flex flex-wrap items-end justify-between gap-4">
            <div>
              <p className="text-xs font-semibold uppercase tracking-[0.16em] text-warm">
                {t('compare.eyebrow')}
              </p>
              <h1 className="mt-2 text-2xl font-semibold text-text md:text-3xl">
                {t('compare.title')}
              </h1>
              <p className="mt-2 max-w-2xl text-sm text-text-muted">{t('compare.subtitle')}</p>
            </div>
            <div className="flex items-center gap-3">
              <button
                type="button"
                onClick={() => void handleClear()}
                className="text-sm font-medium text-text-muted hover:text-text"
              >
                {t('compare.clearAll')}
              </button>
              <span className="rounded-sm bg-warm px-3 py-1.5 text-xs font-semibold text-warm-text shadow-sm">
                {t('compare.slotCount', { count: products.length, max: MAX_COMPARE_PRODUCTS })}
              </span>
            </div>
          </div>
        </Container>
      </div>

      <Container className="py-8 md:py-10">
        {loading ? (
          <div className="flex justify-center py-20">
            <InlineLoading label={t('common.loading')} />
          </div>
        ) : error ? (
          <div className="rounded-sm border border-border bg-surface px-6 py-16 text-center shadow-sm">
            <p className="text-sm text-text-muted">{t('common.error')}</p>
          </div>
        ) : (
          <div className="overflow-hidden rounded-sm border border-border bg-surface shadow-md">
            <div className="overflow-x-auto">
              <div
                className="grid min-w-[720px]"
                style={{
                  gridTemplateColumns: `minmax(9rem, 11rem) repeat(${MAX_COMPARE_PRODUCTS}, minmax(10rem, 1fr))`,
                }}
              >
                <div className="sticky start-0 z-20 border-b border-e border-border bg-surface-muted/60 p-4" />
                {products.map((product, index) => (
                  <ProductColumnHeader
                    key={product.slug}
                    product={product}
                    isBestPrice={bestPriceIndex === index}
                    onRemove={() => void handleRemove(product.slug, product.title)}
                  />
                ))}
                {Array.from({ length: emptySlots }, (_, i) => (
                  <EmptySlot key={`empty-${i}`} />
                ))}

                {rows.map((row) => (
                  <CompareRowCells
                    key={row.id}
                    row={row}
                    bestPriceIndex={row.id === 'price' ? bestPriceIndex : null}
                  />
                ))}
              </div>
            </div>
          </div>
        )}

        {products.length >= 2 && !loading && (
          <p className="mt-4 text-center text-xs text-text-muted">{t('compare.diffHint')}</p>
        )}
      </Container>
    </div>
  )
}

function ProductColumnHeader({
  product,
  isBestPrice,
  onRemove,
}: {
  product: ProductDetail
  isBestPrice: boolean
  onRemove: () => void
}) {
  const { t } = useTranslation()
  const { currency } = useLocaleSettings()
  const discount = product.compareAtPrice
    ? calcDiscountPercent(product.price, product.compareAtPrice)
    : null

  return (
    <div
      className={`relative border-b border-border p-4 ${
        isBestPrice ? 'bg-warm-soft/50' : 'bg-surface'
      }`}
    >
      {isBestPrice && (
        <span className="absolute start-3 top-3 rounded-sm bg-accent px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide text-text-inverse">
          {t('compare.bestValue')}
        </span>
      )}
      <button
        type="button"
        onClick={onRemove}
        aria-label={t('compare.removeProduct')}
        className="absolute end-3 top-3 flex size-8 items-center justify-center rounded-full border border-border bg-surface text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
      >
        <CloseIcon />
      </button>

      <Link to={`/product/${product.slug}`} className="group block">
        <div className="mx-auto aspect-[4/5] w-full max-w-[11rem] overflow-hidden rounded-sm bg-surface-muted ring-1 ring-border transition-shadow group-hover:shadow-md">
          <LocalImage
            image={product.image}
            className="size-full object-cover transition-transform duration-500 group-hover:scale-105"
          />
        </div>
        <p className="mt-4 line-clamp-2 text-sm font-semibold text-text transition-colors group-hover:text-warm">
          {product.title}
        </p>
      </Link>

      {currency && (
        <div className="mt-2 flex flex-wrap items-baseline gap-2">
          <PriceDisplay
            money={product.price}
            currency={currency}
            className="text-base font-semibold text-text"
          />
          {product.compareAtPrice && (
            <PriceDisplay
              money={product.compareAtPrice}
              currency={currency}
              className="text-xs text-text-muted"
              strike
            />
          )}
          {discount && (
            <span className="text-[10px] font-semibold text-sale">-{discount}%</span>
          )}
        </div>
      )}

      <p className="mt-1 text-[11px] text-text-muted">
        {product.inStock ? t('listing.inStock') : t('listing.madeToOrder')}
      </p>

      <div className="mt-4 space-y-2">
        <AddToBagButton
          item={{
            sku: product.id,
            title: product.title,
            image: product.image,
            unitPrice: product.price,
            quantity: 1,
            inStock: product.inStock,
            productSlug: product.slug,
          }}
          className="w-full py-2 text-xs"
        />
        <CompareButton slug={product.slug} variant="compact" className="w-full" />
      </div>
    </div>
  )
}

function EmptySlot() {
  const { t } = useTranslation()

  return (
    <div className="flex min-h-[20rem] flex-col items-center justify-center border-b border-dashed border-border bg-surface-muted/20 p-6 text-center">
      <span className="flex size-12 items-center justify-center rounded-full border border-dashed border-border text-2xl text-text-muted">
        +
      </span>
      <p className="mt-4 text-sm font-medium text-text-muted">{t('compare.addSlot')}</p>
      <Link to="/shop" className="mt-3 text-xs font-semibold text-warm hover:underline">
        {t('compare.browseProducts')}
      </Link>
    </div>
  )
}

function CompareRowCells({
  row,
  bestPriceIndex,
}: {
  row: CompareRow
  bestPriceIndex: number | null
}) {
  const { t } = useTranslation()
  const diffClass = row.highlightDiff ? 'bg-warm-soft/35' : 'bg-surface'

  return (
    <>
      <div
        className={`sticky start-0 z-10 border-b border-e border-border px-4 py-4 text-xs font-semibold uppercase tracking-[0.1em] text-text-muted ${diffClass}`}
      >
        <span className="flex items-center gap-2">
          {row.label}
          {row.highlightDiff && (
            <span className="size-1.5 rounded-full bg-warm" aria-hidden title={t('compare.differs')} />
          )}
        </span>
      </div>
      {row.values.map((value, index) => (
        <div
          key={`${row.id}-${index}`}
          className={`border-b border-border px-4 py-4 text-sm ${diffClass} ${
            bestPriceIndex === index ? 'ring-1 ring-inset ring-accent/30' : ''
          }`}
        >
          <CompareCell value={value} type={row.type} />
        </div>
      ))}
      {Array.from({ length: MAX_COMPARE_PRODUCTS - row.values.length }, (_, i) => (
        <div key={`${row.id}-pad-${i}`} className="border-b border-border bg-surface-muted/10 px-4 py-4" />
      ))}
    </>
  )
}

function CompareCell({
  value,
  type,
}: {
  value: string | string[] | boolean | number | null
  type: CompareRow['type']
}) {
  const { t } = useTranslation()
  const { currency } = useLocaleSettings()

  if (value === null || value === undefined || value === '') {
    return <span className="text-text-muted">—</span>
  }

  if (type === 'boolean') {
    return (
      <span
        className={`inline-flex items-center gap-1.5 font-medium ${
          value ? 'text-accent' : 'text-text-muted'
        }`}
      >
        <span
          className={`flex size-6 items-center justify-center rounded-full text-xs ${
            value ? 'bg-accent/10 text-accent' : 'bg-surface-muted text-text-muted'
          }`}
        >
          {value ? '✓' : '—'}
        </span>
        {value ? t('listing.inStock') : t('listing.madeToOrder')}
      </span>
    )
  }

  if (type === 'list' && Array.isArray(value)) {
    return (
      <ul className="space-y-1.5">
        {value.map((item) => (
          <li key={item} className="flex items-start gap-2 text-text-muted">
            <span className="mt-1.5 size-1 shrink-0 rounded-full bg-warm" aria-hidden />
            <span>{item}</span>
          </li>
        ))}
      </ul>
    )
  }

  if (type === 'price' && typeof value === 'number') {
    if (!currency) return <span className="font-semibold tabular-nums text-text">{value}</span>
    return (
      <PriceDisplay
        money={{ amount: value, currencyCode: currency.code }}
        currency={currency}
        className="font-semibold text-text"
      />
    )
  }

  return <span className="leading-relaxed text-text">{String(value)}</span>
}
