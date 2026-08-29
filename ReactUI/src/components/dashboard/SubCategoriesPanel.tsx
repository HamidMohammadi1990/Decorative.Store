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

function mergeCategory(items: AdminCategory[], candidate: AdminCategory | null): AdminCategory[] {
  if (!candidate || items.some((item) => item.id === candidate.id)) return items
  return [candidate, ...items]
}

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
  const [formLoading, setFormLoading] = useState(false)
  const [saving, setSaving] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [editingItem, setEditingItem] = useState<AdminSubCategory | null>(null)
  const [filterCategoryId, setFilterCategoryId] = useState('')

  const [title, setTitle] = useState('')
  const [slug, setSlug] = useState('')
  const [code, setCode] = useState('')
  const [categoryId, setCategoryId] = useState('')
  const [categoryLabel, setCategoryLabel] = useState('')
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
      const categoryResult = await adminCategoryService.getAllForSelect(accessToken, locale, {
        languageId: contentLanguageId ?? undefined,
      })
      setCategories(categoryResult.items)
    } catch {
      setCategories([])
    } finally {
      setCategoriesLoading(false)
    }
  }, [accessToken, contentLanguageId, locale])

  const resolveCategorySelection = useCallback(
    async (
      targetCategoryId: string,
      items: AdminCategory[],
      options?: { fallbackTitle?: string; categoryCode?: string },
    ): Promise<AdminCategory[]> => {
      if (!accessToken || accessToken === 'mock-access-token') {
        return items
      }

      let resolvedId = targetCategoryId.trim()
      const categoryCode = options?.categoryCode?.trim()

      if (categoryCode) {
        const byCode = items.find((item) => item.code === categoryCode)
        if (byCode && (!resolvedId || !items.some((item) => item.id === resolvedId))) {
          resolvedId = byCode.id
        }
      }

      if (!resolvedId) {
        setCategoryLabel(options?.fallbackTitle ?? '')
        setCategoryId('')
        return items
      }

      let nextItems = items
      if (!nextItems.some((item) => item.id === resolvedId)) {
        const category = await adminCategoryService.get(accessToken, locale, resolvedId)
        if (category) {
          nextItems = mergeCategory(nextItems, category)
        } else if (options?.fallbackTitle) {
          nextItems = mergeCategory(nextItems, {
            id: resolvedId,
            code: categoryCode ?? '',
            isActive: true,
            title: options.fallbackTitle,
            slug: '',
            translations: [],
          })
        }
      }

      const match = nextItems.find((item) => item.id === resolvedId)
      setCategoryLabel(match?.title ?? options?.fallbackTitle ?? '')
      setCategoryId(resolvedId)
      return nextItems
    },
    [accessToken, locale],
  )

  useEffect(() => {
    if (mode !== 'list') return
    if (!languageLoading && !contentLanguageLoading && contentLanguageId != null) {
      void loadCategories()
    }
  }, [mode, languageLoading, contentLanguageLoading, contentLanguageId, loadCategories])

  const resetForm = () => {
    setTitle('')
    setSlug('')
    setCode('')
    setCategoryId(categories[0]?.id ?? '')
    setCategoryLabel(categories[0]?.title ?? '')
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
    setCategoryLabel(categories[0]?.title ?? '')
    setMode('create')
  }

  const openEdit = (item: AdminSubCategory) => {
    if (!accessToken || accessToken === 'mock-access-token') return

    setEditingId(item.id)
    setEditingItem(item)
    setFormError(null)
    setMode('edit')
    setFormLoading(true)

    void (async () => {
      try {
        const detail = (await adminSubCategoryService.get(accessToken, locale, item.id)) ?? item
        const categoryResult = await adminCategoryService.getAllForSelect(accessToken, locale, {
          languageId: contentLanguageId ?? undefined,
        })
        const categoryItems = await resolveCategorySelection(
          detail.categoryId || item.categoryId,
          categoryResult.items,
          {
            fallbackTitle: detail.categoryTitle || item.categoryTitle,
            categoryCode: detail.categoryCode || item.categoryCode,
          },
        )
        setCategories(categoryItems)
        setCode(detail.code)
        setIsActive(detail.isActive)
        setEditingItem(detail)
        if (contentLanguageId != null) {
          applySubCategoryTranslation(detail, contentLanguageId)
        }
      } catch {
        setFormError(t('dashboard.subCategories.loadFailed'))
        setCode(item.code)
        setCategoryId(item.categoryId)
        setCategoryLabel(item.categoryTitle)
        setIsActive(item.isActive)
        setEditingItem(item)
        if (contentLanguageId != null) {
          applySubCategoryTranslation(item, contentLanguageId)
        }
      } finally {
        setFormLoading(false)
      }
    })()
  }

  const handleCategoryChange = (value: string) => {
    setCategoryId(value)
    const match = categories.find((category) => category.id === value)
    setCategoryLabel(match?.title ?? '')
  }

  useEffect(() => {
    if (mode !== 'edit' || !editingItem || contentLanguageId == null || formLoading) return
    applySubCategoryTranslation(editingItem, contentLanguageId)
  }, [contentLanguageId, editingItem, formLoading, mode])

  useEffect(() => {
    if (
      mode !== 'edit' ||
      formLoading ||
      !accessToken ||
      accessToken === 'mock-access-token' ||
      contentLanguageId == null
    ) {
      return
    }

    void adminCategoryService
      .getAllForSelect(accessToken, locale, { languageId: contentLanguageId })
      .then((result) => {
        setCategories((prev) => {
          const preserved =
            categoryId
              ? prev.find((item) => item.id === categoryId) ??
                result.items.find((item) => item.id === categoryId)
              : null
          let items = result.items
          if (preserved && !items.some((item) => item.id === preserved.id)) {
            items = mergeCategory(items, preserved)
          }
          return items
        })
        const match = result.items.find((item) => item.id === categoryId)
        if (match) setCategoryLabel(match.title)
      })
  }, [accessToken, categoryId, contentLanguageId, formLoading, locale, mode])

  const selectedCategory = categories.find((category) => category.id === categoryId)
  const displayedCategoryLabel = selectedCategory?.title ?? categoryLabel

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
          {formLoading ? (
            <div className="flex justify-center py-12">
              <InlineLoading label={t('dashboard.subCategories.loading')} />
            </div>
          ) : (
            <>
          <AdminContentLanguageField
            value={contentLanguageId}
            onChange={setContentLanguageId}
            languages={formLanguages}
            disabled={contentLanguageLoading}
          />
          <AdminField label={t('dashboard.subCategories.fieldCategory')}>
            <select
              value={categoryId}
              onChange={(e) => handleCategoryChange(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.subCategories.selectCategory')}</option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.title}
                </option>
              ))}
              {categoryId &&
                !categories.some((category) => category.id === categoryId) &&
                displayedCategoryLabel && (
                  <option value={categoryId}>{displayedCategoryLabel}</option>
                )}
            </select>
            {mode === 'edit' && displayedCategoryLabel && (
              <p className="mt-1.5 text-xs font-medium text-warm">
                {t('dashboard.subCategories.currentCategory', { name: displayedCategoryLabel })}
              </p>
            )}
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
            <Button variant="secondary" onClick={backToList} disabled={saving || formLoading}>
              {t('dashboard.subCategories.cancel')}
            </Button>
          </div>
            </>
          )}
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
