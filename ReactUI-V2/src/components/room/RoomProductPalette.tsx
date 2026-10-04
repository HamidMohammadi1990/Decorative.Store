import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'
import { LocalImage } from '@/components/ui/LocalImage'
import { Button } from '@/components/ui/Button'
import { useRoomLayout } from '@/hooks/useRoomLayout'

export function RoomProductPalette() {
  const { t } = useTranslation()
  const { lines, paletteLines, placeFromLine } = useRoomLayout()

  if (lines.length === 0) {
    return (
      <div className="rounded-sm border border-border bg-surface p-5 shadow-sm">
        <h2 className="text-base font-semibold text-text">{t('roomLayout.paletteTitle')}</h2>
        <p className="mt-2 text-sm leading-relaxed text-text-muted">
          {t('roomLayout.paletteEmpty')}
        </p>
        <Link to="/" className="mt-4 inline-block">
          <Button variant="warm" className="text-sm">
            {t('roomLayout.browseProducts')}
          </Button>
        </Link>
      </div>
    )
  }

  return (
    <div className="rounded-sm border border-border bg-surface shadow-sm">
      <div className="border-b border-border/70 px-4 py-3.5">
        <h2 className="text-base font-semibold text-text">{t('roomLayout.paletteTitle')}</h2>
        <p className="mt-1 text-xs text-text-muted">{t('roomLayout.paletteHint')}</p>
      </div>

      <ul className="max-h-[min(28rem,50vh)] space-y-2 overflow-y-auto p-3 sm:p-4">
        {paletteLines.map(({ line, remaining }) => (
          <li key={line.lineId}>
            <div className="flex gap-3 rounded-sm border border-border bg-surface-muted/20 p-2.5">
              <div className="size-14 shrink-0 overflow-hidden rounded-sm border border-border bg-surface">
                <LocalImage
                  image={line.layoutImage ?? line.image}
                  className="size-full object-contain"
                />
              </div>
              <div className="min-w-0 flex-1">
                <p className="line-clamp-2 text-sm font-medium leading-snug text-text">
                  {line.title}
                </p>
                <p className="mt-1 text-xs text-text-muted">
                  {t('roomLayout.inCart', { count: line.quantity })}
                  {remaining > 0 && (
                    <span className="text-warm">
                      {' · '}
                      {t('roomLayout.canPlace', { count: remaining })}
                    </span>
                  )}
                </p>
                <div className="mt-2 flex flex-wrap gap-2">
                  <button
                    type="button"
                    draggable={remaining > 0}
                    disabled={remaining === 0}
                    onDragStart={(e) => {
                      e.dataTransfer.setData('application/x-room-line-id', line.lineId)
                      e.dataTransfer.effectAllowed = 'copy'
                    }}
                    onClick={() => {
                      const offset = (line.lineId.length % 5) * 4
                      placeFromLine(line, 48 + offset, 52 - offset)
                    }}
                    className="rounded-sm bg-warm px-2.5 py-1 text-[11px] font-semibold text-warm-text transition-opacity hover:bg-warm-hover disabled:cursor-not-allowed disabled:opacity-40"
                  >
                    {t('roomLayout.addToRoom')}
                  </button>
                </div>
              </div>
            </div>
          </li>
        ))}
      </ul>
    </div>
  )
}
