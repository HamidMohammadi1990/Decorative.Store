import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { AddressCard } from '@/components/address/AddressCard'
import { AddressFormFields } from '@/components/address/AddressFormFields'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { AddressesIcon } from '@/components/dashboard/DashboardIcons'
import { Button } from '@/components/ui/Button'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import {
  addressFormToInput,
  emptyAddressForm,
  validateAddressForm,
  type AddressFormField,
  type AddressFormValues,
} from '@/extensions/validateAddressForm'
import { useAddressStore } from '@/stores/addressStore'

type PanelMode = 'list' | 'add' | 'edit'

export function AddressesPanel() {
  const { t } = useTranslation()
  const addresses = useAddressStore((s) => s.addresses)
  const addAddress = useAddressStore((s) => s.addAddress)
  const updateAddress = useAddressStore((s) => s.updateAddress)
  const removeAddress = useAddressStore((s) => s.removeAddress)
  const setDefaultAddress = useAddressStore((s) => s.setDefaultAddress)

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
    }
  }, [mode])

  const startAdd = () => {
    setMode('add')
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
          <AddressFormFields
            values={form}
            errors={errors}
            onChange={updateField}
            showDefaultCheckbox
            isDefault={makeDefault}
            onDefaultChange={setMakeDefault}
          />

          <div className="mt-6 flex flex-wrap gap-3">
            <Button variant="warm" onClick={handleSave}>
              {t('address.save')}
            </Button>
            <Button variant="secondary" onClick={() => setMode('list')}>
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
          <Button variant="warm" onClick={startAdd}>
            {t('address.addNew')}
          </Button>
        }
      />

      {addresses.length === 0 ? (
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
              onDelete={() => removeAddress(address.id)}
              onSetDefault={() => setDefaultAddress(address.id)}
            />
          ))}
        </div>
      )}
    </div>
  )
}
