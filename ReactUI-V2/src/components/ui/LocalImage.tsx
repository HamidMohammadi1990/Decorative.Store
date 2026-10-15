import { useEffect, useState } from 'react'
import type { ImageAsset } from '@/models/shared/image.model'
import { resolveHomeImageSrc } from '@/extensions/resolveHomeImage'

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
}: LocalImageProps) {
  const resolvedLoading = loading ?? (priority ? 'eager' : 'lazy')
  const resolvedFetchPriority = fetchPriority ?? (priority ? 'high' : undefined)
  const primarySrc = resolveHomeImageSrc(image.src)
  const [displaySrc, setDisplaySrc] = useState(primarySrc)

  useEffect(() => {
    setDisplaySrc(primarySrc)
  }, [primarySrc])

  const webpSrc = displaySrc ? webpCandidate(displaySrc) : null

  const onImgError = () => {
    if (!displaySrc?.startsWith('/images/home/')) return
    const svgFallback = displaySrc.replace(/\.(jpg|jpeg|png|webp)$/i, '.svg')
    if (svgFallback !== displaySrc) setDisplaySrc(svgFallback)
  }

  const imgProps = {
    alt: image.alt,
    width: image.width,
    height: image.height,
    loading: resolvedLoading,
    decoding,
    fetchPriority: resolvedFetchPriority,
    sizes,
    className,
    onError: onImgError,
  }

  if (webpSrc) {
    return (
      <picture>
        <source srcSet={webpSrc} type="image/webp" sizes={sizes} />
        <img {...imgProps} src={displaySrc} />
      </picture>
    )
  }

  return <img {...imgProps} src={displaySrc} />
}
