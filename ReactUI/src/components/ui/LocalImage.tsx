import { useEffect, useState } from 'react'
import type { ImageAsset } from '@/models/shared/image.model'
import { resolveHomeImageSrc } from '@/extensions/resolveHomeImage'

interface LocalImageProps {
  image: ImageAsset
  className?: string
  loading?: 'eager' | 'lazy'
}

export function LocalImage({ image, className = '', loading = 'lazy' }: LocalImageProps) {
  const primary = resolveHomeImageSrc(image.src)
  const fallback = primary.endsWith('.jpg')
    ? primary.replace(/\.jpg$/i, '.svg')
    : primary.replace(/\.svg$/i, '.jpg')

  const [src, setSrc] = useState(primary)

  useEffect(() => {
    setSrc(primary)
  }, [primary])

  return (
    <img
      src={src}
      alt={image.alt}
      width={image.width}
      height={image.height}
      loading={loading}
      className={className}
      onError={() => {
        if (src !== fallback) setSrc(fallback)
      }}
    />
  )
}
