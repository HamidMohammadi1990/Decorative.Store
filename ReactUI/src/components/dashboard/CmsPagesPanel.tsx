import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminCmsPage } from '@/models/admin/cms.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { CmsPagesIcon, EditIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminGridActions,
  AdminGridIconButton,
} from '@/components/dashboard/admin/AdminGridActions'
import { AdminDataGrid } from '@/components/dashboard/admin/AdminDataGrid'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useAdminPagedList } from '@/hooks/useAdminPagedList'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminPageService } from '@/services/adminPageService'
import { slugifyTitle } from '@/services/admin/adminCatalogNormalize'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

const PAGE_TYPES = [
  { value: 1, labelKey: 'dashboard.cms.pages.typeGeneral' },
  { value: 2, labelKey: 'dashboard.cms.pages.typeHome' },
  { value: 3, labelKey: 'dashboard.cms.pages.typeCategory' },
  { value: 4, labelKey: 'dashboard.cms.pages.typeProduct' },
  { value: 5, labelKey: 'dashboard.cms.pages.typeCheckout' },
]

function pageTypeLabel(type: number, t: (key: string) => string): string {
  if (!Number.isFinite(type)) return '—'
  const match = PAGE_TYPES.find((item) => item.value === type)
  return match ? t(match.labelKey) : String(type)
}

export function CmsPagesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [saving, setSaving] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)

  const [title, setTitle] = useState('')
  const [slug, setSlug] = useState('')
  const [type, setType] = useState(2)
  const [metaTitle, setMetaTitle] = useState('')
  const [metaDescription, setMetaDescription] = useState('')
  const [isActive, setIsActive] = useState(true)
  const [slugTouched, setSlugTouched] = useState(false)

  const canLoad =
    !languageLoading &&
    Boolean(accessToken) &&
    accessToken !== 'mock-access-token'

  const fetchPage = useCallback(
    async (pageNumber: number, pageSize: number) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        throw new Error(t('dashboard.cms.pages.authRequired'))
      }
      return adminPageService.getAll(accessToken, locale, {
        pageNumber,
        pageSize,
        languageId: languageId ?? undefined,
      })
    },
    [accessToken, languageId, locale, t],
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
    changePageSize,
    reload,
  } = useAdminPagedList<AdminCmsPage>({
    fetchPage,
    initialPageSize: 20,
    enabled: canLoad && mode === 'list',
  })

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.cms.pages.loadFailed'))
    : null

  const resetForm = () => {
    setTitle('')
    setSlug('')
    setType(2)
    setMetaTitle('')
    setMetaDescription('')
    setIsActive(true)
    setSlugTouched(false)
    setEditingId(null)
    setFormError(null)
  }

  const openCreate = () => {
    resetForm()
    setMode('create')
  }

  const openEdit = (item: AdminCmsPage) => {
    setEditingId(item.id)
    setTitle(item.title)
    setSlug(item.slug)
    setType(item.type)
    setMetaTitle(item.metaTitle ?? '')
    setMetaDescription(item.metaDescription ?? '')
    setIsActive(item.isActive)
    setSlugTouched(true)
    setFormError(null)
    setMode('edit')
  }

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || languageId == null) {
      setFormError(t('dashboard.cms.pages.saveFailed'))
      return
    }

    if (!title.trim() || !slug.trim()) {
      setFormError(t('dashboard.cms.pages.validationRequired'))
      return
    }

    setSaving(true)
    setFormError(null)
    try {
      const payload = {
        languageId,
        title: title.trim(),
        slug: slug.trim(),
        type,
        metaTitle: metaTitle.trim() || undefined,
        metaDescription: metaDescription.trim() || undefined,
        isActive,
      }

      if (mode === 'edit' && editingId) {
        await adminPageService.update(accessToken, locale, { ...payload, id: editingId })
      } else {
        await adminPageService.create(accessToken, locale, payload)
      }
      backToList()
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.cms.pages.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        <DashboardPageHeader
          title={t(mode === 'edit' ? 'dashboard.cms.pages.editTitle' : 'dashboard.cms.pages.createTitle')}
          description={t(mode === 'edit' ? 'dashboard.cms.pages.editDescription' : 'dashboard.cms.pages.createDescription')}
          icon={<CmsPagesIcon size={22} />}
        />
        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.cms.pages.fieldTitle')}>
            <input
              value={title}
              onChange={(e) => {
                setTitle(e.target.value)
                if (!slugTouched) setSlug(slugifyTitle(e.target.value))
              }}
              className={adminInputClass}
            />
          </AdminField>
          <AdminField label={t('dashboard.cms.pages.fieldSlug')}>
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
          <AdminField label={t('dashboard.cms.pages.fieldType')}>
            <select value={type} onChange={(e) => setType(Number(e.target.value))} className={adminInputClass}>
              {PAGE_TYPES.map((item) => (
                <option key={item.value} value={item.value}>
                  {t(item.labelKey)}
                </option>
              ))}
            </select>
          </AdminField>
          <AdminField label={t('dashboard.cms.pages.fieldMetaTitle')}>
            <input value={metaTitle} onChange={(e) => setMetaTitle(e.target.value)} className={adminInputClass} />
          </AdminField>
          <AdminField label={t('dashboard.cms.pages.fieldMetaDescription')}>
            <textarea value={metaDescription} onChange={(e) => setMetaDescription(e.target.value)} className={adminInputClass} rows={3} />
          </AdminField>
          {mode === 'edit' && (
            <label className="flex items-center gap-2 text-sm text-text">
              <input type="checkbox" checked={isActive} onChange={(e) => setIsActive(e.target.checked)} className="size-4 rounded border-border text-warm focus:ring-warm" />
              {t('dashboard.cms.pages.fieldActive')}
            </label>
          )}
          {formError && <p className="text-sm text-sale">{formError}</p>}
          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? <InlineLoading label={t('dashboard.cms.pages.saving')} /> : t('dashboard.cms.pages.save')}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.cms.pages.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  if (!accessToken || accessToken === 'mock-access-token') {
    return (
      <DashboardEmptyState
        icon={<CmsPagesIcon size={28} />}
        title={t('dashboard.cms.pages.loadFailedTitle')}
        message={t('dashboard.cms.pages.authRequired')}
      />
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.cms.pages.title')}
        description={t('dashboard.cms.pages.description')}
        icon={<CmsPagesIcon size={22} />}
        action={<Button variant="warm" onClick={openCreate}>{t('dashboard.cms.pages.add')}</Button>}
      />

      {languageLoading || (listLoading && items.length === 0) ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.cms.pages.loading')} />
        </div>
      ) : listErrorMessage && items.length === 0 ? (
        <DashboardEmptyState
          icon={<CmsPagesIcon size={28} />}
          title={t('dashboard.cms.pages.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.cms.pages.retry')}
            </Button>
          }
        />
      ) : totalCount === 0 && !listLoading ? (
        <DashboardEmptyState
          icon={<CmsPagesIcon size={28} />}
          title={t('dashboard.cms.pages.emptyTitle')}
          message={t('dashboard.cms.pages.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.cms.pages.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {formError && <p className="text-sm text-sale">{formError}</p>}
          {listErrorMessage && <p className="text-sm text-sale">{listErrorMessage}</p>}

          <AdminDataGrid
            columns={[
              {
                id: 'title',
                header: t('dashboard.cms.pages.fieldTitle'),
                cell: (item) => (
                  <div className="min-w-0">
                    <p className="truncate font-semibold text-text">{item.title}</p>
                    <p className="mt-0.5 text-xs text-text-muted" dir="ltr">
                      {item.slug || '—'}
                    </p>
                  </div>
                ),
              },
              {
                id: 'type',
                header: t('dashboard.cms.pages.fieldType'),
                cell: (item) => (
                  <span className="text-sm text-text-muted">
                    {pageTypeLabel(item.type, t)}
                  </span>
                ),
              },
              {
                id: 'status',
                header: t('dashboard.cms.pages.fieldActive'),
                align: 'center',
                cell: (item) => (
                  <span
                    className={`inline-flex rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                      item.isActive
                        ? 'bg-warm-soft text-warm'
                        : 'bg-surface-muted text-text-muted'
                    }`}
                  >
                    {item.isActive
                      ? t('dashboard.cms.pages.statusActive')
                      : t('dashboard.cms.pages.statusInactive')}
                  </span>
                ),
              },
              {
                id: 'actions',
                header: '',
                align: 'right',
                cell: (item) => (
                  <AdminGridActions>
                    <AdminGridIconButton
                      label={t('dashboard.cms.pages.edit')}
                      icon={<EditIcon size={15} />}
                      onClick={() => openEdit(item)}
                      disabled={saving}
                    />
                  </AdminGridActions>
                ),
              },
            ]}
            rows={items}
            rowKey={(item) => item.id}
            loading={listLoading}
            loadingLabel={t('dashboard.cms.pages.loading')}
            pagination={{
              pageNumber,
              pageSize,
              totalCount,
              totalPages,
              onPageChange: goToPage,
              onPageSizeChange: changePageSize,
            }}
          />
        </div>
      )}
    </div>
  )
}
