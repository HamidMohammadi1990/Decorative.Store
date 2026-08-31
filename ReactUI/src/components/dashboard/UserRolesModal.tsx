import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminRole } from '@/models/admin/role.model'
import type { AdminUser } from '@/models/admin/user.model'
import type { AdminUserRole } from '@/models/admin/userRole.model'
import { AdminLargeModal } from '@/components/dashboard/admin/AdminLargeModal'
import { resolveAdminMutationError } from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminRoleService } from '@/services/adminRoleService'
import { adminUserRoleService } from '@/services/adminUserRoleService'
import { useUserStore } from '@/stores/userStore'

export function UserRolesModal({
  user,
  open,
  onClose,
  onSaved,
}: {
  user: AdminUser | null
  open: boolean
  onClose: () => void
  onSaved?: () => void
}) {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale } = useCurrentLanguageId()

  const [loading, setLoading] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [allRoles, setAllRoles] = useState<AdminRole[]>([])
  const [currentAssignments, setCurrentAssignments] = useState<AdminUserRole[]>([])
  const [selectedRoleIds, setSelectedRoleIds] = useState<Set<string>>(new Set())

  const loadData = useCallback(async () => {
    if (!user || !accessToken || accessToken === 'mock-access-token') return

    setLoading(true)
    setError(null)
    try {
      const [rolesResult, userRolesResult] = await Promise.all([
        adminRoleService.getAll(accessToken, locale, { pageNumber: 1, pageSize: 100, isActive: true }),
        adminUserRoleService.getAll(accessToken, locale, {
          pageNumber: 1,
          pageSize: 100,
          userId: user.id,
        }),
      ])

      setAllRoles(rolesResult.items)
      setCurrentAssignments(userRolesResult.items)
      setSelectedRoleIds(new Set(userRolesResult.items.map((item) => item.roleId)))
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.users.rolesLoadFailed')))
    } finally {
      setLoading(false)
    }
  }, [accessToken, locale, t, user])

  useEffect(() => {
    if (open && user) {
      void loadData()
    } else if (!open) {
      setAllRoles([])
      setCurrentAssignments([])
      setSelectedRoleIds(new Set())
      setError(null)
    }
  }, [open, user, loadData])

  const toggleRole = (roleId: string) => {
    setSelectedRoleIds((prev) => {
      const next = new Set(prev)
      if (next.has(roleId)) {
        next.delete(roleId)
      } else {
        next.add(roleId)
      }
      return next
    })
  }

  const handleSave = async () => {
    if (!user || !accessToken || accessToken === 'mock-access-token') return

    const currentRoleIds = new Set(currentAssignments.map((item) => item.roleId))
    const toAdd = [...selectedRoleIds].filter((id) => !currentRoleIds.has(id))
    const toRemove = currentAssignments.filter((item) => !selectedRoleIds.has(item.roleId))

    if (toAdd.length === 0 && toRemove.length === 0) {
      onClose()
      return
    }

    setSaving(true)
    setError(null)
    try {
      await Promise.all([
        ...toAdd.map((roleId) =>
          adminUserRoleService.create(accessToken, locale, { userId: user.id, roleId }),
        ),
        ...toRemove.map((item) => adminUserRoleService.delete(accessToken, item.id)),
      ])
      onSaved?.()
      onClose()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.users.rolesSaveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const displayName = user
    ? [user.firstName, user.lastName].filter(Boolean).join(' ') || user.userName
    : ''

  return (
    <AdminLargeModal
      open={open}
      title={t('dashboard.users.rolesModalTitle')}
      description={t('dashboard.users.rolesModalDescription', { name: displayName })}
      onClose={onClose}
    >
      {loading ? (
        <div className="flex justify-center py-12">
          <InlineLoading label={t('dashboard.users.rolesLoading')} />
        </div>
      ) : (
        <div className="space-y-5">
          {allRoles.length === 0 ? (
            <p className="text-sm text-text-muted">{t('dashboard.users.rolesEmpty')}</p>
          ) : (
            <div className="grid gap-2 sm:grid-cols-2">
              {allRoles.map((role) => (
                <label
                  key={role.id}
                  className="flex cursor-pointer items-center gap-3 rounded-sm border border-border bg-surface-muted/20 px-3 py-2.5 transition-colors hover:bg-surface-muted/40"
                >
                  <input
                    type="checkbox"
                    checked={selectedRoleIds.has(role.id)}
                    onChange={() => toggleRole(role.id)}
                    disabled={saving}
                    className="size-4 shrink-0 rounded border-border text-warm focus:ring-warm"
                  />
                  <span className="text-sm font-medium text-text">{role.title}</span>
                </label>
              ))}
            </div>
          )}

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3 border-t border-border pt-4">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving || loading}>
              {saving ? (
                <InlineLoading label={t('dashboard.users.rolesSaving')} />
              ) : (
                t('dashboard.users.rolesSave')
              )}
            </Button>
            <Button variant="secondary" onClick={onClose} disabled={saving}>
              {t('dashboard.users.cancel')}
            </Button>
          </div>
        </div>
      )}
    </AdminLargeModal>
  )
}
