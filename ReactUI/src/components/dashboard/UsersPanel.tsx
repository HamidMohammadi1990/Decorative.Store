import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminUser, AdminUserGender } from '@/models/admin/user.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { UsersIcon } from '@/components/dashboard/DashboardIcons'
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
import { adminUserService } from '@/services/adminUserService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

const GENDER_OPTIONS: AdminUserGender[] = [1, 2]

export function UsersPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const currentUserId = useUserStore((s) => s.user?.id)
  const { locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [saving, setSaving] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)

  const [userName, setUserName] = useState('')
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [email, setEmail] = useState('')
  const [phoneNumber, setPhoneNumber] = useState('')
  const [password, setPassword] = useState('')
  const [gender, setGender] = useState<AdminUserGender>(2)
  const [isActive, setIsActive] = useState(true)
  const [loginPermission, setLoginPermission] = useState(true)

  const fetchPage = useCallback(
    (pageNumber: number, pageSize: number) =>
      adminUserService.getAll(accessToken!, locale, { pageNumber, pageSize }),
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
  } = useAdminPagedList<AdminUser>({
    fetchPage,
    initialPageSize: 20,
    enabled: Boolean(accessToken && accessToken !== 'mock-access-token' && !languageLoading),
  })

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.users.loadFailed'))
    : null

  const resetForm = () => {
    setUserName('')
    setFirstName('')
    setLastName('')
    setEmail('')
    setPhoneNumber('')
    setPassword('')
    setGender(2)
    setIsActive(true)
    setLoginPermission(true)
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

  const openEdit = (item: AdminUser) => {
    setEditingId(item.id)
    setUserName(item.userName)
    setFirstName(item.firstName)
    setLastName(item.lastName)
    setEmail(item.email)
    setPhoneNumber(item.phoneNumber)
    setPassword('')
    setGender(item.gender)
    setIsActive(item.isActive)
    setLoginPermission(item.loginPermission)
    setFormError(null)
    setMode('edit')
  }

  const genderLabel = (value: AdminUserGender) =>
    value === 2 ? t('dashboard.users.genderMale') : t('dashboard.users.genderFemale')

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setFormError(t('dashboard.users.authRequired'))
      return
    }

    if (
      !userName.trim() ||
      !firstName.trim() ||
      !lastName.trim() ||
      !email.trim() ||
      !phoneNumber.trim()
    ) {
      setFormError(t('dashboard.users.validationRequired'))
      return
    }

    if (mode === 'create' && !password.trim()) {
      setFormError(t('dashboard.users.passwordRequired'))
      return
    }

    setSaving(true)
    setFormError(null)
    try {
      if (mode === 'edit' && editingId) {
        await adminUserService.update(accessToken, locale, {
          id: editingId,
          userName: userName.trim(),
          firstName: firstName.trim(),
          lastName: lastName.trim(),
          email: email.trim(),
          phoneNumber: phoneNumber.trim(),
          password: password.trim() || undefined,
          gender,
          isActive,
          loginPermission,
        })
      } else {
        await adminUserService.create(accessToken, locale, {
          userName: userName.trim(),
          firstName: firstName.trim(),
          lastName: lastName.trim(),
          email: email.trim(),
          phoneNumber: phoneNumber.trim(),
          password: password.trim(),
          gender,
        })
      }
      reload()
      backToList()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.users.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (id === currentUserId) {
      setFormError(t('dashboard.users.deleteSelfForbidden'))
      return
    }
    if (!window.confirm(t('dashboard.users.deleteConfirm'))) return

    setSaving(true)
    setFormError(null)
    try {
      await adminUserService.delete(accessToken, id)
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.users.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        <DashboardPageHeader
          title={
            mode === 'edit' ? t('dashboard.users.editTitle') : t('dashboard.users.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.users.editDescription')
              : t('dashboard.users.createDescription')
          }
          icon={<UsersIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <div className="grid gap-5 sm:grid-cols-2">
            <AdminField label={t('dashboard.users.fieldUserName')}>
              <input
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
                className={adminInputClass}
                dir="ltr"
                placeholder="09123456789"
              />
            </AdminField>

            <AdminField label={t('dashboard.users.fieldPhoneNumber')}>
              <input
                value={phoneNumber}
                onChange={(e) => setPhoneNumber(e.target.value)}
                className={adminInputClass}
                dir="ltr"
                placeholder="09123456789"
              />
            </AdminField>

            <AdminField label={t('dashboard.users.fieldFirstName')}>
              <input
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                className={adminInputClass}
              />
            </AdminField>

            <AdminField label={t('dashboard.users.fieldLastName')}>
              <input
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                className={adminInputClass}
              />
            </AdminField>

            <AdminField label={t('dashboard.users.fieldEmail')}>
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className={adminInputClass}
                dir="ltr"
              />
            </AdminField>

            <AdminField label={t('dashboard.users.fieldGender')}>
              <select
                value={gender}
                onChange={(e) => setGender(Number(e.target.value) as AdminUserGender)}
                className={adminInputClass}
              >
                {GENDER_OPTIONS.map((option) => (
                  <option key={option} value={option}>
                    {genderLabel(option)}
                  </option>
                ))}
              </select>
            </AdminField>

            <AdminField
              label={
                mode === 'edit'
                  ? t('dashboard.users.fieldPasswordOptional')
                  : t('dashboard.users.fieldPassword')
              }
            >
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className={adminInputClass}
                dir="ltr"
                autoComplete="new-password"
              />
            </AdminField>
          </div>

          {mode === 'edit' && (
            <div className="flex flex-wrap gap-6">
              <label className="flex cursor-pointer items-center gap-2 text-sm text-text">
                <input
                  type="checkbox"
                  checked={isActive}
                  onChange={(e) => setIsActive(e.target.checked)}
                  className="size-4 rounded border-border"
                />
                {t('dashboard.users.fieldActive')}
              </label>
              <label className="flex cursor-pointer items-center gap-2 text-sm text-text">
                <input
                  type="checkbox"
                  checked={loginPermission}
                  onChange={(e) => setLoginPermission(e.target.checked)}
                  className="size-4 rounded border-border"
                />
                {t('dashboard.users.fieldLoginPermission')}
              </label>
            </div>
          )}

          {formError && (
            <p className="rounded-sm border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
              {formError}
            </p>
          )}

          <div className="flex flex-wrap gap-3">
            <Button onClick={handleSave} disabled={saving}>
              {saving ? t('dashboard.users.saving') : t('dashboard.users.save')}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.users.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.users.title')}
        description={t('dashboard.users.description')}
        icon={<UsersIcon size={22} />}
        action={
          <Button onClick={openCreate} size="sm">
            {t('dashboard.users.add')}
          </Button>
        }
      />

      {formError && (
        <p className="mb-4 rounded-sm border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
          {formError}
        </p>
      )}

      {listErrorMessage ? (
        <DashboardEmptyState
          title={t('dashboard.users.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" size="sm" onClick={() => reload()}>
              {t('dashboard.users.retry')}
            </Button>
          }
        />
      ) : (
        <AdminDataGrid
          columns={[
            {
              id: 'name',
              header: t('dashboard.users.colName'),
              cell: (row) => (
                <div>
                  <p className="font-medium text-text">
                    {[row.firstName, row.lastName].filter(Boolean).join(' ') || '—'}
                  </p>
                  <p className="text-xs text-text-muted" dir="ltr">
                    {row.userName}
                  </p>
                </div>
              ),
            },
            {
              id: 'contact',
              header: t('dashboard.users.colContact'),
              cell: (row) => (
                <div className="text-xs text-text-muted">
                  <p dir="ltr">{row.phoneNumber || '—'}</p>
                  <p dir="ltr">{row.email || '—'}</p>
                </div>
              ),
            },
            {
              id: 'gender',
              header: t('dashboard.users.colGender'),
              cell: (row) => genderLabel(row.gender),
            },
            {
              id: 'status',
              header: t('dashboard.users.colStatus'),
              cell: (row) => (
                <span
                  className={
                    row.isActive
                      ? 'text-emerald-700'
                      : 'text-text-muted line-through decoration-text-muted/40'
                  }
                >
                  {row.isActive
                    ? t('dashboard.users.statusActive')
                    : t('dashboard.users.statusInactive')}
                </span>
              ),
            },
            {
              id: 'actions',
              header: t('dashboard.users.colActions'),
              align: 'right',
              cell: (row) => (
                <div className="flex justify-end gap-2">
                  <Button variant="ghost" size="sm" onClick={() => openEdit(row)}>
                    {t('dashboard.users.edit')}
                  </Button>
                  <Button
                    variant="ghost"
                    size="sm"
                    className="text-red-600 hover:text-red-700"
                    disabled={row.id === currentUserId || saving}
                    onClick={() => handleDelete(row.id)}
                  >
                    {t('dashboard.users.delete')}
                  </Button>
                </div>
              ),
            },
          ]}
          rows={items}
          rowKey={(row) => row.id}
          loading={listLoading}
          loadingLabel={t('dashboard.users.loading')}
          pagination={{
            pageNumber,
            pageSize,
            totalCount,
            totalPages,
            onPageChange: goToPage,
          }}
          emptyState={
            <DashboardEmptyState
              icon={<UsersIcon size={28} />}
              title={t('dashboard.users.emptyTitle')}
              message={t('dashboard.users.emptyMessage')}
              action={
                <Button size="sm" onClick={openCreate}>
                  {t('dashboard.users.add')}
                </Button>
              }
            />
          }
        />
      )}
    </div>
  )
}
