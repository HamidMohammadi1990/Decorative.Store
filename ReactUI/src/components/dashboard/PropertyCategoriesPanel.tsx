import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminPropertyCategory } from '@/models/admin/property.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { PropertyCategoriesIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminPropertyCategoryService } from '@/services/adminPropertyCategoryService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function PropertyCategoriesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminPropertyCategory[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)

  const [title, setTitle] = useState('')
  const [code, setCode] = useState('')
  const [isActive, setIsActive] = useState(true)

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.propertyCategories.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const result = await adminPropertyCategoryService.getAll(accessToken, locale, {
        pageSize: 100,
        languageId: languageId ?? undefined,
      })
      setItems(result.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.propertyCategories.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, languageId, locale, t])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  const resetForm = () => {
    setTitle('')
    setCode('')
    setIsActive(true)
    setEditingId(null)
    setError(null)
  }

  const openCreate = () => {
    resetForm()
    setMode('create')
  }

  const openEdit = (item: AdminPropertyCategory) => {
    setEditingId(item.id)
    setTitle(item.title)
    setCode(item.code)
    setIsActive(item.isActive)
    setError(null)
    setMode('edit')
  }

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || languageId == null) {
      setError(t('dashboard.propertyCategories.saveFailed'))
      return
    }

    if (!title.trim() || !code.trim()) {
      setError(t('dashboard.propertyCategories.validationRequired'))
      return
    }

    setSaving(true)
    setError(null)
    try {
      if (mode === 'edit' && editingId) {
        await adminPropertyCategoryService.update(accessToken, locale, {
          id: editingId,
          languageId,
          title: title.trim(),
          code: code.trim(),
          isActive,
        })
      } else {
        await adminPropertyCategoryService.create(accessToken, locale, {
          languageId,
          title: title.trim(),
          code: code.trim(),
        })
      }
      await load()
      backToList()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.propertyCategories.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.propertyCategories.deleteConfirm'))) return

    setSaving(true)
    setError(null)
    try {
      await adminPropertyCategoryService.delete(accessToken, id)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.propertyCategories.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        <DashboardPageHeader
          title={
            mode === 'edit'
              ? t('dashboard.propertyCategories.editTitle')
              : t('dashboard.propertyCategories.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.propertyCategories.editDescription')
              : t('dashboard.propertyCategories.createDescription')
          }
          icon={<PropertyCategoriesIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.propertyCategories.fieldTitle')}>
            <input
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className={adminInputClass}
              placeholder={t('dashboard.propertyCategories.titlePlaceholder')}
            />
          </AdminField>

          <AdminField label={t('dashboard.propertyCategories.fieldCode')}>
            <input
              value={code}
              onChange={(e) => setCode(e.target.value)}
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
              {t('dashboard.propertyCategories.fieldActive')}
            </label>
          )}

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.propertyCategories.saving')} />
              ) : (
                t('dashboard.propertyCategories.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.propertyCategories.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.propertyCategories.title')}
        description={t('dashboard.propertyCategories.description')}
        icon={<PropertyCategoriesIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate}>
            {t('dashboard.propertyCategories.add')}
          </Button>
        }
      />

      {loading || languageLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.propertyCategories.loading')} />
        </div>
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<PropertyCategoriesIcon size={28} />}
          title={t('dashboard.propertyCategories.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.propertyCategories.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<PropertyCategoriesIcon size={28} />}
          title={t('dashboard.propertyCategories.emptyTitle')}
          message={t('dashboard.propertyCategories.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.propertyCategories.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.propertyCategories.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            {items.map((item) => (
              <li
                key={item.id}
                className="flex flex-wrap items-center justify-between gap-3 px-4 py-3 sm:px-5"
              >
                <div className="min-w-0">
                  <p className="truncate text-sm font-semibold text-text">{item.title}</p>
                  <p className="mt-0.5 text-xs text-text-muted" dir="ltr">{item.code}</p>
                </div>
                <div className="flex flex-wrap items-center gap-2">
                  <span
                    className={`rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                      item.isActive
                        ? 'bg-warm-soft text-warm'
                        : 'bg-surface-muted text-text-muted'
                    }`}
                  >
                    {item.isActive
                      ? t('dashboard.propertyCategories.statusActive')
                      : t('dashboard.propertyCategories.statusInactive')}
                  </span>
                  <Button
                    variant="secondary"
                    className="py-1.5 text-xs"
                    onClick={() => openEdit(item)}
                    disabled={saving}
                  >
                    {t('dashboard.propertyCategories.edit')}
                  </Button>
                  <Button
                    variant="ghost"
                    className="py-1.5 text-xs text-sale hover:bg-sale/10"
                    onClick={() => void handleDelete(item.id)}
                    disabled={saving}
                  >
                    {t('dashboard.propertyCategories.delete')}
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
