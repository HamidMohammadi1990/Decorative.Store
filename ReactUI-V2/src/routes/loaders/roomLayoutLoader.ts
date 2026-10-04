import type { LoaderFunctionArgs } from 'react-router-dom'
import { ROOM_TYPE_FALLBACKS } from '@/models/room/roomLayout.model'
import { languageService } from '@/services/languageService'
import { roomTypeService } from '@/services/roomTypeService'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { RoomLayoutLoaderData } from '@/routes/loaders/types'

export async function roomLayoutLoader({
  request,
}: LoaderFunctionArgs): Promise<RoomLayoutLoaderData> {
  const locale = resolveLocale(request)

  try {
    const languageId = await languageService.resolveLanguageId(locale)
    const items = await roomTypeService.list(locale, languageId)

    if (items.length > 0) {
      return { locale, languageId, items, fromApi: true }
    }

    return {
      locale,
      languageId,
      items: ROOM_TYPE_FALLBACKS,
      fromApi: false,
    }
  } catch {
    return {
      locale,
      languageId: 0,
      items: ROOM_TYPE_FALLBACKS,
      fromApi: false,
      error: 'failed',
    }
  }
}
