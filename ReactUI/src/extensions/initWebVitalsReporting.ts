/**
 * Optional Core Web Vitals reporting.
 * Set VITE_WEB_VITALS_ENDPOINT to POST metrics to your analytics backend.
 */
export function initWebVitalsReporting() {
  if (typeof window === 'undefined') return

  const endpoint = import.meta.env.VITE_WEB_VITALS_ENDPOINT as string | undefined
  if (!endpoint) return

  void import(/* @vite-ignore */ 'web-vitals')
    .then(({ onCLS, onINP, onLCP }) => {
      const send = (metric: { name: string; value: number; id: string }) => {
        const body = JSON.stringify({
          name: metric.name,
          value: metric.value,
          id: metric.id,
          path: window.location.pathname,
        })
        if (navigator.sendBeacon) {
          navigator.sendBeacon(endpoint, body)
        } else {
          void fetch(endpoint, {
            method: 'POST',
            body,
            headers: { 'Content-Type': 'application/json' },
            keepalive: true,
          })
        }
      }

      onCLS(send)
      onINP(send)
      onLCP(send)
    })
    .catch(() => {
      // web-vitals is optional — ignore if not installed
    })
}
