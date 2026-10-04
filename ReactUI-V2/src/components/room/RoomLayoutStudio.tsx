import { RoomCanvas } from '@/components/room/RoomCanvas'
import { RoomLayoutToolbar } from '@/components/room/RoomLayoutToolbar'
import { RoomProductPalette } from '@/components/room/RoomProductPalette'

export function RoomLayoutStudio() {
  return (
    <div className="grid gap-5 lg:grid-cols-[minmax(0,1fr)_minmax(260px,300px)] lg:items-start">
      <div className="space-y-5">
        <RoomCanvas />
        <div className="lg:hidden">
          <RoomLayoutToolbar />
        </div>
      </div>

      <div className="space-y-5">
        <RoomProductPalette />
        <div className="hidden lg:block">
          <RoomLayoutToolbar />
        </div>
      </div>
    </div>
  )
}
