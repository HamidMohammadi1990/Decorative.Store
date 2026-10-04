import { useCallback, useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import type { AdminRole } from '@/models/admin/role.model'
import type { AdminPermission } from '@/models/admin/permission.model'
import type { AdminRolePermission } from '@/models/admin/rolePermission.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { RolesIcon, EditIcon, DeleteIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminGridActionButton,
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
import { PermissionTreeList } from '@/components/dashboard/PermissionTreeList'
import { buildPermissionTree, filterPermissionTree } from '@/extensions/buildPermissionTree'
import { adminRoleService } from '@/services/adminRoleService'
import { adminPermissionService } from '@/services/adminPermissionService'
import { adminRolePermissionService } from '@/services/adminRolePermissionService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit' | 'permissions'

export function RolesPanel() {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [saving, setSaving] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [title, setTitle] = useState('')
  const [isActive, setIsActive] = useState(true)

  const [permissionsRole, setPermissionsRole] = useState<AdminRole | null>(null)
  const [permissionsLoading, setPermissionsLoading] = useState(false)
  const [permissionsSaving, setPermissionsSaving] = useState(false)
  const [permissionsError, setPermissionsError] = useState<string | null>(null)
  const [allPermissions, setAllPermissions] = useState<AdminPermission[]>([])
  const [currentRolePermissions, setCurrentRolePermissions] = useState<AdminRolePermission[]>([])
  const [selectedPermissionIds, setSelectedPermissionIds] = useState<Set<string>>(new Set())
  const [permissionSearch, setPermissionSearch] = useState('')

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

  const loadRolePermissions = useCallback(
    async (role: AdminRole) => {
      if (!accessToken || accessToken === 'mock-access-token') return

      setPermissionsLoading(true)
      setPermissionsError(null)
      try {
        const [permissions, rolePermissions] = await Promise.all([
          adminPermissionService.getAllPages(accessToken, locale),
          adminRolePermissionService.getAllPages(accessToken, locale, { roleId: role.id }),
        ])

        setAllPermissions(permissions)
        setCurrentRolePermissions(rolePermissions)
        setSelectedPermissionIds(new Set(rolePermissions.map((item) => item.permissionId)))
      } catch (err) {
        setPermissionsError(
          resolveAdminMutationError(err, t('dashboard.roles.permissionsLoadFailed')),
        )
      } finally {
        setPermissionsLoading(false)
      }
    },
    [accessToken, locale, t],
  )

  const openPermissions = (item: AdminRole) => {
    setPermissionsRole(item)
    setPermissionSearch('')
    setPermissionsError(null)
    setMode('permissions')
    void loadRolePermissions(item)
  }

  const backFromPermissions = () => {
    setPermissionsRole(null)
    setAllPermissions([])
    setCurrentRolePermissions([])
    setSelectedPermissionIds(new Set())
    setPermissionSearch('')
    setPermissionsError(null)
    setMode('list')
  }

  const handlePermissionSelectionChange = (ids: Set<string>) => {
    setSelectedPermissionIds(ids)
  }

  const handleSavePermissions = async () => {
    if (!permissionsRole || !accessToken || accessToken === 'mock-access-token') return

    const currentIds = new Set(currentRolePermissions.map((item) => item.permissionId))
    const toAdd = [...selectedPermissionIds].filter((id) => !currentIds.has(id))
    const toRemove = currentRolePermissions.filter(
      (item) => !selectedPermissionIds.has(item.permissionId),
    )

    if (toAdd.length === 0 && toRemove.length === 0) {
      backFromPermissions()
      return
    }

    setPermissionsSaving(true)
    setPermissionsError(null)
    try {
      await Promise.all([
        ...toAdd.map((permissionId) =>
          adminRolePermissionService.create(accessToken, locale, {
            roleId: permissionsRole.id,
            permissionId,
          }),
        ),
        ...toRemove.map((item) => adminRolePermissionService.delete(accessToken, item.id)),
      ])
      backFromPermissions()
    } catch (err) {
      setPermissionsError(
        resolveAdminMutationError(err, t('dashboard.roles.permissionsSaveFailed')),
      )
    } finally {
      setPermissionsSaving(false)
    }
  }

  const visiblePermissionTree = useMemo(
    () => filterPermissionTree(buildPermissionTree(allPermissions), permissionSearch),
    [allPermissions, permissionSearch],
  )

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
    if (!(await confirm({ message: t('dashboard.roles.deleteConfirm') }))) return

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

  if (mode === 'permissions' && permissionsRole) {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.roles.permissionsTitle')}
          description={t('dashboard.roles.permissionsDescription', {
            role: permissionsRole.title,
          })}
          icon={<RolesIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <div>
            <input
              value={permissionSearch}
              onChange={(e) => setPermissionSearch(e.target.value)}
              className={adminInputClass}
              placeholder={t('dashboard.roles.permissionsSearch')}
              aria-describedby="roles-permissions-search-hint"
            />
            <p id="roles-permissions-search-hint" className="mt-1.5 text-xs text-text-muted">
              {t('dashboard.roles.permissionsSearchHint')}
            </p>
          </div>

          {permissionsLoading ? (
            <div className="flex justify-center py-12">
              <InlineLoading label={t('dashboard.roles.permissionsLoading')} />
            </div>
          ) : visiblePermissionTree.length === 0 ? (
            <p className="text-sm text-text-muted">{t('dashboard.roles.permissionsEmpty')}</p>
          ) : (
            <PermissionTreeList
              permissions={allPermissions}
              searchQuery={permissionSearch}
              selectedIds={selectedPermissionIds}
              disabled={permissionsSaving}
              onSelectedIdsChange={handlePermissionSelectionChange}
            />
          )}

          {permissionsError && <p className="text-sm text-sale">{permissionsError}</p>}

          <div className="flex flex-wrap gap-3 border-t border-border pt-4">
            <Button
              variant="warm"
              onClick={() => void handleSavePermissions()}
              disabled={permissionsSaving || permissionsLoading}
            >
              {permissionsSaving ? (
                <InlineLoading label={t('dashboard.roles.permissionsSaving')} />
              ) : (
                t('dashboard.roles.permissionsSave')
              )}
            </Button>
            <Button
              variant="secondary"
              onClick={backFromPermissions}
              disabled={permissionsSaving}
            >
              {t('dashboard.roles.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
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
                  <AdminGridActions>
                    <AdminGridActionButton
                      label={t('dashboard.roles.permissions')}
                      icon={<RolesIcon size={14} />}
                      onClick={() => openPermissions(item)}
                    />
                    <AdminGridIconButton
                      label={t('dashboard.roles.edit')}
                      icon={<EditIcon size={15} />}
                      onClick={() => openEdit(item)}
                    />
                    <AdminGridIconButton
                      label={t('dashboard.roles.delete')}
                      icon={<DeleteIcon size={15} />}
                      tone="danger"
                      disabled={saving}
                      onClick={() => void handleDelete(item.id)}
                    />
                  </AdminGridActions>
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
