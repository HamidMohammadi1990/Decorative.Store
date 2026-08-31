import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminCmsSectionType } from '@/models/admin/cms.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { CmsSectionTypesIcon, EditIcon } from '@/components/dashboard/DashboardIcons'
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
import { adminSectionTypeService } from '@/services/adminSectionTypeService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function CmsSectionTypesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [saving, setSaving] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [name, setName] = useState('')
  const [isActive, setIsActive] = useState(true)

  const fetchPage = useCallback(
    (pageNumber: number, pageSize: number) =>
      adminSectionTypeService.getAll(accessToken!, locale, {
        pageNumber,
        pageSize,
        languageId: languageId ?? undefined,
      }),
    [accessToken, languageId, locale],
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
  } = useAdminPagedList<AdminCmsSectionType>({
    fetchPage,
    enabled: Boolean(accessToken && accessToken !== 'mock-access-token' && !languageLoading),
  })

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.cms.sectionTypes.loadFailed'))
    : null

  const resetForm = () => {
    setName('')
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

  const openEdit = (item: AdminCmsSectionType) => {
    setEditingId(item.id)
    setName(item.name)
    setIsActive(item.isActive)
    setFormError(null)
    setMode('edit')
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setFormError(t('dashboard.cms.sectionTypes.authRequired'))
      return
    }
    if (languageId == null || !name.trim()) {
      setFormError(t('dashboard.cms.sectionTypes.validationRequired'))
      return
    }

    setSaving(true)
    setFormError(null)
    try {
      const payload = { languageId, name: name.trim(), isActive }
      if (mode === 'edit' && editingId) {
        await adminSectionTypeService.update(accessToken, locale, { ...payload, id: editingId })
      } else {
        await adminSectionTypeService.create(accessToken, locale, payload)
      }
      reload()
      backToList()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.cms.sectionTypes.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        <DashboardPageHeader
          title={t(
            mode === 'edit'
              ? 'dashboard.cms.sectionTypes.editTitle'
              : 'dashboard.cms.sectionTypes.createTitle',
          )}
          icon={<CmsSectionTypesIcon size={22} />}
        />
        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.cms.sectionTypes.fieldName')}>
            <input
              value={name}
              onChange={(e) => setName(e.target.value)}
              className={adminInputClass}
              dir="ltr"
            />
          </AdminField>
          {mode === 'edit' && (
            <label className="flex items-center gap-2 text-sm text-text">
              <input
                type="checkbox"
                checked={isActive}
                onChange={(e) => setIsActive(e.target.checked)}
                className="size-4 rounded border-border text-warm focus:ring-warm"
              />
              {t('dashboard.cms.sectionTypes.fieldActive')}
            </label>
          )}
          {formError && <p className="text-sm text-sale">{formError}</p>}
          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.cms.sectionTypes.saving')} />
              ) : (
                t('dashboard.cms.sectionTypes.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.cms.sectionTypes.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  if (!accessToken || accessToken === 'mock-access-token') {
    return (
      <DashboardEmptyState
        icon={<CmsSectionTypesIcon size={28} />}
        title={t('dashboard.cms.sectionTypes.emptyTitle')}
        message={t('dashboard.cms.sectionTypes.authRequired')}
      />
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.cms.sectionTypes.title')}
        description={t('dashboard.cms.sectionTypes.description')}
        icon={<CmsSectionTypesIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate}>
            {t('dashboard.cms.sectionTypes.add')}
          </Button>
        }
      />

      {languageLoading || (listLoading && items.length === 0) ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.cms.sectionTypes.loading')} />
        </div>
      ) : listErrorMessage && items.length === 0 ? (
        <DashboardEmptyState
          icon={<CmsSectionTypesIcon size={28} />}
          title={t('dashboard.cms.sectionTypes.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.cms.sectionTypes.retry')}
            </Button>
          }
        />
      ) : totalCount === 0 && !listLoading ? (
        <DashboardEmptyState
          icon={<CmsSectionTypesIcon size={28} />}
          title={t('dashboard.cms.sectionTypes.emptyTitle')}
          message={t('dashboard.cms.sectionTypes.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.cms.sectionTypes.add')}
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
                id: 'name',
                header: t('dashboard.cms.sectionTypes.fieldName'),
                cell: (item) => (
                  <span className="font-semibold text-text" dir="ltr">
                    {item.name || '—'}
                  </span>
                ),
              },
              {
                id: 'status',
                header: t('dashboard.cms.sectionTypes.fieldActive'),
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
                      ? t('dashboard.cms.sectionTypes.statusActive')
                      : t('dashboard.cms.sectionTypes.statusInactive')}
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
                      label={t('dashboard.cms.sectionTypes.edit')}
                      icon={<EditIcon size={15} />}
                      onClick={() => openEdit(item)}
                    />
                  </AdminGridActions>
                ),
              },
            ]}
            rows={items}
            rowKey={(item) => item.id}
            loading={listLoading}
            loadingLabel={t('dashboard.cms.sectionTypes.loading')}
            pagination={{
              pageNumber,
              pageSize,
              totalCount,
              totalPages,
              onPageChange: goToPage,
            }}
          />
        </div>
      )}
    </div>
  )
}
