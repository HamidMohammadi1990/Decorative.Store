import type { CheckoutUserAddress } from '@/models/checkout/checkout.model'
import type { SavedAddress } from '@/models/address/savedAddress.model'

export function mapCheckoutAddressToSaved(address: CheckoutUserAddress): SavedAddress {
  return {
    id: address.id,
    label: address.title,
    firstName: address.title,
    lastName: '',
    address: address.address,
    apartment: '',
    city: '',
    postcode: '',
    phone: '',
    isDefault: false,
  }
}

export function mergeCheckoutAddresses(
  localAddresses: SavedAddress[],
  apiAddresses: CheckoutUserAddress[],
): SavedAddress[] {
  if (apiAddresses.length === 0) return localAddresses

  const apiMapped = apiAddresses.map(mapCheckoutAddressToSaved)
  const apiIds = new Set(apiMapped.map((address) => address.id))
  const remainingLocal = localAddresses.filter((address) => !apiIds.has(address.id))
  return [...apiMapped, ...remainingLocal]
}
