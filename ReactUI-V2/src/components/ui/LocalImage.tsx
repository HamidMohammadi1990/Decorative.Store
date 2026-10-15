import { useEffect, useState, type RefObject } from 'react'
import type { ImageAsset } from '@/models/shared/image.model'
import { resolveHomeImageSrc } from '@/extensions/resolveHomeImage'
import { useLazyImageSource } from '@/hooks/useLazyImageSource'

interface LocalImageProps {
  image: ImageAsset
  className?: string
  loading?: 'eager' | 'lazy'
  /** Shorthand for LCP images — eager load + fetchpriority high. */
  priority?: boolean
  fetchPriority?: 'high' | 'low' | 'auto'
  decoding?: 'async' | 'sync' | 'auto'
  sizes?: string
  rootMargin?: string
}

function webpCandidate(src: string): string | null {
  if (!src.startsWith('/images/home/')) return null
  if (/\.webp(\?|$)/i.test(src)) return null
  if (/\.(jpg|jpeg|png)$/i.test(src)) return src.replace(/\.(jpg|jpeg|png)$/i, '.webp')
  if (/\.svg$/i.test(src)) return null
  if (!/\.\w+$/.test(src)) return `${src}.webp`
  return null
}

export function LocalImage({
  image,
  className = '',
  loading,
  priority = false,
  fetchPriority,
  decoding = 'async',
  sizes,
  rootMargin,
}: LocalImageProps) {
  const resolvedLoading = loading ?? (priority ? 'eager' : 'lazy')
  const resolvedFetchPriority = fetchPriority ?? (priority ? 'high' : undefined)
  const src = resolveHomeImageSrc(image.src)
  const [displaySrc, setDisplaySrc] = useState<string | undefined>(undefined)

  useEffect(() => {
    setDisplaySrc(undefined)
  }, [src])
  const { ref, resolvedSrc, isLoaded } = useLazyImageSource(src, {
    priority,
    rootMargin,
  })
  const effectiveSrc = displaySrc ?? resolvedSrc
  const webpSrc = effectiveSrc ? webpCandidate(effectiveSrc) : null

  const onImgError = () => {
    if (!effectiveSrc?.startsWith('/images/home/')) return
    const svgFallback = effectiveSrc.replace(/\.(jpg|jpeg|png|webp)$/i, '.svg')
    if (svgFallback !== effectiveSrc) setDisplaySrc(svgFallback)
  }

  const imgClass = `${className}${!isLoaded ? ' bg-surface-muted animate-pulse' : ''}`.trim()

  const imgProps = {
    alt: image.alt,
    width: image.width,
    height: image.height,
    loading: resolvedLoading,
    decoding,
    fetchPriority: resolvedFetchPriority,
    sizes,
    className: imgClass,
  }

  if (webpSrc) {
    return (
      <picture>
        <source srcSet={webpSrc} type="image/webp" sizes={sizes} />
        <img
          ref={ref as RefObject<HTMLImageElement>}
          {...imgProps}
          src={effectiveSrc}
          onError={onImgError}
        />
      </picture>
    )
  }

  return (
    <img
      ref={ref as RefObject<HTMLImageElement>}
      {...imgProps}
      src={effectiveSrc}
      onError={onImgError}
    />
  )
}
