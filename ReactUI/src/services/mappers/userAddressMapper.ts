import type { SavedAddress, SavedAddressInput } from '@/models/address/savedAddress.model'
import { readBooleanField, readRecord, readStringField } from '@/services/api/apiNormalize'

export function normalizeUserAddress(data: unknown): SavedAddress {
  const record = readRecord(data) ?? {}

  return {
    id: readStringField(record, 'id', 'Id'),
    label: readStringField(record, 'title', 'Title'),
    firstName: readStringField(record, 'recipientFirstName', 'RecipientFirstName'),
    lastName: readStringField(record, 'recipientLastName', 'RecipientLastName'),
    address: readStringField(record, 'address', 'Address'),
    apartment: readStringField(record, 'apartment', 'Apartment'),
    postcode: readStringField(record, 'postalCode', 'PostalCode'),
    phone: readStringField(record, 'phoneNumber', 'PhoneNumber'),
    isDefault: readBooleanField(record, 'isDefault', 'IsDefault'),
  }
}

export function buildSavedAddressFromInput(id: string, input: SavedAddressInput): SavedAddress {
  return {
    id,
    label: input.label.trim(),
    firstName: input.firstName.trim(),
    lastName: input.lastName.trim(),
    address: input.address.trim(),
    apartment: input.apartment.trim(),
    postcode: input.postcode.trim(),
    phone: input.phone.trim(),
    isDefault: input.isDefault ?? false,
  }
}

export function buildCreateUserAddressPayload(input: SavedAddressInput) {
  return {
    title: input.label.trim(),
    address: input.address.trim(),
    apartment: input.apartment.trim() || null,
    postalCode: input.postcode.trim() || null,
    cityId: null,
    recipientFirstName: input.firstName.trim(),
    recipientLastName: input.lastName.trim(),
    phoneNumber: input.phone.trim(),
    isDefault: input.isDefault ?? false,
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

export function savedAddressToInput(address: SavedAddress, isDefault?: boolean): SavedAddressInput {
  return {
    label: address.label,
    firstName: address.firstName,
    lastName: address.lastName,
    address: address.address,
    apartment: address.apartment,
    postcode: address.postcode,
    phone: address.phone,
    isDefault: isDefault ?? address.isDefault,
  }
}
