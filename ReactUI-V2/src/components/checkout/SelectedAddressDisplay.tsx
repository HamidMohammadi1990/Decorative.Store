import { useTranslation } from 'react-i18next'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import { formatSavedAddressLines } from '@/extensions/formatSavedAddress'

interface SelectedAddressDisplayProps {
  address: SavedAddress
  onChange: () => void
}

export function SelectedAddressDisplay({ address, onChange }: SelectedAddressDisplayProps) {
  const { t } = useTranslation()
  const lines = formatSavedAddressLines(address)

  return (
    <div className="rounded-sm border border-warm-muted bg-warm-soft/60 p-4">
      <div className="flex items-start justify-between gap-3">
        <div className="min-w-0">
          <p className="text-xs font-semibold uppercase tracking-wider text-warm">
            {t('checkout.shipToSelected')}
          </p>
          <div className="mt-2 flex flex-wrap items-center gap-2">
            <p className="text-sm font-semibold text-text">{address.label}</p>
            {address.isDefault && (
              <span className="rounded-sm bg-surface px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide text-warm">
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
        <button
          type="button"
          onClick={onChange}
          className="shrink-0 rounded-sm border border-warm px-3 py-1.5 text-xs font-semibold text-warm transition-colors hover:bg-warm hover:text-warm-text"
        >
          {t('checkout.changeAddress')}
        </button>
      </div>
    </div>
  )
}
