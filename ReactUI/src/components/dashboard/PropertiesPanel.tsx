import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { PropertyTypeValue } from '@/models/checkout/checkout.model'
import type { AdminProperty, AdminPropertyCategory } from '@/models/admin/property.model'
import { PROPERTY_TYPE_OPTIONS } from '@/models/admin/property.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { PropertiesIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminPropertyCategoryService } from '@/services/adminPropertyCategoryService'
import { adminPropertyService } from '@/services/adminPropertyService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

function propertyTypeLabel(t: (key: string) => string, type: PropertyTypeValue) {
  const keys: Record<PropertyTypeValue, string> = {
    1: 'dashboard.propertyTypes.boolean',
    2: 'dashboard.propertyTypes.hasParents',
    3: 'dashboard.propertyTypes.numeric',
    4: 'dashboard.propertyTypes.numericWithItem',
    5: 'dashboard.propertyTypes.dimensions',
    6: 'dashboard.propertyTypes.text',
    7: 'dashboard.propertyTypes.select',
  }
  return t(keys[type])
}

export function PropertiesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminProperty[]>([])
  const [categories, setCategories] = useState<AdminPropertyCategory[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [filterCategoryId, setFilterCategoryId] = useState('')

  const [code, setCode] = useState('')
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [priority, setPriority] = useState('0')
  const [propertyCategoryId, setPropertyCategoryId] = useState('')
  const [propertyType, setPropertyType] = useState<PropertyTypeValue>(1)
  const [parentId, setParentId] = useState('')
  const [isActive, setIsActive] = useState(true)

  const categoryTitleById = useMemo(() => {
    const map = new Map<string, string>()
    categories.forEach((c) => map.set(c.id, c.title))
    return map
  }, [categories])

  const parentOptions = useMemo(
    () => items.filter((item) => item.id !== editingId),
    [items, editingId],
  )

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.properties.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const [propertyResult, categoryResult] = await Promise.all([
        adminPropertyService.getAll(accessToken, locale, {
          pageSize: 200,
          languageId: languageId ?? undefined,
          propertyCategoryId: filterCategoryId || null,
        }),
        adminPropertyCategoryService.getAll(accessToken, locale, {
          pageSize: 100,
          languageId: languageId ?? undefined,
        }),
      ])
      setItems(propertyResult.items)
      setCategories(categoryResult.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.properties.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, filterCategoryId, languageId, locale, t])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  const resetForm = () => {
    setCode('')
    setTitle('')
    setDescription('')
    setPriority('0')
    setPropertyCategoryId(categories[0]?.id ?? '')
    setPropertyType(1)
    setParentId('')
    setIsActive(true)
    setEditingId(null)
    setError(null)
  }

  const openCreate = () => {
    resetForm()
    setPropertyCategoryId(categories[0]?.id ?? '')
    setMode('create')
  }

  const openEdit = (item: AdminProperty) => {
    setEditingId(item.id)
    setCode(item.code)
    setTitle(item.title)
    setDescription(item.description ?? '')
    setPriority(String(item.priority))
    setPropertyCategoryId(item.propertyCategoryId)
    setPropertyType(item.propertyType)
    setParentId(item.parentId ?? '')
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
      setError(t('dashboard.properties.saveFailed'))
      return
    }

    const parsedPriority = Number(priority)
    if (!code.trim() || !title.trim() || !propertyCategoryId || !Number.isFinite(parsedPriority)) {
      setError(t('dashboard.properties.validationRequired'))
      return
    }

    setSaving(true)
    setError(null)
    try {
      if (mode === 'edit' && editingId) {
        await adminPropertyService.update(accessToken, locale, {
          id: editingId,
          languageId,
          code: code.trim(),
          title: title.trim(),
          description: description.trim() || null,
          parentId: parentId || null,
          priority: parsedPriority,
          propertyCategoryId,
          propertyType,
          status: isActive,
        })
      } else {
        await adminPropertyService.create(accessToken, locale, {
          languageId,
          code: code.trim(),
          title: title.trim(),
          description: description.trim() || null,
          parentId: parentId || null,
          priority: parsedPriority,
          propertyCategoryId,
          propertyType,
        })
      }
      await load()
      backToList()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.properties.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.properties.deleteConfirm'))) return

    setSaving(true)
    setError(null)
    try {
      await adminPropertyService.delete(accessToken, id)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.properties.deleteFailed')))
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
              ? t('dashboard.properties.editTitle')
              : t('dashboard.properties.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.properties.editDescription')
              : t('dashboard.properties.createDescription')
          }
          icon={<PropertiesIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.properties.fieldCategory')}>
            <select
              value={propertyCategoryId}
              onChange={(e) => setPropertyCategoryId(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.properties.selectCategory')}</option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.title}
                </option>
              ))}
            </select>
          </AdminField>

          <AdminField label={t('dashboard.properties.fieldCode')}>
            <input
              value={code}
              onChange={(e) => setCode(e.target.value)}
              className={adminInputClass}
              dir="ltr"
            />
          </AdminField>

          <AdminField label={t('dashboard.properties.fieldTitle')}>
            <input
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className={adminInputClass}
              placeholder={t('dashboard.properties.titlePlaceholder')}
            />
          </AdminField>

          <AdminField label={t('dashboard.properties.fieldDescription')}>
            <textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              rows={3}
              className={adminInputClass}
            />
          </AdminField>

          <div className="grid gap-5 sm:grid-cols-2">
            <AdminField label={t('dashboard.properties.fieldType')}>
              <select
                value={propertyType}
                onChange={(e) => setPropertyType(Number(e.target.value) as PropertyTypeValue)}
                className={adminInputClass}
              >
                {PROPERTY_TYPE_OPTIONS.map((type) => (
                  <option key={type} value={type}>
                    {propertyTypeLabel(t, type)}
                  </option>
                ))}
              </select>
            </AdminField>

            <AdminField label={t('dashboard.properties.fieldPriority')}>
              <input
                type="number"
                min="0"
                value={priority}
                onChange={(e) => setPriority(e.target.value)}
                className={adminInputClass}
                dir="ltr"
              />
            </AdminField>
          </div>

          <AdminField label={t('dashboard.properties.fieldParent')}>
            <select
              value={parentId}
              onChange={(e) => setParentId(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.properties.noParent')}</option>
              {parentOptions.map((item) => (
                <option key={item.id} value={item.id}>
                  {item.title} ({item.code})
                </option>
              ))}
            </select>
          </AdminField>

          {mode === 'edit' && (
            <label className="flex items-center gap-2 text-sm text-text">
              <input
                type="checkbox"
                checked={isActive}
                onChange={(e) => setIsActive(e.target.checked)}
                className="size-4 rounded border-border text-warm focus:ring-warm"
              />
              {t('dashboard.properties.fieldActive')}
            </label>
          )}

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.properties.saving')} />
              ) : (
                t('dashboard.properties.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.properties.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.properties.title')}
        description={t('dashboard.properties.description')}
        icon={<PropertiesIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate} disabled={categories.length === 0}>
            {t('dashboard.properties.add')}
          </Button>
        }
      />

      <div className="mb-4">
        <AdminField label={t('dashboard.properties.filterCategory')}>
          <select
            value={filterCategoryId}
            onChange={(e) => setFilterCategoryId(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.properties.allCategories')}</option>
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
          <InlineLoading label={t('dashboard.properties.loading')} />
        </div>
      ) : categories.length === 0 ? (
        <DashboardEmptyState
          icon={<PropertiesIcon size={28} />}
          title={t('dashboard.properties.noCategoriesTitle')}
          message={t('dashboard.properties.noCategoriesMessage')}
          action={
            <Link
              to="/account/dashboard/property-categories"
              className="inline-flex items-center justify-center rounded-sm bg-warm px-4 py-2 text-sm font-medium text-warm-text transition-colors hover:bg-warm-hover"
            >
              {t('dashboard.properties.goToPropertyCategories')}
            </Link>
          }
        />
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<PropertiesIcon size={28} />}
          title={t('dashboard.properties.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.properties.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<PropertiesIcon size={28} />}
          title={t('dashboard.properties.emptyTitle')}
          message={t('dashboard.properties.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.properties.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.properties.itemCount', { count: items.length })}
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
                    {categoryTitleById.get(item.propertyCategoryId) ?? item.propertyCategoryCode}
                    <span className="mx-1.5 text-border-strong">·</span>
                    <span dir="ltr">{item.code}</span>
                    <span className="mx-1.5 text-border-strong">·</span>
                    {propertyTypeLabel(t, item.propertyType)}
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
                      ? t('dashboard.properties.statusActive')
                      : t('dashboard.properties.statusInactive')}
                  </span>
                  <Button
                    variant="secondary"
                    className="py-1.5 text-xs"
                    onClick={() => openEdit(item)}
                    disabled={saving}
                  >
                    {t('dashboard.properties.edit')}
                  </Button>
                  <Button
                    variant="ghost"
                    className="py-1.5 text-xs text-sale hover:bg-sale/10"
                    onClick={() => void handleDelete(item.id)}
                    disabled={saving}
                  >
                    {t('dashboard.properties.delete')}
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
