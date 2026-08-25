import { useCallback, useEffect, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { AdminSubCategory } from '@/models/admin/catalog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { ProductsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminProductService } from '@/services/adminProductService'
import { adminSubCategoryService } from '@/services/adminSubCategoryService'
import { slugifyTitle } from '@/services/admin/adminCatalogNormalize'
import { useUserStore } from '@/stores/userStore'

interface ProductFormPanelProps {
  mode: 'create' | 'edit'
}

export function ProductFormPanel({ mode }: ProductFormPanelProps) {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const { productId: rawProductId } = useParams<{ productId: string }>()
  const productId = rawProductId ? decodeURIComponent(rawProductId) : null
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [subCategories, setSubCategories] = useState<AdminSubCategory[]>([])
  const [loading, setLoading] = useState(mode === 'edit')
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const [title, setTitle] = useState('')
  const [slug, setSlug] = useState('')
  const [description, setDescription] = useState('')
  const [productCode, setProductCode] = useState('')
  const [price, setPrice] = useState('')
  const [compareAtPrice, setCompareAtPrice] = useState('')
  const [subCategoryId, setSubCategoryId] = useState('')
  const [isActive, setIsActive] = useState(true)
  const [slugTouched, setSlugTouched] = useState(false)

  const loadLookups = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.products.authRequired'))
      return
    }

    try {
      const result = await adminSubCategoryService.getAll(accessToken, locale, {
        pageSize: 100,
        languageId: languageId ?? undefined,
      })
      setSubCategories(result.items)
      if (mode === 'create' && !subCategoryId && result.items[0]) {
        setSubCategoryId(result.items[0].id)
      }
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.products.loadFailed')))
    }
  }, [accessToken, languageId, locale, mode, subCategoryId, t])

  const loadProduct = useCallback(async () => {
    if (mode !== 'edit' || !productId || !accessToken || accessToken === 'mock-access-token') {
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const product = await adminProductService.get(accessToken, locale, productId)
      if (!product) {
        setError(t('dashboard.products.notFound'))
        return
      }

      setTitle(product.title)
      setSlug(product.slug)
      setDescription(product.description)
      setProductCode(product.productCode)
      setPrice(String(product.price))
      setCompareAtPrice(product.compareAtPrice != null ? String(product.compareAtPrice) : '')
      setSubCategoryId(product.subCategoryId)
      setIsActive(product.isActive)
      setSlugTouched(true)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.products.loadFailed')))
    } finally {
      setLoading(false)
    }
  }, [accessToken, locale, mode, productId, t])

  useEffect(() => {
    if (languageLoading) return
    void loadLookups()
    void loadProduct()
  }, [languageLoading, loadLookups, loadProduct])

  const handleTitleChange = (value: string) => {
    setTitle(value)
    if (!slugTouched) setSlug(slugifyTitle(value))
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || languageId == null) {
      setError(t('dashboard.products.saveFailed'))
      return
    }

    const parsedPrice = Number(price)
    const parsedCompare =
      compareAtPrice.trim().length > 0 ? Number(compareAtPrice) : null

    if (
      !title.trim() ||
      !slug.trim() ||
      !productCode.trim() ||
      !subCategoryId ||
      !Number.isFinite(parsedPrice) ||
      parsedPrice < 0 ||
      (parsedCompare != null && (!Number.isFinite(parsedCompare) || parsedCompare < 0))
    ) {
      setError(t('dashboard.products.validationRequired'))
      return
    }

    setSaving(true)
    setError(null)
    try {
      if (mode === 'edit' && productId) {
        await adminProductService.update(accessToken, locale, {
          id: productId,
          languageId,
          title: title.trim(),
          slug: slug.trim(),
          description: description.trim(),
          productCode: productCode.trim(),
          price: parsedPrice,
          compareAtPrice: parsedCompare,
          subCategoryId,
          status: isActive,
        })
      } else {
        await adminProductService.create(accessToken, locale, {
          languageId,
          title: title.trim(),
          slug: slug.trim(),
          description: description.trim(),
          productCode: productCode.trim(),
          price: parsedPrice,
          compareAtPrice: parsedCompare,
          subCategoryId,
        })
      }
      navigate('/account/dashboard/products')
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.products.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (loading || languageLoading) {
    return (
      <div>
        <DashboardPageHeader
          title={
            mode === 'edit'
              ? t('dashboard.products.editTitle')
              : t('dashboard.products.createTitle')
          }
          icon={<ProductsIcon size={22} />}
        />
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.products.loading')} />
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={
          mode === 'edit' ? t('dashboard.products.editTitle') : t('dashboard.products.createTitle')
        }
        description={
          mode === 'edit'
            ? t('dashboard.products.editDescription')
            : t('dashboard.products.createDescription')
        }
        icon={<ProductsIcon size={22} />}
      />

      <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
        <AdminField label={t('dashboard.products.fieldSubCategory')}>
          <select
            value={subCategoryId}
            onChange={(e) => setSubCategoryId(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.products.selectSubCategory')}</option>
            {subCategories.map((item) => (
              <option key={item.id} value={item.id}>
                {item.categoryTitle} / {item.title}
              </option>
            ))}
          </select>
        </AdminField>

        <AdminField label={t('dashboard.products.fieldTitle')}>
          <input
            value={title}
            onChange={(e) => handleTitleChange(e.target.value)}
            className={adminInputClass}
            placeholder={t('dashboard.products.titlePlaceholder')}
          />
        </AdminField>

        <AdminField label={t('dashboard.products.fieldSlug')}>
          <input
            value={slug}
            onChange={(e) => {
              setSlugTouched(true)
              setSlug(e.target.value)
            }}
            className={adminInputClass}
            dir="ltr"
          />
        </AdminField>

        <AdminField label={t('dashboard.products.fieldCode')}>
          <input
            value={productCode}
            onChange={(e) => setProductCode(e.target.value)}
            className={adminInputClass}
            dir="ltr"
          />
        </AdminField>

        <AdminField label={t('dashboard.products.fieldDescription')}>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            rows={4}
            className={adminInputClass}
            placeholder={t('dashboard.products.descriptionPlaceholder')}
          />
        </AdminField>

        <div className="grid gap-5 sm:grid-cols-2">
          <AdminField label={t('dashboard.products.fieldPrice')}>
            <input
              type="number"
              min={0}
              step="any"
              value={price}
              onChange={(e) => setPrice(e.target.value)}
              className={adminInputClass}
              dir="ltr"
            />
          </AdminField>
          <AdminField label={t('dashboard.products.fieldCompareAtPrice')}>
            <input
              type="number"
              min={0}
              step="any"
              value={compareAtPrice}
              onChange={(e) => setCompareAtPrice(e.target.value)}
              className={adminInputClass}
              dir="ltr"
              placeholder={t('dashboard.products.compareAtOptional')}
            />
          </AdminField>
        </div>

        {mode === 'edit' && (
          <label className="flex items-center gap-2 text-sm text-text">
            <input
              type="checkbox"
              checked={isActive}
              onChange={(e) => setIsActive(e.target.checked)}
              className="size-4 rounded border-border text-warm focus:ring-warm"
            />
            {t('dashboard.products.fieldActive')}
          </label>
        )}

        {error && <p className="text-sm text-sale">{error}</p>}

        {subCategories.length === 0 && (
          <p className="text-sm text-text-muted">{t('dashboard.products.noSubCategoriesHint')}</p>
        )}

        <div className="flex flex-wrap gap-3">
          <Button
            variant="warm"
            onClick={() => void handleSave()}
            disabled={saving || subCategories.length === 0}
          >
            {saving ? (
              <InlineLoading label={t('dashboard.products.saving')} />
            ) : (
              t('dashboard.products.save')
            )}
          </Button>
          <Link to="/account/dashboard/products">
            <Button variant="secondary" disabled={saving}>
              {t('dashboard.products.cancel')}
            </Button>
          </Link>
        </div>
      </div>
    </div>
  )
}
