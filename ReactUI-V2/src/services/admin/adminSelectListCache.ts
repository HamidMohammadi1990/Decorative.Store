export function createAdminSelectCache<T>() {
  const resultCache = new Map<string, T>()
  const inFlight = new Map<string, Promise<T>>()

  return {
    get(key: string, loader: () => Promise<T>): Promise<T> {
      const cached = resultCache.get(key)
      if (cached) return Promise.resolve(cached)

      const pending = inFlight.get(key)
      if (pending) return pending

      const promise = loader()
        .then((result) => {
          resultCache.set(key, result)
          return result
        })
        .catch((error) => {
          inFlight.delete(key)
          throw error
        })

      inFlight.set(key, promise)
      return promise.finally(() => {
        if (inFlight.get(key) === promise) {
          inFlight.delete(key)
        }
      })
    },

    invalidate() {
      resultCache.clear()
      inFlight.clear()
    },
  }
}

export function buildAdminSelectCacheKey(
  accessToken: string,
  locale: string,
  languageId?: number,
  scope = '',
) {
  return `${accessToken}:${locale}:${languageId ?? ''}:${scope}`
}
