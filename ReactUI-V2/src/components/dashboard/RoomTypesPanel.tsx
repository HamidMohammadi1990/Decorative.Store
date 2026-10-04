import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { AdminRoomType } from '@/models/admin/roomType.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { RoomTypeIcon, EditIcon, DeleteIcon } from '@/components/dashboard/DashboardIcons'
import { AdminDataGrid } from '@/components/dashboard/admin/AdminDataGrid'
import {
  AdminGridActions,
  AdminGridIconButton,
} from '@/components/dashboard/admin/AdminGridActions'
import { AdminContentLanguageField } from '@/components/dashboard/admin/AdminContentLanguageField'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useAdminContentLanguage } from '@/hooks/useAdminContentLanguage'
import { useAdminPagedList } from '@/hooks/useAdminPagedList'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminRoomTypeService } from '@/services/roomTypeService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

const MAX_FILE_MB = 5

export function RoomTypesPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()
  const {
    contentLanguageId,
    setContentLanguageId,
    languages: formLanguages,
    loading: contentLanguageLoading,
  } = useAdminContentLanguage()

  const [mode, setMode] = useState<Mode>('list')
  const [saving, setSaving] = useState(false)
  const [uploading, setUploading] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [code, setCode] = useState('')
  const [title, setTitle] = useState('')
  const [imageFileName, setImageFileName] = useState('')
  const [imagePreview, setImagePreview] = useState('')
  const [priority, setPriority] = useState(0)
  const [isActive, setIsActive] = useState(true)

  const canLoad =
    !languageLoading &&
    !contentLanguageLoading &&
    contentLanguageId != null &&
    Boolean(accessToken) &&
    accessToken !== 'mock-access-token'

  const fetchPage = useCallback(
    async (pageNumber: number, pageSize: number) => {
      if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
        throw new Error(t('dashboard.roomTypes.authRequired'))
      }
      return adminRoomTypeService.getAll(accessToken, locale, {
        pageNumber,
        pageSize,
        languageId: contentLanguageId,
      })
    },
    [accessToken, contentLanguageId, locale, t],
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
    reload,
  } = useAdminPagedList<AdminRoomType>({
    fetchPage,
    initialPageSize: 20,
    enabled: canLoad && mode === 'list',
  })

  useEffect(() => {
    return () => {
      if (imagePreview.startsWith('blob:')) URL.revokeObjectURL(imagePreview)
    }
  }, [imagePreview])

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.roomTypes.loadFailed'))
    : null

  const resetForm = () => {
    setCode('')
    setTitle('')
    setImageFileName('')
    setImagePreview('')
    setPriority(0)
    setIsActive(true)
    setEditingId(null)
    setFormError(null)
  }

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const openCreate = () => {
    resetForm()
    setMode('create')
  }

  const openEdit = (item: AdminRoomType) => {
    setEditingId(item.id)
    setCode(item.code)
    setTitle(item.title ?? '')
    setImageFileName(item.imageFileName)
    setImagePreview(item.imageUrl)
    setPriority(item.priority)
    setIsActive(item.isActive)
    setFormError(null)
    setMode('edit')
  }

  const handleImagePick = async (file: File | null) => {
    if (!file) return
    if (file.size > MAX_FILE_MB * 1024 * 1024) {
      setFormError(t('dashboard.roomTypes.fileTooLarge', { max: MAX_FILE_MB }))
      return
    }
    if (!accessToken || accessToken === 'mock-access-token') {
      setFormError(t('dashboard.roomTypes.authRequired'))
      return
    }

    setUploading(true)
    setFormError(null)
    try {
      const uploaded = await adminRoomTypeService.uploadImage(accessToken, locale, file)
      setImageFileName(uploaded.imageFileName)
      setImagePreview(uploaded.imageUrl)
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.roomTypes.uploadFailed')))
    } finally {
      setUploading(false)
    }
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || contentLanguageId == null) {
      setFormError(t('dashboard.roomTypes.authRequired'))
      return
    }
    if (!code.trim() || !title.trim() || !imageFileName.trim()) {
      setFormError(t('dashboard.roomTypes.validationRequired'))
      return
    }

    setSaving(true)
    setFormError(null)
    try {
      if (mode === 'create') {
        await adminRoomTypeService.create(accessToken, locale, {
          languageId: contentLanguageId,
          code: code.trim(),
          title: title.trim(),
          imageFileName: imageFileName.trim(),
          priority,
        })
      } else if (editingId) {
        await adminRoomTypeService.update(accessToken, locale, {
          id: editingId,
          languageId: contentLanguageId,
          code: code.trim(),
          title: title.trim(),
          imageFileName: imageFileName.trim(),
          priority,
          isActive,
        })
      }
      backToList()
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.roomTypes.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (item: AdminRoomType) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!window.confirm(t('dashboard.roomTypes.deleteConfirm'))) return

    try {
      await adminRoomTypeService.delete(accessToken, item.id)
      reload()
    } catch (err) {
      setFormError(resolveAdminMutationError(err, t('dashboard.roomTypes.deleteFailed')))
    }
  }

  if (mode !== 'list') {
    return (
      <div>
        <DashboardPageHeader
          title={t(mode === 'edit' ? 'dashboard.roomTypes.editTitle' : 'dashboard.roomTypes.createTitle')}
          icon={<RoomTypeIcon size={22} />}
        />
        <div className="mx-auto max-w-xl space-y-4 rounded-sm border border-border bg-surface p-5">
          <AdminContentLanguageField
            languages={formLanguages}
            value={contentLanguageId}
            onChange={setContentLanguageId}
          />

          <AdminField label={t('dashboard.roomTypes.fieldCode')}>
            <input
              value={code}
              onChange={(e) => setCode(e.target.value)}
              className={adminInputClass}
              dir="ltr"
              placeholder="living"
            />
          </AdminField>

          <AdminField label={t('dashboard.roomTypes.fieldTitle')}>
            <input value={title} onChange={(e) => setTitle(e.target.value)} className={adminInputClass} />
          </AdminField>

          <AdminField label={t('dashboard.roomTypes.fieldImage')}>
            <div className="space-y-3">
              {imagePreview && (
                <img
                  src={imagePreview}
                  alt=""
                  className="h-32 w-full rounded-sm border border-border object-cover"
                />
              )}
              <input
                type="file"
                accept="image/*"
                disabled={uploading}
                onChange={(e) => void handleImagePick(e.target.files?.[0] ?? null)}
                className="block w-full text-sm text-text-muted file:me-3 file:rounded-sm file:border-0 file:bg-warm-soft file:px-3 file:py-2 file:text-sm file:font-medium file:text-warm"
              />
              {uploading && <InlineLoading label={t('dashboard.roomTypes.uploading')} />}
            </div>
          </AdminField>

          <AdminField label={t('dashboard.roomTypes.fieldPriority')}>
            <input
              value={priority}
              onChange={(e) => setPriority(Number(e.target.value) || 0)}
              className={adminInputClass}
              dir="ltr"
              type="number"
            />
          </AdminField>

          {mode === 'edit' && (
            <label className="flex items-center gap-2 text-sm">
              <input
                type="checkbox"
                checked={isActive}
                onChange={(e) => setIsActive(e.target.checked)}
                className="size-4"
              />
              {t('dashboard.roomTypes.fieldActive')}
            </label>
          )}

          {formError && <p className="text-sm text-sale">{formError}</p>}

          <div className="flex flex-wrap gap-2 pt-2">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving || uploading}>
              {saving ? t('dashboard.roomTypes.saving') : t('dashboard.roomTypes.save')}
            </Button>
            <Button variant="secondary" onClick={backToList}>
              {t('dashboard.roomTypes.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.roomTypes.title')}
        description={t('dashboard.roomTypes.description')}
        icon={<RoomTypeIcon size={22} />}
        action={
          <Button variant="warm" onClick={openCreate}>
            {t('dashboard.roomTypes.add')}
          </Button>
        }
      />

      {!canLoad ? (
        <DashboardEmptyState
          icon={<RoomTypeIcon size={28} />}
          title={t('dashboard.roomTypes.emptyTitle')}
          message={t('dashboard.roomTypes.authRequired')}
        />
      ) : listLoading && items.length === 0 ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.roomTypes.loading')} />
        </div>
      ) : listErrorMessage && items.length === 0 ? (
        <DashboardEmptyState
          icon={<RoomTypeIcon size={28} />}
          title={t('dashboard.roomTypes.emptyTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.assistantFaq.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<RoomTypeIcon size={28} />}
          title={t('dashboard.roomTypes.emptyTitle')}
          message={t('dashboard.roomTypes.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.roomTypes.add')}
            </Button>
          }
        />
      ) : (
        <AdminDataGrid
          rows={items}
          rowKey={(item) => item.id}
          loading={listLoading}
          loadingLabel={t('dashboard.roomTypes.loading')}
          pagination={{
            pageNumber,
            pageSize,
            totalCount,
            totalPages,
            onPageChange: goToPage,
          }}
          columns={[
            {
              id: 'preview',
              header: t('dashboard.roomTypes.fieldImage'),
              cell: (item) => (
                <img
                  src={item.imageUrl}
                  alt=""
                  className="size-14 rounded-sm border border-border object-cover"
                />
              ),
            },
            {
              id: 'title',
              header: t('dashboard.roomTypes.fieldTitle'),
              cell: (item) => (
                <div>
                  <p className="font-medium text-text">{item.title ?? '—'}</p>
                  <p className="text-xs text-text-muted" dir="ltr">
                    {item.code}
                  </p>
                </div>
              ),
            },
            {
              id: 'priority',
              header: t('dashboard.roomTypes.colPriority'),
              cell: (item) => item.priority,
            },
            {
              id: 'status',
              header: t('dashboard.roomTypes.fieldActive'),
              cell: (item) => (
                <span
                  className={`inline-flex rounded-full px-2 py-0.5 text-xs font-medium ${
                    item.isActive ? 'bg-emerald-100 text-emerald-800' : 'bg-surface-muted text-text-muted'
                  }`}
                >
                  {item.isActive ? t('dashboard.assistantFaq.statusActive') : t('dashboard.roomTypes.inactive')}
                </span>
              ),
            },
            {
              id: 'actions',
              header: t('dashboard.assistantFaq.colActions'),
              cell: (item) => (
                <AdminGridActions>
                  <AdminGridIconButton
                    label={t('dashboard.roomTypes.edit')}
                    icon={<EditIcon size={16} />}
                    onClick={() => openEdit(item)}
                  />
                  <AdminGridIconButton
                    label={t('dashboard.roomTypes.delete')}
                    icon={<DeleteIcon size={16} />}
                    onClick={() => void handleDelete(item)}
                  />
                </AdminGridActions>
              ),
            },
          ]}
        />
      )}
    </div>
  )
}
