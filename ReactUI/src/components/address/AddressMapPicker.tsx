import { useEffect, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import {
  DEFAULT_MAP_CENTER,
  DEFAULT_MAP_ZOOM,
  MAP_TILE_ATTRIBUTION,
  MAP_TILE_URL,
  roundCoordinate,
  type MapCoordinates,
} from '@/config/map'
import {
  loadLeaflet,
  resetLeafletLoader,
  type LeafletMapInstance,
  type LeafletMarkerInstance,
} from '@/extensions/loadLeaflet'
import { InlineLoading } from '@/components/ui/Spinner'

interface AddressMapPickerProps {
  value: MapCoordinates | null
  onChange: (coords: MapCoordinates) => void
  onAddressHint?: (addressLine: string) => void
  error?: string
  readOnly?: boolean
  className?: string
  /** Force a fresh map instance when switching add/edit sessions. */
  sessionKey?: string
}

export function AddressMapPicker({
  value,
  onChange,
  onAddressHint,
  error,
  readOnly = false,
  className = '',
  sessionKey = 'default',
}: AddressMapPickerProps) {
  const { t, i18n } = useTranslation()
  const containerRef = useRef<HTMLDivElement>(null)
  const mapRef = useRef<LeafletMapInstance | null>(null)
  const markerRef = useRef<LeafletMarkerInstance | null>(null)
  const onChangeRef = useRef(onChange)
  const onAddressHintRef = useRef(onAddressHint)
  const [mapStatus, setMapStatus] = useState<'loading' | 'ready' | 'error'>('loading')
  const [locating, setLocating] = useState(false)
  const [retryTick, setRetryTick] = useState(0)

  onChangeRef.current = onChange
  onAddressHintRef.current = onAddressHint

  useEffect(() => {
    if (typeof window === 'undefined' || !containerRef.current) return

    let disposed = false
    setMapStatus('loading')

    void (async () => {
      try {
        const L = await loadLeaflet()
        if (disposed || !containerRef.current) return

        const initial = value ?? DEFAULT_MAP_CENTER
        const map = L.map(containerRef.current, {
          center: [initial.latitude, initial.longitude],
          zoom: DEFAULT_MAP_ZOOM,
          scrollWheelZoom: !readOnly,
          dragging: !readOnly,
          doubleClickZoom: !readOnly,
          touchZoom: !readOnly,
        })

        L.tileLayer(MAP_TILE_URL, {
          attribution: MAP_TILE_ATTRIBUTION,
          maxZoom: 19,
        }).addTo(map)

        const marker = L.marker([initial.latitude, initial.longitude], {
          draggable: !readOnly,
        }).addTo(map)

        const emitChange = (lat: number, lng: number) => {
          const coords = {
            latitude: roundCoordinate(lat),
            longitude: roundCoordinate(lng),
          }
          onChangeRef.current(coords)
          void reverseGeocode(coords.latitude, coords.longitude, i18n.language).then((line) => {
            if (line) onAddressHintRef.current?.(line)
          })
        }

        if (!readOnly) {
          map.on('click', (event) => {
            marker.setLatLng(event.latlng)
            emitChange(event.latlng.lat, event.latlng.lng)
          })

          marker.on('dragend', () => {
            const pos = marker.getLatLng()
            emitChange(pos.lat, pos.lng)
          })
        }

        mapRef.current = map
        markerRef.current = marker
        setMapStatus('ready')

        window.setTimeout(() => map.invalidateSize(), 50)
        window.setTimeout(() => map.invalidateSize(), 300)
      } catch {
        if (!disposed) setMapStatus('error')
      }
    })()

    return () => {
      disposed = true
      mapRef.current?.remove()
      mapRef.current = null
      markerRef.current = null
    }
    // value changes are synced in the effect below — do not re-init the map on every pin move
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [readOnly, sessionKey, retryTick])

  useEffect(() => {
    if (mapStatus !== 'ready' || !mapRef.current || !markerRef.current || !value) return
    markerRef.current.setLatLng([value.latitude, value.longitude])
    mapRef.current.panTo([value.latitude, value.longitude], { animate: true })
  }, [mapStatus, value?.latitude, value?.longitude])

  const handleLocateMe = () => {
    if (readOnly || !navigator.geolocation) return

    setLocating(true)
    navigator.geolocation.getCurrentPosition(
      (position) => {
        const coords = {
          latitude: roundCoordinate(position.coords.latitude),
          longitude: roundCoordinate(position.coords.longitude),
        }
        onChange(coords)
        mapRef.current?.setView([coords.latitude, coords.longitude], 16, { animate: true })
        markerRef.current?.setLatLng([coords.latitude, coords.longitude])
        setLocating(false)
      },
      () => setLocating(false),
      { enableHighAccuracy: true, timeout: 12000 },
    )
  }

  const handleRetry = () => {
    resetLeafletLoader()
    setRetryTick((tick) => tick + 1)
  }

  return (
    <div className={className}>
      <div className="mb-2 flex flex-wrap items-center justify-between gap-2">
        <div>
          <p className="text-sm font-medium text-text">{t('address.mapTitle')}</p>
          <p className="text-xs text-text-muted">{t('address.mapHint')}</p>
        </div>
        {!readOnly && mapStatus === 'ready' && (
          <button
            type="button"
            onClick={handleLocateMe}
            disabled={locating}
            className="rounded-sm border border-border bg-surface px-3 py-1.5 text-xs font-medium text-text transition-colors hover:border-warm hover:text-warm disabled:opacity-60"
          >
            {locating ? t('address.mapLocating') : t('address.mapLocateMe')}
          </button>
        )}
      </div>

      <div
        className={`relative h-56 w-full overflow-hidden rounded-sm border bg-surface-muted sm:h-64 ${
          error ? 'border-sale ring-1 ring-sale/20' : 'border-border'
        }`}
      >
        <div
          ref={containerRef}
          className="absolute inset-0 z-0"
          aria-label={t('address.mapTitle')}
        />

        {mapStatus === 'loading' && (
          <div className="absolute inset-0 z-10 flex items-center justify-center bg-surface-muted/80">
            <InlineLoading label={t('address.mapLoading')} />
          </div>
        )}

        {mapStatus === 'error' && (
          <div className="absolute inset-0 z-10 flex flex-col items-center justify-center gap-3 bg-surface-muted/95 px-4 text-center">
            <p className="text-sm text-text-muted">{t('address.mapLoadFailed')}</p>
            <button
              type="button"
              onClick={handleRetry}
              className="rounded-sm border border-border bg-surface px-3 py-1.5 text-xs font-medium text-warm hover:border-warm"
            >
              {t('common.retry')}
            </button>
          </div>
        )}
      </div>

      {value && (
        <p className="mt-2 font-mono text-[11px] tabular-nums text-text-muted" dir="ltr">
          {value.latitude.toFixed(7)}, {value.longitude.toFixed(7)}
        </p>
      )}

      {error && (
        <p role="alert" className="mt-1.5 text-xs text-sale">
          {error}
        </p>
      )}
    </div>
  )
}

async function reverseGeocode(
  latitude: number,
  longitude: number,
  language: string,
): Promise<string | null> {
  try {
    const lang = language === 'fa' ? 'fa' : 'en'
    const url = new URL('https://nominatim.openstreetmap.org/reverse')
    url.searchParams.set('format', 'json')
    url.searchParams.set('lat', String(latitude))
    url.searchParams.set('lon', String(longitude))
    url.searchParams.set('accept-language', lang)

    const response = await fetch(url.toString(), {
      headers: { Accept: 'application/json' },
    })
    if (!response.ok) return null

    const data = (await response.json()) as { display_name?: string }
    return data.display_name?.trim() || null
  } catch {
    return null
  }
}
