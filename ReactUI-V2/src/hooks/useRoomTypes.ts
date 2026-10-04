import { useEffect, useState } from 'react'
import { useRouteLoaderData } from 'react-router-dom'
import type { RoomTypeItem } from '@/models/admin/roomType.model'
import { ROOM_TYPE_FALLBACKS } from '@/models/room/roomLayout.model'
import { roomTypeService } from '@/services/roomTypeService'
import type { RoomLayoutLoaderData } from '@/routes/loaders/types'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'

export function useRoomTypes() {
  const { locale, languageId, loading: languageLoading } = useCurrentLanguageId()
  const loaderData = useRouteLoaderData('room-layout') as RoomLayoutLoaderData | undefined
  const loaderFresh = Boolean(loaderData && loaderData.locale === locale)

  const [items, setItems] = useState<RoomTypeItem[]>(() =>
    loaderFresh ? loaderData!.items : ROOM_TYPE_FALLBACKS,
  )
  const [loading, setLoading] = useState(() => !loaderFresh)
  const [fromApi, setFromApi] = useState(() => (loaderFresh ? loaderData!.fromApi : false))

  useEffect(() => {
    if (languageLoading || languageId == null) return

    if (loaderFresh) {
      setItems(loaderData!.items)
      setFromApi(loaderData!.fromApi)
      setLoading(false)
      return
    }

    let cancelled = false

    const load = async () => {
      setLoading(true)
      try {
        const types = await roomTypeService.list(locale, languageId)
        if (cancelled) return
        if (types.length > 0) {
          setItems(types)
          setFromApi(true)
        } else {
          setItems(ROOM_TYPE_FALLBACKS)
          setFromApi(false)
        }
      } catch {
        if (!cancelled) {
          setItems(ROOM_TYPE_FALLBACKS)
          setFromApi(false)
        }
      } finally {
        if (!cancelled) setLoading(false)
      }
    }

    void load()
    return () => {
      cancelled = true
    }
  }, [languageId, languageLoading, locale, loaderFresh, loaderData])

  return { items, loading: loading || languageLoading, fromApi }
}
