import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminMarketingPromo, MarketingPromoType } from '@/models/admin/marketingPromo.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { MarketingPromoIcon, EditIcon, DeleteIcon } from '@/components/dashboard/DashboardIcons'
import { AdminDataGrid } from '@/components/dashboard/admin/AdminDataGrid'
import {
  AdminGridActions,
  AdminGridIconButton,
} from '@/components/dashboard/admin/AdminGridActions'
import { AdminContentLanguageField } from '@/components/dashboard/admin/AdminContentLanguageField'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useAdminContentLanguage } from '@/hooks/useAdminContentLanguage'
import { useAdminPagedList } from '@/hooks/useAdminPagedList'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import {
  adminMarketingPromoService,
  marketingPromoService,
  resolveMarketingImageSrc,
} from '@/services/marketingPromoService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

const MAX_FILE_MB = 5

export function MarketingPromosPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()
  const {
    contentLanguageId,
    setContentLanguageId,
    languages: formLanguages,
    loading: contentLanguageLoading,
  } = useAdminContentLanguage()

  const [mode, setMode] = useState<Mode>('list')
  const [saving, setSaving] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [promoType, setPromoType] = useState<MarketingPromoType>(1)
  const [title, setTitle] = useState('')
  const [subtitle, setSubtitle] = useState('')
  const [linkLabel, setLinkLabel] = useState('')
  const [linkHref, setLinkHref] = useState('')
  const [imageFileName, setImageFileName] = useState('')
  const [imagePreview, setImagePreview] = useState('')
  const [uploading, setUploading] = useState(false)
  const [priority, setPriority] = useState(0)
  const [isActive, setIsActive] = useState(true)
  const [disclaimer, setDisclaimer] = useState('')
  const [disclaimerLinkLabel, setDisclaimerLinkLabel] = useState('')
  const [disclaimerLinkHref, setDisclaimerLinkHref] = useState('')
  const [disclaimerSaving, setDisclaimerSaving] = useState(false)
  const [disclaimerLoading, setDisclaimerLoading] = useState(false)

  const canLoad =
    !languageLoading &&
    !contentLanguageLoading &&
    contentLanguageId != null &&
    Boolean(accessToken) &&
    accessToken !== 'mock-access-token'

  const fetchPage = useCallback(
    async (pageNumber: number, pageSize: number) => {
      if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
        throw new Error(t('dashboard.marketingPromos.authRequired'))
      }
      return adminMarketingPromoService.getAll(accessToken, locale, {
        pageNumber,
        pageSize,
        languageId: contentLanguageId,
      })
    },
    [accessToken, contentLanguageId, locale, t],
  )

  const {
    items,
    loading: listLoading,
    error: listError,
    pageNumber,
    pageSize,
    totalCount,
    totalPages,
    goToPage,
    reload,
  } = useAdminPagedList<AdminMarketingPromo>({
    fetchPage,
    initialPageSize: 20,
    enabled: canLoad && mode === 'list',
  })

  const loadDisclaimer = useCallback(async () => {
    if (!contentLanguageId) return
    setDisclaimerLoading(true)
    try {
      const strip = await marketingPromoService.getPromoStrip(locale, contentLanguageId)
      setDisclaimer(strip?.disclaimer ?? '')
      setDisclaimerLinkLabel(strip?.disclaimerLink?.label ?? '')
      setDisclaimerLinkHref(strip?.disclaimerLink?.href ?? '')
    } catch {
      setDisclaimer('')
      setDisclaimerLinkLabel('')
      setDisclaimerLinkHref('')
    } finally {
      setDisclaimerLoading(false)
    }
  }, [contentLanguageId, locale])

  useEffect(() => {
    if (canLoad && mode === 'list') {
      void loadDisclaimer()
    }
  }, [canLoad, loadDisclaimer, mode])

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.marketingPromos.loadFailed'))
    : null

  const resetForm = () => {
    setPromoType(1)
    setTitle('')
    setSubtitle('')
    setLinkLabel('')
    setLinkHref('')
    setImageFileName('')
    setImagePreview('')
    setPriority(0)
    setIsActive(true)
    setEditingId(null)
    setFormError(null)
  }

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const openCreate = () => {
    resetForm()
    setMode('create')
  }

  const openEdit = (item: AdminMarketingPromo) => {
    setEditingId(item.id)
    setPromoType(item.promoType)
    setTitle(item.title)
    setSubtitle(item.subtitle ?? '')
    setLinkLabel(item.linkLabel)
    setLinkHref(item.linkHref)
    setImageFileName(item.imageFileName)
    setImagePreview(resolveMarketingImageSrc(item.imageFileName))
    setPriority(item.priority)
    setIsActive(item.isActive)
    setFormError(null)
    setMode('edit')
  }

  const handleImagePick = async (file: File | null) => {
    if (!file) return
    if (file.size > MAX_FILE_MB * 1024 * 1024) {
      setFormError(t('dashboard.marketingPromos.fileTooLarge', { max: MAX_FILE_MB }))
      return
    }
    if (!accessToken || accessToken === 'mock-access-token') {
      setFormError(t('dashboard.marketingPromos.authRequired'))
      return
    }

    setUploading(true)
    setFormError(null)
    try {
      const uploaded = await adminMarketingPromoService.uploadImage(accessToken, locale, file)
      setImageFileName(uploaded.imageFileName)
      setImagePreview(uploaded.imageUrl)
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.marketingPromos.uploadFailed')))
    } finally {
      setUploading(false)
    }
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
      setFormError(t('dashboard.marketingPromos.authRequired'))
      return
    }
    if (!title.trim() || !linkLabel.trim() || !linkHref.trim() || !imageFileName.trim()) {
      setFormError(t('dashboard.marketingPromos.validationRequired'))
      return
    }

    setSaving(true)
    setFormError(null)
    try {
      const payload = {
        languageId: contentLanguageId,
        promoType,
        title: title.trim(),
        subtitle: subtitle.trim() || null,
        linkLabel: linkLabel.trim(),
        linkHref: linkHref.trim(),
        imageFileName: imageFileName.trim(),
        priority,
      }

      if (mode === 'edit' && editingId) {
        await adminMarketingPromoService.update(accessToken, locale, {
          ...payload,
          id: editingId,
          isActive,
        })
      } else {
        await adminMarketingPromoService.create(accessToken, locale, payload)
      }

      backToList()
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.marketingPromos.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.marketingPromos.deleteConfirm'))) return

    try {
      await adminMarketingPromoService.delete(accessToken, locale, id)
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.marketingPromos.deleteFailed')))
    }
  }

  const handleSaveDisclaimer = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) return

    setDisclaimerSaving(true)
    try {
      await adminMarketingPromoService.updateDisclaimer(accessToken, locale, {
        languageId: contentLanguageId,
        disclaimer: disclaimer.trim() || null,
        disclaimerLinkLabel: disclaimerLinkLabel.trim() || null,
        disclaimerLinkHref: disclaimerLinkHref.trim() || null,
      })
      await loadDisclaimer()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.marketingPromos.disclaimerSaveFailed')))
    } finally {
      setDisclaimerSaving(false)
    }
  }

  const promoTypeLabel = (type: MarketingPromoType) =>
    type === 2
      ? t('dashboard.marketingPromos.typeNewArrival')
      : t('dashboard.marketingPromos.typeSale')

  if (mode !== 'list') {
    return (
      <div>
        <DashboardPageHeader
          title={
            mode === 'edit'
              ? t('dashboard.marketingPromos.editTitle')
              : t('dashboard.marketingPromos.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.marketingPromos.editDescription')
              : t('dashboard.marketingPromos.createDescription')
          }
          icon={<MarketingPromoIcon size={22} />}
        />

        <div className="mx-auto max-w-2xl space-y-5">
          <AdminContentLanguageField
            languages={formLanguages}
            value={contentLanguageId}
            onChange={setContentLanguageId}
            disabled={mode === 'edit'}
          />

          <AdminField label={t('dashboard.marketingPromos.fieldType')}>
            <select
              value={promoType}
              onChange={(e) => setPromoType(Number(e.target.value) as MarketingPromoType)}
              className={adminInputClass}
            >
              <option value={1}>{t('dashboard.marketingPromos.typeSale')}</option>
              <option value={2}>{t('dashboard.marketingPromos.typeNewArrival')}</option>
            </select>
          </AdminField>

          <AdminField label={t('dashboard.marketingPromos.fieldTitle')}>
            <input value={title} onChange={(e) => setTitle(e.target.value)} className={adminInputClass} />
          </AdminField>

          <AdminField label={t('dashboard.marketingPromos.fieldSubtitle')}>
            <input
              value={subtitle}
              onChange={(e) => setSubtitle(e.target.value)}
              className={adminInputClass}
            />
          </AdminField>

          <div className="grid gap-4 sm:grid-cols-2">
            <AdminField label={t('dashboard.marketingPromos.fieldLinkLabel')}>
              <input
                value={linkLabel}
                onChange={(e) => setLinkLabel(e.target.value)}
                className={adminInputClass}
              />
            </AdminField>
            <AdminField label={t('dashboard.marketingPromos.fieldLinkHref')}>
              <input
                value={linkHref}
                onChange={(e) => setLinkHref(e.target.value)}
                className={adminInputClass}
                dir="ltr"
              />
            </AdminField>
          </div>

          <AdminField label={t('dashboard.marketingPromos.fieldImage')}>
            <div className="space-y-3">
              {imagePreview && (
                <img
                  src={imagePreview}
                  alt=""
                  className="h-40 w-full rounded-sm border border-border object-cover"
                />
              )}
              <input
                type="file"
                accept="image/*"
                disabled={uploading}
                onChange={(e) => void handleImagePick(e.target.files?.[0] ?? null)}
                className="block w-full text-sm text-text-muted file:me-3 file:rounded-sm file:border-0 file:bg-warm-soft file:px-3 file:py-2 file:text-sm file:font-medium file:text-warm"
              />
              {uploading && <InlineLoading label={t('dashboard.marketingPromos.uploading')} />}
              <p className="text-xs text-text-muted">{t('dashboard.marketingPromos.fieldImageHint')}</p>
            </div>
          </AdminField>

          <AdminField label={t('dashboard.marketingPromos.fieldPriority')}>
            <input
              type="number"
              value={priority}
              onChange={(e) => setPriority(Number(e.target.value) || 0)}
              className={adminInputClass}
            />
          </AdminField>

          {mode === 'edit' && (
            <label className="flex items-center gap-2 text-sm text-text">
              <input
                type="checkbox"
                checked={isActive}
                onChange={(e) => setIsActive(e.target.checked)}
                className="size-4 rounded border-border"
              />
              {t('dashboard.marketingPromos.fieldActive')}
            </label>
          )}

          {formError && <p className="text-sm text-sale">{formError}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" disabled={saving} onClick={() => void handleSave()}>
              {saving ? t('dashboard.marketingPromos.saving') : t('dashboard.marketingPromos.save')}
            </Button>
            <Button variant="secondary" onClick={backToList}>
              {t('dashboard.marketingPromos.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  if (!canLoad) {
    return (
      <DashboardEmptyState
        icon={<MarketingPromoIcon size={28} />}
        title={t('dashboard.marketingPromos.emptyTitle')}
        message={t('dashboard.marketingPromos.authRequired')}
      />
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.marketingPromos.title')}
        description={t('dashboard.marketingPromos.description')}
        icon={<MarketingPromoIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate}>
            {t('dashboard.marketingPromos.add')}
          </Button>
        }
      />

      <AdminContentLanguageField
        className="mb-5"
        languages={formLanguages}
        value={contentLanguageId}
        onChange={setContentLanguageId}
      />

      {listLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.marketingPromos.loading')} />
        </div>
      ) : listErrorMessage ? (
        <DashboardEmptyState
          icon={<MarketingPromoIcon size={28} />}
          title={t('dashboard.marketingPromos.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.marketingPromos.retry')}
            </Button>
          }
        />
      ) : (items ?? []).length === 0 ? (
        <DashboardEmptyState
          icon={<MarketingPromoIcon size={28} />}
          title={t('dashboard.marketingPromos.emptyTitle')}
          message={t('dashboard.marketingPromos.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.marketingPromos.add')}
            </Button>
          }
        />
      ) : (
        <AdminDataGrid
          rows={items ?? []}
          rowKey={(item) => item.id}
          loading={listLoading}
          loadingLabel={t('dashboard.marketingPromos.loading')}
          pagination={{
            pageNumber,
            pageSize,
            totalCount,
            totalPages,
            onPageChange: goToPage,
          }}
          columns={[
            {
              id: 'title',
              header: t('dashboard.marketingPromos.colTitle'),
              cell: (item) => (
                <div>
                  <p className="font-medium text-text">{item.title}</p>
                  {item.subtitle && <p className="text-xs text-text-muted">{item.subtitle}</p>}
                </div>
              ),
            },
            {
              id: 'type',
              header: t('dashboard.marketingPromos.colType'),
              cell: (item) => promoTypeLabel(item.promoType),
            },
            {
              id: 'priority',
              header: t('dashboard.marketingPromos.colPriority'),
              cell: (item) => item.priority,
            },
            {
              id: 'status',
              header: t('dashboard.marketingPromos.colStatus'),
              cell: (item) => (
                <span
                  className={`inline-flex rounded-full px-2 py-0.5 text-xs font-medium ${
                    item.isActive ? 'bg-emerald-100 text-emerald-800' : 'bg-surface-muted text-text-muted'
                  }`}
                >
                  {item.isActive
                    ? t('dashboard.marketingPromos.statusActive')
                    : t('dashboard.marketingPromos.statusInactive')}
                </span>
              ),
            },
            {
              id: 'actions',
              header: t('dashboard.marketingPromos.colActions'),
              cell: (item) => (
                <AdminGridActions>
                  <AdminGridIconButton
                    label={t('dashboard.marketingPromos.edit')}
                    icon={<EditIcon size={15} />}
                    onClick={() => openEdit(item)}
                  />
                  <AdminGridIconButton
                    label={t('dashboard.marketingPromos.delete')}
                    icon={<DeleteIcon size={15} />}
                    onClick={() => void handleDelete(item.id)}
                  />
                </AdminGridActions>
              ),
            },
          ]}
        />
      )}

      <section className="mt-8 rounded-xl border border-border bg-surface-muted/30 p-5">
        <h3 className="text-base font-semibold text-text">{t('dashboard.marketingPromos.disclaimerTitle')}</h3>
        <p className="mt-1 text-sm text-text-muted">{t('dashboard.marketingPromos.disclaimerDescription')}</p>

        {disclaimerLoading ? (
          <div className="mt-4">
            <InlineLoading label={t('dashboard.marketingPromos.loading')} />
          </div>
        ) : (
          <div className="mt-4 space-y-4">
            <AdminField label={t('dashboard.marketingPromos.fieldDisclaimer')}>
              <input
                value={disclaimer}
                onChange={(e) => setDisclaimer(e.target.value)}
                className={adminInputClass}
              />
            </AdminField>
            <div className="grid gap-4 sm:grid-cols-2">
              <AdminField label={t('dashboard.marketingPromos.fieldDisclaimerLinkLabel')}>
                <input
                  value={disclaimerLinkLabel}
                  onChange={(e) => setDisclaimerLinkLabel(e.target.value)}
                  className={adminInputClass}
                />
              </AdminField>
              <AdminField label={t('dashboard.marketingPromos.fieldDisclaimerLinkHref')}>
                <input
                  value={disclaimerLinkHref}
                  onChange={(e) => setDisclaimerLinkHref(e.target.value)}
                  className={adminInputClass}
                  dir="ltr"
                />
              </AdminField>
            </div>
            <Button variant="secondary" disabled={disclaimerSaving} onClick={() => void handleSaveDisclaimer()}>
              {disclaimerSaving
                ? t('dashboard.marketingPromos.saving')
                : t('dashboard.marketingPromos.saveDisclaimer')}
            </Button>
          </div>
        )}
      </section>
    </div>
  )
}
