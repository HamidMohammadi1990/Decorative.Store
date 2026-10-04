const LEAFLET_VERSION = '1.9.4'

const CDN_BASES = [
  `https://cdn.jsdelivr.net/npm/leaflet@${LEAFLET_VERSION}/dist`,
  `https://unpkg.com/leaflet@${LEAFLET_VERSION}/dist`,
]

export const LEAFLET_MARKER_ICON_URL = `${CDN_BASES[0]}/images/marker-icon.png`
export const LEAFLET_MARKER_ICON_RETINA_URL = `${CDN_BASES[0]}/images/marker-icon-2x.png`
export const LEAFLET_MARKER_SHADOW_URL = `${CDN_BASES[0]}/images/marker-shadow.png`

export interface LeafletLatLng {
  lat: number
  lng: number
}

export interface LeafletMapInstance {
  on(event: 'click', handler: (event: { latlng: LeafletLatLng }) => void): void
  remove(): void
  panTo(latlng: [number, number], options?: { animate?: boolean }): void
  setView(latlng: [number, number], zoom: number, options?: { animate?: boolean }): void
  invalidateSize(): void
}

export interface LeafletMarkerInstance {
  setLatLng(latlng: [number, number] | LeafletLatLng): LeafletMarkerInstance
  getLatLng(): LeafletLatLng
  on(event: 'dragend', handler: () => void): LeafletMarkerInstance
  addTo(map: LeafletMapInstance): LeafletMarkerInstance
}

export interface LeafletModule {
  map(element: HTMLElement, options: Record<string, unknown>): LeafletMapInstance
  tileLayer(url: string, options: Record<string, unknown>): { addTo(map: LeafletMapInstance): void }
  marker(latlng: [number, number], options?: Record<string, unknown>): LeafletMarkerInstance
  Icon: {
    Default: {
      prototype: Record<string, unknown>
      mergeOptions(options: Record<string, string>): void
    }
  }
}

let loadPromise: Promise<LeafletModule> | null = null

function injectStylesheet(href: string, key: string) {
  if (document.querySelector(`link[data-leaflet="${key}"]`)) return

  const link = document.createElement('link')
  link.rel = 'stylesheet'
  link.href = href
  link.dataset.leaflet = key
  document.head.appendChild(link)
}

function injectScript(src: string): Promise<void> {
  return new Promise((resolve, reject) => {
    const existing = document.querySelector(`script[data-leaflet-src="${src}"]`)
    if (existing) {
      existing.addEventListener('load', () => resolve(), { once: true })
      existing.addEventListener('error', () => reject(new Error('Leaflet script failed')), {
        once: true,
      })
      if ((window as LeafletWindow).L) resolve()
      return
    }

    const script = document.createElement('script')
    script.src = src
    script.async = true
    script.dataset.leafletSrc = src
    script.onload = () => resolve()
    script.onerror = () => reject(new Error('Leaflet script failed'))
    document.head.appendChild(script)
  })
}

interface LeafletWindow extends Window {
  L?: LeafletModule
}

function configureDefaultMarkerIcon(L: LeafletModule) {
  delete L.Icon.Default.prototype._getIconUrl
  L.Icon.Default.mergeOptions({
    iconUrl: LEAFLET_MARKER_ICON_URL,
    iconRetinaUrl: LEAFLET_MARKER_ICON_RETINA_URL,
    shadowUrl: LEAFLET_MARKER_SHADOW_URL,
  })
}

async function loadFromPackage(): Promise<LeafletModule> {
  const pkg = 'leaflet'
  const cssPkg = 'leaflet/dist/leaflet.css'
  const [leafletModule] = await Promise.all([
    import(/* @vite-ignore */ pkg),
    import(/* @vite-ignore */ cssPkg),
  ])
  const L = (leafletModule.default ?? leafletModule) as LeafletModule
  configureDefaultMarkerIcon(L)
  return L
}

async function loadFromCdnBase(base: string): Promise<LeafletModule> {
  injectStylesheet(`${base}/leaflet.css`, `css-${base}`)
  await injectScript(`${base}/leaflet.js`)

  const L = (window as LeafletWindow).L
  if (!L) throw new Error('Leaflet failed to initialize')

  configureDefaultMarkerIcon(L)
  return L
}

async function loadFromAnyCdn(): Promise<LeafletModule> {
  let lastError: unknown
  for (const base of CDN_BASES) {
    try {
      return await loadFromCdnBase(base)
    } catch (error) {
      lastError = error
    }
  }
  throw lastError ?? new Error('Leaflet CDN unavailable')
}

async function loadLeafletInternal(): Promise<LeafletModule> {
  try {
    return await loadFromPackage()
  } catch {
    return loadFromAnyCdn()
  }
}

/** Loads Leaflet — npm package when installed, otherwise CDN with fallback. */
export function loadLeaflet(): Promise<LeafletModule> {
  if (!loadPromise) {
    loadPromise = loadLeafletInternal().catch((error) => {
      loadPromise = null
      throw error
    })
  }
  return loadPromise
}

/** Clears cached load state so the next call retries (e.g. after CDN failure). */
export function resetLeafletLoader() {
  loadPromise = null
}
