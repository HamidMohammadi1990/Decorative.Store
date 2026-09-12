import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { AddressCard } from '@/components/address/AddressCard'
import { AddressFormFields } from '@/components/address/AddressFormFields'
import { Button } from '@/components/ui/Button'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'
import { InlineLoading } from '@/components/ui/Spinner'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import type { MapCoordinates } from '@/config/map'
import {
  addressFormToInput,
  emptyAddressForm,
  validateAddressForm,
  type AddressFormErrorField,
  type AddressFormField,
  type AddressFormValues,
} from '@/extensions/validateAddressForm'
import { useAddressMutations } from '@/hooks/useAddressMutations'
import { useAddressStore } from '@/stores/addressStore'
import { useUserStore } from '@/stores/userStore'

type ModalMode = 'list' | 'add' | 'edit'

export function AddressBookModal() {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const isOpen = useAddressStore((s) => s.isModalOpen)
  const closeModal = useAddressStore((s) => s.closeModal)
  const addresses = useAddressStore((s) => s.addresses)
  const isLoading = useAddressStore((s) => s.isLoading)
  const loadAddresses = useAddressStore((s) => s.loadAddresses)
  const { locale } = useLocaleSettings()
  const { isSaving, mutationError, saveAddress, setAsDefault, deleteAddress, clearMutationError } =
    useAddressMutations()

  const [mode, setMode] = useState<ModalMode>('list')
  const [editingId, setEditingId] = useState<string | null>(null)
  const [form, setForm] = useState<AddressFormValues>(emptyAddressForm)
  const [coordinates, setCoordinates] = useState<MapCoordinates | null>(null)
  const [errors, setErrors] = useState<Partial<Record<AddressFormErrorField, string>>>({})
  const [makeDefault, setMakeDefault] = useState(false)

  useEffect(() => {
    if (!isOpen) {
      setMode('list')
      setEditingId(null)
      setForm(emptyAddressForm)
      setCoordinates(null)
      setErrors({})
      setMakeDefault(false)
      clearMutationError()
      return
    }

    const accessToken = useUserStore.getState().accessToken
    if (!accessToken || accessToken === 'mock-access-token') return

    void loadAddresses(accessToken, locale)
  }, [clearMutationError, isOpen, loadAddresses, locale])

  if (!isOpen) return null

  const startAdd = () => {
    clearMutationError()
    setMode('add')
    setEditingId(null)
    setForm(emptyAddressForm)
    setCoordinates(null)
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
    setCoordinates(
      address.latitude != null && address.longitude != null ?
        { latitude: address.latitude, longitude: address.longitude }
      : null,
    )
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

  const handleCoordinatesChange = (coords: MapCoordinates) => {
    setCoordinates(coords)
    if (errors.map) {
      setErrors((prev) => {
        const next = { ...prev }
        delete next.map
        return next
      })
    }
  }

  const handleAddressHint = (addressLine: string) => {
    setForm((prev) => ({ ...prev, address: addressLine }))
  }

  const handleSave = async () => {
    const nextErrors = validateAddressForm(
      form,
      {
        required: t('address.validation.required'),
        mapRequired: t('address.validation.mapRequired'),
      },
      coordinates,
    )
    setErrors(nextErrors)
    if (Object.keys(nextErrors).length > 0) return

    const input = addressFormToInput(form, { isDefault: makeDefault, coordinates })
    const saved = await saveAddress(input, {
      editingId,
      makeDefault,
    })

    if (!saved) return

    setMode('list')
    setEditingId(null)
    setForm(emptyAddressForm)
    setCoordinates(null)
    setMakeDefault(false)
  }

  const handleDelete = async (id: string) => {
    if (!(await confirm({ message: t('address.deleteConfirm') }))) return
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
            <p className="mb-4 rounded-sm border border-red-200 bg-red-50 px-3 py-2 text-sm text-sale">
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
                        onSetDefault={() => void setAsDefault(address)}
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
                coordinates={coordinates}
                mapSessionKey={editingId ?? 'new'}
                onChange={updateField}
                onCoordinatesChange={handleCoordinatesChange}
                onAddressHint={handleAddressHint}
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
