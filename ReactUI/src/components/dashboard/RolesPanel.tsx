import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminRole } from '@/models/admin/role.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { RolesIcon } from '@/components/dashboard/DashboardIcons'
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
import { adminRoleService } from '@/services/adminRoleService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function RolesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [saving, setSaving] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [title, setTitle] = useState('')
  const [isActive, setIsActive] = useState(true)

  const fetchPage = useCallback(
    (pageNumber: number, pageSize: number) =>
      adminRoleService.getAll(accessToken!, locale, { pageNumber, pageSize }),
    [accessToken, locale],
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
  } = useAdminPagedList<AdminRole>({
    fetchPage,
    initialPageSize: 20,
    enabled: Boolean(accessToken && accessToken !== 'mock-access-token' && !languageLoading),
  })

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.roles.loadFailed'))
    : null

  const resetForm = () => {
    setTitle('')
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

  const openEdit = (item: AdminRole) => {
    setEditingId(item.id)
    setTitle(item.title)
    setIsActive(item.isActive)
    setFormError(null)
    setMode('edit')
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setFormError(t('dashboard.roles.authRequired'))
      return
    }
    if (!title.trim()) {
      setFormError(t('dashboard.roles.validationRequired'))
      return
    }

    setSaving(true)
    setFormError(null)
    try {
      if (mode === 'edit' && editingId) {
        await adminRoleService.update(accessToken, locale, {
          id: editingId,
          title: title.trim(),
          isActive,
        })
      } else {
        await adminRoleService.create(accessToken, locale, { title: title.trim() })
      }
      reload()
      backToList()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.roles.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.roles.deleteConfirm'))) return

    setSaving(true)
    setFormError(null)
    try {
      await adminRoleService.delete(accessToken, id)
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.roles.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        <DashboardPageHeader
          title={mode === 'edit' ? t('dashboard.roles.editTitle') : t('dashboard.roles.createTitle')}
          description={
            mode === 'edit'
              ? t('dashboard.roles.editDescription')
              : t('dashboard.roles.createDescription')
          }
          icon={<RolesIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.roles.fieldTitle')}>
            <input
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className={adminInputClass}
              placeholder={t('dashboard.roles.titlePlaceholder')}
            />
          </AdminField>

          {mode === 'edit' && (
            <label className="flex cursor-pointer items-center gap-2 text-sm text-text">
              <input
                type="checkbox"
                checked={isActive}
                onChange={(e) => setIsActive(e.target.checked)}
                className="size-4 rounded border-border text-warm focus:ring-warm"
              />
              {t('dashboard.roles.fieldActive')}
            </label>
          )}

          {formError && <p className="text-sm text-sale">{formError}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.roles.saving')} />
              ) : (
                t('dashboard.roles.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.roles.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  if (!accessToken || accessToken === 'mock-access-token') {
    return (
      <DashboardEmptyState
        icon={<RolesIcon size={28} />}
        title={t('dashboard.roles.emptyTitle')}
        message={t('dashboard.roles.authRequired')}
      />
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.roles.title')}
        description={t('dashboard.roles.description')}
        icon={<RolesIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate}>
            {t('dashboard.roles.add')}
          </Button>
        }
      />

      {languageLoading || (listLoading && items.length === 0) ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.roles.loading')} />
        </div>
      ) : listErrorMessage && items.length === 0 ? (
        <DashboardEmptyState
          icon={<RolesIcon size={28} />}
          title={t('dashboard.roles.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.roles.retry')}
            </Button>
          }
        />
      ) : totalCount === 0 && !listLoading ? (
        <DashboardEmptyState
          icon={<RolesIcon size={28} />}
          title={t('dashboard.roles.emptyTitle')}
          message={t('dashboard.roles.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.roles.add')}
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
                header: t('dashboard.roles.colTitle'),
                cell: (item) => <span className="font-semibold text-text">{item.title}</span>,
              },
              {
                id: 'status',
                header: t('dashboard.roles.colStatus'),
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
                      ? t('dashboard.roles.statusActive')
                      : t('dashboard.roles.statusInactive')}
                  </span>
                ),
              },
              {
                id: 'actions',
                header: t('dashboard.roles.colActions'),
                align: 'right',
                cell: (item) => (
                  <div className="flex justify-end gap-2">
                    <Button
                      variant="secondary"
                      className="py-1.5 text-xs"
                      onClick={() => openEdit(item)}
                    >
                      {t('dashboard.roles.edit')}
                    </Button>
                    <Button
                      variant="ghost"
                      className="py-1.5 text-xs text-red-600 hover:text-red-700"
                      disabled={saving}
                      onClick={() => void handleDelete(item.id)}
                    >
                      {t('dashboard.roles.delete')}
                    </Button>
                  </div>
                ),
              },
            ]}
            rows={items}
            rowKey={(item) => item.id}
            loading={listLoading}
            loadingLabel={t('dashboard.roles.loading')}
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
