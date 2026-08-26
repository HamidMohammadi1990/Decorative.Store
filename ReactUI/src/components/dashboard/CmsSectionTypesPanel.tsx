import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminCmsSectionType } from '@/models/admin/cms.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { CmsSectionTypesIcon } from '@/components/dashboard/DashboardIcons'
import { AdminField, adminInputClass, resolveAdminMutationError } from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminSectionTypeService } from '@/services/adminSectionTypeService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function CmsSectionTypesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()
  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminCmsSectionType[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [name, setName] = useState('')
  const [isActive, setIsActive] = useState(true)

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.cms.sectionTypes.authRequired'))
      setLoading(false)
      return
    }
    setLoading(true)
    setError(null)
    try {
      const result = await adminSectionTypeService.getAll(accessToken, locale, { pageSize: 100, languageId: languageId ?? undefined })
      setItems(result.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.sectionTypes.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, languageId, locale, t])

  useEffect(() => { if (!languageLoading) void load() }, [languageLoading, load])

  const resetForm = () => { setName(''); setIsActive(true); setEditingId(null); setError(null) }
  const backToList = () => { resetForm(); setMode('list') }

  const handleSave = async () => {
    if (!accessToken || languageId == null || !name.trim()) {
      setError(t('dashboard.cms.sectionTypes.validationRequired'))
      return
    }
    setSaving(true)
    try {
      const payload = { languageId, name: name.trim(), isActive }
      if (mode === 'edit' && editingId) await adminSectionTypeService.update(accessToken, locale, { ...payload, id: editingId })
      else await adminSectionTypeService.create(accessToken, locale, payload)
      await load()
      backToList()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.sectionTypes.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode !== 'list') {
    return (
      <div>
        <DashboardPageHeader title={t(mode === 'edit' ? 'dashboard.cms.sectionTypes.editTitle' : 'dashboard.cms.sectionTypes.createTitle')} icon={<CmsSectionTypesIcon size={22} />} />
        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5">
          <AdminField label={t('dashboard.cms.sectionTypes.fieldName')}>
            <input value={name} onChange={(e) => setName(e.target.value)} className={adminInputClass} dir="ltr" />
          </AdminField>
          {mode === 'edit' && (
            <label className="flex items-center gap-2 text-sm"><input type="checkbox" checked={isActive} onChange={(e) => setIsActive(e.target.checked)} className="size-4" />{t('dashboard.cms.sectionTypes.fieldActive')}</label>
          )}
          {error && <p className="text-sm text-sale">{error}</p>}
          <div className="flex gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>{t('dashboard.cms.sectionTypes.save')}</Button>
            <Button variant="secondary" onClick={backToList}>{t('dashboard.cms.sectionTypes.cancel')}</Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader title={t('dashboard.cms.sectionTypes.title')} description={t('dashboard.cms.sectionTypes.description')} icon={<CmsSectionTypesIcon size={22} />} action={<Button variant="warm" onClick={() => { resetForm(); setMode('create') }}>{t('dashboard.cms.sectionTypes.add')}</Button>} />
      {loading || languageLoading ? <div className="flex justify-center py-16"><InlineLoading label={t('dashboard.cms.sectionTypes.loading')} /></div> : items.length === 0 ? (
        <DashboardEmptyState icon={<CmsSectionTypesIcon size={28} />} title={t('dashboard.cms.sectionTypes.emptyTitle')} message={error ?? t('dashboard.cms.sectionTypes.emptyMessage')} />
      ) : (
        <ul className="divide-y divide-border rounded-sm border border-border">
          {items.map((item) => (
            <li key={item.id} className="flex items-center justify-between px-4 py-3">
              <span className="font-medium" dir="ltr">{item.name}</span>
              <Button variant="secondary" size="sm" onClick={() => { setEditingId(item.id); setName(item.name); setIsActive(item.isActive); setMode('edit') }}>{t('dashboard.cms.sectionTypes.edit')}</Button>
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
