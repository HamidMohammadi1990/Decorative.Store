import { useEffect, useState, type ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { StoriesIcon } from '@/components/dashboard/DashboardIcons'
import { Button } from '@/components/ui/Button'
import { LocalImage } from '@/components/ui/LocalImage'
import type { UserStoryDraft } from '@/models/stories/story.model'
import { getProductsMock } from '@/data/mock'
import { readMediaFile, useUserStoryStore } from '@/stores/userStoryStore'
import { useSettingsStore } from '@/stores/settingsStore'

type PanelMode = 'list' | 'create'

const MAX_FILE_MB = 8

export function StoriesPanel() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const stories = useUserStoryStore((s) => s.stories)
  const addStory = useUserStoryStore((s) => s.addStory)
  const removeStory = useUserStoryStore((s) => s.removeStory)
  const toggleActive = useUserStoryStore((s) => s.toggleActive)

  const [mode, setMode] = useState<PanelMode>('list')
  const [title, setTitle] = useState('')
  const [caption, setCaption] = useState('')
  const [productSlug, setProductSlug] = useState('')
  const [preview, setPreview] = useState<string | null>(null)
  const [mediaType, setMediaType] = useState<'image' | 'video'>('image')
  const [mediaSrc, setMediaSrc] = useState('')
  const [mediaAlt, setMediaAlt] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [saving, setSaving] = useState(false)

  const products = getProductsMock(locale).slice(0, 12)

  useEffect(() => {
    if (mode === 'list') {
      setTitle('')
      setCaption('')
      setProductSlug('')
      setPreview(null)
      setMediaSrc('')
      setMediaAlt('')
      setError(null)
    }
  }, [mode])

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
      setSaving(true)
      const media = await readMediaFile(file)
      setMediaType(media.mediaType)
      setMediaSrc(media.mediaSrc)
      setMediaAlt(file.name)
      setPreview(media.mediaSrc)
    } catch {
      setError(t('common.error'))
    } finally {
      setSaving(false)
    }
  }

  const handlePublish = () => {
    const trimmedTitle = title.trim()
    if (!trimmedTitle || !mediaSrc) {
      setError(t('dashboard.stories.validationRequired'))
      return
    }

    addStory({
      title: trimmedTitle,
      caption: caption.trim(),
      mediaType,
      mediaSrc,
      mediaAlt: mediaAlt || trimmedTitle,
      productSlugs: productSlug ? [productSlug] : [],
    })
    setMode('list')
  }

  if (mode === 'create') {
    return (
      <div>
        <DashboardPageHeader
          title={t('dashboard.stories.createTitle')}
          description={t('dashboard.stories.createDescription')}
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
              />
            </label>
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

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={handlePublish} disabled={saving || !mediaSrc}>
              {t('dashboard.stories.publish')}
            </Button>
            <Button variant="secondary" onClick={() => setMode('list')}>
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
          <Button variant="warm" onClick={() => setMode('create')}>
            {t('dashboard.stories.addStory')}
          </Button>
        }
      />

      {stories.length === 0 ? (
        <DashboardEmptyState
          icon={<StoriesIcon size={28} />}
          title={t('dashboard.stories.emptyTitle')}
          message={t('dashboard.stories.emptyMessage')}
          action={
            <Button variant="warm" onClick={() => setMode('create')}>
              {t('dashboard.stories.addStory')}
            </Button>
          }
        />
      ) : (
        <div className="grid gap-4 sm:grid-cols-2">
          {stories.map((story) => (
            <StoryManageCard
              key={story.id}
              story={story}
              onToggle={() => toggleActive(story.id)}
              onDelete={() => removeStory(story.id)}
            />
          ))}
        </div>
      )}
    </div>
  )
}

function StoryManageCard({
  story,
  onToggle,
  onDelete,
}: {
  story: UserStoryDraft
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
            story.isActive
              ? 'bg-accent/90 text-text-inverse'
              : 'bg-black/50 text-white'
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
          <Button variant="secondary" className="py-2 text-xs" onClick={onToggle}>
            {story.isActive ? t('dashboard.stories.hide') : t('dashboard.stories.show')}
          </Button>
          <Button variant="ghost" className="py-2 text-xs text-sale hover:bg-sale/10" onClick={onDelete}>
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
