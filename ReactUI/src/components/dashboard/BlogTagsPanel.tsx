import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminTag } from '@/models/admin/blog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { BlogTagsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminTagService } from '@/services/adminTagService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function BlogTagsPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminTag[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)

  const [title, setTitle] = useState('')
  const [isActive, setIsActive] = useState(true)

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.blogTags.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const result = await adminTagService.getAll(accessToken, locale, { pageSize: 200 })
      setItems(result.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogTags.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, locale, t])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  const resetForm = () => {
    setTitle('')
    setIsActive(true)
    setEditingId(null)
    setError(null)
  }

  const openCreate = () => {
    resetForm()
    setMode('create')
  }

  const openEdit = (item: AdminTag) => {
    setEditingId(item.id)
    setTitle(item.title)
    setIsActive(item.isActive)
    setError(null)
    setMode('edit')
  }

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.blogTags.saveFailed'))
      return
    }

    if (!title.trim()) {
      setError(t('dashboard.blogTags.validationRequired'))
      return
    }

    setSaving(true)
    setError(null)
    try {
      if (mode === 'edit' && editingId) {
        await adminTagService.update(accessToken, locale, {
          id: editingId,
          title: title.trim(),
          isActive,
        })
      } else {
        await adminTagService.create(accessToken, locale, { title: title.trim() })
      }
      await load()
      backToList()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogTags.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.blogTags.deleteConfirm'))) return

    setSaving(true)
    setError(null)
    try {
      await adminTagService.delete(accessToken, id)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogTags.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        <DashboardPageHeader
          title={
            mode === 'edit' ? t('dashboard.blogTags.editTitle') : t('dashboard.blogTags.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.blogTags.editDescription')
              : t('dashboard.blogTags.createDescription')
          }
          icon={<BlogTagsIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.blogTags.fieldTitle')}>
            <input
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className={adminInputClass}
              placeholder={t('dashboard.blogTags.titlePlaceholder')}
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
              {t('dashboard.blogTags.fieldActive')}
            </label>
          )}

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.blogTags.saving')} />
              ) : (
                t('dashboard.blogTags.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.blogTags.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.blogTags.title')}
        description={t('dashboard.blogTags.description')}
        icon={<BlogTagsIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate}>
            {t('dashboard.blogTags.add')}
          </Button>
        }
      />

      {loading || languageLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.blogTags.loading')} />
        </div>
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogTagsIcon size={28} />}
          title={t('dashboard.blogTags.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.blogTags.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogTagsIcon size={28} />}
          title={t('dashboard.blogTags.emptyTitle')}
          message={t('dashboard.blogTags.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.blogTags.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.blogTags.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            {items.map((item) => (
              <li
                key={item.id}
                className="flex flex-wrap items-center justify-between gap-3 px-4 py-3 sm:px-5"
              >
                <p className="truncate text-sm font-semibold text-text">{item.title}</p>
                <div className="flex flex-wrap items-center gap-2">
                  <span
                    className={`rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                      item.isActive
                        ? 'bg-warm-soft text-warm'
                        : 'bg-surface-muted text-text-muted'
                    }`}
                  >
                    {item.isActive
                      ? t('dashboard.blogTags.statusActive')
                      : t('dashboard.blogTags.statusInactive')}
                  </span>
                  <Button
                    variant="secondary"
                    className="py-1.5 text-xs"
                    onClick={() => openEdit(item)}
                    disabled={saving}
                  >
                    {t('dashboard.blogTags.edit')}
                  </Button>
                  <Button
                    variant="ghost"
                    className="py-1.5 text-xs text-sale hover:bg-sale/10"
                    onClick={() => void handleDelete(item.id)}
                    disabled={saving}
                  >
                    {t('dashboard.blogTags.delete')}
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
