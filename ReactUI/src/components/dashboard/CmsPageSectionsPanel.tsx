import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminCmsPage, AdminCmsPageSection, AdminCmsSection } from '@/models/admin/cms.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { CmsPageSectionsIcon, EditIcon } from '@/components/dashboard/DashboardIcons'
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
import { adminPageSectionService } from '@/services/adminPageSectionService'
import { adminPageService } from '@/services/adminPageService'
import { adminSectionService } from '@/services/adminSectionService'
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

export function CmsPageSectionsPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()
  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminCmsPageSection[]>([])
  const [pages, setPages] = useState<AdminCmsPage[]>([])
  const [sections, setSections] = useState<AdminCmsSection[]>([])
  const [loading, setLoading] = useState(true)
  const [formLoading, setFormLoading] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [pageId, setPageId] = useState('')
  const [sectionId, setSectionId] = useState('')
  const [priority, setPriority] = useState('0')

  const loadList = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.cms.pageSections.authRequired'))
      setLoading(false)
      return
    }
    setLoading(true)
    try {
      const pageSections = await adminPageSectionService.getAll(accessToken, locale, { pageSize: 200 })
      setItems(pageSections.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.pageSections.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, locale, t])

  const loadFormOptions = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') return
    setFormLoading(true)
    try {
      const [pageList, sectionList] = await Promise.all([
        adminPageService.getAll(accessToken, locale, { pageSize: 200, languageId: languageId ?? undefined }),
        adminSectionService.getAll(accessToken, locale, { pageSize: 200, languageId: languageId ?? undefined }),
      ])
      setPages(pageList.items)
      setSections(sectionList.items)
      if (!pageId && pageList.items[0]) setPageId(pageList.items[0].id)
      if (!sectionId && sectionList.items[0]) setSectionId(sectionList.items[0].id)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.pageSections.loadFailed')))
    } finally {
      setFormLoading(false)
    }
  }, [accessToken, languageId, locale, pageId, sectionId, t])

  useEffect(() => { if (!languageLoading) void loadList() }, [languageLoading, loadList])

  useEffect(() => {
    if (mode !== 'list' && !languageLoading) void loadFormOptions()
  }, [languageLoading, loadFormOptions, mode])

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
      await loadList(); resetForm(); setMode('list')
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
        {formLoading ? (
          <div className="flex justify-center py-16"><InlineLoading label={t('dashboard.cms.pageSections.loading')} /></div>
        ) : (
          <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5">
            <AdminField label={t('dashboard.cms.pageSections.fieldPage')}>
              <select value={pageId} onChange={(e) => setPageId(e.target.value)} className={adminInputClass}>
                {pages.map((page) => <option key={page.id} value={page.id}>{page.title}{page.slug ? ` (${page.slug})` : ''}</option>)}
              </select>
            </AdminField>
            <AdminField label={t('dashboard.cms.pageSections.fieldSection')}>
              <select value={sectionId} onChange={(e) => setSectionId(e.target.value)} className={adminInputClass}>
                {sections.map((section) => (
                  <option key={section.id} value={section.id}>
                    {section.title}{section.sectionTypeName ? ` · ${section.sectionTypeName}` : ''}
                  </option>
                ))}
              </select>
            </AdminField>
            <AdminField label={t('dashboard.cms.pageSections.fieldPriority')}><input value={priority} onChange={(e) => setPriority(e.target.value)} className={adminInputClass} dir="ltr" type="number" /></AdminField>
            {error && <p className="text-sm text-sale">{error}</p>}
            <div className="flex gap-3">
              <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>{t('dashboard.cms.pageSections.save')}</Button>
              <Button variant="secondary" onClick={() => { resetForm(); setMode('list') }}>{t('dashboard.cms.pageSections.cancel')}</Button>
            </div>
          </div>
        )}
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
          <AdminListGridHeader />
          {items.map((item, index) => (
            <li key={item.id} className="flex items-center gap-3 px-4 py-3">
              <AdminRowNumber value={index + 1} />
              <div className="flex-1 min-w-0">
                <p className="font-medium truncate">
                  {item.pageTitle || item.pageId}
                  <span className="mx-2 text-text-muted">→</span>
                  {item.sectionTitle || item.sectionId}
                </p>
                <p className="mt-1 flex flex-wrap gap-x-3 gap-y-1 text-xs text-text-muted">
                  <RelatedMetaLine label={t('dashboard.cms.pageSections.colPageSlug')} value={item.pageSlug} />
                  <RelatedMetaLine label={t('dashboard.cms.pageSections.colSectionType')} value={item.sectionTypeName} />
                  <span>{t('dashboard.cms.pageSections.colPriority')}: {item.priority}</span>
                </p>
              </div>
              <AdminGridActions>
                <AdminGridIconButton
                  label={t('dashboard.cms.pageSections.edit')}
                  icon={<EditIcon size={15} />}
                  onClick={() => {
                    setEditingId(item.id)
                    setPageId(item.pageId)
                    setSectionId(item.sectionId)
                    setPriority(String(item.priority))
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
