import { Link } from 'react-router-dom'
import { useCallback, useEffect, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { Container } from '@/components/ui/Container'
import { LocalImage } from '@/components/ui/LocalImage'
import { getDirection } from '@/extensions/getDirection'
import { useHeroCarouselDrag } from '@/hooks/useHeroCarouselDrag'
import { useSettingsStore } from '@/stores/settingsStore'
import type { BlogPostSummary } from '@/models/blog/blog.model'

interface BlogFeaturedSliderProps {
  posts: BlogPostSummary[]
  getCategoryLabel: (slug: string) => string
}

export function BlogFeaturedSlider({ posts, getCategoryLabel }: BlogFeaturedSliderProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const isRtl = getDirection(locale) === 'rtl'
  const [index, setIndex] = useState(0)
  const viewportRef = useRef<HTMLDivElement>(null)
  const [viewportWidth, setViewportWidth] = useState(0)
  const slideCount = posts.length

  const goToSlide = useCallback((nextIndex: number) => {
    setIndex(nextIndex)
  }, [])

  const { dragOffset, isGrabbing, onPointerDown, onPointerUp, onLostPointerCapture, onClickCapture } =
    useHeroCarouselDrag({
      slideCount,
      index,
      isRtl,
      onIndexChange: goToSlide,
    })

  useEffect(() => {
    setIndex(0)
  }, [locale, posts])

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
    }, 7000)
    return () => window.clearInterval(timer)
  }, [slideCount])

  if (posts.length === 0) return null

  const trackOffset =
    viewportWidth > 0
      ? isRtl
        ? index * viewportWidth + dragOffset
        : -index * viewportWidth + dragOffset
      : dragOffset

  return (
    <section className="relative overflow-hidden bg-surface-inverse text-text-inverse">
      <div
        ref={viewportRef}
        className={`relative overflow-hidden ${isGrabbing ? 'cursor-grabbing' : 'cursor-grab'}`}
        onPointerDown={onPointerDown}
        onPointerUp={onPointerUp}
        onLostPointerCapture={onLostPointerCapture}
        onClickCapture={onClickCapture}
      >
        <div
          className="flex transition-transform duration-500 ease-out will-change-transform"
          style={{
            transform: `translate3d(${trackOffset}px, 0, 0)`,
            transitionDuration: isGrabbing ? '0ms' : undefined,
          }}
        >
          {posts.map((post) => (
            <article key={post.id} className="relative w-full shrink-0">
              <div className="relative aspect-[16/9] max-h-[28rem] w-full md:aspect-[21/9]">
                <LocalImage image={post.coverImage} className="size-full object-cover opacity-80" />
                <div className="absolute inset-0 bg-gradient-to-t from-black/80 via-black/30 to-transparent" />
              </div>

              <Container className="absolute inset-x-0 bottom-0 pb-8 pt-16 md:pb-10">
                <span className="inline-flex rounded-full bg-warm px-3 py-1 text-xs font-semibold text-warm-text">
                  {getCategoryLabel(post.categorySlug)}
                </span>
                <h2 className="mt-3 max-w-3xl text-2xl font-bold leading-tight md:text-4xl">
                  <Link to={`/blog/${post.slug}`} className="hover:text-warm-soft">
                    {post.title}
                  </Link>
                </h2>
                <p className="mt-3 max-w-2xl text-sm leading-relaxed text-white/80 md:text-base">
                  {post.excerpt}
                </p>
                <div className="mt-4 flex flex-wrap items-center gap-4 text-xs text-white/70 md:text-sm">
                  <span>{formatBlogDate(post.publishedAt, locale)}</span>
                  <span>{t('blog.readTime', { count: post.readTimeMinutes })}</span>
                  <Link
                    to={`/blog/${post.slug}`}
                    className="font-semibold text-warm-soft hover:text-white"
                  >
                    {t('blog.readArticle')}
                  </Link>
                </div>
              </Container>
            </article>
          ))}
        </div>
      </div>

      {slideCount > 1 && (
        <div className="absolute bottom-4 start-1/2 z-10 flex -translate-x-1/2 gap-2 rtl:translate-x-1/2">
          {posts.map((post, dotIndex) => (
            <button
              key={post.id}
              type="button"
              aria-label={t('blog.goToSlide', { index: dotIndex + 1 })}
              onClick={() => goToSlide(dotIndex)}
              className={`h-1.5 rounded-full transition-all ${
                dotIndex === index ? 'w-8 bg-warm' : 'w-2 bg-white/50 hover:bg-white/80'
              }`}
            />
          ))}
        </div>
      )}
    </section>
  )
}
