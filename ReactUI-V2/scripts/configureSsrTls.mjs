/** Allow Node fetch to call local dev APIs using HTTPS + self-signed certs. */
export function configureSsrTlsForLocalApi(apiTarget) {
  if (!apiTarget) return

  try {
    const url = new URL(apiTarget)
    const isLocalHost = url.hostname === 'localhost' || url.hostname === '127.0.0.1'

    if (url.protocol === 'https:' && isLocalHost) {
      process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'
      console.warn(
        `[ssr] TLS verification disabled for local API (${url.origin}). Use a trusted cert in production.`,
      )
    }
  } catch {
    // ignore invalid SSR_API_TARGET
  }
}
