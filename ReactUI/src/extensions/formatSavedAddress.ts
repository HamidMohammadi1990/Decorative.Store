import type { SavedAddress } from '@/models/address/savedAddress.model'

export function formatSavedAddressLines(address: SavedAddress) {
  const name = `${address.firstName} ${address.lastName}`.trim()
  const street = [address.address, address.apartment].filter(Boolean).join(', ')
  const locality = [address.city, address.postcode].filter(Boolean).join(', ')

  return [name, street, locality, address.phone].filter(Boolean)
}

export function savedAddressToCheckoutFields(address: SavedAddress) {
  return {
    firstName: address.firstName,
    lastName: address.lastName,
    address: address.address,
    apartment: address.apartment,
    city: address.city,
    postcode: address.postcode,
    phone: address.phone,
  }
}
