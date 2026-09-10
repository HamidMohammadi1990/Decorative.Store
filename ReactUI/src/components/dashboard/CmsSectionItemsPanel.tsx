import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminCmsSection, AdminCmsSectionItem } from '@/models/admin/cms.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { CmsSectionItemsIcon, EditIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminGridActions,
  AdminGridIconButton,
} from '@/components/dashboard/admin/AdminGridActions'
import { AdminField, adminInputClass, resolveAdminMutationError } from '@/components/dashboard/admin/adminFormShared'
import { AdminListSearchField } from '@/components/dashboard/admin/AdminListSearchField'
import {
  CmsAdminDescriptionField,
  CmsAdminDescriptionNote,
} from '@/components/dashboard/admin/CmsAdminDescriptionField'
import { CMS_ENTITY_ADMIN_HINT_FA } from '@/data/cmsAdminGuideFa'
import { AdminListGridHeader } from '@/components/dashboard/admin/AdminListGridHeader'
import { AdminRowNumber } from '@/components/dashboard/admin/AdminRowNumber'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminSectionItemService } from '@/services/adminSectionItemService'
import { adminSectionService } from '@/services/adminSectionService'
import {
  adminCmsMediaService,
  normalizeCmsImageStorageValue,
  resolveCmsImageSrc,
} from '@/services/adminCmsMediaService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

const MAX_FILE_MB = 5

function RelatedMetaLine({ label, value }: { label: string; value?: string | null }) {
  if (!value?.trim()) return null
  return (
    <span>
      <span className="font-medium text-text-muted">{label}:</span> {value}
    </span>
  )
}

export function CmsSectionItemsPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()
  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminCmsSectionItem[]>([])
  const [sections, setSections] = useState<AdminCmsSection[]>([])
  const [loading, setLoading] = useState(true)
  const [formLoading, setFormLoading] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [sectionId, setSectionId] = useState('')
  const [title, setTitle] = useState('')
  const [priority, setPriority] = useState('0')
  const [url, setUrl] = useState('')
  const [description, setDescription] = useState('')
  const [icon, setIcon] = useState('')
  const [imageFileName, setImageFileName] = useState('')
  const [imagePreview, setImagePreview] = useState('')
  const [uploading, setUploading] = useState(false)
  const [isActive, setIsActive] = useState(true)
  const [search, setSearch] = useState('')
  const [appliedSearch, setAppliedSearch] = useState('')
  const [filterSectionId, setFilterSectionId] = useState('')
  const [adminDescription, setAdminDescription] = useState('')

  const loadList = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.cms.sectionItems.authRequired'))
      setLoading(false)
      return
    }
    setLoading(true)
    try {
      const sectionItems = await adminSectionItemService.getAll(accessToken, locale, {
        pageSize: 200,
        languageId: languageId ?? undefined,
        title: appliedSearch.trim() || null,
        sectionId: filterSectionId || null,
      })
      setItems(sectionItems.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.sectionItems.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, appliedSearch, filterSectionId, languageId, locale, t])

  const loadFormOptions = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') return
    setFormLoading(true)
    try {
      const sectionList = await adminSectionService.getAll(accessToken, locale, {
        pageSize: 200,
        languageId: languageId ?? undefined,
      })
      setSections(sectionList.items)
      if (!sectionId && sectionList.items[0]) setSectionId(sectionList.items[0].id)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.sectionItems.loadFailed')))
    } finally {
      setFormLoading(false)
    }
  }, [accessToken, languageId, locale, sectionId, t])

  useEffect(() => {
    if (!languageLoading) void loadList()
  }, [languageLoading, loadList])

  useEffect(() => {
    if (!languageLoading && mode === 'list' && sections.length === 0) {
      void adminSectionService
        .getAll(accessToken!, locale, { pageSize: 200, languageId: languageId ?? undefined })
        .then((result) => setSections(result.items))
        .catch(() => setSections([]))
    }
  }, [accessToken, languageId, languageLoading, locale, mode, sections.length])

  useEffect(() => {
    if (mode !== 'list' && !languageLoading) void loadFormOptions()
  }, [languageLoading, loadFormOptions, mode])

  const resetForm = () => {
    setTitle(''); setPriority('0'); setUrl(''); setDescription(''); setAdminDescription(''); setIcon(''); setImageFileName(''); setImagePreview(''); setIsActive(true); setEditingId(null); setError(null)
    if (sections[0]) setSectionId(sections[0].id)
  }

  const handleImagePick = async (file: File | null) => {
    if (!file) return
    if (file.size > MAX_FILE_MB * 1024 * 1024) {
      setError(t('dashboard.cms.sectionItems.fileTooLarge', { max: MAX_FILE_MB }))
      return
    }
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.cms.sectionItems.authRequired'))
      return
    }

    setUploading(true)
    setError(null)
    try {
      const uploaded = await adminCmsMediaService.uploadSectionItemImage(accessToken, locale, file)
      setImageFileName(uploaded.storageValue)
      setImagePreview(uploaded.imageUrl)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.cms.sectionItems.uploadFailed')))
    } finally {
      setUploading(false)
    }
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
        icon: icon.trim() || undefined,
        url: url.trim() || undefined,
        description: description.trim() || undefined,
        adminDescription: adminDescription.trim() || undefined,
        imageUrl: imageFileName.trim() || undefined,
        isActive,
      }
      if (mode === 'edit' && editingId) await adminSectionItemService.update(accessToken, locale, { ...payload, id: editingId })
      else await adminSectionItemService.create(accessToken, locale, payload)
      await loadList(); resetForm(); setMode('list')
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
        {formLoading ? (
          <div className="flex justify-center py-16"><InlineLoading label={t('dashboard.cms.sectionItems.loading')} /></div>
        ) : (
          <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5">
            <AdminField label={t('dashboard.cms.sectionItems.fieldSection')}>
              <select value={sectionId} onChange={(e) => setSectionId(e.target.value)} className={adminInputClass}>
                {sections.map((section) => (
                  <option key={section.id} value={section.id}>
                    {section.title}{section.sectionTypeName ? ` · ${section.sectionTypeName}` : ''}
                  </option>
                ))}
              </select>
            </AdminField>
            <AdminField label={t('dashboard.cms.sectionItems.fieldTitle')}><input value={title} onChange={(e) => setTitle(e.target.value)} className={adminInputClass} /></AdminField>
            <AdminField label={t('dashboard.cms.sectionItems.fieldPriority')}><input value={priority} onChange={(e) => setPriority(e.target.value)} className={adminInputClass} dir="ltr" type="number" /></AdminField>
            <AdminField label={t('dashboard.cms.sectionItems.fieldIcon')}>
              <input
                value={icon}
                onChange={(e) => setIcon(e.target.value)}
                className={adminInputClass}
                dir="ltr"
                placeholder={t('dashboard.cms.sectionItems.fieldIconHint')}
              />
            </AdminField>
            <AdminField label={t('dashboard.cms.sectionItems.fieldUrl')}><input value={url} onChange={(e) => setUrl(e.target.value)} className={adminInputClass} dir="ltr" /></AdminField>
            <AdminField label={t('dashboard.cms.sectionItems.fieldImage')}>
              <div className="space-y-3">
                {imagePreview && (
                  <img src={imagePreview} alt="" className="h-40 w-full rounded-sm border border-border object-cover" />
                )}
                <input
                  type="file"
                  accept="image/*"
                  disabled={uploading}
                  onChange={(e) => void handleImagePick(e.target.files?.[0] ?? null)}
                  className="block w-full text-sm text-text-muted file:me-3 file:rounded-sm file:border-0 file:bg-warm-soft file:px-3 file:py-2 file:text-sm file:font-medium file:text-warm"
                />
                {uploading && <InlineLoading label={t('dashboard.cms.sectionItems.uploading')} />}
                <p className="text-xs text-text-muted">{t('dashboard.cms.sectionItems.fieldImageHint')}</p>
              </div>
            </AdminField>
            <AdminField label={t('dashboard.cms.sectionItems.fieldDescription')}><textarea value={description} onChange={(e) => setDescription(e.target.value)} className={adminInputClass} rows={3} /></AdminField>
            <CmsAdminDescriptionField
              label={t('dashboard.cms.common.fieldAdminDescription')}
              hint={CMS_ENTITY_ADMIN_HINT_FA.sectionItem}
              value={adminDescription}
              onChange={setAdminDescription}
            />
            {mode === 'edit' && <label className="flex items-center gap-2 text-sm"><input type="checkbox" checked={isActive} onChange={(e) => setIsActive(e.target.checked)} className="size-4" />{t('dashboard.cms.sectionItems.fieldActive')}</label>}
            {error && <p className="text-sm text-sale">{error}</p>}
            <div className="flex gap-3">
              <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>{t('dashboard.cms.sectionItems.save')}</Button>
              <Button variant="secondary" onClick={() => { resetForm(); setMode('list') }}>{t('dashboard.cms.sectionItems.cancel')}</Button>
            </div>
          </div>
        )}
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader title={t('dashboard.cms.sectionItems.title')} description={t('dashboard.cms.sectionItems.description')} icon={<CmsSectionItemsIcon size={22} />} action={<Button variant="warm" onClick={() => { resetForm(); setMode('create') }}>{t('dashboard.cms.sectionItems.add')}</Button>} />
      <div className="mb-5 grid gap-3 sm:grid-cols-2">
        <AdminListSearchField
          label={t('dashboard.cms.common.search')}
          placeholder={t('dashboard.cms.sectionItems.searchPlaceholder')}
          value={search}
          onChange={setSearch}
          onApply={() => setAppliedSearch(search)}
        />
        <AdminField label={t('dashboard.cms.sectionItems.fieldSection')}>
          <select
            value={filterSectionId}
            onChange={(e) => setFilterSectionId(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.cms.common.allSections')}</option>
            {sections.map((section) => (
              <option key={section.id} value={section.id}>
                {section.title}{section.sectionTypeName ? ` · ${section.sectionTypeName}` : ''}
              </option>
            ))}
          </select>
        </AdminField>
      </div>
      {loading || languageLoading ? <div className="flex justify-center py-16"><InlineLoading label={t('dashboard.cms.sectionItems.loading')} /></div> : items.length === 0 ? (
        <DashboardEmptyState icon={<CmsSectionItemsIcon size={28} />} title={t('dashboard.cms.sectionItems.emptyTitle')} message={error ?? t('dashboard.cms.sectionItems.emptyMessage')} />
      ) : (
        <ul className="divide-y divide-border rounded-sm border border-border">
          <AdminListGridHeader />
          {items.map((item, index) => (
            <li key={item.id} className="flex items-center gap-3 px-4 py-3">
              <AdminRowNumber value={index + 1} />
              <div className="flex-1 min-w-0">
                <p className="font-medium truncate">{item.title}</p>
                <p className="mt-1 flex flex-wrap gap-x-3 gap-y-1 text-xs text-text-muted">
                  <RelatedMetaLine label={t('dashboard.cms.sectionItems.colSection')} value={item.sectionTitle} />
                  <RelatedMetaLine label={t('dashboard.cms.sectionItems.colSectionType')} value={item.sectionTypeName} />
                  <span>{t('dashboard.cms.sectionItems.colPriority')}: {item.priority}</span>
                  {item.icon && <RelatedMetaLine label={t('dashboard.cms.sectionItems.colIcon')} value={item.icon} />}
                </p>
                <CmsAdminDescriptionNote text={item.adminDescription} />
              </div>
              <AdminGridActions>
                <AdminGridIconButton
                  label={t('dashboard.cms.sectionItems.edit')}
                  icon={<EditIcon size={15} />}
                  onClick={() => {
                    setEditingId(item.id)
                    setSectionId(item.sectionId)
                    setTitle(item.title)
                    setPriority(String(item.priority))
                    setUrl(item.url ?? '')
                    setDescription(item.description ?? '')
                    setAdminDescription(item.adminDescription ?? '')
                    setIcon(item.icon ?? '')
                    setImageFileName(normalizeCmsImageStorageValue(item.imageUrl))
                    setImagePreview(item.imageUrl ? resolveCmsImageSrc(item.imageUrl) : '')
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
