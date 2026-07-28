import { useCallback, useRef, useState, type TouchEvent } from 'react'
import { useTranslation } from 'react-i18next'
import { getDirection } from '@/extensions/getDirection'
import { ImageLightbox } from '@/components/ui/ImageLightbox'
import { ScrollArrowButton } from '@/components/ui/ScrollArrowButton'
import { LocalImage } from '@/components/ui/LocalImage'
import { useSettingsStore } from '@/stores/settingsStore'
import type { ImageAsset } from '@/models/shared/image.model'

const SWIPE_THRESHOLD_PX = 48

interface BlogDetailGalleryProps {
  images: ImageAsset[]
  title: string
}

export function BlogDetailGallery({ images, title }: BlogDetailGalleryProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const isRtl = getDirection(locale) === 'rtl'
  const [index, setIndex] = useState(0)
  const [lightboxOpen, setLightboxOpen] = useState(false)
  const touchStartX = useRef(0)
  const slideCount = images.length

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
    const endX = e.changedTouches[0]?.clientX ?? 0
    const delta = endX - touchStartX.current

    if (isRtl) {
      if (delta >= SWIPE_THRESHOLD_PX) goNext()
      else if (delta <= -SWIPE_THRESHOLD_PX) goPrev()
      return
    }

    if (delta <= -SWIPE_THRESHOLD_PX) goNext()
    else if (delta >= SWIPE_THRESHOLD_PX) goPrev()
  }

  if (slideCount === 0) return null

  const hasMultiple = slideCount > 1

  return (
    <div className="overflow-hidden rounded-2xl border border-border bg-surface">
      <div
        className="relative aspect-[16/10] bg-surface-muted"
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
            {images.map((image, slideIndex) => (
              <div
                key={`${image.src}-${slideIndex}`}
                className="relative h-full w-full shrink-0 grow-0 basis-full"
                aria-hidden={slideIndex !== index}
              >
                <button
                  type="button"
                  onClick={() => {
                    setIndex(slideIndex)
                    setLightboxOpen(true)
                  }}
                  className="group/img relative block size-full focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-warm focus-visible:ring-offset-2"
                  aria-label={t('lightbox.openImage', { title, index: slideIndex + 1 })}
                >
                  <LocalImage
                    image={image}
                    loading={slideIndex <= 1 ? 'eager' : 'lazy'}
                    className="size-full object-cover transition-transform duration-500 group-hover/img:scale-[1.03]"
                  />
                  <span className="pointer-events-none absolute inset-0 bg-black/0 transition-colors duration-300 group-hover/img:bg-black/10" />
                  <span className="pointer-events-none absolute bottom-4 end-4 flex items-center gap-1.5 rounded-full bg-black/55 px-3 py-1.5 text-[11px] font-medium text-white backdrop-blur-sm transition-all duration-200 group-hover/img:bg-black/70">
                    <ZoomHintIcon />
                    {t('lightbox.enlarge')}
                  </span>
                </button>
              </div>
            ))}
          </div>
        </div>

        {hasMultiple && (
          <>
            <div className="absolute inset-y-0 start-0 z-10 flex items-center ps-3">
              <ScrollArrowButton
                direction={isRtl ? 'next' : 'prev'}
                label={isRtl ? t('product.nextImage') : t('product.prevImage')}
                disabled={isRtl ? index === slideCount - 1 : index === 0}
                onClick={isRtl ? goNext : goPrev}
                className="bg-surface/95 shadow-md backdrop-blur-sm"
              />
            </div>
            <div className="absolute inset-y-0 end-0 z-10 flex items-center pe-3">
              <ScrollArrowButton
                direction={isRtl ? 'prev' : 'next'}
                label={isRtl ? t('product.prevImage') : t('product.nextImage')}
                disabled={isRtl ? index === 0 : index === slideCount - 1}
                onClick={isRtl ? goPrev : goNext}
                className="bg-surface/95 shadow-md backdrop-blur-sm"
              />
            </div>
            <span className="absolute bottom-3 end-3 z-10 rounded-full bg-black/50 px-2.5 py-1 text-xs font-medium text-white">
              {index + 1} / {slideCount}
            </span>
          </>
        )}
      </div>

      {hasMultiple && (
        <div className="flex gap-2 overflow-x-auto border-t border-border p-3">
          {images.map((image, thumbIndex) => (
            <button
              key={`${image.src}-thumb-${thumbIndex}`}
              type="button"
              onClick={() => setIndex(thumbIndex)}
              aria-label={`${title} ${thumbIndex + 1}`}
              aria-current={thumbIndex === index}
              className={`size-16 shrink-0 overflow-hidden rounded-lg border-2 transition-colors ${
                thumbIndex === index
                  ? 'border-warm'
                  : 'border-transparent opacity-70 hover:opacity-100'
              }`}
            >
              <LocalImage image={image} className="size-full object-cover" />
            </button>
          ))}
        </div>
      )}

      <ImageLightbox
        images={images}
        title={title}
        index={index}
        isOpen={lightboxOpen}
        onClose={() => setLightboxOpen(false)}
        onIndexChange={setIndex}
        objectFit="cover"
      />
    </div>
  )
}

function ZoomHintIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 14 14" fill="none" aria-hidden>
      <circle cx="6" cy="6" r="4" stroke="currentColor" strokeWidth="1.2" />
      <path d="M9 9 12 12" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
      <path d="M6 4v4M4 6h4" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
    </svg>
  )
}
