import { useCallback, useEffect, useMemo, useRef, useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { AdminCategory, AdminProductDetail, AdminSubCategory } from '@/models/admin/catalog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { ProductsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { AdminContentLanguageField } from '@/components/dashboard/admin/AdminContentLanguageField'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { localeFromLanguageId } from '@/extensions/languageCode'
import { useAdminContentLanguage } from '@/hooks/useAdminContentLanguage'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminProductService } from '@/services/adminProductService'
import { adminCategoryService } from '@/services/adminCategoryService'
import { adminSubCategoryService } from '@/services/adminSubCategoryService'
import { slugifyTitle } from '@/services/admin/adminCatalogNormalize'
import { useUserStore } from '@/stores/userStore'
import { ProductImagesPanel } from '@/components/dashboard/ProductImagesPanel'

function formatSubCategoryLabel(item: AdminSubCategory): string {
  return item.title
}

function formatSubCategoryTitles(categoryTitle: string, subCategoryTitle: string): string {
  if (categoryTitle && subCategoryTitle) return `${categoryTitle} / ${subCategoryTitle}`
  return subCategoryTitle || categoryTitle
}

export function ProductFormPanel() {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const [searchParams] = useSearchParams()
  const productId = searchParams.get('id') ?? undefined
  const subCategoryIdFromUrl = searchParams.get('subCategoryId') ?? ''
  const isEdit = Boolean(productId)

  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()
  const {
    contentLanguageId,
    setContentLanguageId,
    languages: formLanguages,
    loading: contentLanguageLoading,
  } = useAdminContentLanguage()

  const [categories, setCategories] = useState<AdminCategory[]>([])
  const [subCategories, setSubCategories] = useState<AdminSubCategory[]>([])
  const [subCategoriesLoading, setSubCategoriesLoading] = useState(false)
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const [categoryId, setCategoryId] = useState('')

  const [title, setTitle] = useState('')
  const [slug, setSlug] = useState('')
  const [description, setDescription] = useState('')
  const [productCode, setProductCode] = useState('')
  const [price, setPrice] = useState('')
  const [compareAtPrice, setCompareAtPrice] = useState('')
  const [subCategoryId, setSubCategoryId] = useState('')
  const [subCategoryLabel, setSubCategoryLabel] = useState('')
  const [isActive, setIsActive] = useState(true)
  const [slugTouched, setSlugTouched] = useState(false)

  const contentLocale = useMemo(
    () =>
      contentLanguageId != null
        ? localeFromLanguageId(formLanguages, contentLanguageId, locale)
        : locale,
    [contentLanguageId, formLanguages, locale],
  )

  const loadRef = useRef<() => Promise<void>>(async () => {})

  const applyProductToForm = useCallback((product: AdminProductDetail) => {
    setTitle(product.title)
    setSlug(product.slug)
    setDescription(product.description)
    setProductCode(product.productCode)
    setPrice(String(product.price))
    setCompareAtPrice(product.compareAtPrice != null ? String(product.compareAtPrice) : '')
    setIsActive(product.isActive)
    setSlugTouched(true)
    if (product.subCategoryTitle) {
      setSubCategoryLabel(formatSubCategoryTitles(product.categoryTitle, product.subCategoryTitle))
    }
  }, [])

  const resetForm = useCallback(() => {
    setTitle('')
    setSlug('')
    setDescription('')
    setProductCode('')
    setPrice('')
    setCompareAtPrice('')
    setCategoryId('')
    setSubCategoryId('')
    setSubCategoryLabel('')
    setSubCategories([])
    setIsActive(true)
    setSlugTouched(false)
    setError(null)
  }, [])

  const loadSubCategoriesForCategory = useCallback(
    async (targetCategoryId: string, preferredSubCategoryId = '') => {
      if (
        !accessToken ||
        accessToken === 'mock-access-token' ||
        contentLanguageId == null ||
        !targetCategoryId
      ) {
        setSubCategories([])
        setSubCategoryId('')
        setSubCategoryLabel('')
        return []
      }

      setSubCategoriesLoading(true)
      try {
        const subResult = await adminSubCategoryService.getAllForSelect(accessToken, contentLocale, {
          languageId: contentLanguageId,
          categoryId: targetCategoryId,
        })
        setSubCategories(subResult.items)

        const nextSubCategoryId =
          preferredSubCategoryId &&
          subResult.items.some((item) => item.id === preferredSubCategoryId)
            ? preferredSubCategoryId
            : ''

        setSubCategoryId(nextSubCategoryId)
        const match = subResult.items.find((item) => item.id === nextSubCategoryId)
        setSubCategoryLabel(match ? formatSubCategoryLabel(match) : '')

        return subResult.items
      } catch (err) {
        setError(resolveAdminMutationError(err, t('dashboard.products.loadFailed')))
        setSubCategories([])
        setSubCategoryId('')
        setSubCategoryLabel('')
        return []
      } finally {
        setSubCategoriesLoading(false)
      }
    },
    [accessToken, contentLanguageId, contentLocale, t],
  )

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
      setError(t('dashboard.products.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const categoryResult = await adminCategoryService.getAllForSelect(accessToken, contentLocale, {
        languageId: contentLanguageId,
      })
      setCategories(categoryResult.items)

      if (isEdit && productId) {
        const product = await adminProductService.get(accessToken, contentLocale, productId)
        if (!product) {
          setError(t('dashboard.products.notFound'))
          return
        }

        applyProductToForm(product)
        const targetSubCategoryId = product.subCategoryId || subCategoryIdFromUrl

        let resolvedCategoryId = ''
        if (targetSubCategoryId) {
          const subCategory = await adminSubCategoryService.get(
            accessToken,
            contentLocale,
            targetSubCategoryId,
          )
          resolvedCategoryId = subCategory?.categoryId ?? ''
          if (subCategory) {
            setSubCategoryLabel(formatSubCategoryLabel(subCategory))
          } else if (product.subCategoryTitle) {
            setSubCategoryLabel(
              formatSubCategoryTitles(product.categoryTitle, product.subCategoryTitle),
            )
          }
        }

        if (resolvedCategoryId) {
          setCategoryId(resolvedCategoryId)
          await loadSubCategoriesForCategory(resolvedCategoryId, targetSubCategoryId)
        } else {
          setSubCategories([])
          setSubCategoryId(targetSubCategoryId)
        }
      } else {
        let initialCategoryId = ''
        let initialSubCategoryId = ''

        if (subCategoryIdFromUrl) {
          const subCategory = await adminSubCategoryService.get(
            accessToken,
            contentLocale,
            subCategoryIdFromUrl,
          )
          if (subCategory) {
            initialCategoryId = subCategory.categoryId
            initialSubCategoryId = subCategory.id
            setSubCategoryLabel(formatSubCategoryLabel(subCategory))
          }
        }

        if (initialCategoryId) {
          setCategoryId(initialCategoryId)
          await loadSubCategoriesForCategory(initialCategoryId, initialSubCategoryId)
        } else {
          setSubCategories([])
          setSubCategoryId('')
          setSubCategoryLabel('')
        }
      }
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.products.loadFailed')))
    } finally {
      setLoading(false)
    }
  }, [
    accessToken,
    applyProductToForm,
    contentLanguageId,
    contentLocale,
    isEdit,
    loadSubCategoriesForCategory,
    productId,
    subCategoryIdFromUrl,
    t,
  ])

  loadRef.current = load

  useEffect(() => {
    if (languageLoading || contentLanguageLoading || contentLanguageId == null) return

    resetForm()
    void loadRef.current()
  }, [
    accessToken,
    contentLanguageId,
    contentLanguageLoading,
    languageLoading,
    productId,
    subCategoryIdFromUrl,
    resetForm,
  ])

  const handleCategoryChange = (value: string) => {
    setCategoryId(value)
    setSubCategoryId('')
    setSubCategoryLabel('')
    if (value) {
      void loadSubCategoriesForCategory(value)
    } else {
      setSubCategories([])
    }
  }

  const handleSubCategoryChange = (value: string) => {
    setSubCategoryId(value)
    const match = subCategories.find((item) => item.id === value)
    setSubCategoryLabel(match ? formatSubCategoryLabel(match) : '')
  }

  const handleTitleChange = (value: string) => {
    setTitle(value)
    if (!slugTouched) setSlug(slugifyTitle(value))
  }

  const selectedSubCategory = subCategories.find((item) => item.id === subCategoryId)
  const displayedSubCategoryLabel =
    selectedSubCategory ? formatSubCategoryLabel(selectedSubCategory) : subCategoryLabel

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
      setError(t('dashboard.products.saveFailed'))
      return
    }

    const parsedPrice = Number(price)
    const parsedCompare = compareAtPrice.trim() ? Number(compareAtPrice) : null

    if (
      !title.trim() ||
      !slug.trim() ||
      !productCode.trim() ||
      !categoryId ||
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
      if (isEdit && productId) {
        await adminProductService.update(accessToken, contentLocale, {
          id: productId,
          languageId: contentLanguageId,
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
        const newId = await adminProductService.create(accessToken, contentLocale, {
          languageId: contentLanguageId,
          title: title.trim(),
          slug: slug.trim(),
          description: description.trim(),
          productCode: productCode.trim(),
          price: parsedPrice,
          compareAtPrice: parsedCompare,
          subCategoryId,
        })
        const params = new URLSearchParams({ id: newId })
        if (subCategoryId) params.set('subCategoryId', subCategoryId)
        navigate(`/account/dashboard/products/edit?${params.toString()}`, { replace: true })
        return
      }
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.products.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (loading || languageLoading || contentLanguageLoading) {
    return (
      <div>
        <DashboardPageHeader
          title={
            isEdit ? t('dashboard.products.editTitle') : t('dashboard.products.createTitle')
          }
          description={
            isEdit
              ? t('dashboard.products.editDescription')
              : t('dashboard.products.createDescription')
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
        title={isEdit ? t('dashboard.products.editTitle') : t('dashboard.products.createTitle')}
        description={
          isEdit
            ? t('dashboard.products.editDescription')
            : t('dashboard.products.createDescription')
        }
        icon={<ProductsIcon size={22} />}
      />

      <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
        <AdminContentLanguageField
          value={contentLanguageId}
          onChange={setContentLanguageId}
          languages={formLanguages}
        />

        <AdminField label={t('dashboard.products.fieldCategory')}>
          <select
            value={categoryId}
            onChange={(e) => handleCategoryChange(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.products.selectCategory')}</option>
            {categories.map((item) => (
              <option key={item.id} value={item.id}>
                {item.title}
              </option>
            ))}
          </select>
        </AdminField>

        <AdminField label={t('dashboard.products.fieldSubCategory')}>
          <select
            value={subCategoryId}
            onChange={(e) => handleSubCategoryChange(e.target.value)}
            className={adminInputClass}
            disabled={!categoryId || subCategoriesLoading}
          >
            <option value="">
              {!categoryId
                ? t('dashboard.products.selectCategoryFirst')
                : t('dashboard.products.selectSubCategory')}
            </option>
            {subCategories.map((item) => (
              <option key={item.id} value={item.id}>
                {formatSubCategoryLabel(item)}
              </option>
            ))}
            {subCategoryId &&
              !subCategories.some((item) => item.id === subCategoryId) &&
              displayedSubCategoryLabel && (
                <option value={subCategoryId}>{displayedSubCategoryLabel}</option>
              )}
          </select>
          {!categoryId && (
            <p className="mt-1.5 text-xs text-text-muted">
              {t('dashboard.products.selectCategoryFirstHint')}
            </p>
          )}
          {categoryId && !subCategoriesLoading && subCategories.length === 0 && (
            <p className="mt-1.5 text-xs text-text-muted">
              {t('dashboard.products.noSubCategoriesInCategory')}
            </p>
          )}
          {isEdit && displayedSubCategoryLabel && (
            <p className="mt-1.5 text-xs font-medium text-warm">
              {t('dashboard.products.currentSubCategory', { label: displayedSubCategoryLabel })}
            </p>
          )}
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
            rows={5}
            className={adminInputClass}
            placeholder={t('dashboard.products.descriptionPlaceholder')}
          />
        </AdminField>

        <div className="grid gap-5 sm:grid-cols-2">
          <AdminField label={t('dashboard.products.fieldPrice')}>
            <input
              type="number"
              min="0"
              step="1"
              value={price}
              onChange={(e) => setPrice(e.target.value)}
              className={adminInputClass}
              dir="ltr"
            />
          </AdminField>
          <AdminField label={t('dashboard.products.fieldCompareAtPrice')}>
            <input
              type="number"
              min="0"
              step="1"
              value={compareAtPrice}
              onChange={(e) => setCompareAtPrice(e.target.value)}
              className={adminInputClass}
              dir="ltr"
              placeholder={t('dashboard.products.compareAtPlaceholder')}
            />
          </AdminField>
        </div>

        {isEdit && (
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

        <div className="flex flex-wrap gap-3">
          <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
            {saving ? (
              <InlineLoading label={t('dashboard.products.saving')} />
            ) : (
              t('dashboard.products.save')
            )}
          </Button>
          <Button
            variant="secondary"
            onClick={() => navigate('/account/dashboard/products')}
            disabled={saving}
          >
            {t('dashboard.products.cancel')}
          </Button>
        </div>
      </div>

      {isEdit && productId && (
        <div id="product-images" className="mt-6">
          <ProductImagesPanel
            embedded
            productId={productId}
            productTitle={title}
            productCode={productCode}
          />
        </div>
      )}

      {!isEdit && (
        <p className="mt-6 rounded-sm border border-dashed border-border bg-surface-muted/20 px-4 py-3 text-sm text-text-muted">
          {t('dashboard.productImages.saveFirstHint')}
        </p>
      )}
    </div>
  )
}
