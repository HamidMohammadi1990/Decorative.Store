import type { SavedAddressInput } from '@/models/address/savedAddress.model'

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
  options: { isDefault?: boolean } = {},
): SavedAddressInput {
  return {
    label: values.label.trim(),
    firstName: values.firstName.trim(),
    lastName: values.lastName.trim(),
    address: values.address.trim(),
    apartment: values.apartment.trim(),
    postcode: values.postcode.trim(),
    phone: values.phone.trim(),
    isDefault: options.isDefault,
  }
}
