import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import type { AdminProductListItem } from '@/models/admin/catalog.model'
import type { AdminProductProperty } from '@/models/admin/property.model'
import type { AdminProperty, AdminPropertyItem } from '@/models/admin/property.model'
import { propertyTypeUsesItems } from '@/models/admin/property.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { ProductPropertiesIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { AdminListGridHeader } from '@/components/dashboard/admin/AdminListGridHeader'
import { AdminRowNumber } from '@/components/dashboard/admin/AdminRowNumber'
import { AdminContentLanguageField } from '@/components/dashboard/admin/AdminContentLanguageField'
import type { ProductPanelEmbedProps } from '@/components/dashboard/admin/productPanelEmbed'
import { embeddedProductLabel } from '@/components/dashboard/admin/productPanelEmbed'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { localeFromLanguageId } from '@/extensions/languageCode'
import { useAdminContentLanguage } from '@/hooks/useAdminContentLanguage'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminProductPropertyService } from '@/services/adminProductPropertyService'
import { adminProductService } from '@/services/adminProductService'
import { adminPropertyItemService } from '@/services/adminPropertyItemService'
import { adminPropertyService } from '@/services/adminPropertyService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function ProductPropertiesPanel({
  embedded = false,
  productId: fixedProductId,
  productTitle: fixedProductTitle,
  productCode: fixedProductCode,
}: ProductPanelEmbedProps = {}) {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const [searchParams, setSearchParams] = useSearchParams()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()
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
  const [items, setItems] = useState<AdminProductProperty[]>([])
  const [products, setProducts] = useState<AdminProductListItem[]>([])
  const [properties, setProperties] = useState<AdminProperty[]>([])
  const [propertyItems, setPropertyItems] = useState<AdminPropertyItem[]>([])
  const [selectedPropertyItems, setSelectedPropertyItems] = useState<AdminPropertyItem[]>([])
  const [loadingPropertyItems, setLoadingPropertyItems] = useState(false)
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [filterProductId, setFilterProductId] = useState(productIdFromSource)

  const [productId, setProductId] = useState(productIdFromSource)
  const [propertyId, setPropertyId] = useState('')
  const [propertyItemId, setPropertyItemId] = useState('')
  const [isActive, setIsActive] = useState(true)

  const productTitleById = useMemo(() => {
    const map = new Map<string, string>()
    products.forEach((p) => map.set(p.id, p.title))
    if (embedded && fixedProductId && fixedProductTitle) {
      map.set(fixedProductId, fixedProductTitle)
    }
    return map
  }, [embedded, fixedProductId, fixedProductTitle, products])

  const propertyTitleById = useMemo(() => {
    const map = new Map<string, string>()
    properties.forEach((p) => map.set(p.id, p.title))
    return map
  }, [properties])

  const propertyItemTitleById = useMemo(() => {
    const map = new Map<string, string>()
    propertyItems.forEach((item) => map.set(item.id, item.title))
    return map
  }, [propertyItems])

  const selectedProperty = useMemo(
    () => properties.find((p) => p.id === propertyId) ?? null,
    [properties, propertyId],
  )

  const availablePropertyItems = useMemo(() => {
    if (!propertyId) return []
    return selectedPropertyItems.filter((item) => item.isActive)
  }, [propertyId, selectedPropertyItems])

  const requiresPropertyItem =
    availablePropertyItems.length > 0 ||
    (selectedProperty != null && propertyTypeUsesItems(selectedProperty.propertyType))

  useEffect(() => {
    if (mode !== 'create' && mode !== 'edit') return
    if (!propertyId || !accessToken || accessToken === 'mock-access-token') {
      setSelectedPropertyItems([])
      return
    }

    let cancelled = false
    setLoadingPropertyItems(true)

    void adminPropertyItemService
      .getAll(accessToken, contentLocale, {
        propertyId,
        pageSize: 200,
        languageId: contentLanguageId ?? undefined,
      })
      .then((result) => {
        if (!cancelled) setSelectedPropertyItems(result.items)
      })
      .catch(() => {
        if (!cancelled) setSelectedPropertyItems([])
      })
      .finally(() => {
        if (!cancelled) setLoadingPropertyItems(false)
      })

    return () => {
      cancelled = true
    }
  }, [accessToken, contentLanguageId, contentLocale, mode, propertyId])

  useEffect(() => {
    if (!embedded) {
      setFilterProductId(productIdFromSource)
      setProductId(productIdFromSource)
    }
  }, [embedded, productIdFromSource])

  useEffect(() => {
    if (embedded && fixedProductId) {
      setFilterProductId(fixedProductId)
      setProductId(fixedProductId)
    }
  }, [embedded, fixedProductId])

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.productProperties.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const [linkResult, productResult, propertyResult, itemResult] = await Promise.all([
        adminProductPropertyService.getAll(accessToken, contentLocale, {
          pageSize: 200,
          productId: filterProductId || null,
        }),
        embedded
          ? Promise.resolve({ items: [] as AdminProductListItem[] })
          : adminProductService.getAll(accessToken, contentLocale, {
              pageSize: 200,
              languageId: contentLanguageId ?? undefined,
            }),
        adminPropertyService.getAll(accessToken, contentLocale, {
          pageSize: 200,
          languageId: contentLanguageId ?? undefined,
        }),
        adminPropertyItemService.getAll(accessToken, contentLocale, {
          pageSize: 300,
          languageId: contentLanguageId ?? undefined,
        }),
      ])
      setItems(linkResult.items)
      if (!embedded) setProducts(productResult.items)
      setProperties(propertyResult.items)
      setPropertyItems(itemResult.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productProperties.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, contentLanguageId, contentLocale, embedded, filterProductId, t])

  useEffect(() => {
    if (!languageLoading && !contentLanguageLoading) void load()
  }, [contentLanguageLoading, languageLoading, load])

  useEffect(() => {
    if (embedded) return
    if (!productIdFromSource && products.length > 0 && !filterProductId) {
      const firstId = products[0].id
      setFilterProductId(firstId)
      setProductId(firstId)
      setSearchParams({ productId: firstId })
    }
  }, [embedded, filterProductId, productIdFromSource, products, setSearchParams])

  const handleProductFilterChange = (id: string) => {
    setFilterProductId(id)
    if (id) setSearchParams({ productId: id })
    else setSearchParams({})
  }

  const resetForm = () => {
    setProductId((embedded && fixedProductId ? fixedProductId : filterProductId || products[0]?.id) ?? '')
    setPropertyId(properties[0]?.id ?? '')
    setPropertyItemId('')
    setIsActive(true)
    setEditingId(null)
    setError(null)
  }

  const openCreate = () => {
    resetForm()
    setProductId((embedded && fixedProductId ? fixedProductId : filterProductId || products[0]?.id) ?? '')
    setPropertyId(properties[0]?.id ?? '')
    setMode('create')
  }

  const openEdit = (item: AdminProductProperty) => {
    setEditingId(item.id)
    setProductId(item.productId)
    setPropertyId(item.propertyId)
    setPropertyItemId(item.propertyItemId ?? '')
    setIsActive(item.isActive)
    setError(null)
    setMode('edit')
  }

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.productProperties.saveFailed'))
      return
    }

    if (!productId || !propertyId) {
      setError(t('dashboard.productProperties.validationRequired'))
      return
    }

    if (requiresPropertyItem && !propertyItemId) {
      setError(t('dashboard.productProperties.itemRequired'))
      return
    }

    setSaving(true)
    setError(null)
    try {
      const payload = {
        productId,
        propertyId,
        propertyItemId: propertyItemId || null,
        isActive,
      }

      if (mode === 'edit' && editingId) {
        await adminProductPropertyService.update(accessToken, contentLocale, {
          id: editingId,
          ...payload,
        })
      } else {
        await adminProductPropertyService.create(accessToken, contentLocale, payload)
      }
      await load()
      backToList()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productProperties.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!(await confirm({ message: t('dashboard.productProperties.deleteConfirm') }))) return

    setSaving(true)
    setError(null)
    try {
      await adminProductPropertyService.delete(accessToken, id)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productProperties.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  const resolveItemLabel = (item: AdminProductProperty) => {
    const propertyTitle = propertyTitleById.get(item.propertyId) ?? item.propertyId
    if (item.propertyItemId) {
      const itemTitle = propertyItemTitleById.get(item.propertyItemId) ?? item.propertyItemId
      return `${propertyTitle} → ${itemTitle}`
    }
    return propertyTitle
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        {!embedded && (
          <DashboardPageHeader
            title={
              mode === 'edit'
                ? t('dashboard.productProperties.editTitle')
                : t('dashboard.productProperties.createTitle')
            }
            description={
              mode === 'edit'
                ? t('dashboard.productProperties.editDescription')
                : t('dashboard.productProperties.createDescription')
            }
            icon={<ProductPropertiesIcon size={22} />}
          />
        )}

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminContentLanguageField
            value={contentLanguageId}
            onChange={setContentLanguageId}
            languages={formLanguages}
          />
          {embedded ? (
            <p className="text-sm font-medium text-text">
              {embeddedProductLabel(fixedProductTitle, fixedProductCode)}
            </p>
          ) : (
            <AdminField label={t('dashboard.productProperties.fieldProduct')}>
              <select
                value={productId}
                onChange={(e) => setProductId(e.target.value)}
                className={adminInputClass}
              >
                <option value="">{t('dashboard.productProperties.selectProduct')}</option>
                {products.map((product) => (
                  <option key={product.id} value={product.id}>
                    {product.title} ({product.productCode})
                  </option>
                ))}
              </select>
            </AdminField>
          )}

          <AdminField label={t('dashboard.productProperties.fieldProperty')}>
            <select
              value={propertyId}
              onChange={(e) => {
                setPropertyId(e.target.value)
                setPropertyItemId('')
              }}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.productProperties.selectProperty')}</option>
              {properties.map((property) => (
                <option key={property.id} value={property.id}>
                  {property.title} ({property.code})
                </option>
              ))}
            </select>
          </AdminField>

          {propertyId && (
            <AdminField label={t('dashboard.productProperties.fieldPropertyItem')}>
              {loadingPropertyItems ? (
                <InlineLoading label={t('dashboard.productProperties.loadingPropertyItems')} />
              ) : (
                <>
                  <select
                    value={propertyItemId}
                    onChange={(e) => setPropertyItemId(e.target.value)}
                    className={adminInputClass}
                    disabled={availablePropertyItems.length === 0}
                  >
                    <option value="">{t('dashboard.productProperties.selectPropertyItem')}</option>
                    {availablePropertyItems.map((item) => (
                      <option key={item.id} value={item.id}>
                        {item.title} ({item.code})
                      </option>
                    ))}
                  </select>
                  {availablePropertyItems.length === 0 && (
                    <p className="mt-2 text-xs text-text-muted">
                      {t('dashboard.productProperties.noPropertyItemsForProperty')}
                    </p>
                  )}
                </>
              )}
            </AdminField>
          )}

          <label className="flex items-center gap-2 text-sm text-text">
            <input
              type="checkbox"
              checked={isActive}
              onChange={(e) => setIsActive(e.target.checked)}
              className="size-4 rounded border-border text-warm focus:ring-warm"
            />
            {t('dashboard.productProperties.fieldActive')}
          </label>

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.productProperties.saving')} />
              ) : (
                t('dashboard.productProperties.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.productProperties.cancel')}
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
          title={t('dashboard.productProperties.title')}
          description={t('dashboard.productProperties.description')}
          icon={<ProductPropertiesIcon size={22} />}
          action={
            <div className="flex flex-wrap gap-2">
              <Link
                to="/account/dashboard/products"
                className="inline-flex items-center justify-center rounded-sm border border-border-strong px-4 py-2 text-sm font-medium text-text transition-colors hover:bg-surface-muted"
              >
                {t('dashboard.productProperties.backToProducts')}
              </Link>
              <Button
                variant="warm"
                onClick={openCreate}
                disabled={products.length === 0 || properties.length === 0}
              >
                {t('dashboard.productProperties.add')}
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
          <Button variant="warm" onClick={openCreate} disabled={properties.length === 0}>
            {t('dashboard.productProperties.add')}
          </Button>
        </div>
      )}

      <AdminContentLanguageField
        className="mb-4"
        value={contentLanguageId}
        onChange={setContentLanguageId}
        languages={formLanguages}
      />

      <div className="mb-4">
        {!embedded && (
          <AdminField label={t('dashboard.productProperties.fieldProduct')}>
            <select
              value={filterProductId}
              onChange={(e) => handleProductFilterChange(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.productProperties.allProducts')}</option>
              {products.map((product) => (
                <option key={product.id} value={product.id}>
                  {product.title} ({product.productCode})
                </option>
              ))}
            </select>
          </AdminField>
        )}
      </div>

      {loading || languageLoading || contentLanguageLoading ? (
        <div className="flex justify-center py-10">
          <InlineLoading label={t('dashboard.productProperties.loading')} />
        </div>
      ) : products.length === 0 && !embedded ? (
        <DashboardEmptyState
          icon={<ProductPropertiesIcon size={28} />}
          title={t('dashboard.productProperties.noProductsTitle')}
          message={t('dashboard.productProperties.noProductsMessage')}
        />
      ) : properties.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductPropertiesIcon size={28} />}
          title={t('dashboard.productProperties.noPropertiesTitle')}
          message={t('dashboard.productProperties.noPropertiesMessage')}
        />
      ) : !filterProductId && !embedded ? (
        <DashboardEmptyState
          icon={<ProductPropertiesIcon size={28} />}
          title={t('dashboard.productProperties.noProductTitle')}
          message={t('dashboard.productProperties.noProductMessage')}
        />
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductPropertiesIcon size={28} />}
          title={t('dashboard.productProperties.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.productProperties.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductPropertiesIcon size={28} />}
          title={t('dashboard.productProperties.emptyTitle')}
          message={t('dashboard.productProperties.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.productProperties.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.productProperties.itemCount', { count: items.length })}
            {filterProductId && productTitleById.get(filterProductId)
              ? ` · ${productTitleById.get(filterProductId)}`
              : ''}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            <AdminListGridHeader />
            {items.map((item, index) => (
              <li
                key={item.id}
                className="flex flex-wrap items-center gap-3 px-4 py-3 sm:px-5"
              >
                <AdminRowNumber value={index + 1} />
                <div className="min-w-0 flex-1">
                  <p className="truncate text-sm font-semibold text-text">
                    {resolveItemLabel(item)}
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
                      ? t('dashboard.productProperties.statusActive')
                      : t('dashboard.productProperties.statusInactive')}
                  </span>
                  <Button
                    variant="secondary"
                    className="py-1.5 text-xs"
                    onClick={() => openEdit(item)}
                    disabled={saving}
                  >
                    {t('dashboard.productProperties.edit')}
                  </Button>
                  <Button
                    variant="ghost"
                    className="py-1.5 text-xs text-sale hover:bg-sale/10"
                    onClick={() => void handleDelete(item.id)}
                    disabled={saving}
                  >
                    {t('dashboard.productProperties.delete')}
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
