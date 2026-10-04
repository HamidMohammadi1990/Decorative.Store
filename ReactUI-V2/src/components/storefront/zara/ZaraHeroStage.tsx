import { useCallback, useEffect, useRef, useState } from 'react'
import type { HeroCarousel as HeroCarouselModel } from '@/models/home/heroCarousel.model'
import type { CategoryNav as CategoryNavModel } from '@/models/home/categoryNav.model'
import type { HeroSlide } from '@/models/home/heroCarousel.model'
import { LocalImage } from '@/components/ui/LocalImage'
import { getDirection } from '@/extensions/getDirection'
import { useHeroCarouselDrag } from '@/hooks/useHeroCarouselDrag'
import { useSettingsStore } from '@/stores/settingsStore'
import { ZaraHeroBottomNav } from '@/components/storefront/zara/ZaraHeroBottomNav'

interface ZaraHeroStageProps {
  hero: HeroCarouselModel
  categoryNav?: CategoryNavModel
  className?: string
}

function HeroSlidePane({ slide, priority }: { slide: HeroSlide; priority?: boolean }) {
  return (
    <div className="relative size-full min-h-svh overflow-hidden bg-neutral-900">
      <LocalImage
        image={slide.image}
        priority={priority}
        loading={priority ? 'eager' : 'lazy'}
        sizes="50vw"
        className="pointer-events-none absolute inset-0 size-full object-cover"
      />
    </div>
  )
}

export function ZaraHeroStage({ hero, categoryNav, className }: ZaraHeroStageProps) {
  const [index, setIndex] = useState(0)
  const viewportRef = useRef<HTMLDivElement>(null)
  const [viewportWidth, setViewportWidth] = useState(0)
  const locale = useSettingsStore((s) => s.locale)
  const isRtl = getDirection(locale) === 'rtl'
  const slideCount = hero.slides.length
  const tabs = categoryNav?.items ?? []

  const goToSlide = useCallback((nextIndex: number) => {
    setIndex(nextIndex)
  }, [])

  const {
    dragOffset,
    isGrabbing,
    onPointerDown,
    onPointerUp,
    onLostPointerCapture,
    onClickCapture,
  } = useHeroCarouselDrag({
    slideCount,
    index,
    isRtl,
    onIndexChange: goToSlide,
  })

  useEffect(() => {
    setIndex(0)
  }, [locale])

  useEffect(() => {
    const el = viewportRef.current
    if (!el) return
    const updateWidth = () => setViewportWidth(el.clientWidth)
    updateWidth()
    const observer = new ResizeObserver(updateWidth)
    observer.observe(el)
    return () => observer.disconnect()
  }, [])

  useEffect(() => {
    if (slideCount <= 1) return
    const timer = window.setInterval(() => {
      setIndex((current) => (current + 1) % slideCount)
    }, 8000)
    return () => window.clearInterval(timer)
  }, [slideCount])

  const trackOffset =
    viewportWidth > 0
      ? isRtl
        ? index * viewportWidth + dragOffset
        : -index * viewportWidth + dragOffset
      : dragOffset

  if (slideCount === 0) {
    return <div className="min-h-svh bg-neutral-900" aria-hidden />
  }

  const activeSlide = hero.slides[index]
  const rightIndex = slideCount > 1 ? (index + 1) % slideCount : index
  const rightSlide = hero.slides[rightIndex]

  return (
    <section
      className={`relative isolate min-h-svh overflow-hidden bg-neutral-900 text-white ${className ?? ''}`}
    >
      {/* Desktop: 50/50 split (Zara Home) */}
      <div className="absolute inset-0 hidden min-h-svh grid-cols-2 lg:grid">
        <HeroSlidePane slide={activeSlide} priority />
        <HeroSlidePane slide={rightSlide} priority={rightIndex <= 1} />
      </div>

      {/* Mobile: horizontal carousel */}
      <div
        ref={viewportRef}
        className={`absolute inset-0 touch-pan-y select-none lg:hidden ${
          slideCount > 1 ? (isGrabbing ? 'cursor-grabbing' : 'cursor-grab') : ''
        }`}
        onPointerDown={onPointerDown}
        onPointerUp={onPointerUp}
        onLostPointerCapture={onLostPointerCapture}
        onClickCapture={onClickCapture}
        onDragStart={(e) => e.preventDefault()}
      >
        <div
          dir="ltr"
          className="flex h-full min-h-svh will-change-transform"
          style={{
            width: viewportWidth > 0 ? viewportWidth * slideCount : '100%',
            transform: `translate3d(${trackOffset}px, 0, 0)`,
            transition: isGrabbing ? 'none' : 'transform 600ms ease-out',
          }}
        >
          {hero.slides.map((s, i) => (
            <article
              key={s.id}
              dir="auto"
              className="relative h-full min-h-svh shrink-0"
              style={{ width: viewportWidth > 0 ? viewportWidth : '100%' }}
              aria-hidden={i !== index}
            >
              <HeroSlidePane slide={s} priority={i === 0} />
            </article>
          ))}
        </div>
      </div>

      <div className="pointer-events-none relative z-[2] flex min-h-svh flex-col items-center justify-center px-6 pb-24 pt-28 text-center md:px-12 md:pb-28 md:pt-32">
        {activeSlide.eyebrow && (
          <p className="zara-hero-eyebrow mb-2 text-[10px] uppercase tracking-[0.28em] text-white/90 md:text-[11px]">
            {activeSlide.eyebrow}
          </p>
        )}
        <h1 className="zara-hero-title font-display text-[clamp(2rem,5.5vw,4.25rem)] font-normal leading-[1.02] tracking-[0.04em] uppercase">
          {activeSlide.title}
        </h1>
        {activeSlide.subtitle && (
          <p className="mt-3 font-display text-[clamp(0.85rem,1.8vw,1.125rem)] font-normal uppercase tracking-[0.22em] text-white/95">
            {activeSlide.subtitle}
          </p>
        )}
      </div>

      <ZaraHeroBottomNav items={tabs} />
    </section>
  )
}
