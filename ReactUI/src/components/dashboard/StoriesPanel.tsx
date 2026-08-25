import { useEffect, useState, type ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { StoriesIcon } from '@/components/dashboard/DashboardIcons'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { LocalImage } from '@/components/ui/LocalImage'
import type { UserStoryDraft } from '@/models/stories/story.model'
import { useUserStoryMutations } from '@/hooks/useUserStoryMutations'
import { useUserStorySync } from '@/hooks/useUserStorySync'
import { catalogListingService } from '@/services/catalogListingService'
import { useUserStoryStore } from '@/stores/userStoryStore'
import { useSettingsStore } from '@/stores/settingsStore'

type PanelMode = 'list' | 'create' | 'edit'

const MAX_FILE_MB = 8

export function StoriesPanel() {
  useUserStorySync()

  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const stories = useUserStoryStore((s) => s.stories)
  const isLoading = useUserStoryStore((s) => s.isLoading)
  const {
    isSaving,
    mutationError,
    publishStory,
    updateStory,
    toggleStoryActive,
    deleteStory,
    uploadMedia,
    clearMutationError,
  } = useUserStoryMutations()

  const [mode, setMode] = useState<PanelMode>('list')
  const [editingId, setEditingId] = useState<string | null>(null)
  const [title, setTitle] = useState('')
  const [caption, setCaption] = useState('')
  const [productSlug, setProductSlug] = useState('')
  const [isActive, setIsActive] = useState(true)
  const [preview, setPreview] = useState<string | null>(null)
  const [mediaType, setMediaType] = useState<'image' | 'video'>('image')
  const [mediaPath, setMediaPath] = useState('')
  const [mediaAlt, setMediaAlt] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [products, setProducts] = useState<{ slug: string; title: string }[]>([])

  useEffect(() => {
    if (mode === 'list') return

    void catalogListingService.getListing('', locale).then((listing) => {
      const next = listing.products.slice(0, 40).map((item) => ({
        slug: item.slug,
        title: item.title,
      }))

      if (productSlug && !next.some((item) => item.slug === productSlug)) {
        next.unshift({ slug: productSlug, title: productSlug })
      }

      setProducts(next)
    })
  }, [locale, mode, productSlug])

  useEffect(() => {
    if (mode === 'list') {
      setEditingId(null)
      setTitle('')
      setCaption('')
      setProductSlug('')
      setIsActive(true)
      setPreview(null)
      setMediaPath('')
      setMediaAlt('')
      setMediaType('image')
      setError(null)
      clearMutationError()
    }
  }, [clearMutationError, mode])

  const resetToList = () => setMode('list')

  const startCreate = () => {
    clearMutationError()
    setEditingId(null)
    setTitle('')
    setCaption('')
    setProductSlug('')
    setIsActive(true)
    setPreview(null)
    setMediaPath('')
    setMediaAlt('')
    setMediaType('image')
    setError(null)
    setMode('create')
  }

  const startEdit = (story: UserStoryDraft) => {
    clearMutationError()
    setEditingId(story.id)
    setTitle(story.title)
    setCaption(story.caption)
    setProductSlug(story.productSlugs[0] ?? '')
    setIsActive(story.isActive)
    setPreview(story.mediaSrc)
    setMediaPath(story.mediaPath)
    setMediaAlt(story.mediaAlt)
    setMediaType(story.mediaType)
    setError(null)
    setMode('edit')
  }

  const handleFile = async (file: File | null) => {
    if (!file) return
    setError(null)

    if (file.size > MAX_FILE_MB * 1024 * 1024) {
      setError(t('dashboard.stories.fileTooLarge', { max: MAX_FILE_MB }))
      return
    }

    const isImage = file.type.startsWith('image/')
    const isVideo = file.type.startsWith('video/')
    if (!isImage && !isVideo) {
      setError(t('dashboard.stories.invalidFile'))
      return
    }

    try {
      const media = await uploadMedia(file)
      setMediaType(media.mediaType)
      setMediaPath(media.mediaPath)
      setMediaAlt(file.name)
      setPreview(media.mediaSrc)
    } catch {
      setError(t('dashboard.stories.uploadFailed'))
    }
  }

  const handleSave = async () => {
    const trimmedTitle = title.trim()
    if (!trimmedTitle || !mediaPath) {
      setError(t('dashboard.stories.validationRequired'))
      return
    }

    const payload = {
      title: trimmedTitle,
      caption: caption.trim(),
      mediaType,
      mediaPath,
      mediaAlt: mediaAlt || trimmedTitle,
      productSlug: productSlug || undefined,
      isActive,
    }

    const success =
      mode === 'edit' && editingId
        ? await updateStory(editingId, payload)
        : await publishStory(payload)

    if (success) resetToList()
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        <DashboardPageHeader
          title={
            mode === 'edit' ? t('dashboard.stories.editTitle') : t('dashboard.stories.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.stories.editDescription')
              : t('dashboard.stories.createDescription')
          }
          icon={<StoriesIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <Field label={t('dashboard.stories.fieldTitle')}>
            <input
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className={inputClass}
              placeholder={t('dashboard.stories.titlePlaceholder')}
            />
          </Field>

          <Field label={t('dashboard.stories.fieldCaption')}>
            <textarea
              value={caption}
              onChange={(e) => setCaption(e.target.value)}
              rows={3}
              className={inputClass}
              placeholder={t('dashboard.stories.captionPlaceholder')}
            />
          </Field>

          <Field label={t('dashboard.stories.fieldMedia')}>
            <label className="flex cursor-pointer flex-col items-center justify-center rounded-lg border-2 border-dashed border-border bg-surface px-4 py-8 transition-colors hover:border-warm hover:bg-warm-soft/30">
              {preview ? (
                mediaType === 'video' ? (
                  <video src={preview} className="max-h-48 w-full rounded-md object-cover" controls muted />
                ) : (
                  <img src={preview} alt="" className="max-h-48 w-full rounded-md object-cover" />
                )
              ) : (
                <>
                  <UploadIcon />
                  <span className="mt-3 text-sm font-medium text-text">
                    {t('dashboard.stories.uploadHint')}
                  </span>
                  <span className="text-xs text-text-muted">
                    {t('dashboard.stories.uploadFormats', { max: MAX_FILE_MB })}
                  </span>
                </>
              )}
              <input
                type="file"
                accept="image/*,video/*"
                className="sr-only"
                onChange={(e) => void handleFile(e.target.files?.[0] ?? null)}
                disabled={isSaving}
              />
            </label>
            {mode === 'edit' && preview && (
              <p className="mt-2 text-xs text-text-muted">{t('dashboard.stories.replaceMediaHint')}</p>
            )}
          </Field>

          <Field label={t('dashboard.stories.fieldProduct')}>
            <select
              value={productSlug}
              onChange={(e) => setProductSlug(e.target.value)}
              className={inputClass}
            >
              <option value="">{t('dashboard.stories.noProduct')}</option>
              {products.map((p) => (
                <option key={p.slug} value={p.slug}>
                  {p.title}
                </option>
              ))}
            </select>
          </Field>

          {mode === 'edit' && (
            <label className="flex items-center gap-2 text-sm text-text">
              <input
                type="checkbox"
                checked={isActive}
                onChange={(e) => setIsActive(e.target.checked)}
                className="size-4 rounded border-border text-warm focus:ring-warm"
              />
              {t('dashboard.stories.publishLive')}
            </label>
          )}

          {(error || mutationError) && (
            <p className="text-sm text-sale">{error ?? mutationError}</p>
          )}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={isSaving || !mediaPath}>
              {isSaving ? (
                <InlineLoading
                  label={
                    mode === 'edit' ? t('dashboard.stories.saveChanges') : t('dashboard.stories.publish')
                  }
                />
              ) : mode === 'edit' ? (
                t('dashboard.stories.saveChanges')
              ) : (
                t('dashboard.stories.publish')
              )}
            </Button>
            <Button variant="secondary" onClick={resetToList} disabled={isSaving}>
              {t('address.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.stories.title')}
        description={t('dashboard.stories.description')}
        icon={<StoriesIcon size={22} />}
        action={
          <Button variant="warm" onClick={startCreate}>
            {t('dashboard.stories.addStory')}
          </Button>
        }
      />

      {isLoading ? (
        <InlineLoading label={t('common.loading')} />
      ) : stories.length === 0 ? (
        <DashboardEmptyState
          icon={<StoriesIcon size={28} />}
          title={t('dashboard.stories.emptyTitle')}
          message={t('dashboard.stories.emptyMessage')}
          action={
            <Button variant="warm" onClick={startCreate}>
              {t('dashboard.stories.addStory')}
            </Button>
          }
        />
      ) : (
        <>
          {mutationError && <p className="mb-4 text-sm text-sale">{mutationError}</p>}
          <div className="grid gap-4 sm:grid-cols-2">
            {stories.map((story) => (
              <StoryManageCard
                key={story.id}
                story={story}
                disabled={isSaving}
                onEdit={() => startEdit(story)}
                onToggle={() => void toggleStoryActive(story)}
                onDelete={() => void deleteStory(story.id)}
              />
            ))}
          </div>
        </>
      )}
    </div>
  )
}

function StoryManageCard({
  story,
  disabled,
  onEdit,
  onToggle,
  onDelete,
}: {
  story: UserStoryDraft
  disabled: boolean
  onEdit: () => void
  onToggle: () => void
  onDelete: () => void
}) {
  const { t } = useTranslation()

  return (
    <article className="overflow-hidden rounded-sm border border-border bg-surface shadow-sm">
      <div className="relative aspect-[4/3] bg-surface-muted">
        {story.mediaType === 'video' ? (
          <video src={story.mediaSrc} className="size-full object-cover" muted />
        ) : (
          <LocalImage
            image={{ src: story.mediaSrc, alt: story.mediaAlt }}
            className="size-full object-cover"
          />
        )}
        <span
          className={`absolute start-3 top-3 rounded-full px-2.5 py-1 text-[10px] font-semibold uppercase tracking-wide ${
            story.isActive ? 'bg-accent/90 text-text-inverse' : 'bg-black/50 text-white'
          }`}
        >
          {story.isActive ? t('dashboard.stories.statusActive') : t('dashboard.stories.statusHidden')}
        </span>
      </div>

      <div className="p-4">
        <h3 className="font-semibold text-text">{story.title}</h3>
        {story.caption && (
          <p className="mt-1 line-clamp-2 text-sm text-text-muted">{story.caption}</p>
        )}
        <p className="mt-2 text-xs text-text-muted">
          {story.mediaType === 'video' ? t('dashboard.stories.typeVideo') : t('dashboard.stories.typeImage')}
        </p>

        <div className="mt-4 flex flex-wrap gap-2">
          <Button variant="secondary" className="py-2 text-xs" onClick={onEdit} disabled={disabled}>
            {t('dashboard.stories.edit')}
          </Button>
          <Button variant="secondary" className="py-2 text-xs" onClick={onToggle} disabled={disabled}>
            {story.isActive ? t('dashboard.stories.hide') : t('dashboard.stories.show')}
          </Button>
          <Button
            variant="ghost"
            className="py-2 text-xs text-sale hover:bg-sale/10"
            onClick={onDelete}
            disabled={disabled}
          >
            {t('dashboard.stories.delete')}
          </Button>
        </div>
      </div>
    </article>
  )
}

function Field({ label, children }: { label: string; children: ReactNode }) {
  return (
    <label className="block">
      <span className="text-sm font-medium text-text">{label}</span>
      <div className="mt-2">{children}</div>
    </label>
  )
}

const inputClass =
  'w-full rounded-lg border border-border bg-surface px-3 py-2.5 text-sm text-text outline-none transition-colors placeholder:text-text-muted focus:border-warm'

function UploadIcon() {
  return (
    <svg width="32" height="32" viewBox="0 0 32 32" fill="none" className="text-warm" aria-hidden>
      <path
        d="M16 22V8m0 0-5 5m5-5 5 5M6 24h20"
        stroke="currentColor"
        strokeWidth="1.6"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  )
}
