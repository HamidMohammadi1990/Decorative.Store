import { useCallback, useEffect, useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import { StoryCommentsSheet } from '@/components/stories/StoryCommentsSheet'
import { StoryProductStrip } from '@/components/stories/StoryProductStrip'
import { StorySlidePlayer } from '@/components/stories/StorySlidePlayer'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { LocalImage } from '@/components/ui/LocalImage'
import { Portal } from '@/components/ui/Portal'
import {
  getDisplayedStoryLikes,
  useStoryInteractionStore,
} from '@/stores/storyInteractionStore'
import { useStoryViewerStore } from '@/stores/storyViewerStore'
import { catalogService } from '@/services/catalogService'
import { useSettingsStore } from '@/stores/settingsStore'

export function StoryViewerModal() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const isOpen = useStoryViewerStore((s) => s.isOpen)
  const groups = useStoryViewerStore((s) => s.groups)
  const groupIndex = useStoryViewerStore((s) => s.groupIndex)
  const slideIndex = useStoryViewerStore((s) => s.slideIndex)
  const commentsOpen = useStoryViewerStore((s) => s.commentsOpen)
  const close = useStoryViewerStore((s) => s.close)
  const nextSlide = useStoryViewerStore((s) => s.nextSlide)
  const prevSlide = useStoryViewerStore((s) => s.prevSlide)
  const openComments = useStoryViewerStore((s) => s.openComments)
  const closeComments = useStoryViewerStore((s) => s.closeComments)
  const markViewed = useStoryInteractionStore((s) => s.markViewed)
  const toggleLike = useStoryInteractionStore((s) => s.toggleStoryLike)
  const toggleFollow = useStoryInteractionStore((s) => s.toggleFollow)

  const [slideProgress, setSlideProgress] = useState(0)
  const [products, setProducts] = useState<ProductDetail[]>([])
  const [visible, setVisible] = useState(false)

  const group = groups[groupIndex]
  const slide = group?.slides[slideIndex]
  const groupId = group?.id
  const productSlugKey = group?.productSlugs.join(',') ?? ''
  const liked = useStoryInteractionStore((s) =>
    groupId ? s.isStoryLiked(groupId) : false,
  )
  const following = useStoryInteractionStore((s) =>
    groupId ? s.isFollowing(groupId) : false,
  )
  const userCommentCount = useStoryInteractionStore((s) =>
    groupId ? (s.userComments[groupId]?.length ?? 0) : 0,
  )

  const likes = group && groupId ? getDisplayedStoryLikes(group.likes, groupId) : 0
  const commentCount = group ? group.comments.length + userCommentCount : 0

  useEffect(() => {
    if (!isOpen || !groupId) return
    markViewed(groupId)
    // markViewed is stable; only run when viewer opens for a story
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isOpen, groupId])

  useEffect(() => {
    if (!isOpen) {
      setVisible(false)
      return
    }

    const frame = requestAnimationFrame(() => setVisible(true))
    const prevOverflow = document.body.style.overflow
    document.body.style.overflow = 'hidden'

    const onKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') {
        if (commentsOpen) closeComments()
        else close()
      }
      if (!commentsOpen && e.key === 'ArrowRight') nextSlide()
      if (!commentsOpen && e.key === 'ArrowLeft') prevSlide()
    }

    window.addEventListener('keydown', onKeyDown)
    return () => {
      cancelAnimationFrame(frame)
      document.body.style.overflow = prevOverflow
      window.removeEventListener('keydown', onKeyDown)
    }
  }, [isOpen, commentsOpen, close, closeComments, nextSlide, prevSlide])

  useEffect(() => {
    if (!groupId) return

    if (!productSlugKey) {
      setProducts((prev) => (prev.length === 0 ? prev : []))
      return
    }

    let cancelled = false
    const slugs = productSlugKey.split(',').filter(Boolean)

    void catalogService.getProductsBySlugs(slugs, locale).then((data) => {
      if (!cancelled) setProducts(data)
    })

    return () => {
      cancelled = true
    }
  }, [groupId, productSlugKey, locale])

  const progressBars = useMemo(() => {
    if (!group) return []
    return group.slides.map((s, i) => {
      if (i < slideIndex) return 1
      if (i > slideIndex) return 0
      return slideProgress
    })
  }, [group, slideIndex, slideProgress])

  const handleTapZone = useCallback(
    (zone: 'prev' | 'next') => {
      if (commentsOpen) return
      if (zone === 'next') nextSlide()
      else prevSlide()
    },
    [commentsOpen, nextSlide, prevSlide],
  )

  const handleSlideComplete = useCallback(() => {
    nextSlide()
  }, [nextSlide])

  if (!isOpen || !group || !slide) return null

  return (
    <Portal>
      <div
        className={`fixed inset-0 z-[70] flex items-center justify-center bg-black/85 p-0 transition-opacity duration-200 sm:p-4 ${
          visible ? 'opacity-100' : 'opacity-0'
        }`}
      >
        <div className="relative flex h-full w-full max-w-md flex-col overflow-hidden bg-black shadow-2xl sm:max-h-[min(92vh,52rem)] sm:rounded-2xl sm:border sm:border-white/10">
          {/* Progress */}
          <div className="absolute inset-x-0 top-0 z-30 flex gap-1 px-3 pt-3">
            {progressBars.map((value, i) => (
              <div
                key={`${group.id}-bar-${i}`}
                className="h-0.5 flex-1 overflow-hidden rounded-full bg-white/25"
              >
                <div
                  className="h-full rounded-full bg-white transition-[width] duration-100 ease-linear"
                  style={{ width: `${value * 100}%` }}
                />
              </div>
            ))}
          </div>

          {/* Header */}
          <header className="absolute inset-x-0 top-0 z-30 flex items-center justify-between gap-3 px-3 pb-2 pt-8">
            <div className="flex min-w-0 flex-1 items-center gap-2.5">
              <div className="size-9 shrink-0 overflow-hidden rounded-full ring-2 ring-white/30">
                <LocalImage image={group.avatar} className="size-full object-cover" />
              </div>
              <div className="min-w-0">
                <p className="truncate text-sm font-semibold text-white">{group.title}</p>
                <p className="text-[10px] text-white/60">{t('stories.brandLabel')}</p>
              </div>
            </div>

            <div className="flex shrink-0 items-center gap-2">
              {!group.ownerId && (
                <button
                  type="button"
                  onClick={() => toggleFollow(group.id)}
                  className={`rounded-full px-3 py-1.5 text-xs font-semibold transition-colors ${
                    following
                      ? 'bg-white/20 text-white'
                      : 'bg-white text-black hover:bg-white/90'
                  }`}
                >
                  {following ? t('stories.following') : t('stories.follow')}
                </button>
              )}
              <button
                type="button"
                onClick={close}
                aria-label={t('common.close')}
                className="flex size-9 items-center justify-center rounded-full bg-black/30 text-white backdrop-blur-sm transition-colors hover:bg-black/50"
              >
                <CloseIcon />
              </button>
            </div>
          </header>

          {/* Media */}
          <div className="relative min-h-0 flex-1 bg-black">
            <StorySlidePlayer
              slide={slide}
              isActive
              paused={commentsOpen}
              onComplete={handleSlideComplete}
              onProgress={setSlideProgress}
            />

            <div className="absolute inset-0 z-10 flex">
              <button
                type="button"
                aria-label={t('stories.prevSlide')}
                className="w-[35%] cursor-w-resize bg-transparent"
                onClick={() => handleTapZone('prev')}
              />
              <button
                type="button"
                aria-label={t('stories.nextSlide')}
                className="w-[65%] cursor-e-resize bg-transparent"
                onClick={() => handleTapZone('next')}
              />
            </div>

            {/* Side actions */}
            <div className="pointer-events-none absolute bottom-28 start-3 z-20 flex flex-col gap-4">
              <button
                type="button"
                onClick={(e) => {
                  e.stopPropagation()
                  toggleLike(group.id)
                }}
                aria-pressed={liked}
                className="pointer-events-auto flex flex-col items-center gap-1 text-white"
              >
                <span
                  className={`flex size-11 items-center justify-center rounded-full backdrop-blur-sm ${
                    liked ? 'bg-warm text-warm-text' : 'bg-black/35 hover:bg-black/50'
                  }`}
                >
                  <HeartIcon filled={liked} />
                </span>
                <span className="text-xs font-semibold tabular-nums drop-shadow">{likes}</span>
              </button>

              <button
                type="button"
                onClick={(e) => {
                  e.stopPropagation()
                  openComments()
                }}
                className="pointer-events-auto flex flex-col items-center gap-1 text-white"
              >
                <span className="flex size-11 items-center justify-center rounded-full bg-black/35 backdrop-blur-sm hover:bg-black/50">
                  <CommentIcon />
                </span>
                <span className="text-xs font-semibold tabular-nums drop-shadow">{commentCount}</span>
              </button>
            </div>

            {slide.caption && (
              <p className="pointer-events-none absolute inset-x-14 bottom-24 z-20 line-clamp-3 text-sm leading-relaxed text-white drop-shadow-lg">
                {slide.caption}
              </p>
            )}

            <StoryProductStrip products={products} />
          </div>
        </div>

        {commentsOpen && (
          <StoryCommentsSheet
            storyId={group.id}
            baseComments={group.comments}
            isOpen={commentsOpen}
            onClose={closeComments}
          />
        )}
      </div>
    </Portal>
  )
}

function HeartIcon({ filled }: { filled: boolean }) {
  return (
    <svg width="20" height="20" viewBox="0 0 20 20" fill={filled ? 'currentColor' : 'none'} aria-hidden>
      <path
        d="M10 17s-6.5-4.2-6.5-8.5C3.5 6.2 5.4 4.5 7.5 4.5c1.2 0 2.3.6 3 1.5.7-.9 1.8-1.5 3-1.5 2.1 0 3.9 1.7 3.9 4 0 4.3-6.5 8.5-6.5 8.5z"
        stroke="currentColor"
        strokeWidth="1.4"
      />
    </svg>
  )
}

function CommentIcon() {
  return (
    <svg width="20" height="20" viewBox="0 0 20 20" fill="none" aria-hidden>
      <path
        d="M4 5.5h12v7.5H7.2L4 16V5.5Z"
        stroke="currentColor"
        strokeWidth="1.4"
        strokeLinejoin="round"
      />
    </svg>
  )
}
