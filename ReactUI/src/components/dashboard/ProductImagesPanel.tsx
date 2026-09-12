import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import type {
  AdminProductFile,
  AdminProductListItem,
  ProductFileKind,
} from '@/models/admin/catalog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { ProductsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { AdminContentLanguageField } from '@/components/dashboard/admin/AdminContentLanguageField'
import { AdminListGridHeader } from '@/components/dashboard/admin/AdminListGridHeader'
import { AdminRowNumber } from '@/components/dashboard/admin/AdminRowNumber'
import type { ProductPanelEmbedProps } from '@/components/dashboard/admin/productPanelEmbed'
import { embeddedProductLabel } from '@/components/dashboard/admin/productPanelEmbed'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { localeFromLanguageId } from '@/extensions/languageCode'
import { useAdminContentLanguage } from '@/hooks/useAdminContentLanguage'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminProductService } from '@/services/adminProductService'
import { adminProductFileService } from '@/services/adminProductFileService'
import { useUserStore } from '@/stores/userStore'

const MAX_FILE_MB = 5

export function ProductImagesPanel({
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
  const [selectedProductId, setSelectedProductId] = useState(productIdFromSource)
  const [products, setProducts] = useState<AdminProductListItem[]>([])
  const [items, setItems] = useState<AdminProductFile[]>([])
  const [loading, setLoading] = useState(true)
  const [uploading, setUploading] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const [title, setTitle] = useState('')
  const [fileKind, setFileKind] = useState<ProductFileKind>('Gallery')
  const [isMain, setIsMain] = useState(false)
  const [pendingFiles, setPendingFiles] = useState<File[]>([])
  const [previews, setPreviews] = useState<string[]>([])

  const selectedProduct = useMemo(() => {
    if (embedded && fixedProductId) {
      return {
        id: fixedProductId,
        title: fixedProductTitle ?? '',
        productCode: fixedProductCode ?? '',
      } as AdminProductListItem
    }
    return products.find((item) => item.id === selectedProductId) ?? null
  }, [
    embedded,
    fixedProductCode,
    fixedProductId,
    fixedProductTitle,
    products,
    selectedProductId,
  ])

  useEffect(() => {
    if (embedded && fixedProductId) setSelectedProductId(fixedProductId)
  }, [embedded, fixedProductId])

  useEffect(() => {
    return () => {
      previews.forEach((url) => URL.revokeObjectURL(url))
    }
  }, [previews])

  const loadProducts = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') return []
    const result = await adminProductService.getAll(accessToken, contentLocale, {
      pageSize: 200,
      languageId: contentLanguageId ?? undefined,
    })
    setProducts(result.items)
    return result.items
  }, [accessToken, contentLanguageId, contentLocale])

  const loadImages = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token' || !selectedProductId) {
      setItems([])
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const result = await adminProductFileService.getAll(accessToken, contentLocale, {
        productId: selectedProductId,
        pageSize: 100,
      })
      setItems(result.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productImages.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, contentLocale, selectedProductId, t])

  useEffect(() => {
    if (embedded || languageLoading || contentLanguageLoading) return

    void loadProducts().catch((err) => {
      setError(resolveAdminMutationError(err, t('dashboard.productImages.loadFailed')))
    })
  }, [embedded, languageLoading, contentLanguageLoading, loadProducts, t])

  useEffect(() => {
    if (embedded) return
    if (!productIdFromSource && products.length > 0 && !selectedProductId) {
      const firstId = products[0].id
      setSelectedProductId(firstId)
      setSearchParams({ productId: firstId })
    }
  }, [embedded, productIdFromSource, products, selectedProductId, setSearchParams])

  useEffect(() => {
    if (!languageLoading && !contentLanguageLoading && selectedProductId) void loadImages()
  }, [contentLanguageLoading, languageLoading, loadImages, selectedProductId])

  const handleProductChange = (productId: string) => {
    setSelectedProductId(productId)
    if (productId) {
      setSearchParams({ productId })
    } else {
      setSearchParams({})
    }
  }

  const handleFileChange = (fileList: FileList | null) => {
    previews.forEach((url) => URL.revokeObjectURL(url))

    if (!fileList || fileList.length === 0) {
      setPendingFiles([])
      setPreviews([])
      return
    }

    const files = Array.from(fileList).filter((file) => file.type.startsWith('image/'))
    setPendingFiles(files)
    setPreviews(files.map((file) => URL.createObjectURL(file)))
  }

  const handleUpload = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
      setError(t('dashboard.productImages.saveFailed'))
      return
    }

    if (!selectedProductId || pendingFiles.length === 0) {
      setError(t('dashboard.productImages.validationRequired'))
      return
    }

    const tooLarge = pendingFiles.some((file) => file.size > MAX_FILE_MB * 1024 * 1024)
    if (tooLarge) {
      setError(t('dashboard.productImages.fileTooLarge', { max: MAX_FILE_MB }))
      return
    }

    setUploading(true)
    setError(null)
    try {
      await adminProductFileService.createRange(
        accessToken,
        contentLocale,
        pendingFiles.map((file, index) => ({
          productId: selectedProductId,
          languageId: contentLanguageId,
          title: title.trim() || file.name.replace(/\.[^.]+$/, ''),
          image: file,
          isIndex: isMain && index === 0,
          kind: fileKind,
        })),
      )
      setTitle('')
      setIsMain(false)
      setFileKind('Gallery')
      setPendingFiles([])
      previews.forEach((url) => URL.revokeObjectURL(url))
      setPreviews([])
      await loadImages()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productImages.saveFailed')))
    } finally {
      setUploading(false)
    }
  }

  const handleToggleStatus = async (item: AdminProductFile) => {
    if (!accessToken || accessToken === 'mock-access-token') return

    setSaving(true)
    setError(null)
    try {
      await adminProductFileService.updateStatus(accessToken, contentLocale, item.id, !item.isActive)
      await loadImages()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productImages.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleSetMain = async (item: AdminProductFile) => {
    if (
      !accessToken
      || accessToken === 'mock-access-token'
      || item.isMain
      || item.kind !== 'Gallery'
    ) {
      return
    }

    setSaving(true)
    setError(null)
    try {
      await adminProductFileService.setMain(accessToken, contentLocale, item.id)
      await loadImages()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productImages.setMainFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!(await confirm({ message: t('dashboard.productImages.deleteConfirm') }))) return

    setSaving(true)
    setError(null)
    try {
      await adminProductFileService.delete(accessToken, id)
      await loadImages()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productImages.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  return (
    <div>
      {!embedded && (
        <DashboardPageHeader
          title={t('dashboard.productImages.title')}
          description={t('dashboard.productImages.description')}
          icon={<ProductsIcon size={22} />}
          action={
            <Link
              to="/account/dashboard/products"
              className="inline-flex items-center justify-center rounded-sm border border-border-strong px-4 py-2 text-sm font-medium text-text transition-colors hover:bg-surface-muted"
            >
              {t('dashboard.productImages.backToProducts')}
            </Link>
          }
        />
      )}

      <AdminContentLanguageField
        className={embedded ? 'mb-4' : 'mb-4'}
        value={contentLanguageId}
        onChange={setContentLanguageId}
        languages={formLanguages}
      />

      <div className="mb-6 space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
        {!embedded ? (
          <AdminField label={t('dashboard.productImages.fieldProduct')}>
            <select
              value={selectedProductId}
              onChange={(e) => handleProductChange(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.productImages.selectProduct')}</option>
              {products.map((product) => (
                <option key={product.id} value={product.id}>
                  {product.title} ({product.productCode})
                </option>
              ))}
            </select>
          </AdminField>
        ) : (
          <p className="text-sm font-medium text-text">
            {embeddedProductLabel(fixedProductTitle, fixedProductCode)}
          </p>
        )}

        {selectedProduct && !embedded && (
          <p className="text-sm text-text-muted">
            {t('dashboard.productImages.selectedProduct', {
              title: selectedProduct.title,
              code: selectedProduct.productCode,
            })}
          </p>
        )}

        <AdminField label={t('dashboard.productImages.fieldTitle')}>
          <input
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className={adminInputClass}
            placeholder={t('dashboard.productImages.titlePlaceholder')}
          />
        </AdminField>

        <AdminField label={t('dashboard.productImages.fieldKind')}>
          <select
            value={fileKind}
            onChange={(e) => {
              const nextKind = e.target.value as ProductFileKind
              setFileKind(nextKind)
              if (nextKind !== 'Gallery') setIsMain(false)
            }}
            className={adminInputClass}
            disabled={!selectedProductId}
          >
            <option value="Gallery">{t('dashboard.productImages.kindGallery')}</option>
            <option value="RoomLayout">{t('dashboard.productImages.kindRoomLayout')}</option>
          </select>
          {fileKind === 'RoomLayout' && (
            <p className="mt-1.5 text-xs text-text-muted">
              {t('dashboard.productImages.kindRoomLayoutHint')}
            </p>
          )}
        </AdminField>

        <AdminField label={t('dashboard.productImages.fieldFiles')}>
          <label className="flex cursor-pointer flex-col items-center justify-center rounded-lg border-2 border-dashed border-border bg-surface px-4 py-8 transition-colors hover:border-warm hover:bg-warm-soft/30">
            {previews.length > 0 ? (
              <div className="grid w-full grid-cols-2 gap-3 sm:grid-cols-3 md:grid-cols-4">
                {previews.map((preview) => (
                  <img
                    key={preview}
                    src={preview}
                    alt=""
                    className={`aspect-square w-full rounded-md ${
                      fileKind === 'RoomLayout' ? 'object-contain' : 'object-cover'
                    }`}
                  />
                ))}
              </div>
            ) : (
              <>
                <span className="text-sm font-medium text-text">
                  {t('dashboard.productImages.uploadHint')}
                </span>
                <span className="mt-1 text-xs text-text-muted">
                  {fileKind === 'RoomLayout'
                    ? t('dashboard.productImages.uploadFormatsLayout', { max: MAX_FILE_MB })
                    : t('dashboard.productImages.uploadFormats', { max: MAX_FILE_MB })}
                </span>
              </>
            )}
            <input
              type="file"
              accept={fileKind === 'RoomLayout' ? 'image/png,image/webp' : 'image/*'}
              multiple
              className="sr-only"
              onChange={(e) => handleFileChange(e.target.files)}
              disabled={uploading || !selectedProductId}
            />
          </label>
        </AdminField>

        {fileKind === 'Gallery' && (
          <label className="flex items-center gap-2 text-sm text-text">
            <input
              type="checkbox"
              checked={isMain}
              onChange={(e) => setIsMain(e.target.checked)}
              className="size-4 rounded border-border text-warm focus:ring-warm"
              disabled={!selectedProductId}
            />
            {t('dashboard.productImages.fieldMain')}
          </label>
        )}

        {error && <p className="text-sm text-sale">{error}</p>}

        <Button
          variant="warm"
          onClick={() => void handleUpload()}
          disabled={uploading || saving || !selectedProductId || pendingFiles.length === 0}
        >
          {uploading ? (
            <InlineLoading label={t('dashboard.productImages.uploading')} />
          ) : (
            t('dashboard.productImages.upload')
          )}
        </Button>
      </div>

      {!selectedProductId ? (
        <DashboardEmptyState
          icon={<ProductsIcon size={28} />}
          title={t('dashboard.productImages.noProductTitle')}
          message={t('dashboard.productImages.noProductMessage')}
        />
      ) : loading || languageLoading || contentLanguageLoading ? (
        <div className="flex justify-center py-10">
          <InlineLoading label={t('dashboard.productImages.loading')} />
        </div>
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductsIcon size={28} />}
          title={t('dashboard.productImages.emptyTitle')}
          message={t('dashboard.productImages.emptyMessage')}
        />
      ) : (
        <div className="space-y-3">
          <p className="text-xs text-text-muted">
            {t('dashboard.productImages.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            <AdminListGridHeader />
            {items.map((item, index) => (
              <li
                key={item.id}
                className="flex flex-wrap items-center gap-3 px-4 py-3 sm:px-5"
              >
                <AdminRowNumber value={index + 1} />
                <div className="size-12 shrink-0 overflow-hidden rounded-sm bg-surface-muted">
                  <img
                    src={item.imageUrl}
                    alt={item.title || item.productTitle}
                    className={`size-full ${
                      item.kind === 'RoomLayout' ? 'object-contain' : 'object-cover'
                    }`}
                  />
                </div>
                <div className="min-w-0 flex-1">
                  <div className="flex items-start gap-2">
                    {item.kind === 'Gallery' && (
                      <label
                        className="mt-0.5 flex shrink-0 cursor-pointer items-center"
                        title={t('dashboard.productImages.setMainHint')}
                      >
                        <input
                          type="radio"
                          name={`product-main-${selectedProductId}`}
                          checked={item.isMain}
                          onChange={() => void handleSetMain(item)}
                          disabled={saving || item.isMain}
                          className="size-4 border-border text-warm focus:ring-warm"
                        />
                      </label>
                    )}
                    <div className="min-w-0 flex-1">
                  <p className="truncate text-sm font-semibold text-text">
                    {item.title || item.fileName}
                  </p>
                  <div className="mt-1.5 flex flex-wrap gap-2">
                    <span className="rounded-sm bg-surface-muted px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide text-text-muted">
                      {item.kind === 'RoomLayout'
                        ? t('dashboard.productImages.kindRoomLayoutBadge')
                        : t('dashboard.productImages.kindGalleryBadge')}
                    </span>
                    {item.isMain && (
                      <span className="rounded-sm bg-warm-soft px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide text-warm">
                        {t('dashboard.productImages.mainBadge')}
                      </span>
                    )}
                    <span
                      className={`rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                        item.isActive
                          ? 'bg-warm-soft text-warm'
                          : 'bg-surface-muted text-text-muted'
                      }`}
                    >
                      {item.isActive
                        ? t('dashboard.productImages.statusActive')
                        : t('dashboard.productImages.statusInactive')}
                    </span>
                  </div>
                    </div>
                  </div>
                </div>
                <div className="flex flex-wrap items-center gap-2">
                  <Button
                    variant="secondary"
                    className="py-1.5 text-xs"
                    onClick={() => void handleToggleStatus(item)}
                    disabled={saving}
                  >
                    {item.isActive
                      ? t('dashboard.productImages.hide')
                      : t('dashboard.productImages.show')}
                  </Button>
                  <Button
                    variant="ghost"
                    className="py-1.5 text-xs text-sale hover:bg-sale/10"
                    onClick={() => void handleDelete(item.id)}
                    disabled={saving}
                  >
                    {t('dashboard.productImages.delete')}
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
