import { useCallback, useEffect, useMemo, useRef, useState, type ReactNode, type TouchEvent } from 'react'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import {
  StoriesIcon,
  EditIcon,
  DeleteIcon,
  CommentIcon,
  EyeIcon,
  EyeOffIcon,
} from '@/components/dashboard/DashboardIcons'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { LocalImage } from '@/components/ui/LocalImage'
import { ScrollArrowButton } from '@/components/ui/ScrollArrowButton'
import type { UserStoryDraft } from '@/models/stories/story.model'
import { useUserStoryMutations } from '@/hooks/useUserStoryMutations'
import { useUserStorySync } from '@/hooks/useUserStorySync'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminProductService } from '@/services/adminProductService'
import { useUserStoryStore } from '@/stores/userStoryStore'
import { useUserStore } from '@/stores/userStore'
import {
  groupUserStoriesForDashboard,
  type DashboardStoryGroup,
} from '@/extensions/groupUserStoriesForDashboard'
import { StoryGroupCommentsModal } from '@/components/dashboard/StoryGroupCommentsModal'
import {
  AdminSearchableSelect,
  type AdminSearchableSelectOption,
} from '@/components/dashboard/admin/AdminSearchableSelect'
import {
  AdminGridIconButton,
} from '@/components/dashboard/admin/AdminGridActions'

const SWIPE_THRESHOLD_PX = 48

type PanelMode = 'list' | 'create' | 'edit'

const MAX_FILE_MB = 8

type StoryMediaDraft = {
  storyId?: string
  mediaType: 'image' | 'video'
  mediaPath: string
  mediaSrc: string
  mediaAlt: string
  posterSrc?: string
}

type StoryProductOption = {
  slug: string
  title: string
  productCode: string
}

export function StoriesPanel() {
  useUserStorySync()

  const { t } = useTranslation()
  const confirm = useConfirm()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, languageId } = useCurrentLanguageId()
  const stories = useUserStoryStore((s) => s.stories)
  const isLoading = useUserStoryStore((s) => s.isLoading)
  const {
    isSaving,
    mutationError,
    publishStories,
    updateStories,
    toggleStoryGroup,
    deleteStories,
    uploadMedia,
    clearMutationError,
  } = useUserStoryMutations()

  const groupedStories = useMemo(() => groupUserStoriesForDashboard(stories), [stories])

  const [mode, setMode] = useState<PanelMode>('list')
  const [editingId, setEditingId] = useState<string | null>(null)
  const [editingSlideIds, setEditingSlideIds] = useState<string[]>([])
  const [title, setTitle] = useState('')
  const [caption, setCaption] = useState('')
  const [productSlug, setProductSlug] = useState('')
  const [productSearch, setProductSearch] = useState('')
  const [isActive, setIsActive] = useState(true)
  const [mediaItems, setMediaItems] = useState<StoryMediaDraft[]>([])
  const [error, setError] = useState<string | null>(null)
  const [products, setProducts] = useState<StoryProductOption[]>([])
  const [productsLoading, setProductsLoading] = useState(false)
  const [commentsGroup, setCommentsGroup] = useState<DashboardStoryGroup | null>(null)

  useEffect(() => {
    if (mode === 'list') return
    if (!accessToken || accessToken === 'mock-access-token') return

    const timer = window.setTimeout(() => {
      setProductsLoading(true)
      void (async () => {
        try {
          const trimmed = productSearch.trim()
          const items = trimmed
            ? (
                await adminProductService.getAll(accessToken, locale, {
                  title: trimmed,
                  pageSize: 100,
                  languageId: languageId ?? undefined,
                })
              ).items
            : await adminProductService.getAllPages(accessToken, locale, {
                languageId: languageId ?? undefined,
              })

          setProducts(
            items
              .filter((item) => item.slug.trim())
              .map((item) => ({
                slug: item.slug,
                title: item.title,
                productCode: item.productCode,
              })),
          )
        } catch {
          setProducts([])
        } finally {
          setProductsLoading(false)
        }
      })()
    }, 300)

    return () => window.clearTimeout(timer)
  }, [accessToken, languageId, locale, mode, productSearch])

  const productSelectOptions = useMemo((): AdminSearchableSelectOption[] => {
    const options = products.map((item) => ({
      value: item.slug,
      label: item.title,
      hint: item.productCode || undefined,
    }))

    if (productSlug && !options.some((item) => item.value === productSlug)) {
      const selected = products.find((item) => item.slug === productSlug)
      options.unshift(
        selected
          ? { value: selected.slug, label: selected.title, hint: selected.productCode || undefined }
          : { value: productSlug, label: productSlug },
      )
    }

    return options
  }, [productSlug, products])

  useEffect(() => {
    if (mode === 'list') {
      setEditingId(null)
      setEditingSlideIds([])
      setTitle('')
      setCaption('')
      setProductSlug('')
      setProductSearch('')
      setIsActive(true)
      setMediaItems([])
      setError(null)
      clearMutationError()
    }
  }, [clearMutationError, mode])

  const resetToList = () => setMode('list')

  const startCreate = () => {
    clearMutationError()
    setEditingId(null)
    setEditingSlideIds([])
    setTitle('')
    setCaption('')
    setProductSlug('')
    setProductSearch('')
    setIsActive(true)
    setMediaItems([])
    setError(null)
    setMode('create')
  }

  const startEditGroup = (group: DashboardStoryGroup) => {
    clearMutationError()
    const slideIds = group.slides.map((slide) => slide.id)
    setEditingId(group.id)
    setEditingSlideIds(slideIds)
    setTitle(group.title)
    setCaption(group.caption)
    setProductSlug(group.productSlugs[0] ?? '')
    setProductSearch('')
    setIsActive(group.isActive)
    const items: StoryMediaDraft[] = group.slides.map((slide) => ({
      storyId: slide.id,
      mediaType: slide.mediaType,
      mediaPath: slide.mediaPath,
      mediaSrc: slide.mediaSrc,
      mediaAlt: slide.mediaAlt,
      posterSrc: slide.posterSrc,
    }))
    const first = items[0]
    setMediaItems(items)
    setError(null)
    setMode('edit')
  }

  const handleFiles = async (files: FileList | null) => {
    if (!files || files.length === 0) return
    setError(null)

    for (const file of files) {
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
        const item: StoryMediaDraft = {
          mediaType: media.mediaType,
          mediaPath: media.mediaPath,
          mediaAlt: file.name,
          mediaSrc: media.mediaSrc,
        }
        setMediaItems((prev) => [...prev, item])
      } catch {
        setError(t('dashboard.stories.uploadFailed'))
        return
      }
    }
  }

  const removeMediaItem = async (index: number) => {
    if (!(await confirm({ message: t('dashboard.stories.removeMediaConfirm') }))) return
    setMediaItems((prev) => prev.filter((_, i) => i !== index))
  }

  const handleSave = async () => {
    const trimmedTitle = title.trim()
    const hasMedia = mediaItems.length > 0
    if (!trimmedTitle || !hasMedia) {
      setError(t('dashboard.stories.validationRequired'))
      return
    }

    const product = productSlug || undefined

    if (mode === 'edit' && editingId) {
      const updates = mediaItems
        .filter((item) => item.storyId)
        .map((item) => ({
          id: item.storyId!,
          input: {
            title: trimmedTitle,
            caption: caption.trim(),
            mediaType: item.mediaType,
            mediaPath: item.mediaPath,
            mediaAlt: item.mediaAlt || trimmedTitle,
            productSlug: product,
            isActive,
          },
        }))

      const currentIds = new Set(
        mediaItems.map((item) => item.storyId).filter((id): id is string => Boolean(id)),
      )
      const removedIds = editingSlideIds.filter((id) => !currentIds.has(id))

      const newPayloads = mediaItems
        .filter((item) => !item.storyId)
        .map((item) => ({
          title: trimmedTitle,
          caption: caption.trim(),
          mediaType: item.mediaType,
          mediaPath: item.mediaPath,
          mediaAlt: item.mediaAlt || trimmedTitle,
          productSlug: product,
          isActive,
        }))

      let success = true
      if (updates.length > 0) {
        success = await updateStories(updates)
      }
      if (success && removedIds.length > 0) {
        success = await deleteStories(removedIds)
      }
      if (success && newPayloads.length > 0) {
        success = await publishStories(newPayloads)
      }
      if (success) resetToList()
      return
    }

    const payloads = mediaItems.map((item) => ({
      title: trimmedTitle,
      caption: caption.trim(),
      mediaType: item.mediaType,
      mediaPath: item.mediaPath,
      mediaAlt: item.mediaAlt || trimmedTitle,
      productSlug: product,
      isActive,
    }))

    const success = await publishStories(payloads)
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
            {mode === 'create' && (
              <p className="mb-2 text-xs text-text-muted">{t('dashboard.stories.multiMediaHint')}</p>
            )}
            {mode === 'edit' && (
              <p className="mb-2 text-xs text-text-muted">{t('dashboard.stories.multiMediaHint')}</p>
            )}
            {mediaItems.length > 0 ? (
              <div className="space-y-3">
                <div className="grid grid-cols-2 gap-3 sm:grid-cols-3">
                  {mediaItems.map((item, index) => (
                    <div
                      key={`${item.mediaPath}-${index}`}
                      className="relative overflow-hidden rounded-lg border border-border bg-surface-muted"
                    >
                      {item.mediaType === 'video' ? (
                        <video
                          src={item.mediaSrc}
                          className="aspect-[4/3] w-full object-cover"
                          muted
                        />
                      ) : (
                        <img
                          src={item.mediaSrc}
                          alt=""
                          className="aspect-[4/3] w-full object-cover"
                        />
                      )}
                      <button
                        type="button"
                        onClick={() => removeMediaItem(index)}
                        disabled={isSaving}
                        className="absolute end-2 top-2 rounded-full bg-black/55 px-2 py-0.5 text-[10px] font-semibold text-white hover:bg-black/70"
                      >
                        {t('dashboard.stories.removeMedia')}
                      </button>
                      <span className="absolute bottom-2 start-2 rounded-full bg-black/55 px-2 py-0.5 text-[10px] font-medium text-white">
                        {t('dashboard.stories.slideIndex', { index: index + 1 })}
                      </span>
                    </div>
                  ))}
                </div>
                <label className="inline-flex cursor-pointer items-center gap-2 text-sm font-medium text-warm hover:text-warm-hover">
                  <UploadIcon />
                  {t('dashboard.stories.addMoreMedia')}
                  <input
                    type="file"
                    accept="image/*,video/*"
                    multiple
                    className="sr-only"
                    onChange={(e) => void handleFiles(e.target.files)}
                    disabled={isSaving}
                  />
                </label>
              </div>
            ) : (
              <label className="flex cursor-pointer flex-col items-center justify-center rounded-lg border-2 border-dashed border-border bg-surface px-4 py-8 transition-colors hover:border-warm hover:bg-warm-soft/30">
                <UploadIcon />
                <span className="mt-3 text-sm font-medium text-text">
                  {t('dashboard.stories.uploadHint')}
                </span>
                <span className="text-xs text-text-muted">
                  {t('dashboard.stories.uploadFormats', { max: MAX_FILE_MB })}
                </span>
                <input
                  type="file"
                  accept="image/*,video/*"
                  multiple
                  className="sr-only"
                  onChange={(e) => void handleFiles(e.target.files)}
                  disabled={isSaving}
                />
              </label>
            )}
            {mode === 'edit' && mediaItems.length > 0 && (
              <p className="mt-2 text-xs text-text-muted">{t('dashboard.stories.replaceMediaHint')}</p>
            )}
          </Field>

          <Field label={t('dashboard.stories.fieldProduct')}>
            <AdminSearchableSelect
              value={productSlug}
              onChange={setProductSlug}
              options={productSelectOptions}
              loading={productsLoading}
              disabled={isSaving}
              placeholder={t('dashboard.stories.noProduct')}
              searchPlaceholder={t('dashboard.stories.productSearchPlaceholder')}
              emptyMessage={t('dashboard.stories.productsEmpty')}
              clearLabel={t('dashboard.stories.noProduct')}
              loadingLabel={t('dashboard.stories.productsLoading')}
              searchValue={productSearch}
              onSearchChange={setProductSearch}
            />
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
            <Button variant="warm" onClick={() => void handleSave()} disabled={isSaving || mediaItems.length === 0}>
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
          <div className="grid gap-3 lg:grid-cols-2">
            {groupedStories.map((group) => (
              <StoryManageCard
                key={group.id}
                group={group}
                disabled={isSaving}
                onEdit={() => startEditGroup(group)}
                onComments={() => setCommentsGroup(group)}
                onToggle={() => void toggleStoryGroup(group.slides)}
                onDelete={() => {
                  void (async () => {
                    if (!(await confirm({ message: t('dashboard.stories.deleteConfirm') }))) return
                    await deleteStories(group.slides.map((slide) => slide.id))
                  })()
                }}
              />
            ))}
          </div>
          <StoryGroupCommentsModal
            open={commentsGroup !== null}
            group={commentsGroup}
            onClose={() => setCommentsGroup(null)}
          />
        </>
      )}
    </div>
  )
}

function StoryManageCard({
  group,
  disabled,
  onEdit,
  onComments,
  onToggle,
  onDelete,
}: {
  group: DashboardStoryGroup
  disabled: boolean
  onEdit: () => void
  onComments: () => void
  onToggle: () => void
  onDelete: () => void
}) {
  const { t } = useTranslation()
  const slideCount = group.slides.length
  const cover = group.slides[0]

  return (
    <article className="flex gap-3 overflow-hidden rounded-xl border border-border bg-surface p-3 shadow-sm transition-shadow hover:shadow-md sm:gap-4 sm:p-3.5">
      <div className="relative h-[6.75rem] w-[3.75rem] shrink-0 overflow-hidden rounded-lg bg-surface-muted ring-1 ring-border/60 sm:h-28 sm:w-[4.25rem]">
        <StoryManageCardMedia slides={group.slides} title={group.title} compact />
        {slideCount > 1 && (
          <span className="pointer-events-none absolute bottom-1 end-1 z-10 rounded bg-black/60 px-1.5 py-0.5 text-[9px] font-semibold text-white">
            {slideCount}
          </span>
        )}
      </div>

      <div className="flex min-w-0 flex-1 flex-col justify-between gap-2">
        <div className="min-w-0 space-y-1">
          <div className="flex items-start gap-2">
            <h3 className="min-w-0 flex-1 truncate text-sm font-semibold text-text">{group.title}</h3>
            <span
              className={`shrink-0 rounded-full px-2 py-0.5 text-[9px] font-semibold uppercase tracking-wide ${
                group.isActive ? 'bg-accent/15 text-accent' : 'bg-surface-muted text-text-muted'
              }`}
            >
              {group.isActive ? t('dashboard.stories.statusActive') : t('dashboard.stories.statusHidden')}
            </span>
          </div>

          {group.caption ? (
            <p className="line-clamp-1 text-xs text-text-muted">{group.caption}</p>
          ) : (
            <p className="text-xs text-text-muted">
              {slideCount > 1
                ? t('stories.slideCount', { count: slideCount })
                : cover.mediaType === 'video'
                  ? t('dashboard.stories.typeVideo')
                  : t('dashboard.stories.typeImage')}
            </p>
          )}

          <div className="flex flex-wrap items-center gap-3 text-xs text-text-muted">
            <span className="inline-flex items-center gap-1 tabular-nums">
              <HeartIcon />
              {group.likeCount}
            </span>
            <span className="inline-flex items-center gap-1 tabular-nums">
              <CommentIcon size={14} className="text-text-muted" />
              {group.commentCount}
            </span>
          </div>
        </div>

        <div className="flex flex-wrap items-center gap-1.5">
          <AdminGridIconButton
            label={t('dashboard.stories.viewComments')}
            icon={<CommentIcon size={15} />}
            onClick={onComments}
            disabled={disabled}
          />
          <AdminGridIconButton
            label={t('dashboard.stories.edit')}
            icon={<EditIcon size={15} />}
            onClick={onEdit}
            disabled={disabled}
          />
          <AdminGridIconButton
            label={group.isActive ? t('dashboard.stories.hide') : t('dashboard.stories.show')}
            icon={group.isActive ? <EyeOffIcon size={15} /> : <EyeIcon size={15} />}
            onClick={onToggle}
            disabled={disabled}
          />
          <AdminGridIconButton
            label={t('dashboard.stories.delete')}
            icon={<DeleteIcon size={15} />}
            onClick={onDelete}
            disabled={disabled}
            tone="danger"
          />
        </div>
      </div>
    </article>
  )
}

function StoryManageCardMedia({
  slides,
  title,
  compact = false,
}: {
  slides: UserStoryDraft[]
  title: string
  compact?: boolean
}) {
  const { t } = useTranslation()
  const [index, setIndex] = useState(0)
  const touchStartX = useRef(0)
  const slideCount = slides.length

  useEffect(() => {
    setIndex(0)
  }, [slides])

  const goTo = useCallback(
    (nextIndex: number) => {
      if (nextIndex < 0 || nextIndex >= slideCount) return
      setIndex(nextIndex)
    },
    [slideCount],
  )

  const goPrev = useCallback(() => goTo(index - 1), [goTo, index])
  const goNext = useCallback(() => goTo(index + 1), [goTo, index])

  const onTouchStart = (e: TouchEvent) => {
    touchStartX.current = e.touches[0]?.clientX ?? 0
  }

  const onTouchEnd = (e: TouchEvent) => {
    if (slideCount <= 1) return
    const endX = e.changedTouches[0]?.clientX ?? 0
    const delta = endX - touchStartX.current
    if (delta <= -SWIPE_THRESHOLD_PX) goNext()
    else if (delta >= SWIPE_THRESHOLD_PX) goPrev()
  }

  if (slideCount === 1) {
    const slide = slides[0]
    return slide.mediaType === 'video' ? (
      <video src={slide.mediaSrc} className="size-full object-cover" muted />
    ) : (
      <LocalImage
        image={{ src: slide.mediaSrc, alt: slide.mediaAlt }}
        className="size-full object-cover"
      />
    )
  }

  return (
    <div
      className="relative size-full"
      onTouchStart={onTouchStart}
      onTouchEnd={onTouchEnd}
      aria-roledescription="carousel"
      aria-label={title}
    >
      <div className="size-full overflow-hidden">
        <div
          dir="ltr"
          className="flex h-full transition-transform duration-400 ease-out"
          style={{ transform: `translate3d(-${index * 100}%, 0, 0)` }}
        >
          {slides.map((slide, slideIndex) => (
            <div
              key={slide.id}
              className="relative h-full w-full shrink-0 grow-0 basis-full"
              aria-hidden={slideIndex !== index}
            >
              {slide.mediaType === 'video' ? (
                <video src={slide.mediaSrc} className="size-full object-cover" muted />
              ) : (
                <LocalImage
                  image={{ src: slide.mediaSrc, alt: slide.mediaAlt }}
                  className="size-full object-cover"
                />
              )}
            </div>
          ))}
        </div>
      </div>

      {!compact && (
        <>
          <div className="absolute inset-y-0 start-0 z-10 flex items-center ps-2">
            <ScrollArrowButton
              direction="prev"
              label={t('product.prevImage')}
              disabled={index === 0}
              onClick={goPrev}
              className="size-8 bg-surface/95 shadow-md backdrop-blur-sm"
            />
          </div>
          <div className="absolute inset-y-0 end-0 z-10 flex items-center pe-2">
            <ScrollArrowButton
              direction="next"
              label={t('product.nextImage')}
              disabled={index === slideCount - 1}
              onClick={goNext}
              className="size-8 bg-surface/95 shadow-md backdrop-blur-sm"
            />
          </div>
          <span className="absolute bottom-3 end-3 z-10 rounded-full bg-black/55 px-2.5 py-1 text-xs font-medium text-white">
            {index + 1} / {slideCount}
          </span>
        </>
      )}
    </div>
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

function HeartIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 20 20" fill="none" className="text-sale" aria-hidden>
      <path
        d="M10 17s-6.5-4.2-6.5-8.5C3.5 6.2 5.4 4.5 7.5 4.5c1.2 0 2.3.6 3 1.5.7-.9 1.8-1.5 3-1.5 2.1 0 3.9 1.7 3.9 4 0 4.3-6.5 8.5-6.5 8.5z"
        stroke="currentColor"
        strokeWidth="1.4"
      />
    </svg>
  )
}
