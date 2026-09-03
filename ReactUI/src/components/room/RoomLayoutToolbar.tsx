import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import { Button } from '@/components/ui/Button'
import { useRoomLayout } from '@/hooks/useRoomLayout'
import { useRoomTypeSelection } from '@/hooks/useRoomTypeSelection'
import { RemoteImage } from '@/components/ui/RemoteImage'

export function RoomLayoutToolbar() {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const { items, loading, roomTypeId, setRoomTypeId } = useRoomTypeSelection()
  const {
    selectedItem,
    showGrid,
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
        {loading ? (
          <p className="mt-2 text-xs text-text-muted">{t('common.loading')}</p>
        ) : (
          <div className="mt-2 flex flex-wrap gap-2">
            {items.map((type) => (
              <button
                key={type.id}
                type="button"
                onClick={() => setRoomTypeId(type.id)}
                className={`overflow-hidden rounded-sm border transition-colors ${
                  roomTypeId === type.id
                    ? 'border-warm ring-2 ring-warm/30'
                    : 'border-border hover:border-warm/40'
                }`}
              >
                <span className="block size-14 overflow-hidden bg-surface-muted sm:size-16">
                  <RemoteImage
                    src={type.imageUrl}
                    alt=""
                    sizes="64px"
                    className="size-full object-cover"
                    draggable={false}
                  />
                </span>
                <span
                  className={`block px-2 py-1.5 text-center text-[11px] font-medium ${
                    roomTypeId === type.id
                      ? 'bg-warm text-warm-text'
                      : 'bg-surface text-text-muted'
                  }`}
                >
                  {type.title}
                </span>
              </button>
            ))}
          </div>
        )}
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
            <ToolbarBtn label="−" onClick={() => scaleItem(selectedItem.id, -0.08)} />
            <ToolbarBtn label="+" onClick={() => scaleItem(selectedItem.id, 0.08)} />
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
