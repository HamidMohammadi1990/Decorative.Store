import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { AdminProductDescription, AdminProductListItem } from '@/models/admin/catalog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { ProductDescriptionsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { AdminContentLanguageField } from '@/components/dashboard/admin/AdminContentLanguageField'
import type { ProductPanelEmbedProps } from '@/components/dashboard/admin/productPanelEmbed'
import { embeddedProductLabel } from '@/components/dashboard/admin/productPanelEmbed'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { localeFromLanguageId } from '@/extensions/languageCode'
import { useAdminContentLanguage } from '@/hooks/useAdminContentLanguage'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { useStoreLanguages } from '@/hooks/useStoreLanguages'
import { adminProductDescriptionService } from '@/services/adminProductDescriptionService'
import { adminProductService } from '@/services/adminProductService'
import { languageNameById } from '@/services/admin/adminCatalogNormalize'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

function truncateText(text: string, max = 120) {
  const trimmed = text.trim()
  if (trimmed.length <= max) return trimmed
  return `${trimmed.slice(0, max).trimEnd()}…`
}

export function ProductDescriptionsPanel({
  embedded = false,
  productId: fixedProductId,
  productTitle: fixedProductTitle,
  productCode: fixedProductCode,
}: ProductPanelEmbedProps = {}) {
  const { t } = useTranslation()
  const [searchParams, setSearchParams] = useSearchParams()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()
  const { languages } = useStoreLanguages()
  const {
    contentLanguageId,
    setContentLanguageId,
    languages: formLanguages,
    loading: contentLanguageLoading,
  } = useAdminContentLanguage()

  const contentLocale =
    contentLanguageId != null
      ? localeFromLanguageId(formLanguages, contentLanguageId, locale)
      : locale

  const productIdFromSource = embedded
    ? (fixedProductId ?? '')
    : (searchParams.get('productId') ?? '')
  const [mode, setMode] = useState<Mode>('list')
  const [selectedProductId, setSelectedProductId] = useState(productIdFromSource)
  const [products, setProducts] = useState<AdminProductListItem[]>([])
  const [items, setItems] = useState<AdminProductDescription[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)

  const [productId, setProductId] = useState(productIdFromSource)
  const [description, setDescription] = useState('')

  const selectedProduct = useMemo(() => {
    if (embedded && fixedProductId) {
      return {
        id: fixedProductId,
        title: fixedProductTitle ?? '',
        productCode: fixedProductCode ?? '',
      } as AdminProductListItem
    }
    return products.find((item) => item.id === selectedProductId) ?? null
  }, [embedded, fixedProductCode, fixedProductId, fixedProductTitle, products, selectedProductId])

  useEffect(() => {
    if (!embedded) {
      setSelectedProductId(productIdFromSource)
      setProductId(productIdFromSource)
    }
  }, [embedded, productIdFromSource])

  useEffect(() => {
    if (embedded && fixedProductId) {
      setSelectedProductId(fixedProductId)
      setProductId(fixedProductId)
    }
  }, [embedded, fixedProductId])

  const loadProducts = useCallback(async () => {
    if (embedded) return []
    if (!accessToken || accessToken === 'mock-access-token') return []
    const result = await adminProductService.getAll(accessToken, contentLocale, {
      pageSize: 200,
      languageId: contentLanguageId ?? undefined,
    })
    setProducts(result.items)
    return result.items
  }, [accessToken, contentLanguageId, contentLocale, embedded])

  const loadDescriptions = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token' || !selectedProductId) {
      setItems([])
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const result = await adminProductDescriptionService.getAll(accessToken, contentLocale, {
        productId: selectedProductId,
        pageSize: 100,
      })
      setItems(result.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productDescriptions.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, contentLocale, selectedProductId, t])

  useEffect(() => {
    if (embedded || languageLoading || contentLanguageLoading) return
    void loadProducts().catch((err) => {
      setError(resolveAdminMutationError(err, t('dashboard.productDescriptions.loadFailed')))
    })
  }, [embedded, languageLoading, contentLanguageLoading, loadProducts, t])

  useEffect(() => {
    if (!languageLoading && !contentLanguageLoading) void loadDescriptions()
  }, [contentLanguageLoading, languageLoading, loadDescriptions])

  useEffect(() => {
    if (embedded) return
    if (!productIdFromSource && products.length > 0 && !selectedProductId) {
      const firstId = products[0].id
      setSelectedProductId(firstId)
      setProductId(firstId)
      setSearchParams({ productId: firstId })
    }
  }, [embedded, productIdFromSource, products, selectedProductId, setSearchParams])

  const handleProductChange = (id: string) => {
    setSelectedProductId(id)
    setProductId(id)
    if (id) setSearchParams({ productId: id })
    else setSearchParams({})
  }

  const resetForm = () => {
    setProductId((selectedProductId || products[0]?.id) ?? '')
    setDescription('')
    setEditingId(null)
    setError(null)
  }

  const openCreate = () => {
    resetForm()
    setProductId((selectedProductId || products[0]?.id) ?? '')
    setMode('create')
  }

  const openEdit = (item: AdminProductDescription) => {
    setEditingId(item.id)
    setContentLanguageId(item.languageId)
    setProductId(item.productId)
    setDescription(item.description)
    setError(null)
    setMode('edit')
  }

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
      setError(t('dashboard.productDescriptions.saveFailed'))
      return
    }

    if (!productId || !description.trim()) {
      setError(t('dashboard.productDescriptions.validationRequired'))
      return
    }

    setSaving(true)
    setError(null)
    try {
      const payload = {
        productId,
        languageId: contentLanguageId,
        description: description.trim(),
      }

      if (mode === 'edit' && editingId) {
        await adminProductDescriptionService.update(accessToken, contentLocale, {
          id: editingId,
          ...payload,
        })
      } else {
        await adminProductDescriptionService.create(accessToken, contentLocale, payload)
      }
      await loadDescriptions()
      backToList()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productDescriptions.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.productDescriptions.deleteConfirm'))) return

    setSaving(true)
    setError(null)
    try {
      await adminProductDescriptionService.delete(accessToken, id)
      await loadDescriptions()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productDescriptions.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        {!embedded && (
          <DashboardPageHeader
            title={
              mode === 'edit'
                ? t('dashboard.productDescriptions.editTitle')
                : t('dashboard.productDescriptions.createTitle')
            }
            description={
              mode === 'edit'
                ? t('dashboard.productDescriptions.editDescription')
                : t('dashboard.productDescriptions.createDescription')
            }
            icon={<ProductDescriptionsIcon size={22} />}
          />
        )}

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminContentLanguageField
            value={contentLanguageId}
            onChange={setContentLanguageId}
            languages={formLanguages}
            disabled={mode === 'edit'}
          />
          {embedded ? (
            <p className="text-sm font-medium text-text">
              {embeddedProductLabel(fixedProductTitle, fixedProductCode)}
            </p>
          ) : (
            <AdminField label={t('dashboard.productDescriptions.fieldProduct')}>
              <select
                value={productId}
                onChange={(e) => setProductId(e.target.value)}
                className={adminInputClass}
              >
                <option value="">{t('dashboard.productDescriptions.selectProduct')}</option>
                {products.map((product) => (
                  <option key={product.id} value={product.id}>
                    {product.title} ({product.productCode})
                  </option>
                ))}
              </select>
            </AdminField>
          )}

          <AdminField label={t('dashboard.productDescriptions.fieldDescription')}>
            <textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className={`${adminInputClass} min-h-[10rem] resize-y`}
              placeholder={t('dashboard.productDescriptions.descriptionPlaceholder')}
            />
          </AdminField>

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.productDescriptions.saving')} />
              ) : (
                t('dashboard.productDescriptions.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.productDescriptions.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      {!embedded && (
        <DashboardPageHeader
          title={t('dashboard.productDescriptions.title')}
          description={t('dashboard.productDescriptions.description')}
          icon={<ProductDescriptionsIcon size={22} />}
          action={
            <div className="flex flex-wrap gap-2">
              <Link
                to="/account/dashboard/products"
                className="inline-flex items-center justify-center rounded-sm border border-border-strong px-4 py-2 text-sm font-medium text-text transition-colors hover:bg-surface-muted"
              >
                {t('dashboard.productDescriptions.backToProducts')}
              </Link>
              <Button
                variant="warm"
                onClick={openCreate}
                disabled={products.length === 0 || !selectedProductId}
              >
                {t('dashboard.productDescriptions.add')}
              </Button>
            </div>
          }
        />
      )}

      {embedded && (
        <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
          <p className="text-sm font-medium text-text">
            {embeddedProductLabel(fixedProductTitle, fixedProductCode)}
          </p>
          <Button variant="warm" onClick={openCreate} disabled={!selectedProductId}>
            {t('dashboard.productDescriptions.add')}
          </Button>
        </div>
      )}

      <AdminContentLanguageField
        className="mb-4"
        value={contentLanguageId}
        onChange={setContentLanguageId}
        languages={languages}
      />

      <div className="mb-4">
        {!embedded && (
          <AdminField label={t('dashboard.productDescriptions.fieldProduct')}>
            <select
              value={selectedProductId}
              onChange={(e) => handleProductChange(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.productDescriptions.selectProduct')}</option>
              {products.map((product) => (
                <option key={product.id} value={product.id}>
                  {product.title} ({product.productCode})
                </option>
              ))}
            </select>
          </AdminField>
        )}
        {selectedProduct && !embedded && (
          <p className="mt-1 text-xs text-text-muted">
            {t('dashboard.productDescriptions.selectedProduct', {
              title: selectedProduct.title,
              code: selectedProduct.productCode,
            })}
          </p>
        )}
      </div>

      {loading || languageLoading || contentLanguageLoading ? (
        <div className="flex justify-center py-10">
          <InlineLoading label={t('dashboard.productDescriptions.loading')} />
        </div>
      ) : products.length === 0 && !embedded ? (
        <DashboardEmptyState
          icon={<ProductDescriptionsIcon size={28} />}
          title={t('dashboard.productDescriptions.noProductsTitle')}
          message={t('dashboard.productDescriptions.noProductsMessage')}
        />
      ) : !selectedProductId ? (
        <DashboardEmptyState
          icon={<ProductDescriptionsIcon size={28} />}
          title={t('dashboard.productDescriptions.noProductTitle')}
          message={t('dashboard.productDescriptions.noProductMessage')}
        />
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductDescriptionsIcon size={28} />}
          title={t('dashboard.productDescriptions.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void loadDescriptions()}>
              {t('dashboard.productDescriptions.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductDescriptionsIcon size={28} />}
          title={t('dashboard.productDescriptions.emptyTitle')}
          message={t('dashboard.productDescriptions.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.productDescriptions.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.productDescriptions.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            {items.map((item) => (
              <li
                key={item.id}
                className="flex flex-wrap items-start justify-between gap-3 px-4 py-3 sm:px-5"
              >
                <div className="min-w-0 flex-1">
                  <p className="text-sm text-text">{truncateText(item.description)}</p>
                  <p className="mt-1 text-xs text-text-muted">
                    {t('dashboard.productDescriptions.languageLabel', {
                      name: languageNameById(languages, item.languageId),
                    })}
                  </p>
                </div>
                <div className="flex flex-wrap items-center gap-2">
                  <Button
                    variant="secondary"
                    className="py-1.5 text-xs"
                    onClick={() => openEdit(item)}
                    disabled={saving}
                  >
                    {t('dashboard.productDescriptions.edit')}
                  </Button>
                  <Button
                    variant="ghost"
                    className="py-1.5 text-xs text-sale hover:bg-sale/10"
                    onClick={() => void handleDelete(item.id)}
                    disabled={saving}
                  >
                    {t('dashboard.productDescriptions.delete')}
                  </Button>
                </div>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  )
}
