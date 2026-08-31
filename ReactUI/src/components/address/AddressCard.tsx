import { useTranslation } from 'react-i18next'
import { AddressesIcon, DeleteIcon, EditIcon, StarIcon } from '@/components/dashboard/DashboardIcons'
import { AdminGridIconButton } from '@/components/dashboard/admin/AdminGridActions'
import type { SavedAddress } from '@/models/address/savedAddress.model'

interface AddressCardProps {
  address: SavedAddress
  selected?: boolean
  selectable?: boolean
  disabled?: boolean
  onSelect?: () => void
  onEdit?: () => void
  onDelete?: () => void
  onSetDefault?: () => void
}

export function AddressCard({
  address,
  selected = false,
  selectable = false,
  disabled = false,
  onSelect,
  onEdit,
  onDelete,
  onSetDefault,
}: AddressCardProps) {
  const { t } = useTranslation()
  const fullName = `${address.firstName} ${address.lastName}`.trim()
  const street = [address.address, address.apartment].filter(Boolean).join(', ')
  const contact = [address.postcode, address.phone].filter(Boolean).join(' · ')

  const iconTile = (
    <div
      className={`flex size-12 shrink-0 items-center justify-center rounded-lg ring-1 ${
        address.isDefault
          ? 'bg-warm-soft text-warm ring-warm/25'
          : 'bg-surface-muted text-text-muted ring-border/60'
      }`}
    >
      <AddressesIcon size={20} />
    </div>
  )

  const body = (
    <div className="min-w-0 flex-1 space-y-1 py-0.5">
      <div className="flex items-center gap-2">
        <h3 className="min-w-0 truncate text-xs font-semibold text-text">{address.label}</h3>
        {address.isDefault && (
          <span className="shrink-0 rounded-full bg-accent/15 px-2 py-0.5 text-[9px] font-semibold uppercase tracking-wide text-accent">
            {t('address.defaultBadge')}
          </span>
        )}
      </div>

      {fullName && <p className="line-clamp-1 text-xs text-text">{fullName}</p>}

      {street && (
        <p className="line-clamp-1 text-xs leading-relaxed text-text-muted">{street}</p>
      )}

      {contact && (
        <p dir="ltr" className="truncate text-[11px] tabular-nums text-text-muted">
          {contact}
        </p>
      )}
    </div>
  )

  const actions =
    !selectable && (onEdit || onDelete || onSetDefault) ? (
      <div className="flex shrink-0 items-center gap-1">
        {onEdit && (
          <AdminGridIconButton
            label={t('address.edit')}
            icon={<EditIcon size={14} />}
            onClick={onEdit}
            disabled={disabled}
          />
        )}
        {!address.isDefault && onSetDefault && (
          <AdminGridIconButton
            label={t('address.makeDefault')}
            icon={<StarIcon size={14} />}
            onClick={onSetDefault}
            disabled={disabled}
          />
        )}
        {onDelete && (
          <AdminGridIconButton
            label={t('address.delete')}
            icon={<DeleteIcon size={14} />}
            onClick={onDelete}
            disabled={disabled}
            tone="danger"
          />
        )}
      </div>
    ) : null

  if (selectable) {
    return (
      <button
        type="button"
        onClick={onSelect}
        disabled={disabled}
        className={`flex w-full items-start gap-3 overflow-hidden rounded-lg border p-3 text-start transition-all ${
          selected
            ? 'border-warm bg-warm-soft/40 ring-1 ring-warm/30'
            : 'border-border bg-surface hover:border-warm/40 hover:shadow-sm'
        } disabled:cursor-not-allowed disabled:opacity-50`}
      >
        {iconTile}
        <div className="flex min-w-0 flex-1 items-start justify-between gap-2">
          {body}
          <span
            className={`mt-0.5 flex size-5 shrink-0 items-center justify-center rounded-full border text-[10px] font-bold ${
              selected ? 'border-warm bg-warm text-warm-text' : 'border-border bg-surface'
            }`}
            aria-hidden
          >
            {selected ? '✓' : ''}
          </span>
        </div>
      </button>
    )
  }

  return (
    <article className="flex items-start gap-3 overflow-hidden rounded-lg border border-border bg-surface p-3 shadow-sm transition-shadow hover:shadow-md">
      {iconTile}
      <div className="flex min-w-0 flex-1 flex-col gap-2.5 sm:flex-row sm:items-end sm:justify-between">
        {body}
        {actions}
      </div>
    </article>
  )
}
