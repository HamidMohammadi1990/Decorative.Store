import { useEffect, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import {
  DEFAULT_MAP_ZOOM,
  MAP_TILE_ATTRIBUTION,
  MAP_TILE_URL,
  type MapCoordinates,
} from '@/config/map'
import { loadLeaflet, type LeafletMapInstance } from '@/extensions/loadLeaflet'

interface AddressMapPreviewProps {
  coordinates: MapCoordinates
  className?: string
  compact?: boolean
}

/** Read-only map preview using the same tile layer as the address picker. */
export function AddressMapPreview({
  coordinates,
  className = '',
  compact = false,
}: AddressMapPreviewProps) {
  const { t } = useTranslation()
  const containerRef = useRef<HTMLDivElement>(null)
  const mapRef = useRef<LeafletMapInstance | null>(null)
  const [failed, setFailed] = useState(false)

  useEffect(() => {
    if (typeof window === 'undefined' || !containerRef.current) return

    let disposed = false
    const container = containerRef.current

    void (async () => {
      try {
        const L = await loadLeaflet()
        if (disposed || !containerRef.current) return

        const map = L.map(container, {
          center: [coordinates.latitude, coordinates.longitude],
          zoom: compact ? DEFAULT_MAP_ZOOM + 1 : DEFAULT_MAP_ZOOM + 2,
          scrollWheelZoom: false,
          dragging: false,
          doubleClickZoom: false,
          touchZoom: false,
          zoomControl: !compact,
        })

        L.tileLayer(MAP_TILE_URL, {
          attribution: MAP_TILE_ATTRIBUTION,
          maxZoom: 19,
        }).addTo(map)

        L.marker([coordinates.latitude, coordinates.longitude], {
          draggable: false,
        }).addTo(map)

        mapRef.current = map

        // Leaflet often renders grey tiles until the container size is measured.
        window.setTimeout(() => {
          mapRef.current?.invalidateSize?.()
        }, 50)
        window.setTimeout(() => {
          mapRef.current?.invalidateSize?.()
        }, 250)
      } catch {
        if (!disposed) setFailed(true)
      }
    })()

    return () => {
      disposed = true
      mapRef.current?.remove()
      mapRef.current = null
    }
  }, [compact, coordinates.latitude, coordinates.longitude])

  if (failed) {
    return (
      <div
        className={`rounded-sm border border-border bg-surface-muted/40 px-3 py-4 text-center ${className}`}
      >
        <p className="text-xs text-text-muted">{t('address.mapPreviewFailed')}</p>
        <p className="mt-1 font-mono text-[10px] tabular-nums text-text-muted" dir="ltr">
          {coordinates.latitude.toFixed(7)}, {coordinates.longitude.toFixed(7)}
        </p>
      </div>
    )
  }

  return (
    <figure className={`overflow-hidden rounded-sm border border-border ${className}`}>
      <div
        ref={containerRef}
        className={`w-full bg-surface-muted ${compact ? 'h-28' : 'h-36 sm:h-40'}`}
        aria-label={t('address.mapPreviewAlt')}
      />
      <figcaption
        className="border-t border-border bg-surface-muted/50 px-3 py-1.5 font-mono text-[10px] tabular-nums text-text-muted"
        dir="ltr"
      >
        {coordinates.latitude.toFixed(7)}, {coordinates.longitude.toFixed(7)}
      </figcaption>
    </figure>
  )
}
