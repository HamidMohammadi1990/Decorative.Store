import { useCallback, useEffect, useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminCmsSection, AdminCmsSectionItem } from '@/models/admin/cms.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { CmsSectionItemsIcon } from '@/components/dashboard/DashboardIcons'
import { AdminField, adminInputClass, resolveAdminMutationError } from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminSectionItemService } from '@/services/adminSectionItemService'
import { adminSectionService } from '@/services/adminSectionService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function CmsSectionItemsPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()
  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminCmsSectionItem[]>([])
  const [sections, setSections] = useState<AdminCmsSection[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [sectionId, setSectionId] = useState('')
  const [title, setTitle] = useState('')
  const [priority, setPriority] = useState('0')
  const [url, setUrl] = useState('')
  const [description, setDescription] = useState('')
  const [isActive, setIsActive] = useState(true)

  const sectionTitleById = useMemo(() => new Map(sections.map((x) => [x.id, x.title])), [sections])

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.cms.sectionItems.authRequired'))
      setLoading(false)
      return
    }
    setLoading(true)
    try {
      const [sectionItems, sectionList] = await Promise.all([
        adminSectionItemService.getAll(accessToken, locale, { pageSize: 200, languageId: languageId ?? undefined }),
        adminSectionService.getAll(accessToken, locale, { pageSize: 100, languageId: languageId ?? undefined }),
      ])
      setItems(sectionItems.items)
      setSections(sectionList.items)
      if (!sectionId && sectionList.items[0]) setSectionId(sectionList.items[0].id)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.sectionItems.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, languageId, locale, sectionId, t])

  useEffect(() => { if (!languageLoading) void load() }, [languageLoading, load])

  const resetForm = () => {
    setTitle(''); setPriority('0'); setUrl(''); setDescription(''); setIsActive(true); setEditingId(null); setError(null)
    if (sections[0]) setSectionId(sections[0].id)
  }

  const handleSave = async () => {
    if (!accessToken || languageId == null || !title.trim() || !sectionId) {
      setError(t('dashboard.cms.sectionItems.validationRequired'))
      return
    }
    setSaving(true)
    try {
      const payload = {
        languageId,
        sectionId,
        title: title.trim(),
        priority: Number(priority) || 0,
        url: url.trim() || undefined,
        description: description.trim() || undefined,
        isActive,
      }
      if (mode === 'edit' && editingId) await adminSectionItemService.update(accessToken, locale, { ...payload, id: editingId })
      else await adminSectionItemService.create(accessToken, locale, payload)
      await load(); resetForm(); setMode('list')
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.sectionItems.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode !== 'list') {
    return (
      <div>
        <DashboardPageHeader title={t(mode === 'edit' ? 'dashboard.cms.sectionItems.editTitle' : 'dashboard.cms.sectionItems.createTitle')} icon={<CmsSectionItemsIcon size={22} />} />
        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5">
          <AdminField label={t('dashboard.cms.sectionItems.fieldSection')}>
            <select value={sectionId} onChange={(e) => setSectionId(e.target.value)} className={adminInputClass}>
              {sections.map((section) => <option key={section.id} value={section.id}>{section.title}</option>)}
            </select>
          </AdminField>
          <AdminField label={t('dashboard.cms.sectionItems.fieldTitle')}><input value={title} onChange={(e) => setTitle(e.target.value)} className={adminInputClass} /></AdminField>
          <AdminField label={t('dashboard.cms.sectionItems.fieldPriority')}><input value={priority} onChange={(e) => setPriority(e.target.value)} className={adminInputClass} dir="ltr" type="number" /></AdminField>
          <AdminField label={t('dashboard.cms.sectionItems.fieldUrl')}><input value={url} onChange={(e) => setUrl(e.target.value)} className={adminInputClass} dir="ltr" /></AdminField>
          <AdminField label={t('dashboard.cms.sectionItems.fieldDescription')}><textarea value={description} onChange={(e) => setDescription(e.target.value)} className={adminInputClass} rows={3} /></AdminField>
          {mode === 'edit' && <label className="flex items-center gap-2 text-sm"><input type="checkbox" checked={isActive} onChange={(e) => setIsActive(e.target.checked)} className="size-4" />{t('dashboard.cms.sectionItems.fieldActive')}</label>}
          {error && <p className="text-sm text-sale">{error}</p>}
          <div className="flex gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>{t('dashboard.cms.sectionItems.save')}</Button>
            <Button variant="secondary" onClick={() => { resetForm(); setMode('list') }}>{t('dashboard.cms.sectionItems.cancel')}</Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader title={t('dashboard.cms.sectionItems.title')} description={t('dashboard.cms.sectionItems.description')} icon={<CmsSectionItemsIcon size={22} />} action={<Button variant="warm" onClick={() => { resetForm(); setMode('create') }}>{t('dashboard.cms.sectionItems.add')}</Button>} />
      {loading || languageLoading ? <div className="flex justify-center py-16"><InlineLoading label={t('dashboard.cms.sectionItems.loading')} /></div> : items.length === 0 ? (
        <DashboardEmptyState icon={<CmsSectionItemsIcon size={28} />} title={t('dashboard.cms.sectionItems.emptyTitle')} message={error ?? t('dashboard.cms.sectionItems.emptyMessage')} />
      ) : (
        <ul className="divide-y divide-border rounded-sm border border-border">
          {items.map((item) => (
            <li key={item.id} className="flex items-center justify-between gap-4 px-4 py-3">
              <div>
                <p className="font-medium">{item.title}</p>
                <p className="text-xs text-text-muted">{sectionTitleById.get(item.sectionId) ?? item.sectionId} · {item.priority}</p>
              </div>
              <Button variant="secondary" size="sm" onClick={() => { setEditingId(item.id); setSectionId(item.sectionId); setTitle(item.title); setPriority(String(item.priority)); setUrl(item.url ?? ''); setDescription(item.description ?? ''); setIsActive(item.isActive); setMode('edit') }}>{t('dashboard.cms.sectionItems.edit')}</Button>
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
