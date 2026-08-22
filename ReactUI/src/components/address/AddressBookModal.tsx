import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { AddressCard } from '@/components/address/AddressCard'
import { AddressFormFields } from '@/components/address/AddressFormFields'
import { Button } from '@/components/ui/Button'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'
import { InlineLoading } from '@/components/ui/Spinner'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import {
  addressFormToInput,
  emptyAddressForm,
  validateAddressForm,
  type AddressFormField,
  type AddressFormValues,
} from '@/extensions/validateAddressForm'
import { useAddressMutations } from '@/hooks/useAddressMutations'
import { useAddressStore } from '@/stores/addressStore'

type ModalMode = 'list' | 'add' | 'edit'

export function AddressBookModal() {
  const { t } = useTranslation()
  const isOpen = useAddressStore((s) => s.isModalOpen)
  const closeModal = useAddressStore((s) => s.closeModal)
  const addresses = useAddressStore((s) => s.addresses)
  const isLoading = useAddressStore((s) => s.isLoading)
  const setDefaultAddress = useAddressStore((s) => s.setDefaultAddress)
  const { isSaving, mutationError, saveAddress, deleteAddress, clearMutationError } =
    useAddressMutations()

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
      clearMutationError()
    }
  }, [clearMutationError, isOpen])

  if (!isOpen) return null

  const startAdd = () => {
    clearMutationError()
    setMode('add')
    setEditingId(null)
    setForm(emptyAddressForm)
    setErrors({})
    setMakeDefault(addresses.length === 0)
  }

  const startEdit = (address: SavedAddress) => {
    clearMutationError()
    setMode('edit')
    setEditingId(address.id)
    setForm({
      label: address.label,
      firstName: address.firstName,
      lastName: address.lastName,
      address: address.address,
      apartment: address.apartment,
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

  const handleSave = async () => {
    const nextErrors = validateAddressForm(form, {
      required: t('address.validation.required'),
    })
    setErrors(nextErrors)
    if (Object.keys(nextErrors).length > 0) return

    const input = addressFormToInput(form, makeDefault)
    const saved = await saveAddress(input, {
      editingId,
      makeDefault,
    })

    if (!saved) return

    setMode('list')
    setEditingId(null)
    setForm(emptyAddressForm)
    setMakeDefault(false)
  }

  const handleDelete = async (id: string) => {
    await deleteAddress(id)
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
          {mutationError && (
            <p className="mb-4 rounded-sm border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
              {mutationError}
            </p>
          )}

          {mode === 'list' ? (
            <>
              {isLoading ? (
                <div className="flex justify-center py-10">
                  <InlineLoading label={t('address.loading')} />
                </div>
              ) : addresses.length === 0 ? (
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
                        onDelete={() => void handleDelete(address.id)}
                        onSetDefault={() => setDefaultAddress(address.id)}
                      />
                    </li>
                  ))}
                </ul>
              )}

              <Button variant="warm" className="mt-4 w-full" onClick={startAdd} disabled={isSaving}>
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
                <Button variant="warm" className="flex-1" onClick={() => void handleSave()} disabled={isSaving}>
                  {isSaving ? <InlineLoading label={t('address.save')} /> : t('address.save')}
                </Button>
                <Button
                  variant="secondary"
                  className="flex-1"
                  disabled={isSaving}
                  onClick={() => {
                    setMode('list')
                    setEditingId(null)
                    setErrors({})
                    clearMutationError()
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
