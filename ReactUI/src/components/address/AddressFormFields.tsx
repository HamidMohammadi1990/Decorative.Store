import { useTranslation } from 'react-i18next'
import { AddressMapPicker } from '@/components/address/AddressMapPicker'
import { AuthCheckbox, AuthField } from '@/components/auth/AuthField'
import type { MapCoordinates } from '@/config/map'
import type { AddressFormErrorField, AddressFormField, AddressFormValues } from '@/extensions/validateAddressForm'

interface AddressFormFieldsProps {
  values: AddressFormValues
  errors: Partial<Record<AddressFormErrorField, string>>
  coordinates: MapCoordinates | null
  mapSessionKey?: string
  onChange: (field: AddressFormField, value: string) => void
  onCoordinatesChange: (coords: MapCoordinates) => void
  onAddressHint?: (addressLine: string) => void
  showDefaultCheckbox?: boolean
  isDefault?: boolean
  onDefaultChange?: (checked: boolean) => void
}

export function AddressFormFields({
  values,
  errors,
  coordinates,
  mapSessionKey = 'new',
  onChange,
  onCoordinatesChange,
  onAddressHint,
  showDefaultCheckbox = false,
  isDefault = false,
  onDefaultChange,
}: AddressFormFieldsProps) {
  const { t } = useTranslation()

  return (
    <div className="space-y-4">
      <AddressMapPicker
        sessionKey={mapSessionKey}
        value={coordinates}
        onChange={onCoordinatesChange}
        onAddressHint={onAddressHint}
        error={errors.map}
      />

      <AuthField
        name="label"
        label={t('address.labelField')}
        placeholder={t('address.labelPlaceholder')}
        value={values.label}
        onChange={(e) => onChange('label', e.target.value)}
        error={errors.label}
      />
      <div className="grid gap-4 sm:grid-cols-2">
        <AuthField
          name="firstName"
          autoComplete="given-name"
          label={t('checkout.firstNameLabel')}
          value={values.firstName}
          onChange={(e) => onChange('firstName', e.target.value)}
          error={errors.firstName}
        />
        <AuthField
          name="lastName"
          autoComplete="family-name"
          label={t('checkout.lastNameLabel')}
          value={values.lastName}
          onChange={(e) => onChange('lastName', e.target.value)}
          error={errors.lastName}
        />
      </div>
      <AuthField
        name="address"
        autoComplete="street-address"
        label={t('checkout.addressLabel')}
        value={values.address}
        onChange={(e) => onChange('address', e.target.value)}
        error={errors.address}
      />
      <AuthField
        name="apartment"
        autoComplete="address-line2"
        label={t('checkout.apartmentLabel')}
        value={values.apartment}
        onChange={(e) => onChange('apartment', e.target.value)}
      />
      <AuthField
        name="postcode"
        autoComplete="postal-code"
        label={t('checkout.postcodeLabel')}
        value={values.postcode}
        onChange={(e) => onChange('postcode', e.target.value)}
        error={errors.postcode}
      />
      <AuthField
        name="phone"
        type="tel"
        autoComplete="tel"
        label={t('checkout.phoneLabel')}
        value={values.phone}
        onChange={(e) => onChange('phone', e.target.value)}
        error={errors.phone}
      />
      {showDefaultCheckbox && onDefaultChange && (
        <AuthCheckbox
          name="isDefault"
          label={t('address.setAsDefault')}
          checked={isDefault}
          onChange={(e) => onDefaultChange(e.target.checked)}
        />
      )}
    </div>
  )
}
