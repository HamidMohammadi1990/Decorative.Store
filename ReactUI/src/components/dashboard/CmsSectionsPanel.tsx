import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminCmsSection, AdminCmsSectionType } from '@/models/admin/cms.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { CmsSectionsIcon, EditIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminGridActions,
  AdminGridIconButton,
} from '@/components/dashboard/admin/AdminGridActions'
import { AdminField, adminInputClass, resolveAdminMutationError } from '@/components/dashboard/admin/adminFormShared'
import { AdminListGridHeader } from '@/components/dashboard/admin/AdminListGridHeader'
import { AdminRowNumber } from '@/components/dashboard/admin/AdminRowNumber'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminSectionService } from '@/services/adminSectionService'
import { adminSectionTypeService } from '@/services/adminSectionTypeService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

function RelatedMetaLine({ label, value }: { label: string; value?: string | null }) {
  if (!value?.trim()) return null
  return (
    <span>
      <span className="font-medium text-text-muted">{label}:</span> {value}
    </span>
  )
}

export function CmsSectionsPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()
  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminCmsSection[]>([])
  const [sectionTypes, setSectionTypes] = useState<AdminCmsSectionType[]>([])
  const [loading, setLoading] = useState(true)
  const [formLoading, setFormLoading] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [sectionTypeId, setSectionTypeId] = useState('')
  const [title, setTitle] = useState('')
  const [url, setUrl] = useState('')
  const [description, setDescription] = useState('')
  const [isActive, setIsActive] = useState(true)

  const loadList = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.cms.sections.authRequired'))
      setLoading(false)
      return
    }
    setLoading(true)
    try {
      const sections = await adminSectionService.getAll(accessToken, locale, {
        pageSize: 200,
        languageId: languageId ?? undefined,
      })
      setItems(sections.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.sections.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, languageId, locale, t])

  const loadFormOptions = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') return
    setFormLoading(true)
    try {
      const types = await adminSectionTypeService.getAll(accessToken, locale, {
        pageSize: 200,
        languageId: languageId ?? undefined,
      })
      setSectionTypes(types.items)
      if (!sectionTypeId && types.items[0]) setSectionTypeId(types.items[0].id)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.sections.loadFailed')))
    } finally {
      setFormLoading(false)
    }
  }, [accessToken, languageId, locale, sectionTypeId, t])

  useEffect(() => { if (!languageLoading) void loadList() }, [languageLoading, loadList])

  useEffect(() => {
    if (mode !== 'list' && !languageLoading) void loadFormOptions()
  }, [languageLoading, loadFormOptions, mode])

  const resetForm = () => {
    setTitle(''); setUrl(''); setDescription(''); setIsActive(true); setEditingId(null); setError(null)
    if (sectionTypes[0]) setSectionTypeId(sectionTypes[0].id)
  }

  const handleSave = async () => {
    if (!accessToken || languageId == null || !title.trim() || !url.trim() || !sectionTypeId) {
      setError(t('dashboard.cms.sections.validationRequired'))
      return
    }
    setSaving(true)
    try {
      const payload = { languageId, sectionTypeId, title: title.trim(), url: url.trim(), description: description.trim() || undefined, isActive }
      if (mode === 'edit' && editingId) await adminSectionService.update(accessToken, locale, { ...payload, id: editingId })
      else await adminSectionService.create(accessToken, locale, payload)
      await loadList(); resetForm(); setMode('list')
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.sections.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode !== 'list') {
    return (
      <div>
        <DashboardPageHeader title={t(mode === 'edit' ? 'dashboard.cms.sections.editTitle' : 'dashboard.cms.sections.createTitle')} icon={<CmsSectionsIcon size={22} />} />
        {formLoading ? (
          <div className="flex justify-center py-16"><InlineLoading label={t('dashboard.cms.sections.loading')} /></div>
        ) : (
          <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5">
            <AdminField label={t('dashboard.cms.sections.fieldSectionType')}>
              <select value={sectionTypeId} onChange={(e) => setSectionTypeId(e.target.value)} className={adminInputClass}>
                {sectionTypes.map((type) => <option key={type.id} value={type.id}>{type.name}</option>)}
              </select>
            </AdminField>
            <AdminField label={t('dashboard.cms.sections.fieldTitle')}><input value={title} onChange={(e) => setTitle(e.target.value)} className={adminInputClass} /></AdminField>
            <AdminField label={t('dashboard.cms.sections.fieldUrl')}><input value={url} onChange={(e) => setUrl(e.target.value)} className={adminInputClass} dir="ltr" /></AdminField>
            <AdminField label={t('dashboard.cms.sections.fieldDescription')}><textarea value={description} onChange={(e) => setDescription(e.target.value)} className={adminInputClass} rows={3} /></AdminField>
            {mode === 'edit' && <label className="flex items-center gap-2 text-sm"><input type="checkbox" checked={isActive} onChange={(e) => setIsActive(e.target.checked)} className="size-4" />{t('dashboard.cms.sections.fieldActive')}</label>}
            {error && <p className="text-sm text-sale">{error}</p>}
            <div className="flex gap-3">
              <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>{t('dashboard.cms.sections.save')}</Button>
              <Button variant="secondary" onClick={() => { resetForm(); setMode('list') }}>{t('dashboard.cms.sections.cancel')}</Button>
            </div>
          </div>
        )}
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader title={t('dashboard.cms.sections.title')} description={t('dashboard.cms.sections.description')} icon={<CmsSectionsIcon size={22} />} action={<Button variant="warm" onClick={() => { resetForm(); setMode('create') }}>{t('dashboard.cms.sections.add')}</Button>} />
      {loading || languageLoading ? <div className="flex justify-center py-16"><InlineLoading label={t('dashboard.cms.sections.loading')} /></div> : items.length === 0 ? (
        <DashboardEmptyState icon={<CmsSectionsIcon size={28} />} title={t('dashboard.cms.sections.emptyTitle')} message={error ?? t('dashboard.cms.sections.emptyMessage')} />
      ) : (
        <ul className="divide-y divide-border rounded-sm border border-border">
          <AdminListGridHeader />
          {items.map((item, index) => (
            <li key={item.id} className="flex items-center gap-3 px-4 py-3">
              <AdminRowNumber value={index + 1} />
              <div className="flex-1 min-w-0">
                <p className="font-medium truncate">{item.title}</p>
                <p className="mt-1 flex flex-wrap gap-x-3 gap-y-1 text-xs text-text-muted">
                  <RelatedMetaLine label={t('dashboard.cms.sections.colSectionType')} value={item.sectionTypeName} />
                  <RelatedMetaLine label={t('dashboard.cms.sections.colParent')} value={item.parentTitle} />
                  {item.url && <RelatedMetaLine label={t('dashboard.cms.sections.fieldUrl')} value={item.url} />}
                </p>
              </div>
              <AdminGridActions>
                <AdminGridIconButton
                  label={t('dashboard.cms.sections.edit')}
                  icon={<EditIcon size={15} />}
                  onClick={() => {
                    setEditingId(item.id)
                    setSectionTypeId(item.sectionTypeId)
                    setTitle(item.title)
                    setUrl(item.url)
                    setDescription(item.description ?? '')
                    setIsActive(item.isActive)
                    setMode('edit')
                  }}
                />
              </AdminGridActions>
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
