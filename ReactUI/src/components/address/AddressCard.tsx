import { useTranslation } from 'react-i18next'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import { formatSavedAddressLines } from '@/extensions/formatSavedAddress'

interface AddressCardProps {
  address: SavedAddress
  selected?: boolean
  selectable?: boolean
  onSelect?: () => void
  onEdit?: () => void
  onDelete?: () => void
  onSetDefault?: () => void
}

export function AddressCard({
  address,
  selected = false,
  selectable = false,
  onSelect,
  onEdit,
  onDelete,
  onSetDefault,
}: AddressCardProps) {
  const { t } = useTranslation()
  const lines = formatSavedAddressLines(address)

  const content = (
    <>
      <div className="flex items-start justify-between gap-3">
        <div className="min-w-0">
          <div className="flex flex-wrap items-center gap-2">
            <p className="text-sm font-semibold text-text">{address.label}</p>
            {address.isDefault && (
              <span className="rounded-sm bg-warm-soft px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide text-warm">
                {t('address.defaultBadge')}
              </span>
            )}
          </div>
          <ul className="mt-2 space-y-0.5 text-sm text-text-muted">
            {lines.map((line) => (
              <li key={line}>{line}</li>
            ))}
          </ul>
        </div>
        {selectable && (
          <span
            className={`mt-0.5 flex size-5 shrink-0 items-center justify-center rounded-full border ${
              selected ? 'border-warm bg-warm text-[10px] text-warm-text' : 'border-border'
            }`}
          >
            {selected ? '✓' : ''}
          </span>
        )}
      </div>

      {(onEdit || onDelete || onSetDefault) && (
        <div className="mt-3 flex flex-wrap gap-3 border-t border-border pt-3">
          {onEdit && (
            <button
              type="button"
              onClick={(e) => {
                e.stopPropagation()
                onEdit()
              }}
              className="text-xs font-medium text-warm hover:underline"
            >
              {t('address.edit')}
            </button>
          )}
          {!address.isDefault && onSetDefault && (
            <button
              type="button"
              onClick={(e) => {
                e.stopPropagation()
                onSetDefault()
              }}
              className="text-xs font-medium text-text-muted hover:text-text"
            >
              {t('address.makeDefault')}
            </button>
          )}
          {onDelete && (
            <button
              type="button"
              onClick={(e) => {
                e.stopPropagation()
                onDelete()
              }}
              className="text-xs font-medium text-sale hover:underline"
            >
              {t('address.delete')}
            </button>
          )}
        </div>
      )}
    </>
  )

  if (selectable) {
    return (
      <button
        type="button"
        onClick={onSelect}
        className={`w-full rounded-sm border p-4 text-start transition-colors ${
          selected
            ? 'border-warm bg-warm-soft ring-1 ring-warm/30'
            : 'border-border bg-surface hover:border-warm-muted'
        }`}
      >
        {content}
      </button>
    )
  }

  return (
    <div className="rounded-sm border border-border bg-surface p-4">{content}</div>
  )
}
