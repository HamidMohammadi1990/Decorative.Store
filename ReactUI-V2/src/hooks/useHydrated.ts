import { useEffect, useState } from 'react'

/** True after the client has hydrated — use to defer browser-only store state. */
export function useHydrated() {
  const [hydrated, setHydrated] = useState(false)

  useEffect(() => {
    setHydrated(true)
  }, [])

  return hydrated
}
