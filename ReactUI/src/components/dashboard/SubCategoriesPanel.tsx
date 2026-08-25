import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminCategory, AdminSubCategory } from '@/models/admin/catalog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { SubCategoriesIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminCategoryService } from '@/services/adminCategoryService'
import { adminSubCategoryService } from '@/services/adminSubCategoryService'
import { slugifyTitle } from '@/services/admin/adminCatalogNormalize'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function SubCategoriesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminSubCategory[]>([])
  const [categories, setCategories] = useState<AdminCategory[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [filterCategoryId, setFilterCategoryId] = useState('')

  const [title, setTitle] = useState('')
  const [slug, setSlug] = useState('')
  const [code, setCode] = useState('')
  const [categoryId, setCategoryId] = useState('')
  const [isActive, setIsActive] = useState(true)
  const [slugTouched, setSlugTouched] = useState(false)

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.subCategories.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const [subResult, categoryResult] = await Promise.all([
        adminSubCategoryService.getAll(accessToken, locale, {
          pageSize: 100,
          languageId: languageId ?? undefined,
          categoryId: filterCategoryId || null,
        }),
        adminCategoryService.getAll(accessToken, locale, {
          pageSize: 100,
          languageId: languageId ?? undefined,
        }),
      ])
      setItems(subResult.items)
      setCategories(categoryResult.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.subCategories.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, filterCategoryId, languageId, locale, t])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  const resetForm = () => {
    setTitle('')
    setSlug('')
    setCode('')
    setCategoryId(categories[0]?.id ?? '')
    setIsActive(true)
    setSlugTouched(false)
    setEditingId(null)
    setError(null)
  }

  const openCreate = () => {
    resetForm()
    setCategoryId(categories[0]?.id ?? '')
    setMode('create')
  }

  const openEdit = (item: AdminSubCategory) => {
    setEditingId(item.id)
    setTitle(item.title)
    setSlug(item.slug)
    setCode(item.code)
    setCategoryId(item.categoryId)
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
      setError(t('dashboard.subCategories.saveFailed'))
      return
    }

    if (!title.trim() || !slug.trim() || !code.trim() || !categoryId) {
      setError(t('dashboard.subCategories.validationRequired'))
      return
    }

    setSaving(true)
    setError(null)
    try {
      if (mode === 'edit' && editingId) {
        await adminSubCategoryService.update(accessToken, locale, {
          id: editingId,
          languageId,
          title: title.trim(),
          slug: slug.trim(),
          code: code.trim(),
          categoryId,
          isActive,
        })
      } else {
        await adminSubCategoryService.create(accessToken, locale, {
          languageId,
          title: title.trim(),
          slug: slug.trim(),
          code: code.trim(),
          categoryId,
        })
      }
      await load()
      backToList()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.subCategories.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.subCategories.deleteConfirm'))) return

    setSaving(true)
    setError(null)
    try {
      await adminSubCategoryService.delete(accessToken, id)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.subCategories.deleteFailed')))
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
              ? t('dashboard.subCategories.editTitle')
              : t('dashboard.subCategories.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.subCategories.editDescription')
              : t('dashboard.subCategories.createDescription')
          }
          icon={<SubCategoriesIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.subCategories.fieldCategory')}>
            <select
              value={categoryId}
              onChange={(e) => setCategoryId(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.subCategories.selectCategory')}</option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.title}
                </option>
              ))}
            </select>
          </AdminField>

          <AdminField label={t('dashboard.subCategories.fieldTitle')}>
            <input
              value={title}
              onChange={(e) => handleTitleChange(e.target.value)}
              className={adminInputClass}
              placeholder={t('dashboard.subCategories.titlePlaceholder')}
            />
          </AdminField>

          <AdminField label={t('dashboard.subCategories.fieldSlug')}>
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

          <AdminField label={t('dashboard.subCategories.fieldCode')}>
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
              {t('dashboard.subCategories.fieldActive')}
            </label>
          )}

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.subCategories.saving')} />
              ) : (
                t('dashboard.subCategories.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.subCategories.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.subCategories.title')}
        description={t('dashboard.subCategories.description')}
        icon={<SubCategoriesIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate} disabled={categories.length === 0}>
            {t('dashboard.subCategories.add')}
          </Button>
        }
      />

      <div className="mb-4">
        <AdminField label={t('dashboard.subCategories.filterCategory')}>
          <select
            value={filterCategoryId}
            onChange={(e) => setFilterCategoryId(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.subCategories.allCategories')}</option>
            {categories.map((category) => (
              <option key={category.id} value={category.id}>
                {category.title}
              </option>
            ))}
          </select>
        </AdminField>
      </div>

      {loading || languageLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.subCategories.loading')} />
        </div>
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          title={t('dashboard.subCategories.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.subCategories.retry')}
            </Button>
          }
        />
      ) : categories.length === 0 ? (
        <DashboardEmptyState
          title={t('dashboard.subCategories.noCategoriesTitle')}
          message={t('dashboard.subCategories.noCategoriesMessage')}
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          title={t('dashboard.subCategories.emptyTitle')}
          message={t('dashboard.subCategories.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.subCategories.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.subCategories.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            {items.map((item) => (
              <li
                key={item.id}
                className="flex flex-wrap items-center justify-between gap-3 px-4 py-3 sm:px-5"
              >
                <div className="min-w-0">
                  <p className="truncate text-sm font-semibold text-text">{item.title}</p>
                  <p className="mt-0.5 text-xs text-text-muted">
                    {item.categoryTitle}
                    <span className="mx-1.5 text-border-strong">·</span>
                    <span dir="ltr">
                      {item.code} · {item.slug || '—'}
                    </span>
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
                      ? t('dashboard.subCategories.statusActive')
                      : t('dashboard.subCategories.statusInactive')}
                  </span>
                  <Button
                    variant="secondary"
                    className="py-1.5 text-xs"
                    onClick={() => openEdit(item)}
                    disabled={saving}
                  >
                    {t('dashboard.subCategories.edit')}
                  </Button>
                  <Button
                    variant="ghost"
                    className="py-1.5 text-xs text-sale hover:bg-sale/10"
                    onClick={() => void handleDelete(item.id)}
                    disabled={saving}
                  >
                    {t('dashboard.subCategories.delete')}
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
