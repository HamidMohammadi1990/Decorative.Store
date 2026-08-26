import { useCallback, useEffect, useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { AdminProductDetail, AdminSubCategory } from '@/models/admin/catalog.model'
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
import { adminSubCategoryService } from '@/services/adminSubCategoryService'
import { slugifyTitle } from '@/services/admin/adminCatalogNormalize'
import { useUserStore } from '@/stores/userStore'
import { ProductManageModal } from '@/components/dashboard/ProductManageModal'

function mergeSubCategory(
  items: AdminSubCategory[],
  candidate: AdminSubCategory | null,
): AdminSubCategory[] {
  if (!candidate || items.some((item) => item.id === candidate.id)) return items
  return [candidate, ...items]
}

function formatSubCategoryLabel(item: AdminSubCategory): string {
  const category = item.categoryTitle || item.categoryCode
  return category ? `${category} / ${item.title}` : item.title
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

  const [subCategories, setSubCategories] = useState<AdminSubCategory[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

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
  const [imagesModalOpen, setImagesModalOpen] = useState(false)

  const contentLocale =
    contentLanguageId != null
      ? localeFromLanguageId(formLanguages, contentLanguageId, locale)
      : locale

  const resolveSubCategorySelection = useCallback(
    async (
      targetSubCategoryId: string,
      items: AdminSubCategory[],
      product?: AdminProductDetail,
    ): Promise<AdminSubCategory[]> => {
      if (!targetSubCategoryId) return items

      let nextItems = items
      if (!nextItems.some((item) => item.id === targetSubCategoryId)) {
        const subCategory = await adminSubCategoryService.get(
          accessToken!,
          contentLocale,
          targetSubCategoryId,
        )
        nextItems = mergeSubCategory(nextItems, subCategory)
      }

      if (product?.subCategoryTitle) {
        setSubCategoryLabel(formatSubCategoryTitles(product.categoryTitle, product.subCategoryTitle))
      } else {
        const match = nextItems.find((item) => item.id === targetSubCategoryId)
        setSubCategoryLabel(match ? formatSubCategoryLabel(match) : '')
      }

      setSubCategoryId(targetSubCategoryId)
      return nextItems
    },
    [accessToken, contentLocale],
  )

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
    setSubCategoryId('')
    setSubCategoryLabel('')
    setIsActive(true)
    setSlugTouched(false)
    setError(null)
  }, [])

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
      setError(t('dashboard.products.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const subResult = await adminSubCategoryService.getAllForSelect(accessToken, contentLocale, {
        languageId: contentLanguageId,
      })

      if (isEdit && productId) {
        const product = await adminProductService.get(accessToken, contentLocale, productId)
        if (!product) {
          setError(t('dashboard.products.notFound'))
          return
        }

        applyProductToForm(product)
        const targetSubCategoryId = product.subCategoryId || subCategoryIdFromUrl
        const subCategoryItems = await resolveSubCategorySelection(
          targetSubCategoryId,
          subResult.items,
          product,
        )
        setSubCategories(subCategoryItems)
      } else {
        setSubCategories(subResult.items)
        const initialSubCategoryId =
          subCategoryIdFromUrl && subResult.items.some((item) => item.id === subCategoryIdFromUrl)
            ? subCategoryIdFromUrl
            : (subResult.items[0]?.id ?? '')
        setSubCategoryId(initialSubCategoryId)
        const match = subResult.items.find((item) => item.id === initialSubCategoryId)
        setSubCategoryLabel(match ? formatSubCategoryLabel(match) : '')
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
    productId,
    resolveSubCategorySelection,
    subCategoryIdFromUrl,
    t,
  ])

  useEffect(() => {
    if (!languageLoading && !contentLanguageLoading && contentLanguageId != null) {
      resetForm()
      void load()
    }
  }, [
    contentLanguageId,
    contentLanguageLoading,
    languageLoading,
    load,
    productId,
    resetForm,
  ])

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
        await adminProductService.create(accessToken, contentLocale, {
          languageId: contentLanguageId,
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
        action={
          isEdit && productId ? (
            <Button variant="secondary" onClick={() => setImagesModalOpen(true)}>
              {t('dashboard.products.manageImages')}
            </Button>
          ) : undefined
        }
      />

      <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
        <AdminContentLanguageField
          value={contentLanguageId}
          onChange={setContentLanguageId}
          languages={formLanguages}
        />
        <AdminField label={t('dashboard.products.fieldSubCategory')}>
          <select
            value={subCategoryId}
            onChange={(e) => handleSubCategoryChange(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.products.selectSubCategory')}</option>
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
        <ProductManageModal
          open={imagesModalOpen}
          tab="images"
          product={{
            id: productId,
            title,
            productCode,
            slug,
            isActive,
            creationDate: '',
            subCategoryId,
            translations: [],
          }}
          onClose={() => setImagesModalOpen(false)}
        />
      )}
    </div>
  )
}
