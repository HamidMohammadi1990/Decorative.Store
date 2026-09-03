import { Link } from 'react-router-dom'
import { useCallback, useEffect, useRef, useState } from 'react'
import type { HeroCarousel as HeroCarouselModel } from '@/models/home/heroCarousel.model'
import { Button } from '@/components/ui/Button'
import { Container } from '@/components/ui/Container'
import { LocalImage } from '@/components/ui/LocalImage'
import { getDirection } from '@/extensions/getDirection'
import { useHeroCarouselDrag } from '@/hooks/useHeroCarouselDrag'
import { useSettingsStore } from '@/stores/settingsStore'

interface HeroCarouselProps {
  data: HeroCarouselModel
}

export function HeroCarousel({ data }: HeroCarouselProps) {
  const [index, setIndex] = useState(0)
  const viewportRef = useRef<HTMLDivElement>(null)
  const [viewportWidth, setViewportWidth] = useState(0)
  const locale = useSettingsStore((s) => s.locale)
  const isRtl = getDirection(locale) === 'rtl'
  const slideCount = data.slides.length

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

    const updateWidth = () => {
      setViewportWidth(el.clientWidth)
    }

    updateWidth()
    const observer = new ResizeObserver(updateWidth)
    observer.observe(el)

    return () => observer.disconnect()
  }, [])

  useEffect(() => {
    if (slideCount <= 1) return

    const timer = window.setInterval(() => {
      setIndex((current) => (current + 1) % slideCount)
    }, 6000)

    return () => window.clearInterval(timer)
  }, [slideCount])

  const trackOffset =
    viewportWidth > 0
      ? isRtl
        ? index * viewportWidth + dragOffset
        : -index * viewportWidth + dragOffset
      : dragOffset

  if (slideCount === 0) return null

  return (
    <section className="relative overflow-hidden bg-surface-muted">
      <div
        ref={viewportRef}
        className={`relative h-[clamp(200px,calc(100svh-var(--shop-chrome-height,14rem)),560px)] overflow-hidden touch-pan-y select-none ${
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
          className="flex h-full will-change-transform"
          style={{
            width: viewportWidth > 0 ? viewportWidth * slideCount : '100%',
            transform: `translate3d(${trackOffset}px, 0, 0)`,
            transition: isGrabbing ? 'none' : 'transform 500ms ease-out',
          }}
        >
          {data.slides.map((s, i) => (
            <article
              key={s.id}
              dir="auto"
              className="relative h-full shrink-0"
              style={{ width: viewportWidth > 0 ? viewportWidth : '100%' }}
              aria-hidden={i !== index}
            >
              <LocalImage
                image={s.image}
                priority={i === 0}
                loading={i <= 1 ? 'eager' : 'lazy'}
                sizes="100vw"
                className="pointer-events-none absolute inset-0 size-full object-cover"
              />
              <div className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/50 via-black/20 to-transparent" />
              <Container className="relative flex h-full flex-col justify-end pb-10 pt-16">
                {s.eyebrow && (
                  <p className="mb-2 text-xs font-medium uppercase tracking-widest text-white/90">
                    {s.eyebrow}
                  </p>
                )}
                <h1 className="max-w-xl text-3xl font-semibold text-white md:text-5xl">
                  {s.title}
                </h1>
                {s.subtitle && (
                  <p className="mt-2 max-w-lg text-sm text-white/90 md:text-base">
                    {s.subtitle}
                  </p>
                )}
                <Link to={s.cta.href} className="mt-5 inline-block w-fit">
                  <Button variant="warm">{s.cta.label}</Button>
                </Link>
              </Container>
            </article>
          ))}
        </div>
      </div>

      {slideCount > 1 && (
        <div className="pointer-events-none absolute bottom-4 start-1/2 flex -translate-x-1/2 gap-2 rtl:translate-x-1/2">
          {data.slides.map((s, i) => (
            <button
              key={s.id}
              type="button"
              aria-label={`Slide ${i + 1}`}
              onClick={() => setIndex(i)}
              className={`pointer-events-auto size-2 rounded-full transition-colors ${
                i === index ? 'bg-white' : 'bg-white/50'
              }`}
            />
          ))}
        </div>
      )}
    </section>
  )
}
