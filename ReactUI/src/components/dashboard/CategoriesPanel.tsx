import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminCategory } from '@/models/admin/catalog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { CategoriesIcon } from '@/components/dashboard/DashboardIcons'
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
import { slugifyTitle, pickTranslation } from '@/services/admin/adminCatalogNormalize'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function CategoriesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()
  const { languages } = useStoreLanguages()
  const {
    contentLanguageId,
    setContentLanguageId,
    languages: formLanguages,
    loading: contentLanguageLoading,
  } = useAdminContentLanguage()

  const [mode, setMode] = useState<Mode>('list')
  const [saving, setSaving] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [editingItem, setEditingItem] = useState<AdminCategory | null>(null)

  const [title, setTitle] = useState('')
  const [slug, setSlug] = useState('')
  const [code, setCode] = useState('')
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
        throw new Error(t('dashboard.categories.authRequired'))
      }
      return adminCategoryService.getAll(accessToken, locale, {
        pageNumber,
        pageSize,
        languageId: contentLanguageId ?? undefined,
      })
    },
    [accessToken, contentLanguageId, locale, t],
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
  } = useAdminPagedList<AdminCategory>({
    fetchPage,
    initialPageSize: 20,
    enabled: canLoad && mode === 'list',
  })

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.categories.loadFailed'))
    : null

  const resetForm = () => {
    setTitle('')
    setSlug('')
    setCode('')
    setIsActive(true)
    setSlugTouched(false)
    setEditingId(null)
    setEditingItem(null)
    setFormError(null)
  }

  const applyCategoryTranslation = (item: AdminCategory, targetLanguageId: number) => {
    const translation = pickTranslation(item.translations, targetLanguageId)
    setTitle(translation?.title ?? '')
    setSlug(translation?.slug ?? '')
    setSlugTouched(Boolean(translation?.slug))
  }

  const openCreate = () => {
    resetForm()
    setMode('create')
  }

  const openEdit = (item: AdminCategory) => {
    setEditingId(item.id)
    setEditingItem(item)
    setCode(item.code)
    setIsActive(item.isActive)
    setFormError(null)
    setMode('edit')
    const langId = contentLanguageId ?? languageId
    if (langId != null) {
      applyCategoryTranslation(item, langId)
    }
  }

  useEffect(() => {
    if (mode !== 'edit' || !editingItem || contentLanguageId == null) return
    applyCategoryTranslation(editingItem, contentLanguageId)
  }, [contentLanguageId, editingItem, mode])

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const handleTitleChange = (value: string) => {
    setTitle(value)
    if (!slugTouched) setSlug(slugifyTitle(value))
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
      setFormError(t('dashboard.categories.saveFailed'))
      return
    }

    if (!title.trim() || !slug.trim() || !code.trim()) {
      setFormError(t('dashboard.categories.validationRequired'))
      return
    }

    setSaving(true)
    setFormError(null)
    try {
      if (mode === 'edit' && editingId) {
        await adminCategoryService.update(accessToken, locale, {
          id: editingId,
          languageId: contentLanguageId,
          title: title.trim(),
          slug: slug.trim(),
          code: code.trim(),
          isActive,
        })
      } else {
        await adminCategoryService.create(accessToken, locale, {
          languageId: contentLanguageId,
          title: title.trim(),
          slug: slug.trim(),
          code: code.trim(),
        })
      }
      backToList()
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.categories.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.categories.deleteConfirm'))) return

    setSaving(true)
    try {
      await adminCategoryService.delete(accessToken, id)
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.categories.deleteFailed')))
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
              ? t('dashboard.categories.editTitle')
              : t('dashboard.categories.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.categories.editDescription')
              : t('dashboard.categories.createDescription')
          }
          icon={<CategoriesIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminContentLanguageField
            value={contentLanguageId}
            onChange={setContentLanguageId}
            languages={formLanguages}
            disabled={contentLanguageLoading}
          />
          <AdminField label={t('dashboard.categories.fieldTitle')}>
            <input
              value={title}
              onChange={(e) => handleTitleChange(e.target.value)}
              className={adminInputClass}
              placeholder={t('dashboard.categories.titlePlaceholder')}
            />
          </AdminField>

          <AdminField label={t('dashboard.categories.fieldSlug')}>
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

          <AdminField label={t('dashboard.categories.fieldCode')}>
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
              {t('dashboard.categories.fieldActive')}
            </label>
          )}

          {formError && <p className="text-sm text-sale">{formError}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.categories.saving')} />
              ) : (
                t('dashboard.categories.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.categories.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  if (!accessToken || accessToken === 'mock-access-token') {
    return (
      <DashboardEmptyState
        icon={<CategoriesIcon size={28} />}
        title={t('dashboard.categories.loadFailedTitle')}
        message={t('dashboard.categories.authRequired')}
      />
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.categories.title')}
        description={t('dashboard.categories.description')}
        icon={<CategoriesIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate}>
            {t('dashboard.categories.add')}
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

      {languageLoading || (listLoading && items.length === 0) ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.categories.loading')} />
        </div>
      ) : listErrorMessage && items.length === 0 ? (
        <DashboardEmptyState
          icon={<CategoriesIcon size={28} />}
          title={t('dashboard.categories.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.categories.retry')}
            </Button>
          }
        />
      ) : totalCount === 0 && !listLoading ? (
        <DashboardEmptyState
          icon={<CategoriesIcon size={28} />}
          title={t('dashboard.categories.emptyTitle')}
          message={t('dashboard.categories.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.categories.add')}
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
                header: t('dashboard.categories.fieldTitle'),
                cell: (item) => (
                  <div className="min-w-0">
                    <p className="truncate font-semibold text-text">{item.title}</p>
                    <p className="mt-0.5 text-xs text-text-muted" dir="ltr">
                      {item.code} · {item.slug || '—'}
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
                header: t('dashboard.categories.fieldActive'),
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
                      ? t('dashboard.categories.statusActive')
                      : t('dashboard.categories.statusInactive')}
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
                      {t('dashboard.categories.edit')}
                    </Button>
                    <Button
                      variant="ghost"
                      className="py-1.5 text-xs text-sale hover:bg-sale/10"
                      onClick={() => void handleDelete(item.id)}
                      disabled={saving}
                    >
                      {t('dashboard.categories.delete')}
                    </Button>
                  </div>
                ),
              },
            ]}
            rows={items}
            rowKey={(item) => item.id}
            loading={listLoading}
            loadingLabel={t('dashboard.categories.loading')}
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
