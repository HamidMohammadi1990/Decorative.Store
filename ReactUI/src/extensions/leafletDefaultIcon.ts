import L from 'leaflet'
import iconUrl from 'leaflet/dist/images/marker-icon.png?url'
import iconRetinaUrl from 'leaflet/dist/images/marker-icon-2x.png?url'
import shadowUrl from 'leaflet/dist/images/marker-shadow.png?url'

// Vite does not resolve Leaflet's default marker asset paths automatically.
// eslint-disable-next-line @typescript-eslint/no-explicit-any
delete (L.Icon.Default.prototype as any)._getIconUrl
L.Icon.Default.mergeOptions({
  iconUrl,
  iconRetinaUrl,
  shadowUrl,
})
