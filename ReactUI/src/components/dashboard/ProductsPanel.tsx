import { useCallback, useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import type { AdminProductListItem, AdminSubCategory } from '@/models/admin/catalog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import {
  ProductsIcon,
  ProductCommentsIcon,
  ProductDescriptionsIcon,
  ProductPropertiesIcon,
  ProductQuestionsIcon,
  EditIcon,
  DeleteIcon,
} from '@/components/dashboard/DashboardIcons'
import {
  AdminGridActionMenu,
  AdminGridActions,
  AdminGridIconButton,
  AdminGridIconLink,
} from '@/components/dashboard/admin/AdminGridActions'
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
import { useAdminPagedList } from '@/hooks/useAdminPagedList'
import { useAdminContentLanguage } from '@/hooks/useAdminContentLanguage'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { useStoreLanguages } from '@/hooks/useStoreLanguages'
import { adminProductService } from '@/services/adminProductService'
import { adminSubCategoryService } from '@/services/adminSubCategoryService'
import { useUserStore } from '@/stores/userStore'
import {
  ProductManageModal,
  type ProductManageTab,
} from '@/components/dashboard/ProductManageModal'

export function ProductsPanel() {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const navigate = useNavigate()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()
  const { languages } = useStoreLanguages()
  const {
    contentLanguageId,
    setContentLanguageId,
    loading: contentLanguageLoading,
  } = useAdminContentLanguage()

  const [subCategories, setSubCategories] = useState<AdminSubCategory[]>([])
  const [subCategoriesLoading, setSubCategoriesLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [actionError, setActionError] = useState<string | null>(null)
  const [search, setSearch] = useState('')
  const [appliedSearch, setAppliedSearch] = useState('')
  const [filterSubCategoryId, setFilterSubCategoryId] = useState('')
  const [manageModal, setManageModal] = useState<{
    tab: ProductManageTab
    product: AdminProductListItem
  } | null>(null)

  const openManageModal = (tab: ProductManageTab, product: AdminProductListItem) => {
    setManageModal({ tab, product })
  }

  const canLoad =
    !languageLoading &&
    !contentLanguageLoading &&
    contentLanguageId != null &&
    Boolean(accessToken) &&
    accessToken !== 'mock-access-token'

  const fetchPage = useCallback(
    async (pageNumber: number, pageSize: number) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        throw new Error(t('dashboard.products.authRequired'))
      }
      return adminProductService.getAll(accessToken, locale, {
        pageNumber,
        pageSize,
        languageId: contentLanguageId ?? undefined,
        title: appliedSearch.trim() || null,
        subCategoryId: filterSubCategoryId || null,
      })
    },
    [accessToken, appliedSearch, contentLanguageId, filterSubCategoryId, locale, t],
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
  } = useAdminPagedList<AdminProductListItem>({
    fetchPage,
    initialPageSize: 20,
    enabled: canLoad,
  })

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.products.loadFailed'))
    : null

  const loadSubCategories = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setSubCategories([])
      setSubCategoriesLoading(false)
      return
    }

    setSubCategoriesLoading(true)
    try {
      const subResult = await adminSubCategoryService.getAll(accessToken, locale, {
        pageSize: 100,
        languageId: contentLanguageId ?? undefined,
      })
      setSubCategories(subResult.items)
    } catch {
      setSubCategories([])
    } finally {
      setSubCategoriesLoading(false)
    }
  }, [accessToken, contentLanguageId, locale])

  useEffect(() => {
    if (!languageLoading) void loadSubCategories()
  }, [languageLoading, loadSubCategories])

  const applySearch = () => {
    setAppliedSearch(search)
    goToPage(1)
  }

  const handleFilterSubCategory = (value: string) => {
    setFilterSubCategoryId(value)
    goToPage(1)
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!(await confirm({ message: t('dashboard.products.deleteConfirm') }))) return

    setSaving(true)
    setActionError(null)
    try {
      await adminProductService.delete(accessToken, id)
      reload()
    } catch (err) {
      setActionError(resolveAdminMutationError(err, t('dashboard.products.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (!accessToken || accessToken === 'mock-access-token') {
    return (
      <DashboardEmptyState
        icon={<ProductsIcon size={28} />}
        title={t('dashboard.products.loadFailedTitle')}
        message={t('dashboard.products.authRequired')}
      />
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.products.title')}
        description={t('dashboard.products.description')}
        icon={<ProductsIcon size={22} />}
        action={
          <Button
            variant="warm"
            onClick={() => navigate('/account/dashboard/products/new')}
            disabled={subCategories.length === 0}
          >
            {t('dashboard.products.add')}
          </Button>
        }
      />

      <div className="mb-5 grid gap-3 sm:grid-cols-2">
        <AdminField label={t('dashboard.products.search')}>
          <input
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === 'Enter') applySearch()
            }}
            onBlur={applySearch}
            className={adminInputClass}
            placeholder={t('dashboard.products.searchPlaceholder')}
          />
        </AdminField>
        <AdminField label={t('dashboard.products.filterSubCategory')}>
          <select
            value={filterSubCategoryId}
            onChange={(e) => handleFilterSubCategory(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.products.allSubCategories')}</option>
            {subCategories.map((item) => (
              <option key={item.id} value={item.id}>
                {item.title}
              </option>
            ))}
          </select>
        </AdminField>
      </div>

      <AdminContentLanguageField
        className="mb-4"
        value={contentLanguageId}
        onChange={(id) => {
          setContentLanguageId(id)
          goToPage(1)
        }}
        languages={languages}
      />

      {languageLoading || contentLanguageLoading || subCategoriesLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.products.loading')} />
        </div>
      ) : subCategories.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductsIcon size={28} />}
          title={t('dashboard.products.noSubCategoriesTitle')}
          message={t('dashboard.products.noSubCategoriesMessage')}
          action={
            <Button
              variant="warm"
              onClick={() => navigate('/account/dashboard/sub-categories')}
            >
              {t('dashboard.products.goToSubCategories')}
            </Button>
          }
        />
      ) : listLoading && items.length === 0 ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.products.loading')} />
        </div>
      ) : listErrorMessage && items.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductsIcon size={28} />}
          title={t('dashboard.products.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.products.retry')}
            </Button>
          }
        />
      ) : totalCount === 0 && !listLoading ? (
        <DashboardEmptyState
          icon={<ProductsIcon size={28} />}
          title={t('dashboard.products.emptyTitle')}
          message={t('dashboard.products.emptyMessage')}
          action={
            <Button variant="warm" onClick={() => navigate('/account/dashboard/products/new')}>
              {t('dashboard.products.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {actionError && <p className="text-sm text-sale">{actionError}</p>}
          {listErrorMessage && <p className="text-sm text-sale">{listErrorMessage}</p>}

          <AdminDataGrid
            columns={[
              {
                id: 'title',
                header: t('dashboard.products.fieldTitle'),
                cell: (item) => (
                  <div className="min-w-0">
                    <p className="truncate font-semibold text-text">{item.title}</p>
                    <p className="mt-0.5 text-xs text-text-muted" dir="ltr">
                      {item.productCode}
                      {item.slug ? ` · ${item.slug}` : ''}
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
                header: t('dashboard.products.fieldActive'),
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
                      ? t('dashboard.products.statusActive')
                      : t('dashboard.products.statusInactive')}
                  </span>
                ),
              },
              {
                id: 'actions',
                header: '',
                align: 'right',
                cell: (item) => (
                  <AdminGridActions>
                    <AdminGridActionMenu
                      label={t('dashboard.products.manage')}
                      icon={<ProductsIcon size={14} />}
                      items={[
                        {
                          id: 'images',
                          label: t('dashboard.products.images'),
                          onClick: () => openManageModal('images', item),
                        },
                        {
                          id: 'properties',
                          label: t('dashboard.products.properties'),
                          icon: <ProductPropertiesIcon size={14} />,
                          onClick: () => openManageModal('properties', item),
                        },
                        {
                          id: 'descriptions',
                          label: t('dashboard.products.descriptions'),
                          icon: <ProductDescriptionsIcon size={14} />,
                          onClick: () => openManageModal('descriptions', item),
                        },
                        {
                          id: 'comments',
                          label: t('dashboard.products.comments'),
                          icon: <ProductCommentsIcon size={14} />,
                          onClick: () => openManageModal('comments', item),
                        },
                        {
                          id: 'questions',
                          label: t('dashboard.products.questions'),
                          icon: <ProductQuestionsIcon size={14} />,
                          onClick: () => openManageModal('questions', item),
                        },
                      ]}
                    />
                    <AdminGridIconLink
                      label={t('dashboard.products.edit')}
                      icon={<EditIcon size={15} />}
                      to={`/account/dashboard/products/edit?id=${encodeURIComponent(item.id)}${
                        item.subCategoryId
                          ? `&subCategoryId=${encodeURIComponent(item.subCategoryId)}`
                          : ''
                      }`}
                    />
                    <AdminGridIconButton
                      label={t('dashboard.products.delete')}
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
            loadingLabel={t('dashboard.products.loading')}
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

      <ProductManageModal
        open={manageModal != null}
        tab={manageModal?.tab ?? 'images'}
        product={manageModal?.product ?? null}
        onClose={() => setManageModal(null)}
      />
    </div>
  )
}
