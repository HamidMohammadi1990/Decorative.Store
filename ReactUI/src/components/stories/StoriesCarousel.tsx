import { useRef } from 'react'
import { useTranslation } from 'react-i18next'
import type { StoryGroup } from '@/models/stories/story.model'
import { LocalImage } from '@/components/ui/LocalImage'
import { ScrollArrowButton } from '@/components/ui/ScrollArrowButton'
import { useHorizontalDragScroll } from '@/hooks/useHorizontalDragScroll'
import { useStoryInteractionStore } from '@/stores/storyInteractionStore'
import { useStoryViewerStore } from '@/stores/storyViewerStore'

interface StoriesCarouselProps {
  stories: StoryGroup[]
}

export function StoriesCarousel({ stories }: StoriesCarouselProps) {
  const { t } = useTranslation()
  const drag = useHorizontalDragScroll<HTMLDivElement>()
  const scrollRef = useRef<HTMLDivElement>(null)
  const openViewer = useStoryViewerStore((s) => s.open)

  if (stories.length === 0) return null

  const scrollBy = (direction: 'prev' | 'next') => {
    const el = scrollRef.current
    if (!el) return
    const amount = Math.min(el.clientWidth * 0.55, 280)
    const delta = direction === 'next' ? amount : -amount
    el.scrollBy({ left: delta, behavior: 'smooth' })
  }

  return (
    <section
      aria-label={t('stories.sectionTitle')}
      className="border-b border-border/70 bg-surface"
    >
      <div className="mx-auto max-w-7xl px-4 sm:px-6 md:px-10 lg:px-14 xl:px-16">
        <div className="relative">
          <div
            ref={(node) => {
              scrollRef.current = node
              drag.ref.current = node
            }}
            className={`overflow-x-auto py-2.5 scrollbar-none sm:py-3 touch-pan-y select-none ${
              drag.isGrabbing ? 'cursor-grabbing' : 'cursor-grab'
            }`}
            onPointerDown={drag.onPointerDown}
            onPointerMove={drag.onPointerMove}
            onPointerUp={drag.onPointerUp}
            onLostPointerCapture={drag.onLostPointerCapture}
            onClickCapture={drag.onClickCapture}
            onDragStart={(e) => e.preventDefault()}
          >
            <div className="flex w-max min-w-full justify-center gap-3 px-1 sm:gap-4 sm:px-[3.25rem] md:gap-5 md:px-14">
              {stories.map((story, index) => (
                <StoryRing
                  key={story.id}
                  story={story}
                  onOpen={() => openViewer(stories, index)}
                />
              ))}
            </div>
          </div>

          <div
            className="pointer-events-none absolute inset-y-0 start-0 z-10 hidden w-10 bg-gradient-to-r from-surface via-surface/80 to-transparent sm:block"
            aria-hidden
          />
          <div
            className="pointer-events-none absolute inset-y-0 end-0 z-10 hidden w-10 bg-gradient-to-l from-surface via-surface/80 to-transparent sm:block"
            aria-hidden
          />

          <div className="absolute start-2 top-[calc(0.625rem+2.0625rem)] z-20 hidden -translate-y-1/2 sm:block sm:top-[calc(0.75rem+2.25rem)] md:start-3">
            <ScrollArrowButton
              direction="prev"
              label={t('stories.scrollPrev')}
              onClick={() => scrollBy('prev')}
              className="size-8 border-border/70 bg-surface/95 shadow-md ring-1 ring-black/5 backdrop-blur-sm hover:border-warm hover:bg-surface hover:text-warm hover:shadow-lg"
            />
          </div>
          <div className="absolute end-2 top-[calc(0.625rem+2.0625rem)] z-20 hidden -translate-y-1/2 sm:block sm:top-[calc(0.75rem+2.25rem)] md:end-3">
            <ScrollArrowButton
              direction="next"
              label={t('stories.scrollNext')}
              onClick={() => scrollBy('next')}
              className="size-8 border-border/70 bg-surface/95 shadow-md ring-1 ring-black/5 backdrop-blur-sm hover:border-warm hover:bg-surface hover:text-warm hover:shadow-lg"
            />
          </div>
        </div>
      </div>
    </section>
  )
}

function StoryRing({ story, onOpen }: { story: StoryGroup; onOpen: () => void }) {
  const { t } = useTranslation()
  const viewed = useStoryInteractionStore((s) => s.isViewed(story.id))
  const hasVideo = story.slides.some((s) => s.media.type === 'video')

  return (
    <button
      type="button"
      onClick={onOpen}
      className="group flex w-[4.5rem] shrink-0 flex-col items-center gap-1.5 sm:w-[4.875rem]"
    >
      <span className="relative flex size-[4.125rem] items-center justify-center sm:size-[4.5rem]">
        {!viewed ? (
          <span className="story-ring-live absolute inset-0 rounded-full" aria-hidden />
        ) : (
          <span className="absolute inset-0 rounded-full bg-border" aria-hidden />
        )}

        <span className="relative flex size-[calc(100%-4px)] items-center justify-center overflow-hidden rounded-full bg-surface p-[2px] shadow-sm ring-1 ring-black/5 transition-transform duration-300 group-hover:scale-[1.05] group-active:scale-[0.97]">
          <span className="relative size-full overflow-hidden rounded-full">
            <LocalImage
              image={story.coverImage}
              className={`size-full object-cover transition-all duration-300 ${
                viewed ? 'opacity-75 saturate-[0.8]' : 'group-hover:scale-105'
              }`}
              loading="eager"
            />
            <span className="pointer-events-none absolute inset-0 flex items-center justify-center rounded-full bg-black/0 opacity-0 transition-all duration-300 group-hover:bg-black/25 group-hover:opacity-100">
              <span className="flex size-7 items-center justify-center rounded-full bg-white text-neutral-900 shadow-md ring-1 ring-black/10">
                <PlayIcon />
              </span>
            </span>
          </span>
        </span>

        {!viewed && (
          <span className="absolute -end-0.5 -top-0.5 rounded-full bg-warm px-1 py-px text-[8px] font-bold uppercase tracking-wide text-warm-text shadow-sm ring-2 ring-surface">
            {t('stories.newBadge')}
          </span>
        )}

        {hasVideo && (
          <span className="absolute bottom-0 end-0 flex size-4 items-center justify-center rounded-full bg-black/55 text-white ring-2 ring-surface">
            <VideoIcon />
          </span>
        )}
      </span>

      <StoryTitle title={story.title} viewed={viewed} />
    </button>
  )
}

function StoryTitle({ title, viewed }: { title: string; viewed: boolean }) {
  return (
    <span className="relative w-full">
      <span
        title={title}
        className={`block h-3.5 w-full truncate text-center text-[10px] font-medium leading-none sm:h-4 sm:text-[11px] ${
          viewed ? 'text-text-muted' : 'text-text group-hover:text-warm'
        }`}
      >
        {title}
      </span>
      <span
        role="tooltip"
        className="pointer-events-none absolute bottom-[calc(100%+0.375rem)] left-1/2 z-50 w-max max-w-[10.5rem] -translate-x-1/2 rounded-md bg-text px-2 py-1 text-center text-[10px] leading-snug text-text-inverse opacity-0 shadow-lg transition-opacity duration-150 group-hover:opacity-100 sm:max-w-[12rem] sm:text-[11px]"
      >
        {title}
        <span
          className="absolute left-1/2 top-full -translate-x-1/2 border-4 border-transparent border-t-text"
          aria-hidden
        />
      </span>
    </span>
  )
}

function PlayIcon() {
  return (
    <svg width="12" height="12" viewBox="0 0 14 14" fill="currentColor" aria-hidden>
      <path d="M4.5 3.2v7.6c0 .5.6.8 1 .5l5.5-3.3c.4-.3.4-.9 0-1.2L5.5 2.7c-.4-.3-1 .1-1 .5Z" />
    </svg>
  )
}

function VideoIcon() {
  return (
    <svg width="9" height="9" viewBox="0 0 10 10" fill="none" aria-hidden>
      <rect x="1" y="2.5" width="8" height="5" rx="1" stroke="currentColor" strokeWidth="1" />
      <path d="M4 4v2l2-1-2-1Z" fill="currentColor" />
    </svg>
  )
}
