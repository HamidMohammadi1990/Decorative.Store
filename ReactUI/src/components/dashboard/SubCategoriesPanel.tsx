import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminCategory, AdminSubCategory } from '@/models/admin/catalog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { SubCategoriesIcon } from '@/components/dashboard/DashboardIcons'
import { AdminDataGrid } from '@/components/dashboard/admin/AdminDataGrid'
import { AdminContentLanguageField } from '@/components/dashboard/admin/AdminContentLanguageField'
import { TranslationLocaleBadges } from '@/components/dashboard/admin/TranslationLocaleBadges'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useAdminContentLanguage } from '@/hooks/useAdminContentLanguage'
import { useAdminPagedList } from '@/hooks/useAdminPagedList'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { useStoreLanguages } from '@/hooks/useStoreLanguages'
import { adminCategoryService } from '@/services/adminCategoryService'
import { adminSubCategoryService } from '@/services/adminSubCategoryService'
import { slugifyTitle, pickTranslation } from '@/services/admin/adminCatalogNormalize'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function SubCategoriesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()
  const { languages } = useStoreLanguages()
  const {
    contentLanguageId,
    setContentLanguageId,
    languages: formLanguages,
    loading: contentLanguageLoading,
  } = useAdminContentLanguage()

  const [mode, setMode] = useState<Mode>('list')
  const [categories, setCategories] = useState<AdminCategory[]>([])
  const [categoriesLoading, setCategoriesLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [editingItem, setEditingItem] = useState<AdminSubCategory | null>(null)
  const [filterCategoryId, setFilterCategoryId] = useState('')

  const [title, setTitle] = useState('')
  const [slug, setSlug] = useState('')
  const [code, setCode] = useState('')
  const [categoryId, setCategoryId] = useState('')
  const [isActive, setIsActive] = useState(true)
  const [slugTouched, setSlugTouched] = useState(false)

  const canLoad =
    !languageLoading &&
    !contentLanguageLoading &&
    contentLanguageId != null &&
    Boolean(accessToken) &&
    accessToken !== 'mock-access-token'

  const fetchPage = useCallback(
    async (pageNumber: number, pageSize: number) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        throw new Error(t('dashboard.subCategories.authRequired'))
      }
      return adminSubCategoryService.getAll(accessToken, locale, {
        pageNumber,
        pageSize,
        languageId: contentLanguageId ?? undefined,
        categoryId: filterCategoryId || null,
      })
    },
    [accessToken, contentLanguageId, filterCategoryId, locale, t],
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
  } = useAdminPagedList<AdminSubCategory>({
    fetchPage,
    initialPageSize: 20,
    enabled: canLoad && mode === 'list',
  })

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.subCategories.loadFailed'))
    : null

  const loadCategories = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setCategories([])
      setCategoriesLoading(false)
      return
    }

    setCategoriesLoading(true)
    try {
      const categoryResult = await adminCategoryService.getAll(accessToken, locale, {
        pageSize: 100,
        languageId: contentLanguageId ?? undefined,
      })
      setCategories(categoryResult.items)
    } catch {
      setCategories([])
    } finally {
      setCategoriesLoading(false)
    }
  }, [accessToken, contentLanguageId, locale])

  useEffect(() => {
    if (!languageLoading && !contentLanguageLoading && contentLanguageId != null) {
      void loadCategories()
    }
  }, [languageLoading, contentLanguageLoading, contentLanguageId, loadCategories])

  const resetForm = () => {
    setTitle('')
    setSlug('')
    setCode('')
    setCategoryId(categories[0]?.id ?? '')
    setIsActive(true)
    setSlugTouched(false)
    setEditingId(null)
    setEditingItem(null)
    setFormError(null)
  }

  const applySubCategoryTranslation = (item: AdminSubCategory, targetLanguageId: number) => {
    const translation = pickTranslation(item.translations, targetLanguageId)
    setTitle(translation?.title ?? '')
    setSlug(translation?.slug ?? '')
    setSlugTouched(Boolean(translation?.slug))
  }

  const openCreate = () => {
    resetForm()
    setCategoryId(categories[0]?.id ?? '')
    setMode('create')
  }

  const openEdit = (item: AdminSubCategory) => {
    setEditingId(item.id)
    setEditingItem(item)
    setCode(item.code)
    setCategoryId(item.categoryId)
    setIsActive(item.isActive)
    setFormError(null)
    setMode('edit')
    if (contentLanguageId != null) {
      applySubCategoryTranslation(item, contentLanguageId)
    }
  }

  useEffect(() => {
    if (mode !== 'edit' || !editingItem || contentLanguageId == null) return
    applySubCategoryTranslation(editingItem, contentLanguageId)
  }, [contentLanguageId, editingItem, mode])

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const handleFilterCategory = (value: string) => {
    setFilterCategoryId(value)
    goToPage(1)
  }

  const handleTitleChange = (value: string) => {
    setTitle(value)
    if (!slugTouched) setSlug(slugifyTitle(value))
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
      setFormError(t('dashboard.subCategories.saveFailed'))
      return
    }

    if (!title.trim() || !slug.trim() || !code.trim() || !categoryId) {
      setFormError(t('dashboard.subCategories.validationRequired'))
      return
    }

    setSaving(true)
    setFormError(null)
    try {
      if (mode === 'edit' && editingId) {
        await adminSubCategoryService.update(accessToken, locale, {
          id: editingId,
          languageId: contentLanguageId,
          title: title.trim(),
          slug: slug.trim(),
          code: code.trim(),
          categoryId,
          isActive,
        })
      } else {
        await adminSubCategoryService.create(accessToken, locale, {
          languageId: contentLanguageId,
          title: title.trim(),
          slug: slug.trim(),
          code: code.trim(),
          categoryId,
        })
      }
      backToList()
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.subCategories.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.subCategories.deleteConfirm'))) return

    setSaving(true)
    setFormError(null)
    try {
      await adminSubCategoryService.delete(accessToken, id)
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.subCategories.deleteFailed')))
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
          <AdminContentLanguageField
            value={contentLanguageId}
            onChange={setContentLanguageId}
            languages={formLanguages}
            disabled={contentLanguageLoading}
          />
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

          {formError && <p className="text-sm text-sale">{formError}</p>}

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

  if (!accessToken || accessToken === 'mock-access-token') {
    return (
      <DashboardEmptyState
        icon={<SubCategoriesIcon size={28} />}
        title={t('dashboard.subCategories.loadFailedTitle')}
        message={t('dashboard.subCategories.authRequired')}
      />
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

      <AdminContentLanguageField
        className="mb-4"
        value={contentLanguageId}
        onChange={(id) => {
          setContentLanguageId(id)
          goToPage(1)
        }}
        languages={languages}
        disabled={contentLanguageLoading}
      />

      <div className="mb-4">
        <AdminField label={t('dashboard.subCategories.filterCategory')}>
          <select
            value={filterCategoryId}
            onChange={(e) => handleFilterCategory(e.target.value)}
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

      {languageLoading ||
      contentLanguageLoading ||
      categoriesLoading ||
      (listLoading && items.length === 0) ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.subCategories.loading')} />
        </div>
      ) : listErrorMessage && items.length === 0 ? (
        <DashboardEmptyState
          icon={<SubCategoriesIcon size={28} />}
          title={t('dashboard.subCategories.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.subCategories.retry')}
            </Button>
          }
        />
      ) : categories.length === 0 ? (
        <DashboardEmptyState
          icon={<SubCategoriesIcon size={28} />}
          title={t('dashboard.subCategories.noCategoriesTitle')}
          message={t('dashboard.subCategories.noCategoriesMessage')}
        />
      ) : totalCount === 0 && !listLoading ? (
        <DashboardEmptyState
          icon={<SubCategoriesIcon size={28} />}
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
          {formError && <p className="text-sm text-sale">{formError}</p>}
          {listErrorMessage && <p className="text-sm text-sale">{listErrorMessage}</p>}

          <AdminDataGrid
            columns={[
              {
                id: 'title',
                header: t('dashboard.subCategories.fieldTitle'),
                cell: (item) => (
                  <div className="min-w-0">
                    <p className="truncate font-semibold text-text">{item.title}</p>
                    <p className="mt-0.5 text-xs text-text-muted">
                      {item.categoryTitle}
                      <span className="mx-1.5 text-border-strong">·</span>
                      <span dir="ltr">
                        {item.code} · {item.slug || '—'}
                      </span>
                    </p>
                  </div>
                ),
              },
              {
                id: 'languages',
                header: t('dashboard.contentLocale.fieldLanguages'),
                align: 'center',
                cell: (item) => (
                  <TranslationLocaleBadges
                    translations={item.translations}
                    languages={languages}
                    currentLanguageId={contentLanguageId ?? undefined}
                  />
                ),
              },
              {
                id: 'status',
                header: t('dashboard.subCategories.fieldActive'),
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
                      ? t('dashboard.subCategories.statusActive')
                      : t('dashboard.subCategories.statusInactive')}
                  </span>
                ),
              },
              {
                id: 'actions',
                header: '',
                align: 'right',
                cell: (item) => (
                  <div className="flex flex-wrap justify-end gap-2">
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
                ),
              },
            ]}
            rows={items}
            rowKey={(item) => item.id}
            loading={listLoading}
            loadingLabel={t('dashboard.subCategories.loading')}
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
