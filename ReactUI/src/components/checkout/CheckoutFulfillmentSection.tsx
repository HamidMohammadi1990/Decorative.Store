import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { AddressPickerModal } from '@/components/address/AddressPickerModal'
import { SelectedAddressDisplay } from '@/components/checkout/SelectedAddressDisplay'
import { CheckoutFormSection } from '@/components/checkout/CheckoutFormSection'
import { Button } from '@/components/ui/Button'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import { savedAddressToCheckoutFields } from '@/extensions/formatSavedAddress'
import { useAddressStore } from '@/stores/addressStore'
import type { FulfillmentType } from '@/extensions/calculateCheckoutTotals'

export type { FulfillmentType }

export interface CheckoutAddressFields {
  firstName: string
  lastName: string
  address: string
  apartment: string
  city: string
  postcode: string
  phone: string
}

interface CheckoutFulfillmentSectionProps {
  fulfillment: FulfillmentType
  onFulfillmentChange: (type: FulfillmentType) => void
  selectedAddressId: string | null
  onSelectedAddressChange: (address: SavedAddress | null) => void
  onAddressFieldsChange: (fields: CheckoutAddressFields) => void
  addressErrors: Partial<Record<keyof CheckoutAddressFields, string>>
}

export function CheckoutFulfillmentSection({
  fulfillment,
  onFulfillmentChange,
  selectedAddressId,
  onSelectedAddressChange,
  onAddressFieldsChange,
  addressErrors,
}: CheckoutFulfillmentSectionProps) {
  const { t } = useTranslation()
  const addresses = useAddressStore((s) => s.addresses)
  const openModal = useAddressStore((s) => s.openModal)
  const getDefaultAddress = useAddressStore((s) => s.getDefaultAddress)

  const [pickerOpen, setPickerOpen] = useState(false)

  const selectedAddress =
    addresses.find((a) => a.id === selectedAddressId) ?? null

  useEffect(() => {
    if (fulfillment !== 'delivery') {
      onSelectedAddressChange(null)
      return
    }

    const defaultAddress = getDefaultAddress() ?? addresses[0]
    if (!defaultAddress) {
      onSelectedAddressChange(null)
      return
    }

    const currentExists = addresses.some((a) => a.id === selectedAddressId)
    if (!currentExists) {
      onSelectedAddressChange(defaultAddress)
      onAddressFieldsChange(savedAddressToCheckoutFields(defaultAddress))
    }
  }, [
    addresses,
    fulfillment,
    getDefaultAddress,
    onAddressFieldsChange,
    onSelectedAddressChange,
    selectedAddressId,
  ])

  const handleSelect = (address: SavedAddress) => {
    onSelectedAddressChange(address)
    onAddressFieldsChange(savedAddressToCheckoutFields(address))
  }

  const hasAddressError = Object.keys(addressErrors).length > 0

  return (
    <>
      <CheckoutFormSection
        step={2}
        title={t('checkout.fulfillmentTitle')}
        description={t('checkout.fulfillmentDescription')}
      >
        <div className="mb-5 grid gap-3 sm:grid-cols-2">
          <FulfillmentOption
            id="fulfillment-delivery"
            name="fulfillment"
            checked={fulfillment === 'delivery'}
            title={t('checkout.fulfillmentDelivery')}
            description={t('checkout.fulfillmentDeliveryHint')}
            onChange={() => onFulfillmentChange('delivery')}
          />
          <FulfillmentOption
            id="fulfillment-pickup"
            name="fulfillment"
            checked={fulfillment === 'pickup'}
            title={t('checkout.fulfillmentPickup')}
            description={t('checkout.fulfillmentPickupHint')}
            onChange={() => onFulfillmentChange('pickup')}
          />
        </div>

        {fulfillment === 'pickup' ? (
          <div className="rounded-sm border border-warm-muted bg-warm-soft p-4">
            <p className="text-sm font-semibold text-text">{t('checkout.pickupStoreName')}</p>
            <p className="mt-1 text-sm text-text-muted">{t('checkout.pickupStoreAddress')}</p>
            <p className="mt-2 text-xs text-text-muted">{t('checkout.pickupStoreHours')}</p>
            <p className="mt-3 text-xs text-warm">{t('checkout.pickupNotice')}</p>
          </div>
        ) : addresses.length === 0 ? (
          <div className="rounded-sm border border-dashed border-border bg-surface-muted px-4 py-8 text-center">
            <p className="text-sm font-medium text-text">{t('checkout.noSavedAddress')}</p>
            <p className="mt-1 text-sm text-text-muted">{t('checkout.noSavedAddressHint')}</p>
            <Button variant="warm" className="mt-4" onClick={openModal}>
              {t('address.addNew')}
            </Button>
          </div>
        ) : selectedAddress ? (
          <SelectedAddressDisplay
            address={selectedAddress}
            onChange={() => setPickerOpen(true)}
          />
        ) : null}

        {hasAddressError && (
          <p className="mt-3 text-sm text-sale">{t('checkout.noAddressSelected')}</p>
        )}
      </CheckoutFormSection>

      <AddressPickerModal
        open={pickerOpen}
        selectedId={selectedAddressId}
        onClose={() => setPickerOpen(false)}
        onSelect={handleSelect}
      />
    </>
  )
}

function FulfillmentOption({
  id,
  name,
  checked,
  title,
  description,
  onChange,
}: {
  id: string
  name: string
  checked: boolean
  title: string
  description: string
  onChange: () => void
}) {
  return (
    <label
      htmlFor={id}
      className={`flex cursor-pointer flex-col rounded-sm border p-4 transition-colors ${
        checked
          ? 'border-warm bg-warm-soft ring-1 ring-warm/30'
          : 'border-border bg-surface hover:border-warm-muted'
      }`}
    >
      <div className="flex items-start gap-3">
        <input
          id={id}
          type="radio"
          name={name}
          checked={checked}
          onChange={onChange}
          className="mt-0.5 size-4 accent-[#9a7448]"
        />
        <div>
          <p className="text-sm font-semibold text-text">{title}</p>
          <p className="mt-1 text-xs text-text-muted">{description}</p>
        </div>
      </div>
    </label>
  )
}
