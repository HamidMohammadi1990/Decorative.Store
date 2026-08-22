import { useCallback, useRef, useState, type TouchEvent } from 'react'
import { useTranslation } from 'react-i18next'
import type { ImageAsset } from '@/models/shared/image.model'
import { ImageLightbox } from '@/components/ui/ImageLightbox'
import { LocalImage } from '@/components/ui/LocalImage'
import { ScrollArrowButton } from '@/components/ui/ScrollArrowButton'
import { WishlistButton } from '@/components/wishlist/WishlistButton'
import { copyToClipboard } from '@/extensions/copyToClipboard'
import { useCompareStore } from '@/stores/compareStore'

interface ProductGalleryProps {
  images: ImageAsset[]
  title: string
  slug: string
  onSale?: boolean
}

const SWIPE_THRESHOLD_PX = 48

export function ProductGallery({ images, title, slug, onSale }: ProductGalleryProps) {
  const { t } = useTranslation()
  const [index, setIndex] = useState(0)
  const [lightboxOpen, setLightboxOpen] = useState(false)
  const [compareNotice, setCompareNotice] = useState<string | null>(null)
  const touchStartX = useRef(0)
  const slideCount = images.length

  const toggleCompare = useCompareStore((s) => s.toggle)
  const isInCompare = useCompareStore((s) => s.isInCompare(slug))

  const shareUrl =
    typeof window !== 'undefined'
      ? `${window.location.origin}/product/${slug}`
      : `/product/${slug}`

  const handleShare = async () => {
    if (navigator.share) {
      try {
        await navigator.share({ title, url: shareUrl })
        return
      } catch {
        // user cancelled or unsupported
      }
    }
    await copyToClipboard(shareUrl)
  }

  const handleCompare = () => {
    const result = toggleCompare(slug)
    if (result === 'full') {
      setCompareNotice(t('compare.maxReached'))
      window.setTimeout(() => setCompareNotice(null), 2200)
    }
  }

  const compareLabel = isInCompare ? t('compare.inCompare') : t('product.compare')

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
    if (delta <= -SWIPE_THRESHOLD_PX) goNext()
    else if (delta >= SWIPE_THRESHOLD_PX) goPrev()
  }

  if (!images[0]) return null

  return (
    <div className="min-w-0">
      <div className="relative flex gap-2">
        <div className="hidden shrink-0 flex-col gap-2 sm:flex">
          <WishlistButton slug={slug} variant="icon" />
          <GalleryActionButton
            label={t('product.share')}
            icon="share"
            onClick={() => void handleShare()}
          />
          <GalleryActionButton
            label={compareNotice ?? compareLabel}
            icon="chart"
            active={isInCompare}
            onClick={handleCompare}
          />
        </div>

        <div className="min-w-0 flex-1">
          <div
            className="relative overflow-hidden rounded-lg border border-border bg-surface"
            onTouchStart={onTouchStart}
            onTouchEnd={onTouchEnd}
          >
            {onSale && (
              <span className="absolute start-3 top-3 z-10 rounded-sm bg-warm px-2 py-1 text-[10px] font-bold uppercase tracking-wide text-warm-text">
                {t('listing.sale')}
              </span>
            )}

            <div
              className="aspect-[4/3] max-h-[20rem] w-full overflow-hidden sm:max-h-[22rem]"
              aria-roledescription="carousel"
              aria-label={title}
            >
              <div
                dir="ltr"
                className="flex h-full transition-transform duration-400 ease-out"
                style={{ transform: `translate3d(-${index * 100}%, 0, 0)` }}
              >
                {images.map((image, i) => (
                  <div
                    key={`${image.src}-slide-${i}`}
                    className="relative flex h-full shrink-0 grow-0 basis-full items-center justify-center bg-surface-muted p-4"
                    aria-hidden={i !== index}
                  >
                    <button
                      type="button"
                      onClick={() => {
                        setIndex(i)
                        setLightboxOpen(true)
                      }}
                      className="group/img relative flex size-full items-center justify-center focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-warm"
                      aria-label={t('lightbox.openImage', { title, index: i + 1 })}
                    >
                      <LocalImage
                        image={image}
                        loading={i <= 1 ? 'eager' : 'lazy'}
                        className="max-h-full max-w-full object-contain transition-transform duration-300 group-hover/img:scale-[1.02]"
                      />
                      <span className="pointer-events-none absolute inset-0 bg-black/0 transition-colors duration-200 group-hover/img:bg-black/5" />
                      <span className="pointer-events-none absolute bottom-3 end-3 flex items-center gap-1.5 rounded-full border border-border/80 bg-surface/90 px-2.5 py-1 text-[10px] font-semibold text-text-muted shadow-sm backdrop-blur-sm transition-all duration-200 group-hover/img:border-warm/40 group-hover/img:text-warm">
                        <ZoomHintIcon />
                        {t('lightbox.enlarge')}
                      </span>
                    </button>
                  </div>
                ))}
              </div>
            </div>

            {slideCount > 1 && (
              <>
                <div className="absolute inset-y-0 start-0 z-10 flex items-center ps-2">
                  <ScrollArrowButton
                    direction="prev"
                    label={t('product.prevImage')}
                    disabled={index === 0}
                    onClick={goPrev}
                    className="bg-surface/95 shadow-sm"
                  />
                </div>
                <div className="absolute inset-y-0 end-0 z-10 flex items-center pe-2">
                  <ScrollArrowButton
                    direction="next"
                    label={t('product.nextImage')}
                    disabled={index === slideCount - 1}
                    onClick={goNext}
                    className="bg-surface/95 shadow-sm"
                  />
                </div>
              </>
            )}
          </div>

          {slideCount > 1 && (
            <div className="mt-3 flex gap-2 overflow-x-auto pb-1">
              {images.map((image, i) => (
                <button
                  key={`${image.src}-thumb-${i}`}
                  type="button"
                  onClick={() => goTo(i)}
                  className={`relative size-16 shrink-0 overflow-hidden rounded-md border-2 bg-surface-muted transition-all sm:size-[4.5rem] ${
                    i === index
                      ? 'border-accent opacity-100'
                      : 'border-border opacity-70 hover:border-border-strong hover:opacity-100'
                  }`}
                  aria-label={`${title} — ${t('product.viewImage', { index: i + 1 })}`}
                  aria-current={i === index}
                >
                  <LocalImage image={image} className="size-full object-cover" />
                </button>
              ))}
            </div>
          )}
        </div>
      </div>

      <ImageLightbox
        images={images}
        title={title}
        index={index}
        isOpen={lightboxOpen}
        onClose={() => setLightboxOpen(false)}
        onIndexChange={setIndex}
        objectFit="contain"
      />
    </div>
  )
}

function ZoomHintIcon() {
  return (
    <svg width="12" height="12" viewBox="0 0 14 14" fill="none" aria-hidden>
      <circle cx="6" cy="6" r="4" stroke="currentColor" strokeWidth="1.2" />
      <path d="M9 9 12 12" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
      <path d="M6 4v4M4 6h4" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
    </svg>
  )
}

function GalleryActionButton({
  label,
  icon,
  active = false,
  onClick,
}: {
  label: string
  icon: 'heart' | 'share' | 'chart'
  active?: boolean
  onClick?: () => void
}) {
  return (
    <button
      type="button"
      aria-label={label}
      aria-pressed={active || undefined}
      title={label}
      onClick={onClick}
      className={`flex size-10 items-center justify-center rounded-full border bg-surface transition-colors hover:border-border-strong ${
        active
          ? 'border-warm text-warm'
          : 'border-border text-text-muted hover:text-text'
      }`}
    >
      <GalleryIcon type={icon} active={active} />
    </button>
  )
}

function GalleryIcon({ type, active = false }: { type: 'heart' | 'share' | 'chart'; active?: boolean }) {
  if (type === 'heart') {
    return (
      <svg width="18" height="18" viewBox="0 0 20 20" fill={active ? 'currentColor' : 'none'} aria-hidden>
        <path
          d="M10 17s-6.5-4.2-6.5-8.5C3.5 6.2 5.4 4.5 7.5 4.5c1.2 0 2.3.6 3 1.5.7-.9 1.8-1.5 3-1.5 2.1 0 3.9 1.7 3.9 4 0 4.3-6.5 8.5-6.5 8.5z"
          stroke="currentColor"
          strokeWidth="1.4"
        />
      </svg>
    )
  }

  if (type === 'share') {
    return (
      <svg width="18" height="18" viewBox="0 0 20 20" fill="none" aria-hidden>
        <path
          d="M14 6.5a2.5 2.5 0 100-5 2.5 2.5 0 000 5zM6 12.5a2.5 2.5 0 100-5 2.5 2.5 0 000 5zM14 18.5a2.5 2.5 0 100-5 2.5 2.5 0 000 5zM8.2 11.2l3.6-2M8.2 8.8l3.6 2"
          stroke="currentColor"
          strokeWidth="1.4"
          strokeLinecap="round"
        />
      </svg>
    )
  }

  return (
    <svg width="18" height="18" viewBox="0 0 20 20" fill="none" aria-hidden>
      <path d="M4 14V8M8 14V5M12 14v-4M16 14V6" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" />
    </svg>
  )
}
