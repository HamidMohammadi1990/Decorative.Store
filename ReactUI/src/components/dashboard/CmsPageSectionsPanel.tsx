import { useCallback, useEffect, useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminCmsPage, AdminCmsPageSection, AdminCmsSection } from '@/models/admin/cms.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { CmsPageSectionsIcon } from '@/components/dashboard/DashboardIcons'
import { AdminField, adminInputClass, resolveAdminMutationError } from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminPageSectionService } from '@/services/adminPageSectionService'
import { adminPageService } from '@/services/adminPageService'
import { adminSectionService } from '@/services/adminSectionService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function CmsPageSectionsPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()
  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminCmsPageSection[]>([])
  const [pages, setPages] = useState<AdminCmsPage[]>([])
  const [sections, setSections] = useState<AdminCmsSection[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [pageId, setPageId] = useState('')
  const [sectionId, setSectionId] = useState('')
  const [priority, setPriority] = useState('0')

  const pageTitleById = useMemo(() => new Map(pages.map((x) => [x.id, x.title])), [pages])
  const sectionTitleById = useMemo(() => new Map(sections.map((x) => [x.id, x.title])), [sections])

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.cms.pageSections.authRequired'))
      setLoading(false)
      return
    }
    setLoading(true)
    try {
      const [pageSections, pageList, sectionList] = await Promise.all([
        adminPageSectionService.getAll(accessToken, locale, { pageSize: 200 }),
        adminPageService.getAll(accessToken, locale, { pageSize: 100, languageId: languageId ?? undefined }),
        adminSectionService.getAll(accessToken, locale, { pageSize: 100, languageId: languageId ?? undefined }),
      ])
      setItems(pageSections.items)
      setPages(pageList.items)
      setSections(sectionList.items)
      if (!pageId && pageList.items[0]) setPageId(pageList.items[0].id)
      if (!sectionId && sectionList.items[0]) setSectionId(sectionList.items[0].id)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.pageSections.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, languageId, locale, pageId, sectionId, t])

  useEffect(() => { if (!languageLoading) void load() }, [languageLoading, load])

  const resetForm = () => {
    setPriority('0'); setEditingId(null); setError(null)
    if (pages[0]) setPageId(pages[0].id)
    if (sections[0]) setSectionId(sections[0].id)
  }

  const handleSave = async () => {
    if (!accessToken || !pageId || !sectionId) {
      setError(t('dashboard.cms.pageSections.validationRequired'))
      return
    }
    setSaving(true)
    try {
      const payload = { pageId, sectionId, priority: Number(priority) || 0 }
      if (mode === 'edit' && editingId) await adminPageSectionService.update(accessToken, locale, { ...payload, id: editingId })
      else await adminPageSectionService.create(accessToken, locale, payload)
      await load(); resetForm(); setMode('list')
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.pageSections.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode !== 'list') {
    return (
      <div>
        <DashboardPageHeader title={t(mode === 'edit' ? 'dashboard.cms.pageSections.editTitle' : 'dashboard.cms.pageSections.createTitle')} icon={<CmsPageSectionsIcon size={22} />} />
        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5">
          <AdminField label={t('dashboard.cms.pageSections.fieldPage')}>
            <select value={pageId} onChange={(e) => setPageId(e.target.value)} className={adminInputClass}>
              {pages.map((page) => <option key={page.id} value={page.id}>{page.title}</option>)}
            </select>
          </AdminField>
          <AdminField label={t('dashboard.cms.pageSections.fieldSection')}>
            <select value={sectionId} onChange={(e) => setSectionId(e.target.value)} className={adminInputClass}>
              {sections.map((section) => <option key={section.id} value={section.id}>{section.title}</option>)}
            </select>
          </AdminField>
          <AdminField label={t('dashboard.cms.pageSections.fieldPriority')}><input value={priority} onChange={(e) => setPriority(e.target.value)} className={adminInputClass} dir="ltr" type="number" /></AdminField>
          {error && <p className="text-sm text-sale">{error}</p>}
          <div className="flex gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>{t('dashboard.cms.pageSections.save')}</Button>
            <Button variant="secondary" onClick={() => { resetForm(); setMode('list') }}>{t('dashboard.cms.pageSections.cancel')}</Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader title={t('dashboard.cms.pageSections.title')} description={t('dashboard.cms.pageSections.description')} icon={<CmsPageSectionsIcon size={22} />} action={<Button variant="warm" onClick={() => { resetForm(); setMode('create') }}>{t('dashboard.cms.pageSections.add')}</Button>} />
      {loading || languageLoading ? <div className="flex justify-center py-16"><InlineLoading label={t('dashboard.cms.pageSections.loading')} /></div> : items.length === 0 ? (
        <DashboardEmptyState icon={<CmsPageSectionsIcon size={28} />} title={t('dashboard.cms.pageSections.emptyTitle')} message={error ?? t('dashboard.cms.pageSections.emptyMessage')} />
      ) : (
        <ul className="divide-y divide-border rounded-sm border border-border">
          {items.map((item) => (
            <li key={item.id} className="flex items-center justify-between gap-4 px-4 py-3">
              <div>
                <p className="font-medium">{pageTitleById.get(item.pageId) ?? item.pageId}</p>
                <p className="text-xs text-text-muted">{sectionTitleById.get(item.sectionId) ?? item.sectionId} · {item.priority}</p>
              </div>
              <Button variant="secondary" size="sm" onClick={() => { setEditingId(item.id); setPageId(item.pageId); setSectionId(item.sectionId); setPriority(String(item.priority)); setMode('edit') }}>{t('dashboard.cms.pageSections.edit')}</Button>
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
