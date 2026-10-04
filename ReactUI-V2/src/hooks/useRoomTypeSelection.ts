import { useEffect } from 'react'
import { useRoomLayoutStore } from '@/stores/roomLayoutStore'
import { findRoomTypeById } from '@/models/room/roomLayout.model'
import { useRoomTypes } from '@/hooks/useRoomTypes'

/** Keeps persisted room type id valid when API types load. */
export function useRoomTypeSelection() {
  const { items, loading } = useRoomTypes()
  const roomTypeId = useRoomLayoutStore((s) => s.roomTypeId)
  const setRoomTypeId = useRoomLayoutStore((s) => s.setRoomTypeId)

  useEffect(() => {
    if (loading || items.length === 0) return

    const current = findRoomTypeById(items, roomTypeId)
    if (!current) {
      setRoomTypeId(items[0].id)
    }
  }, [items, loading, roomTypeId, setRoomTypeId])

  const active = findRoomTypeById(items, roomTypeId) ?? items[0] ?? null

  return { items, loading, active, roomTypeId, setRoomTypeId }
}
