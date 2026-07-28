import { useCallback, useRef, type PointerEvent as ReactPointerEvent } from 'react'
import { useTranslation } from 'react-i18next'
import type { PlacedLayoutItem } from '@/models/room/roomLayout.model'
import { ROOM_PRESETS } from '@/models/room/roomLayout.model'
import { LocalImage } from '@/components/ui/LocalImage'
import { useRoomLayout } from '@/hooks/useRoomLayout'

function clamp(value: number, min: number, max: number) {
  return Math.min(max, Math.max(min, value))
}

export function RoomCanvas() {
  const { t } = useTranslation()
  const canvasRef = useRef<HTMLDivElement>(null)
  const dragRef = useRef<{ id: string; offsetX: number; offsetY: number } | null>(null)
  const {
    roomPreset,
    placedItems,
    selectedId,
    showGrid,
    selectItem,
    updateItem,
    placeFromLine,
    lines,
  } = useRoomLayout()

  const preset = ROOM_PRESETS.find((p) => p.id === roomPreset) ?? ROOM_PRESETS[0]

  const toPercent = useCallback((clientX: number, clientY: number) => {
    const rect = canvasRef.current?.getBoundingClientRect()
    if (!rect) return { x: 50, y: 50 }
    return {
      x: clamp(((clientX - rect.left) / rect.width) * 100, 4, 96),
      y: clamp(((clientY - rect.top) / rect.height) * 100, 6, 94),
    }
  }, [])

  const onCanvasPointerDown = (e: ReactPointerEvent<HTMLDivElement>) => {
    if (e.target === e.currentTarget || (e.target as HTMLElement).dataset.canvasBg !== undefined) {
      selectItem(null)
    }
  }

  const onItemPointerDown = (e: ReactPointerEvent<HTMLButtonElement>, item: PlacedLayoutItem) => {
    e.stopPropagation()
    selectItem(item.id)
    const rect = canvasRef.current?.getBoundingClientRect()
    if (!rect) return
    const centerX = rect.left + (item.x / 100) * rect.width
    const centerY = rect.top + (item.y / 100) * rect.height
    dragRef.current = {
      id: item.id,
      offsetX: e.clientX - centerX,
      offsetY: e.clientY - centerY,
    }
    e.currentTarget.setPointerCapture(e.pointerId)
  }

  const onItemPointerMove = (e: ReactPointerEvent<HTMLButtonElement>) => {
    const drag = dragRef.current
    if (!drag) return
    const { x, y } = toPercent(e.clientX - drag.offsetX, e.clientY - drag.offsetY)
    updateItem(drag.id, { x, y })
  }

  const onItemPointerUp = () => {
    dragRef.current = null
  }

  const onCanvasDrop = (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault()
    const lineId = e.dataTransfer.getData('application/x-room-line-id')
    const line = lines.find((l) => l.lineId === lineId)
    if (!line) return
    const { x, y } = toPercent(e.clientX, e.clientY)
    placeFromLine(line, x, y)
  }

  return (
    <div className="relative overflow-hidden rounded-sm border border-border bg-surface shadow-md">
      <div className="border-b border-border/70 px-4 py-3 sm:px-5">
        <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-warm">
          {t('roomLayout.canvasEyebrow')}
        </p>
        <p className="mt-0.5 text-sm text-text-muted">{t('roomLayout.canvasHint')}</p>
      </div>

      <div
        ref={canvasRef}
        role="application"
        aria-label={t('roomLayout.canvasAria')}
        data-canvas-bg
        className={`room-layout-canvas relative aspect-[4/3] w-full touch-none select-none sm:aspect-[16/10] ${
          showGrid ? 'room-layout-grid' : ''
        }`}
        onPointerDown={onCanvasPointerDown}
        onDragOver={(e) => e.preventDefault()}
        onDrop={onCanvasDrop}
      >
        <img
          src={preset.imageSrc}
          alt=""
          className="pointer-events-none absolute inset-0 size-full object-cover"
          draggable={false}
        />
        <div
          data-canvas-bg
          className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/35 via-black/10 to-black/5"
        />

        {placedItems.length === 0 && (
          <div className="pointer-events-none absolute inset-0 flex items-center justify-center p-6">
            <p className="max-w-xs rounded-sm border border-white/25 bg-black/35 px-4 py-3 text-center text-sm leading-relaxed text-white/90 backdrop-blur-sm">
              {t('roomLayout.canvasEmpty')}
            </p>
          </div>
        )}

        {placedItems.map((item) => (
          <button
            key={item.id}
            type="button"
            data-item-id={item.id}
            aria-label={item.title}
            onPointerDown={(e) => onItemPointerDown(e, item)}
            onPointerMove={onItemPointerMove}
            onPointerUp={onItemPointerUp}
            onLostPointerCapture={onItemPointerUp}
            className={`room-layout-item absolute flex items-center justify-center overflow-hidden rounded-sm border bg-white/90 p-1 shadow-lg transition-shadow ${
              selectedId === item.id
                ? 'border-warm ring-2 ring-warm/40'
                : 'border-white/80 hover:border-warm/50'
            } ${dragRef.current?.id === item.id ? 'cursor-grabbing' : 'cursor-grab'}`}
            style={{
              left: `${item.x}%`,
              top: `${item.y}%`,
              width: `${item.widthPct}%`,
              height: `${item.heightPct}%`,
              zIndex: item.zIndex,
              transform: `translate(-50%, -50%) rotate(${item.rotation}deg) scale(${item.scale})`,
            }}
          >
            <LocalImage image={item.image} className="max-h-full max-w-full object-contain" />
          </button>
        ))}
      </div>
    </div>
  )
}
