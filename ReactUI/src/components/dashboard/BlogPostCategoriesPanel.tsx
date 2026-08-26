import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminBlogPostCategory } from '@/models/admin/blog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { BlogCategoriesIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminBlogPostCategoryService } from '@/services/adminBlogPostCategoryService'
import { slugifyTitle } from '@/services/admin/adminCatalogNormalize'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function BlogPostCategoriesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminBlogPostCategory[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)

  const [title, setTitle] = useState('')
  const [slug, setSlug] = useState('')
  const [code, setCode] = useState('')
  const [isActive, setIsActive] = useState(true)
  const [slugTouched, setSlugTouched] = useState(false)

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.blogCategories.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const result = await adminBlogPostCategoryService.getAll(accessToken, locale, {
        pageSize: 100,
        languageId: languageId ?? undefined,
      })
      setItems(result.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogCategories.loadFailed')))
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
    setSlug('')
    setCode('')
    setIsActive(true)
    setSlugTouched(false)
    setEditingId(null)
    setError(null)
  }

  const openCreate = () => {
    resetForm()
    setMode('create')
  }

  const openEdit = (item: AdminBlogPostCategory) => {
    setEditingId(item.id)
    setTitle(item.title)
    setSlug(item.slug)
    setCode(item.code)
    setIsActive(item.isActive)
    setSlugTouched(true)
    setError(null)
    setMode('edit')
  }

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const handleTitleChange = (value: string) => {
    setTitle(value)
    if (!slugTouched) setSlug(slugifyTitle(value))
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || languageId == null) {
      setError(t('dashboard.blogCategories.saveFailed'))
      return
    }

    if (!title.trim() || !slug.trim() || !code.trim()) {
      setError(t('dashboard.blogCategories.validationRequired'))
      return
    }

    setSaving(true)
    setError(null)
    try {
      if (mode === 'edit' && editingId) {
        await adminBlogPostCategoryService.update(accessToken, locale, {
          id: editingId,
          languageId,
          title: title.trim(),
          slug: slug.trim(),
          code: code.trim(),
          isActive,
        })
      } else {
        await adminBlogPostCategoryService.create(accessToken, locale, {
          languageId,
          title: title.trim(),
          slug: slug.trim(),
          code: code.trim(),
        })
      }
      await load()
      backToList()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogCategories.saveFailed')))
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
              ? t('dashboard.blogCategories.editTitle')
              : t('dashboard.blogCategories.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.blogCategories.editDescription')
              : t('dashboard.blogCategories.createDescription')
          }
          icon={<BlogCategoriesIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.blogCategories.fieldTitle')}>
            <input
              value={title}
              onChange={(e) => handleTitleChange(e.target.value)}
              className={adminInputClass}
              placeholder={t('dashboard.blogCategories.titlePlaceholder')}
            />
          </AdminField>

          <AdminField label={t('dashboard.blogCategories.fieldSlug')}>
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

          <AdminField label={t('dashboard.blogCategories.fieldCode')}>
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
              {t('dashboard.blogCategories.fieldActive')}
            </label>
          )}

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.blogCategories.saving')} />
              ) : (
                t('dashboard.blogCategories.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.blogCategories.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.blogCategories.title')}
        description={t('dashboard.blogCategories.description')}
        icon={<BlogCategoriesIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate}>
            {t('dashboard.blogCategories.add')}
          </Button>
        }
      />

      {loading || languageLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.blogCategories.loading')} />
        </div>
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogCategoriesIcon size={28} />}
          title={t('dashboard.blogCategories.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.blogCategories.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogCategoriesIcon size={28} />}
          title={t('dashboard.blogCategories.emptyTitle')}
          message={t('dashboard.blogCategories.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.blogCategories.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.blogCategories.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            {items.map((item) => (
              <li
                key={item.id}
                className="flex flex-wrap items-center justify-between gap-3 px-4 py-3 sm:px-5"
              >
                <div className="min-w-0">
                  <p className="truncate text-sm font-semibold text-text">{item.title}</p>
                  <p className="mt-0.5 text-xs text-text-muted" dir="ltr">
                    {item.slug}
                    <span className="mx-1.5 text-border-strong">·</span>
                    {item.code}
                  </p>
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
                      ? t('dashboard.blogCategories.statusActive')
                      : t('dashboard.blogCategories.statusInactive')}
                  </span>
                  <Button
                    variant="secondary"
                    className="py-1.5 text-xs"
                    onClick={() => openEdit(item)}
                    disabled={saving}
                  >
                    {t('dashboard.blogCategories.edit')}
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
