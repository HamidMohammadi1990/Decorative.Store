import type { SavedAddressInput } from '@/models/address/savedAddress.model'

export type AddressFormField = keyof SavedAddressInput

export interface AddressFormValues {
  label: string
  firstName: string
  lastName: string
  address: string
  apartment: string
  city: string
  postcode: string
  phone: string
}

export const emptyAddressForm: AddressFormValues = {
  label: '',
  firstName: '',
  lastName: '',
  address: '',
  apartment: '',
  city: '',
  postcode: '',
  phone: '',
}

type ValidationMessages = Record<string, string>

export function validateAddressForm(
  values: AddressFormValues,
  messages: ValidationMessages,
) {
  const errors: Partial<Record<AddressFormField, string>> = {}
  const required: (keyof AddressFormValues)[] = [
    'label',
    'firstName',
    'lastName',
    'address',
    'city',
    'postcode',
    'phone',
  ]

  for (const field of required) {
    if (!values[field]?.trim()) {
      errors[field] = messages.required
    }
  }

  return errors
}

export function addressFormToInput(
  values: AddressFormValues,
  isDefault?: boolean,
): SavedAddressInput {
  return {
    label: values.label.trim(),
    firstName: values.firstName.trim(),
    lastName: values.lastName.trim(),
    address: values.address.trim(),
    apartment: values.apartment.trim(),
    city: values.city.trim(),
    postcode: values.postcode.trim(),
    phone: values.phone.trim(),
    isDefault,
  }
}
