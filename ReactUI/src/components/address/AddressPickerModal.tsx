import { useTranslation } from 'react-i18next'
import { AddressCard } from '@/components/address/AddressCard'
import { Button } from '@/components/ui/Button'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import { useAddressStore } from '@/stores/addressStore'

interface AddressPickerModalProps {
  open: boolean
  selectedId: string | null
  onClose: () => void
  onSelect: (address: SavedAddress) => void
}

export function AddressPickerModal({
  open,
  selectedId,
  onClose,
  onSelect,
}: AddressPickerModalProps) {
  const { t } = useTranslation()
  const addresses = useAddressStore((s) => s.addresses)
  const openAddressBook = useAddressStore((s) => s.openModal)

  if (!open) return null

  return (
    <Portal>
      <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 sm:p-6">
        <button
          type="button"
          aria-label={t('common.close')}
          className="absolute inset-0 bg-black/40"
          onClick={onClose}
        />
        <div
          role="dialog"
          aria-modal="true"
          aria-label={t('checkout.changeAddress')}
          className="relative z-10 flex max-h-[min(88vh,36rem)] w-full max-w-md flex-col overflow-hidden rounded-sm bg-surface shadow-2xl"
        >
        <header className="flex items-center justify-between border-b border-border px-5 py-4">
          <h2 className="text-lg font-semibold text-text">{t('checkout.changeAddress')}</h2>
          <button
            type="button"
            onClick={onClose}
            aria-label={t('common.close')}
            className="flex size-9 items-center justify-center rounded-full text-text-muted hover:bg-surface-muted"
          >
            <CloseIcon />
          </button>
        </header>

        <div className="overflow-y-auto px-5 py-4">
          {addresses.length === 0 ? (
            <div className="rounded-sm border border-dashed border-border bg-surface-muted px-4 py-8 text-center">
              <p className="text-sm text-text-muted">{t('checkout.noSavedAddressHint')}</p>
              <Button
                variant="warm"
                className="mt-4"
                onClick={() => {
                  onClose()
                  openAddressBook()
                }}
              >
                {t('address.addNew')}
              </Button>
            </div>
          ) : (
            <ul className="space-y-3">
              {addresses.map((address) => (
                <li key={address.id}>
                  <AddressCard
                    address={address}
                    selectable
                    selected={selectedId === address.id}
                    onSelect={() => {
                      onSelect(address)
                      onClose()
                    }}
                  />
                </li>
              ))}
            </ul>
          )}
        </div>

        {addresses.length > 0 && (
          <footer className="border-t border-border px-5 py-4">
            <button
              type="button"
              onClick={() => {
                onClose()
                openAddressBook()
              }}
              className="w-full text-center text-sm font-medium text-warm hover:underline"
            >
              {t('checkout.manageAddresses')}
            </button>
          </footer>
        )}
        </div>
      </div>
    </Portal>
  )
}
