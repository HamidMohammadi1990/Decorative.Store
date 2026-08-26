import { useCallback, useEffect, useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminProperty, AdminPropertyItem } from '@/models/admin/property.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { PropertyItemsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminPropertyItemService } from '@/services/adminPropertyItemService'
import { adminPropertyService } from '@/services/adminPropertyService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function PropertyItemsPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminPropertyItem[]>([])
  const [properties, setProperties] = useState<AdminProperty[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [filterPropertyId, setFilterPropertyId] = useState('')

  const [code, setCode] = useState('')
  const [title, setTitle] = useState('')
  const [propertyId, setPropertyId] = useState('')
  const [priority, setPriority] = useState('0')
  const [isActive, setIsActive] = useState(true)

  const propertyTitleById = useMemo(() => {
    const map = new Map<string, string>()
    properties.forEach((p) => map.set(p.id, p.title))
    return map
  }, [properties])

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.propertyItems.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const [itemResult, propertyResult] = await Promise.all([
        adminPropertyItemService.getAll(accessToken, locale, {
          pageSize: 200,
          languageId: languageId ?? undefined,
          propertyId: filterPropertyId || null,
        }),
        adminPropertyService.getAll(accessToken, locale, {
          pageSize: 200,
          languageId: languageId ?? undefined,
        }),
      ])
      setItems(itemResult.items)
      setProperties(propertyResult.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.propertyItems.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, filterPropertyId, languageId, locale, t])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  const resetForm = () => {
    setCode('')
    setTitle('')
    setPropertyId(properties[0]?.id ?? '')
    setPriority('0')
    setIsActive(true)
    setEditingId(null)
    setError(null)
  }

  const openCreate = () => {
    resetForm()
    setPropertyId(properties[0]?.id ?? '')
    setMode('create')
  }

  const openEdit = (item: AdminPropertyItem) => {
    setEditingId(item.id)
    setCode(item.code)
    setTitle(item.title)
    setPropertyId(item.propertyId)
    setPriority(String(item.priority))
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
      setError(t('dashboard.propertyItems.saveFailed'))
      return
    }

    const parsedPriority = Number(priority)
    if (!code.trim() || !title.trim() || !propertyId || !Number.isFinite(parsedPriority)) {
      setError(t('dashboard.propertyItems.validationRequired'))
      return
    }

    setSaving(true)
    setError(null)
    try {
      if (mode === 'edit' && editingId) {
        await adminPropertyItemService.update(accessToken, locale, {
          id: editingId,
          languageId,
          code: code.trim(),
          title: title.trim(),
          propertyId,
          priority: parsedPriority,
          status: isActive,
        })
      } else {
        await adminPropertyItemService.create(accessToken, locale, {
          languageId,
          code: code.trim(),
          title: title.trim(),
          propertyId,
          priority: parsedPriority,
        })
      }
      await load()
      backToList()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.propertyItems.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.propertyItems.deleteConfirm'))) return

    setSaving(true)
    setError(null)
    try {
      await adminPropertyItemService.delete(accessToken, id)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.propertyItems.deleteFailed')))
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
              ? t('dashboard.propertyItems.editTitle')
              : t('dashboard.propertyItems.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.propertyItems.editDescription')
              : t('dashboard.propertyItems.createDescription')
          }
          icon={<PropertyItemsIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.propertyItems.fieldProperty')}>
            <select
              value={propertyId}
              onChange={(e) => setPropertyId(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.propertyItems.selectProperty')}</option>
              {properties.map((property) => (
                <option key={property.id} value={property.id}>
                  {property.title} ({property.code})
                </option>
              ))}
            </select>
          </AdminField>

          <AdminField label={t('dashboard.propertyItems.fieldCode')}>
            <input
              value={code}
              onChange={(e) => setCode(e.target.value)}
              className={adminInputClass}
              dir="ltr"
            />
          </AdminField>

          <AdminField label={t('dashboard.propertyItems.fieldTitle')}>
            <input
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className={adminInputClass}
              placeholder={t('dashboard.propertyItems.titlePlaceholder')}
            />
          </AdminField>

          <AdminField label={t('dashboard.propertyItems.fieldPriority')}>
            <input
              type="number"
              min="0"
              value={priority}
              onChange={(e) => setPriority(e.target.value)}
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
              {t('dashboard.propertyItems.fieldActive')}
            </label>
          )}

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.propertyItems.saving')} />
              ) : (
                t('dashboard.propertyItems.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.propertyItems.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.propertyItems.title')}
        description={t('dashboard.propertyItems.description')}
        icon={<PropertyItemsIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate} disabled={properties.length === 0}>
            {t('dashboard.propertyItems.add')}
          </Button>
        }
      />

      <div className="mb-4">
        <AdminField label={t('dashboard.propertyItems.filterProperty')}>
          <select
            value={filterPropertyId}
            onChange={(e) => setFilterPropertyId(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.propertyItems.allProperties')}</option>
            {properties.map((property) => (
              <option key={property.id} value={property.id}>
                {property.title}
              </option>
            ))}
          </select>
        </AdminField>
      </div>

      {loading || languageLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.propertyItems.loading')} />
        </div>
      ) : properties.length === 0 ? (
        <DashboardEmptyState
          icon={<PropertyItemsIcon size={28} />}
          title={t('dashboard.propertyItems.noPropertiesTitle')}
          message={t('dashboard.propertyItems.noPropertiesMessage')}
        />
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<PropertyItemsIcon size={28} />}
          title={t('dashboard.propertyItems.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.propertyItems.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<PropertyItemsIcon size={28} />}
          title={t('dashboard.propertyItems.emptyTitle')}
          message={t('dashboard.propertyItems.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.propertyItems.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.propertyItems.itemCount', { count: items.length })}
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
                    {propertyTitleById.get(item.propertyId) ?? item.propertyCode}
                    <span className="mx-1.5 text-border-strong">·</span>
                    <span dir="ltr">{item.code}</span>
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
                      ? t('dashboard.propertyItems.statusActive')
                      : t('dashboard.propertyItems.statusInactive')}
                  </span>
                  <Button
                    variant="secondary"
                    className="py-1.5 text-xs"
                    onClick={() => openEdit(item)}
                    disabled={saving}
                  >
                    {t('dashboard.propertyItems.edit')}
                  </Button>
                  <Button
                    variant="ghost"
                    className="py-1.5 text-xs text-sale hover:bg-sale/10"
                    onClick={() => void handleDelete(item.id)}
                    disabled={saving}
                  >
                    {t('dashboard.propertyItems.delete')}
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
