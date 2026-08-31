import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import type { RoomPresetId } from '@/models/room/roomLayout.model'
import { Button } from '@/components/ui/Button'
import { useRoomLayout } from '@/hooks/useRoomLayout'

const PRESETS: RoomPresetId[] = ['living', 'bedroom', 'dining']

export function RoomLayoutToolbar() {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const {
    roomPreset,
    selectedItem,
    showGrid,
    setRoomPreset,
    setShowGrid,
    rotateItem,
    scaleItem,
    bringToFront,
    removeItem,
    clearLayout,
    placedItems,
  } = useRoomLayout()

  const handleClearLayout = async () => {
    if (!(await confirm({ message: t('roomLayout.clearConfirm') }))) return
    clearLayout()
  }

  const handleRemoveItem = async (id: string) => {
    if (!(await confirm({ message: t('roomLayout.removeConfirm') }))) return
    removeItem(id)
  }

  return (
    <div className="flex flex-col gap-4 rounded-sm border border-border bg-surface p-4 shadow-sm sm:p-5">
      <div>
        <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-text-muted">
          {t('roomLayout.roomType')}
        </p>
        <div className="mt-2 flex flex-wrap gap-2">
          {PRESETS.map((preset) => (
            <button
              key={preset}
              type="button"
              onClick={() => setRoomPreset(preset)}
              className={`rounded-sm border px-3 py-1.5 text-xs font-medium transition-colors ${
                roomPreset === preset
                  ? 'border-warm bg-warm text-warm-text shadow-sm'
                  : 'border-border bg-surface text-text-muted hover:border-warm/40 hover:text-warm'
              }`}
            >
              {t(`roomLayout.presets.${preset}`)}
            </button>
          ))}
        </div>
      </div>

      <div className="flex flex-wrap items-center gap-2">
        <button
          type="button"
          onClick={() => setShowGrid(!showGrid)}
          className={`rounded-sm border px-3 py-1.5 text-xs font-medium ${
            showGrid
              ? 'border-warm/40 bg-warm-soft text-warm'
              : 'border-border text-text-muted hover:bg-surface-muted'
          }`}
        >
          {t('roomLayout.toggleGrid')}
        </button>
        <button
          type="button"
          onClick={() => void handleClearLayout()}
          disabled={placedItems.length === 0}
          className="rounded-sm border border-border px-3 py-1.5 text-xs font-medium text-text-muted hover:bg-surface-muted disabled:opacity-40"
        >
          {t('roomLayout.clearAll')}
        </button>
      </div>

      {selectedItem && (
        <div className="border-t border-border/70 pt-4">
          <p className="text-xs font-semibold text-text">{t('roomLayout.selectedItem')}</p>
          <p className="mt-0.5 line-clamp-1 text-xs text-text-muted">{selectedItem.title}</p>
          <div className="mt-3 flex flex-wrap gap-2">
            <ToolbarBtn
              label={t('roomLayout.rotateLeft')}
              onClick={() => rotateItem(selectedItem.id, -15)}
            />
            <ToolbarBtn
              label={t('roomLayout.rotateRight')}
              onClick={() => rotateItem(selectedItem.id, 15)}
            />
            <ToolbarBtn
              label="−"
              onClick={() => scaleItem(selectedItem.id, -0.08)}
            />
            <ToolbarBtn
              label="+"
              onClick={() => scaleItem(selectedItem.id, 0.08)}
            />
            <ToolbarBtn
              label={t('roomLayout.bringFront')}
              onClick={() => bringToFront(selectedItem.id)}
            />
            <ToolbarBtn
              label={t('roomLayout.remove')}
              onClick={() => void handleRemoveItem(selectedItem.id)}
              danger
            />
          </div>
        </div>
      )}

      <LinkToCheckout />
    </div>
  )
}

function ToolbarBtn({
  label,
  onClick,
  danger = false,
}: {
  label: string
  onClick: () => void
  danger?: boolean
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      className={`rounded-sm border px-2.5 py-1.5 text-xs font-medium ${
        danger
          ? 'border-sale/30 text-sale hover:bg-sale/5'
          : 'border-border text-text hover:bg-surface-muted'
      }`}
    >
      {label}
    </button>
  )
}

function LinkToCheckout() {
  const { t } = useTranslation()
  const { lines } = useRoomLayout()
  if (lines.length === 0) return null

  return (
    <div className="border-t border-border/70 pt-4">
      <Link to="/checkout">
        <Button variant="warm" className="w-full text-sm">
          {t('roomLayout.goCheckout')}
        </Button>
      </Link>
    </div>
  )
}
