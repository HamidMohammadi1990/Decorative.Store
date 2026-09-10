import { useTranslation } from 'react-i18next'
import type { MapCoordinates } from '@/config/map'

interface AddressMapThumbnailProps {
  coordinates: MapCoordinates
  className?: string
}

/** Lightweight static preview — same coordinates as stored in the database. */
export function AddressMapThumbnail({ coordinates, className = '' }: AddressMapThumbnailProps) {
  const { t } = useTranslation()
  const { latitude, longitude } = coordinates
  const src = `https://staticmap.openstreetmap.de/staticmap.php?center=${latitude},${longitude}&zoom=16&size=480x180&markers=${latitude},${longitude},red`

  return (
    <figure className={`overflow-hidden rounded-sm border border-border ${className}`}>
      <img
        src={src}
        alt={t('address.mapPreviewAlt')}
        className="h-36 w-full object-cover"
        loading="lazy"
        decoding="async"
      />
      <figcaption className="border-t border-border bg-surface-muted/50 px-3 py-1.5 font-mono text-[10px] tabular-nums text-text-muted" dir="ltr">
        {latitude.toFixed(7)}, {longitude.toFixed(7)}
      </figcaption>
    </figure>
  )
}
