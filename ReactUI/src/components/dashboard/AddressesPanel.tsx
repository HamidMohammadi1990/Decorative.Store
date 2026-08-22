import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { AddressCard } from '@/components/address/AddressCard'
import { AddressFormFields } from '@/components/address/AddressFormFields'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { AddressesIcon } from '@/components/dashboard/DashboardIcons'
import { Button } from '@/components/ui/Button'
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

type PanelMode = 'list' | 'add' | 'edit'

export function AddressesPanel() {
  const { t } = useTranslation()
  const addresses = useAddressStore((s) => s.addresses)
  const isLoading = useAddressStore((s) => s.isLoading)
  const setDefaultAddress = useAddressStore((s) => s.setDefaultAddress)
  const { isSaving, mutationError, saveAddress, deleteAddress, clearMutationError } =
    useAddressMutations()

  const [mode, setMode] = useState<PanelMode>('list')
  const [editingId, setEditingId] = useState<string | null>(null)
  const [form, setForm] = useState<AddressFormValues>(emptyAddressForm)
  const [errors, setErrors] = useState<Partial<Record<AddressFormField, string>>>({})
  const [makeDefault, setMakeDefault] = useState(false)

  useEffect(() => {
    if (mode === 'list') {
      setEditingId(null)
      setForm(emptyAddressForm)
      setErrors({})
      setMakeDefault(false)
      clearMutationError()
    }
  }, [clearMutationError, mode])

  const startAdd = () => {
    clearMutationError()
    setMode('add')
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
  }

  const handleDelete = async (id: string) => {
    await deleteAddress(id)
  }

  if (mode === 'add' || mode === 'edit') {
    return (
      <div>
        <DashboardPageHeader
          title={mode === 'add' ? t('address.addTitle') : t('address.editTitle')}
          description={t('dashboard.addresses.formDescription')}
          icon={<AddressesIcon size={22} />}
        />

        <div className="rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm ring-1 ring-border/50 sm:p-6">
          {mutationError && (
            <p className="mb-4 rounded-sm border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
              {mutationError}
            </p>
          )}

          <AddressFormFields
            values={form}
            errors={errors}
            onChange={updateField}
            showDefaultCheckbox
            isDefault={makeDefault}
            onDefaultChange={setMakeDefault}
          />

          <div className="mt-6 flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={isSaving}>
              {isSaving ? <InlineLoading label={t('address.save')} /> : t('address.save')}
            </Button>
            <Button variant="secondary" onClick={() => setMode('list')} disabled={isSaving}>
              {t('address.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.addresses.title')}
        description={t('dashboard.addresses.description')}
        icon={<AddressesIcon size={22} />}
        action={
          <Button variant="warm" onClick={startAdd} disabled={isSaving}>
            {t('address.addNew')}
          </Button>
        }
      />

      {mutationError && (
        <p className="mb-4 rounded-sm border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
          {mutationError}
        </p>
      )}

      {isLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('address.loading')} />
        </div>
      ) : addresses.length === 0 ? (
        <DashboardEmptyState
          icon={<AddressesIcon size={28} />}
          title={t('address.emptyTitle')}
          message={t('address.emptyMessage')}
          action={
            <Button variant="warm" onClick={startAdd}>
              {t('address.addNew')}
            </Button>
          }
        />
      ) : (
        <div className="grid gap-4 sm:grid-cols-2">
          {addresses.map((address) => (
            <AddressCard
              key={address.id}
              address={address}
              onEdit={() => startEdit(address)}
              onDelete={() => void handleDelete(address.id)}
              onSetDefault={() => setDefaultAddress(address.id)}
            />
          ))}
        </div>
      )}
    </div>
  )
}
