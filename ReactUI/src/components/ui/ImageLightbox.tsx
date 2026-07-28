import { useCallback, useEffect, useId, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { ImageAsset } from '@/models/shared/image.model'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { LocalImage } from '@/components/ui/LocalImage'
import { Portal } from '@/components/ui/Portal'
import { ScrollArrowButton } from '@/components/ui/ScrollArrowButton'

interface ImageLightboxProps {
  images: ImageAsset[]
  title: string
  index: number
  isOpen: boolean
  onClose: () => void
  onIndexChange: (index: number) => void
  objectFit?: 'contain' | 'cover'
}

const ZOOM_LEVELS = [1, 1.75, 2.5] as const

export function ImageLightbox({
  images,
  title,
  index,
  isOpen,
  onClose,
  onIndexChange,
  objectFit = 'contain',
}: ImageLightboxProps) {
  const { t } = useTranslation()
  const titleId = useId()
  const [zoomIndex, setZoomIndex] = useState(0)
  const [visible, setVisible] = useState(false)
  const slideCount = images.length
  const current = images[index]
  const zoom = ZOOM_LEVELS[zoomIndex]
  const isZoomed = zoom > 1

  const goPrev = useCallback(() => {
    if (index > 0) onIndexChange(index - 1)
  }, [index, onIndexChange])

  const goNext = useCallback(() => {
    if (index < slideCount - 1) onIndexChange(index + 1)
  }, [index, onIndexChange, slideCount])

  const cycleZoom = useCallback(() => {
    setZoomIndex((prev) => (prev + 1) % ZOOM_LEVELS.length)
  }, [])

  useEffect(() => {
    if (!isOpen) {
      setVisible(false)
      return
    }

    setZoomIndex(0)
    const frame = requestAnimationFrame(() => setVisible(true))

    const prevOverflow = document.body.style.overflow
    document.body.style.overflow = 'hidden'

    const isRtl = document.documentElement.dir === 'rtl'

    const onKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose()
      if (e.key === 'ArrowLeft') isRtl ? goNext() : goPrev()
      if (e.key === 'ArrowRight') isRtl ? goPrev() : goNext()
    }

    window.addEventListener('keydown', onKeyDown)
    return () => {
      cancelAnimationFrame(frame)
      document.body.style.overflow = prevOverflow
      window.removeEventListener('keydown', onKeyDown)
    }
  }, [isOpen, onClose, goPrev, goNext])

  useEffect(() => {
    setZoomIndex(0)
  }, [index])

  if (!isOpen || !current) return null

  return (
    <Portal>
      <div
        role="dialog"
        aria-modal="true"
        aria-labelledby={titleId}
        className={`fixed inset-0 z-[120] flex flex-col transition-opacity duration-300 ${
          visible ? 'opacity-100' : 'opacity-0'
        }`}
      >
        <button
          type="button"
          aria-label={t('common.close')}
          className="absolute inset-0 bg-[#0a0a0a]/94 backdrop-blur-md"
          onClick={onClose}
        />

        <header className="relative z-10 flex items-center justify-between gap-4 border-b border-white/10 px-4 py-3 sm:px-6">
          <div className="min-w-0">
            <p id={titleId} className="truncate text-sm font-semibold text-white">
              {title}
            </p>
            {slideCount > 1 && (
              <p className="mt-0.5 text-xs text-white/60">
                {t('lightbox.counter', { current: index + 1, total: slideCount })}
              </p>
            )}
          </div>

          <div className="flex shrink-0 items-center gap-2">
            <button
              type="button"
              onClick={cycleZoom}
              aria-label={isZoomed ? t('lightbox.zoomOut') : t('lightbox.zoomIn')}
              className="flex size-10 items-center justify-center rounded-full border border-white/15 bg-white/10 text-white transition-colors hover:bg-white/20"
            >
              <ZoomIcon zoomed={isZoomed} />
            </button>
            <button
              type="button"
              onClick={onClose}
              aria-label={t('common.close')}
              className="flex size-10 items-center justify-center rounded-full border border-white/15 bg-white/10 text-white transition-colors hover:bg-white/20"
            >
              <CloseIcon />
            </button>
          </div>
        </header>

        <div className="relative z-10 flex min-h-0 flex-1 items-center justify-center px-3 py-4 sm:px-8">
          {slideCount > 1 && (
            <div className="absolute inset-y-0 start-2 z-20 flex items-center sm:start-4">
              <ScrollArrowButton
                direction="prev"
                label={t('product.prevImage')}
                disabled={index === 0}
                onClick={goPrev}
                className="border-white/20 bg-white/10 text-white hover:border-white/40 hover:text-white disabled:opacity-25"
              />
            </div>
          )}

          <div
            className="relative max-h-full max-w-full overflow-auto rounded-sm"
            onClick={(e) => e.stopPropagation()}
            onKeyDown={(e) => e.stopPropagation()}
            role="presentation"
          >
            <button
              type="button"
              aria-label={isZoomed ? t('lightbox.zoomOut') : t('lightbox.zoomIn')}
              onClick={cycleZoom}
              className={`block transition-transform duration-300 ease-out ${
                isZoomed ? 'cursor-zoom-out' : 'cursor-zoom-in'
              }`}
              style={{ transform: `scale(${zoom})` }}
            >
              <LocalImage
                image={current}
                loading="eager"
                className={`max-h-[min(72vh,52rem)] w-auto max-w-[min(92vw,72rem)] ${
                  objectFit === 'cover' ? 'object-cover' : 'object-contain'
                }`}
              />
            </button>
          </div>

          {slideCount > 1 && (
            <div className="absolute inset-y-0 end-2 z-20 flex items-center sm:end-4">
              <ScrollArrowButton
                direction="next"
                label={t('product.nextImage')}
                disabled={index === slideCount - 1}
                onClick={goNext}
                className="border-white/20 bg-white/10 text-white hover:border-white/40 hover:text-white disabled:opacity-25"
              />
            </div>
          )}
        </div>

        {slideCount > 1 && (
          <footer className="relative z-10 border-t border-white/10 bg-black/30 px-4 py-3 sm:px-6">
            <div className="mx-auto flex max-w-3xl justify-center gap-2 overflow-x-auto pb-1">
              {images.map((image, i) => (
                <button
                  key={`${image.src}-lightbox-${i}`}
                  type="button"
                  onClick={() => onIndexChange(i)}
                  aria-label={t('product.viewImage', { index: i + 1 })}
                  aria-current={i === index}
                  className={`size-14 shrink-0 overflow-hidden rounded-sm border-2 transition-all sm:size-16 ${
                    i === index
                      ? 'border-warm opacity-100 ring-2 ring-warm/40'
                      : 'border-white/20 opacity-60 hover:border-white/40 hover:opacity-100'
                  }`}
                >
                  <LocalImage image={image} className="size-full object-cover" />
                </button>
              ))}
            </div>
          </footer>
        )}

        <p className="relative z-10 pb-3 text-center text-[11px] text-white/45">
          {t('lightbox.hint')}
        </p>
      </div>
    </Portal>
  )
}

function ZoomIcon({ zoomed }: { zoomed: boolean }) {
  return (
    <svg width="18" height="18" viewBox="0 0 20 20" fill="none" aria-hidden>
      <circle cx="9" cy="9" r="5.5" stroke="currentColor" strokeWidth="1.4" />
      <path d="M13.5 13.5 17 17" stroke="currentColor" strokeWidth="1.4" strokeLinecap="round" />
      {zoomed ? (
        <path d="M7 9h4" stroke="currentColor" strokeWidth="1.4" strokeLinecap="round" />
      ) : (
        <>
          <path d="M9 7v4M7 9h4" stroke="currentColor" strokeWidth="1.4" strokeLinecap="round" />
        </>
      )}
    </svg>
  )
}
