import type { SavedAddressInput } from '@/models/address/savedAddress.model'
import { isValidMapCoordinates, type MapCoordinates } from '@/config/map'

export type AddressFormField = keyof AddressFormValues

export interface AddressFormValues {
  label: string
  firstName: string
  lastName: string
  address: string
  apartment: string
  postcode: string
  phone: string
}

export const emptyAddressForm: AddressFormValues = {
  label: '',
  firstName: '',
  lastName: '',
  address: '',
  apartment: '',
  postcode: '',
  phone: '',
}

type ValidationMessages = Record<string, string>

export type AddressFormErrorField = AddressFormField | 'map'

export function validateAddressForm(
  values: AddressFormValues,
  messages: ValidationMessages,
  coordinates?: MapCoordinates | null,
) {
  const errors: Partial<Record<AddressFormErrorField, string>> = {}
  const required: (keyof AddressFormValues)[] = [
    'label',
    'firstName',
    'lastName',
    'address',
    'postcode',
    'phone',
  ]

  for (const field of required) {
    if (!values[field]?.trim()) {
      errors[field] = messages.required
    }
  }

  if (!isValidMapCoordinates(coordinates)) {
    errors.map = messages.mapRequired
  }

  return errors
}

export function addressFormToInput(
  values: AddressFormValues,
  options: { isDefault?: boolean; coordinates?: MapCoordinates | null } = {},
): SavedAddressInput {
  return {
    label: values.label.trim(),
    firstName: values.firstName.trim(),
    lastName: values.lastName.trim(),
    address: values.address.trim(),
    apartment: values.apartment.trim(),
    postcode: values.postcode.trim(),
    phone: values.phone.trim(),
    latitude: options.coordinates?.latitude ?? null,
    longitude: options.coordinates?.longitude ?? null,
    isDefault: options.isDefault,
  }
}
