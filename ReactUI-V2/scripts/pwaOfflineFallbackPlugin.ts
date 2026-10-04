/** Workbox plugin — serialized into the service worker at build time. */
export function offlineFallbackPlugin() {
  return {
    handlerDidError: async (): Promise<Response> => {
      const cacheStorage = (globalThis as unknown as { caches: CacheStorage }).caches
      const cached = await cacheStorage.match('/offline.html', { ignoreSearch: true })
      if (cached) return cached

      return new Response('Offline', {
        status: 503,
        headers: { 'Content-Type': 'text/plain; charset=utf-8' },
      })
    },
  }
}
