import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { AddressCard } from '@/components/address/AddressCard'
import { AddressFormFields } from '@/components/address/AddressFormFields'
import { Button } from '@/components/ui/Button'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import {
  addressFormToInput,
  emptyAddressForm,
  validateAddressForm,
  type AddressFormField,
  type AddressFormValues,
} from '@/extensions/validateAddressForm'
import { useAddressStore } from '@/stores/addressStore'

type ModalMode = 'list' | 'add' | 'edit'

export function AddressBookModal() {
  const { t } = useTranslation()
  const isOpen = useAddressStore((s) => s.isModalOpen)
  const closeModal = useAddressStore((s) => s.closeModal)
  const addresses = useAddressStore((s) => s.addresses)
  const addAddress = useAddressStore((s) => s.addAddress)
  const updateAddress = useAddressStore((s) => s.updateAddress)
  const removeAddress = useAddressStore((s) => s.removeAddress)
  const setDefaultAddress = useAddressStore((s) => s.setDefaultAddress)

  const [mode, setMode] = useState<ModalMode>('list')
  const [editingId, setEditingId] = useState<string | null>(null)
  const [form, setForm] = useState<AddressFormValues>(emptyAddressForm)
  const [errors, setErrors] = useState<Partial<Record<AddressFormField, string>>>({})
  const [makeDefault, setMakeDefault] = useState(false)

  useEffect(() => {
    if (!isOpen) {
      setMode('list')
      setEditingId(null)
      setForm(emptyAddressForm)
      setErrors({})
      setMakeDefault(false)
    }
  }, [isOpen])

  if (!isOpen) return null

  const startAdd = () => {
    setMode('add')
    setEditingId(null)
    setForm(emptyAddressForm)
    setErrors({})
    setMakeDefault(addresses.length === 0)
  }

  const startEdit = (address: SavedAddress) => {
    setMode('edit')
    setEditingId(address.id)
    setForm({
      label: address.label,
      firstName: address.firstName,
      lastName: address.lastName,
      address: address.address,
      apartment: address.apartment,
      city: address.city,
      postcode: address.postcode,
      phone: address.phone,
    })
    setErrors({})
    setMakeDefault(address.isDefault)
  }

  const updateField = (field: AddressFormField, value: string) => {
    setForm((prev) => ({ ...prev, [field]: value }))
    if (errors[field]) {
      setErrors((prev) => {
        const next = { ...prev }
        delete next[field]
        return next
      })
    }
  }

  const handleSave = () => {
    const nextErrors = validateAddressForm(form, {
      required: t('address.validation.required'),
    })
    setErrors(nextErrors)
    if (Object.keys(nextErrors).length > 0) return

    const input = addressFormToInput(form, makeDefault)

    if (mode === 'edit' && editingId) {
      updateAddress(editingId, input)
    } else {
      addAddress(input)
    }

    setMode('list')
    setEditingId(null)
    setForm(emptyAddressForm)
    setMakeDefault(false)
  }

  return (
    <Portal>
      <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 sm:p-6">
        <button
          type="button"
          aria-label={t('common.close')}
          className="absolute inset-0 bg-black/40"
          onClick={closeModal}
        />
        <div
          role="dialog"
          aria-modal="true"
          aria-label={t('address.modalTitle')}
          className="relative z-10 flex max-h-[min(88vh,40rem)] w-full max-w-lg flex-col overflow-hidden rounded-sm bg-surface shadow-2xl"
        >
        <header className="flex items-center justify-between border-b border-border px-5 py-4">
          <div>
            <h2 className="text-lg font-semibold text-text">
              {mode === 'list'
                ? t('address.modalTitle')
                : mode === 'add'
                  ? t('address.addTitle')
                  : t('address.editTitle')}
            </h2>
            {mode === 'list' && (
              <p className="mt-0.5 text-sm text-text-muted">{t('address.modalSubtitle')}</p>
            )}
          </div>
          <button
            type="button"
            onClick={closeModal}
            aria-label={t('common.close')}
            className="flex size-9 items-center justify-center rounded-full text-text-muted hover:bg-surface-muted"
          >
            <CloseIcon />
          </button>
        </header>

        <div className="overflow-y-auto px-5 py-4">
          {mode === 'list' ? (
            <>
              {addresses.length === 0 ? (
                <div className="rounded-sm border border-dashed border-border bg-surface-muted px-4 py-10 text-center">
                  <p className="text-sm font-medium text-text">{t('address.emptyTitle')}</p>
                  <p className="mt-1 text-sm text-text-muted">{t('address.emptyMessage')}</p>
                </div>
              ) : (
                <ul className="space-y-3">
                  {addresses.map((address) => (
                    <li key={address.id}>
                      <AddressCard
                        address={address}
                        onEdit={() => startEdit(address)}
                        onDelete={() => removeAddress(address.id)}
                        onSetDefault={() => setDefaultAddress(address.id)}
                      />
                    </li>
                  ))}
                </ul>
              )}

              <Button variant="warm" className="mt-4 w-full" onClick={startAdd}>
                {t('address.addNew')}
              </Button>
            </>
          ) : (
            <>
              <AddressFormFields
                values={form}
                errors={errors}
                onChange={updateField}
                showDefaultCheckbox
                isDefault={makeDefault}
                onDefaultChange={setMakeDefault}
              />
              <div className="mt-5 flex gap-3">
                <Button variant="warm" className="flex-1" onClick={handleSave}>
                  {t('address.save')}
                </Button>
                <Button
                  variant="secondary"
                  className="flex-1"
                  onClick={() => {
                    setMode('list')
                    setEditingId(null)
                    setErrors({})
                  }}
                >
                  {t('address.cancel')}
                </Button>
              </div>
            </>
          )}
        </div>
        </div>
      </div>
    </Portal>
  )
}
