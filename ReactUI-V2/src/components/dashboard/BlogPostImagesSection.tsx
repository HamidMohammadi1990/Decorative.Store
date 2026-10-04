import { useCallback, useEffect, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import type { AdminBlogPostFile } from '@/models/admin/blog.model'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { AdminListGridHeader } from '@/components/dashboard/admin/AdminListGridHeader'
import { AdminRowNumber } from '@/components/dashboard/admin/AdminRowNumber'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminBlogPostFileService } from '@/services/adminBlogPostFileService'
import { useUserStore } from '@/stores/userStore'

const MAX_FILE_MB = 5

interface BlogPostImagesSectionProps {
  blogPostId: string
  blogPostTitle?: string
}

export function BlogPostImagesSection({ blogPostId, blogPostTitle }: BlogPostImagesSectionProps) {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [items, setItems] = useState<AdminBlogPostFile[]>([])
  const [loading, setLoading] = useState(true)
  const [uploading, setUploading] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const [title, setTitle] = useState('')
  const [isMain, setIsMain] = useState(false)
  const [pendingFiles, setPendingFiles] = useState<File[]>([])
  const [previews, setPreviews] = useState<string[]>([])

  useEffect(() => {
    return () => {
      previews.forEach((url) => URL.revokeObjectURL(url))
    }
  }, [previews])

  const loadImages = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token' || !blogPostId) {
      setItems([])
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const result = await adminBlogPostFileService.getAll(accessToken, locale, {
        blogPostId,
        pageSize: 100,
      })
      setItems(result.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPosts.images.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, blogPostId, locale, t])

  const loadImagesRef = useRef(loadImages)
  loadImagesRef.current = loadImages

  useEffect(() => {
    if (languageLoading || !blogPostId) return
    void loadImagesRef.current()
  }, [accessToken, blogPostId, languageLoading, locale])

  const handleFileChange = (fileList: FileList | null) => {
    previews.forEach((url) => URL.revokeObjectURL(url))

    if (!fileList || fileList.length === 0) {
      setPendingFiles([])
      setPreviews([])
      return
    }

    const files = Array.from(fileList).filter((file) => file.type.startsWith('image/'))
    setPendingFiles(files)
    setPreviews(files.map((file) => URL.createObjectURL(file)))
  }

  const handleUpload = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || languageId == null) {
      setError(t('dashboard.blogPosts.images.saveFailed'))
      return
    }

    if (!blogPostId || pendingFiles.length === 0) {
      setError(t('dashboard.blogPosts.images.validationRequired'))
      return
    }

    const tooLarge = pendingFiles.some((file) => file.size > MAX_FILE_MB * 1024 * 1024)
    if (tooLarge) {
      setError(t('dashboard.blogPosts.images.fileTooLarge', { max: MAX_FILE_MB }))
      return
    }

    setUploading(true)
    setError(null)
    try {
      await adminBlogPostFileService.createRange(
        accessToken,
        locale,
        pendingFiles.map((file, index) => ({
          blogPostId,
          languageId,
          title: title.trim() || file.name.replace(/\.[^.]+$/, ''),
          image: file,
          isIndex: isMain && index === 0,
        })),
      )
      setTitle('')
      setIsMain(false)
      setPendingFiles([])
      previews.forEach((url) => URL.revokeObjectURL(url))
      setPreviews([])
      await loadImages()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPosts.images.saveFailed')))
    } finally {
      setUploading(false)
    }
  }

  const handleToggleStatus = async (item: AdminBlogPostFile) => {
    if (!accessToken || accessToken === 'mock-access-token') return

    setSaving(true)
    setError(null)
    try {
      await adminBlogPostFileService.updateStatus(accessToken, locale, item.id, !item.isActive)
      await loadImages()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPosts.images.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleSetMain = async (item: AdminBlogPostFile) => {
    if (!accessToken || accessToken === 'mock-access-token' || item.isMain) return

    setSaving(true)
    setError(null)
    try {
      await adminBlogPostFileService.setMain(accessToken, locale, item.id)
      await loadImages()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPosts.images.setMainFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!(await confirm({ message: t('dashboard.blogPosts.images.deleteConfirm') }))) return

    setSaving(true)
    setError(null)
    try {
      await adminBlogPostFileService.delete(accessToken, id)
      await loadImages()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPosts.images.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  return (
    <div className="space-y-5 rounded-sm border border-border bg-surface p-5 shadow-sm sm:p-6">
      <div>
        <h2 className="text-base font-semibold text-text">{t('dashboard.blogPosts.images.title')}</h2>
        <p className="mt-1 text-sm text-text-muted">
          {blogPostTitle
            ? t('dashboard.blogPosts.images.selectedPost', { title: blogPostTitle })
            : t('dashboard.blogPosts.images.description')}
        </p>
      </div>

      <AdminField label={t('dashboard.blogPosts.images.fieldTitle')}>
        <input
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          className={adminInputClass}
          placeholder={t('dashboard.blogPosts.images.titlePlaceholder')}
        />
      </AdminField>

      <AdminField label={t('dashboard.blogPosts.images.fieldFiles')}>
        <label className="flex cursor-pointer flex-col items-center justify-center rounded-lg border-2 border-dashed border-border bg-surface-muted/20 px-4 py-8 transition-colors hover:border-warm hover:bg-warm-soft/30">
          {previews.length > 0 ? (
            <div className="grid w-full grid-cols-2 gap-3 sm:grid-cols-3 md:grid-cols-4">
              {previews.map((preview) => (
                <img
                  key={preview}
                  src={preview}
                  alt=""
                  className="aspect-square w-full rounded-md object-cover"
                />
              ))}
            </div>
          ) : (
            <>
              <span className="text-sm font-medium text-text">
                {t('dashboard.blogPosts.images.uploadHint')}
              </span>
              <span className="mt-1 text-xs text-text-muted">
                {t('dashboard.blogPosts.images.uploadFormats', { max: MAX_FILE_MB })}
              </span>
            </>
          )}
          <input
            type="file"
            accept="image/*"
            multiple
            className="sr-only"
            onChange={(e) => handleFileChange(e.target.files)}
            disabled={uploading}
          />
        </label>
      </AdminField>

      <label className="flex items-center gap-2 text-sm text-text">
        <input
          type="checkbox"
          checked={isMain}
          onChange={(e) => setIsMain(e.target.checked)}
          className="size-4 rounded border-border text-warm focus:ring-warm"
        />
        {t('dashboard.blogPosts.images.fieldMain')}
      </label>

      {error && <p className="text-sm text-sale">{error}</p>}

      <Button
        variant="warm"
        onClick={() => void handleUpload()}
        disabled={uploading || saving || pendingFiles.length === 0}
      >
        {uploading ? (
          <InlineLoading label={t('dashboard.blogPosts.images.uploading')} />
        ) : (
          t('dashboard.blogPosts.images.upload')
        )}
      </Button>

      {loading || languageLoading ? (
        <div className="flex justify-center py-8">
          <InlineLoading label={t('dashboard.blogPosts.images.loading')} />
        </div>
      ) : items.length === 0 ? (
        <p className="rounded-sm border border-dashed border-border px-4 py-8 text-center text-sm text-text-muted">
          {t('dashboard.blogPosts.images.emptyMessage')}
        </p>
      ) : (
        <div className="space-y-3">
          <p className="text-xs text-text-muted">
            {t('dashboard.blogPosts.images.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            <AdminListGridHeader />
            {items.map((item, index) => (
              <li key={item.id} className="flex flex-wrap items-center gap-3 px-4 py-3 sm:px-5">
                <AdminRowNumber value={index + 1} />
                <div className="size-12 shrink-0 overflow-hidden rounded-sm bg-surface-muted">
                  <img
                    src={item.imageUrl}
                    alt={item.title || item.blogPostTitle}
                    className="size-full object-cover"
                  />
                </div>
                <div className="min-w-0 flex-1">
                  <div className="flex items-start gap-2">
                    <label
                      className="mt-0.5 flex shrink-0 cursor-pointer items-center"
                      title={t('dashboard.blogPosts.images.setMainHint')}
                    >
                      <input
                        type="radio"
                        name={`blog-post-main-${blogPostId}`}
                        checked={item.isMain}
                        onChange={() => void handleSetMain(item)}
                        disabled={saving || item.isMain}
                        className="size-4 border-border text-warm focus:ring-warm"
                      />
                    </label>
                    <div className="min-w-0 flex-1">
                  <p className="truncate text-sm font-semibold text-text">
                    {item.title || item.fileName}
                  </p>
                  <div className="mt-1.5 flex flex-wrap gap-2">
                    {item.isMain && (
                      <span className="rounded-sm bg-warm-soft px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide text-warm">
                        {t('dashboard.blogPosts.images.mainBadge')}
                      </span>
                    )}
                    <span
                      className={`rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                        item.isActive
                          ? 'bg-warm-soft text-warm'
                          : 'bg-surface-muted text-text-muted'
                      }`}
                    >
                      {item.isActive
                        ? t('dashboard.blogPosts.images.statusActive')
                        : t('dashboard.blogPosts.images.statusInactive')}
                    </span>
                  </div>
                    </div>
                  </div>
                </div>
                <div className="flex flex-wrap items-center gap-2">
                  <Button
                    variant="secondary"
                    className="py-1.5 text-xs"
                    onClick={() => void handleToggleStatus(item)}
                    disabled={saving}
                  >
                    {item.isActive
                      ? t('dashboard.blogPosts.images.hide')
                      : t('dashboard.blogPosts.images.show')}
                  </Button>
                  <Button
                    variant="ghost"
                    className="py-1.5 text-xs text-sale hover:bg-sale/10"
                    onClick={() => void handleDelete(item.id)}
                    disabled={saving}
                  >
                    {t('dashboard.blogPosts.images.delete')}
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
