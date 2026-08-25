import { useCallback, useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { AdminProductListItem } from '@/models/admin/catalog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { ProductsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminProductService } from '@/services/adminProductService'
import { useUserStore } from '@/stores/userStore'

export function ProductsPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [items, setItems] = useState<AdminProductListItem[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [search, setSearch] = useState('')
  const [appliedSearch, setAppliedSearch] = useState('')

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.products.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const result = await adminProductService.getAll(accessToken, locale, {
        pageSize: 100,
        languageId: languageId ?? undefined,
        title: appliedSearch || null,
      })
      setItems(result.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.products.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, appliedSearch, languageId, locale, t])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.products.deleteConfirm'))) return

    setSaving(true)
    setError(null)
    try {
      await adminProductService.delete(accessToken, id)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.products.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.products.title')}
        description={t('dashboard.products.description')}
        icon={<ProductsIcon size={22} />}
        action={
          <Link to="/account/dashboard/products/new">
            <Button variant="warm">{t('dashboard.products.add')}</Button>
          </Link>
        }
      />

      <div className="mb-4 flex flex-wrap gap-3">
        <div className="min-w-[16rem] flex-1">
          <AdminField label={t('dashboard.products.searchLabel')}>
            <input
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === 'Enter') setAppliedSearch(search.trim())
              }}
              className={adminInputClass}
              placeholder={t('dashboard.products.searchPlaceholder')}
            />
          </AdminField>
        </div>
        <div className="flex items-end gap-2">
          <Button variant="secondary" onClick={() => setAppliedSearch(search.trim())}>
            {t('dashboard.products.search')}
          </Button>
          {appliedSearch && (
            <Button
              variant="ghost"
              onClick={() => {
                setSearch('')
                setAppliedSearch('')
              }}
            >
              {t('dashboard.products.clearSearch')}
            </Button>
          )}
        </div>
      </div>

      {loading || languageLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.products.loading')} />
        </div>
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          title={t('dashboard.products.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.products.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          title={t('dashboard.products.emptyTitle')}
          message={t('dashboard.products.emptyMessage')}
          action={
            <Link to="/account/dashboard/products/new">
              <Button variant="warm">{t('dashboard.products.add')}</Button>
            </Link>
          }
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.products.itemCount', { count: items.length })}
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
                    {item.productCode}
                    {item.slug ? ` · ${item.slug}` : ''}
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
                      ? t('dashboard.products.statusActive')
                      : t('dashboard.products.statusInactive')}
                  </span>
                  <Link to={`/account/dashboard/products/${encodeURIComponent(item.id)}/edit`}>
                    <Button variant="secondary" className="py-1.5 text-xs" disabled={saving}>
                      {t('dashboard.products.edit')}
                    </Button>
                  </Link>
                  <Button
                    variant="ghost"
                    className="py-1.5 text-xs text-sale hover:bg-sale/10"
                    onClick={() => void handleDelete(item.id)}
                    disabled={saving}
                  >
                    {t('dashboard.products.delete')}
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
