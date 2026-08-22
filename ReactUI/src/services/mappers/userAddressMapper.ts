import type { SavedAddress, SavedAddressInput } from '@/models/address/savedAddress.model'
import { readRecord, readStringField } from '@/services/api/apiNormalize'

export function normalizeUserAddress(data: unknown): SavedAddress {
  const record = readRecord(data) ?? {}

  return {
    id: readStringField(record, 'id', 'Id'),
    label: readStringField(record, 'title', 'Title'),
    firstName: readStringField(record, 'recipientFirstName', 'RecipientFirstName'),
    lastName: readStringField(record, 'recipientLastName', 'RecipientLastName'),
    address: readStringField(record, 'address', 'Address'),
    apartment: '',
    postcode: readStringField(record, 'postalCode', 'PostalCode'),
    phone: readStringField(record, 'phoneNumber', 'PhoneNumber'),
    isDefault: false,
  }
}

export function buildSavedAddressFromInput(id: string, input: SavedAddressInput): SavedAddress {
  const addressLine = [input.address.trim(), input.apartment.trim()].filter(Boolean).join(', ')

  return {
    id,
    label: input.label.trim(),
    firstName: input.firstName.trim(),
    lastName: input.lastName.trim(),
    address: addressLine || input.address.trim(),
    apartment: input.apartment.trim(),
    postcode: input.postcode.trim(),
    phone: input.phone.trim(),
    isDefault: input.isDefault ?? false,
  }
}

export function buildCreateUserAddressPayload(input: SavedAddressInput) {
  const addressLine = [input.address.trim(), input.apartment.trim()].filter(Boolean).join(', ')

  return {
    title: input.label.trim(),
    address: addressLine,
    postalCode: input.postcode.trim() || null,
    recipientFirstName: input.firstName.trim(),
    recipientLastName: input.lastName.trim(),
    phoneNumber: input.phone.trim(),
  }
}

export function buildUpdateUserAddressPayload(
  id: string,
  input: SavedAddressInput,
  isActive = true,
) {
  return {
    id,
    ...buildCreateUserAddressPayload(input),
    isActive,
  }
}
