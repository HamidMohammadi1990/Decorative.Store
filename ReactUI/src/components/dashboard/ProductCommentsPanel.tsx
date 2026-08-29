import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { AdminProductComment } from '@/models/admin/catalog.model'
import type { AdminProductListItem } from '@/models/admin/catalog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { ProductCommentsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import type { ProductPanelEmbedProps } from '@/components/dashboard/admin/productPanelEmbed'
import { embeddedProductLabel } from '@/components/dashboard/admin/productPanelEmbed'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminProductCommentService } from '@/services/adminProductCommentService'
import { adminProductService } from '@/services/adminProductService'
import { useUserStore } from '@/stores/userStore'

function truncateText(text: string, max = 160) {
  const trimmed = text.trim()
  if (trimmed.length <= max) return trimmed
  return `${trimmed.slice(0, max).trimEnd()}…`
}

export function ProductCommentsPanel({
  embedded = false,
  productId: fixedProductId,
  productTitle: fixedProductTitle,
  productCode: fixedProductCode,
}: ProductPanelEmbedProps = {}) {
  const { t } = useTranslation()
  const [searchParams, setSearchParams] = useSearchParams()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const productIdFromSource = embedded
    ? (fixedProductId ?? '')
    : (searchParams.get('productId') ?? '')

  const [items, setItems] = useState<AdminProductComment[]>([])
  const [products, setProducts] = useState<AdminProductListItem[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [filterProductId, setFilterProductId] = useState(productIdFromSource)
  const [filterStatus, setFilterStatus] = useState('')

  const selectedProduct = useMemo(() => {
    if (embedded && fixedProductId) {
      return {
        id: fixedProductId,
        title: fixedProductTitle ?? '',
        productCode: fixedProductCode ?? '',
      } as AdminProductListItem
    }
    return products.find((p) => p.id === filterProductId) ?? null
  }, [embedded, fixedProductCode, fixedProductId, fixedProductTitle, filterProductId, products])

  useEffect(() => {
    if (!embedded) setFilterProductId(productIdFromSource)
  }, [embedded, productIdFromSource])

  useEffect(() => {
    if (embedded && fixedProductId) setFilterProductId(fixedProductId)
  }, [embedded, fixedProductId])

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.productComments.authRequired'))
      setLoading(false)
      return
    }

    const productIdForQuery = embedded
      ? (fixedProductId ?? filterProductId)
      : filterProductId

    setLoading(true)
    setError(null)
    try {
      if (!embedded) {
        const productResult = await adminProductService.getAll(accessToken, locale, {
          pageSize: 200,
          languageId: languageId ?? undefined,
        })
        setProducts(productResult.items)
      }

      const commentResult = await adminProductCommentService.getAll(accessToken, locale, {
        pageSize: 200,
        productId: productIdForQuery || null,
        isActive:
          filterStatus === 'active' ? true : filterStatus === 'inactive' ? false : null,
      })
      setItems(commentResult.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productComments.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [
    accessToken,
    embedded,
    fixedProductId,
    filterProductId,
    filterStatus,
    languageId,
    locale,
    t,
  ])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  useEffect(() => {
    if (embedded) return
    if (!productIdFromSource && products.length > 0 && !filterProductId) {
      const firstId = products[0].id
      setFilterProductId(firstId)
      setSearchParams({ productId: firstId })
    }
  }, [embedded, filterProductId, productIdFromSource, products, setSearchParams])

  const handleProductFilterChange = (id: string) => {
    setFilterProductId(id)
    if (id) setSearchParams({ productId: id })
    else setSearchParams({})
  }

  const handleChangeStatus = async (id: string, isActive: boolean) => {
    if (!accessToken || accessToken === 'mock-access-token') return

    setSaving(true)
    setError(null)
    try {
      await adminProductCommentService.changeStatus(accessToken, locale, id, isActive)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productComments.statusFailed')))
    } finally {
      setSaving(false)
    }
  }

  return (
    <div>
      {!embedded && (
        <DashboardPageHeader
          title={t('dashboard.productComments.title')}
          description={t('dashboard.productComments.description')}
          icon={<ProductCommentsIcon size={22} />}
          action={
            <Link
              to="/account/dashboard/products"
              className="inline-flex items-center justify-center rounded-sm border border-border-strong px-4 py-2 text-sm font-medium text-text transition-colors hover:bg-surface-muted"
            >
              {t('dashboard.productComments.backToProducts')}
            </Link>
          }
        />
      )}

      {embedded && (
        <p className="mb-4 text-sm font-medium text-text">
          {embeddedProductLabel(fixedProductTitle, fixedProductCode)}
        </p>
      )}

      <div className={`mb-4 grid gap-4 ${embedded ? '' : 'sm:grid-cols-2'}`}>
        {!embedded && (
          <AdminField label={t('dashboard.productComments.fieldProduct')}>
            <select
              value={filterProductId}
              onChange={(e) => handleProductFilterChange(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.productComments.allProducts')}</option>
              {products.map((product) => (
                <option key={product.id} value={product.id}>
                  {product.title} ({product.productCode})
                </option>
              ))}
            </select>
          </AdminField>
        )}

        <AdminField label={t('dashboard.productComments.filterStatus')}>
          <select
            value={filterStatus}
            onChange={(e) => setFilterStatus(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.productComments.allStatuses')}</option>
            <option value="active">{t('dashboard.productComments.statusActive')}</option>
            <option value="inactive">{t('dashboard.productComments.statusInactive')}</option>
          </select>
        </AdminField>
      </div>

      {selectedProduct && (
        <p className="mb-4 text-xs text-text-muted">
          {t('dashboard.productComments.selectedProduct', {
            title: selectedProduct.title,
            code: selectedProduct.productCode,
          })}
        </p>
      )}

      {loading || languageLoading ? (
        <div className="flex justify-center py-10">
          <InlineLoading label={t('dashboard.productComments.loading')} />
        </div>
      ) : products.length === 0 && !embedded ? (
        <DashboardEmptyState
          icon={<ProductCommentsIcon size={28} />}
          title={t('dashboard.productComments.noProductsTitle')}
          message={t('dashboard.productComments.noProductsMessage')}
        />
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductCommentsIcon size={28} />}
          title={t('dashboard.productComments.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.productComments.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductCommentsIcon size={28} />}
          title={t('dashboard.productComments.emptyTitle')}
          message={t('dashboard.productComments.emptyMessage')}
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.productComments.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            {items.map((item) => (
              <li
                key={item.id}
                className="flex flex-wrap items-start justify-between gap-3 px-4 py-3 sm:px-5"
              >
                <div className="min-w-0 flex-1">
                  <p className="text-sm text-text">
                    {item.description ? truncateText(item.description) : '—'}
                  </p>
                  <p className="mt-1 text-xs text-text-muted">
                    {item.authorName || t('dashboard.productComments.unknownAuthor')}
                    {!filterProductId && item.productTitle ? ` · ${item.productTitle}` : ''}
                    {item.commentTopicTitle ? ` · ${item.commentTopicTitle}` : ''}
                  </p>
                  <p className="mt-1 text-xs text-text-muted" dir="ltr">
                    {t('dashboard.productComments.ratings', {
                      rate: item.commentRate,
                      quality: item.qualityRating,
                      affordable: item.affordableRating,
                    })}
                  </p>
                </div>
                <div className="flex flex-wrap items-center gap-2">
                  <span
                    className={`rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                      item.isActive
                        ? 'bg-warm-soft text-warm'
                        : 'bg-surface-muted text-text-muted'
                    }`}
                  >
                    {item.isActive
                      ? t('dashboard.productComments.statusActive')
                      : t('dashboard.productComments.statusInactive')}
                  </span>
                  {item.isActive ? (
                    <Button
                      variant="ghost"
                      className="py-1.5 text-xs text-sale hover:bg-sale/10"
                      onClick={() => void handleChangeStatus(item.id, false)}
                      disabled={saving}
                    >
                      {t('dashboard.productComments.deactivate')}
                    </Button>
                  ) : (
                    <Button
                      variant="secondary"
                      className="py-1.5 text-xs"
                      onClick={() => void handleChangeStatus(item.id, true)}
                      disabled={saving}
                    >
                      {t('dashboard.productComments.activate')}
                    </Button>
                  )}
                </div>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  )
}
