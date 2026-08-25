import type { CheckoutUserAddress } from '@/models/checkout/checkout.model'
import type { SavedAddress } from '@/models/address/savedAddress.model'

export function mapCheckoutAddressToSaved(address: CheckoutUserAddress): SavedAddress {
  return {
    id: address.id,
    label: address.title,
    firstName: '',
    lastName: '',
    address: address.address,
    apartment: '',
    postcode: '',
    phone: '',
    isDefault: false,
  }
}

/** Prefer fully-hydrated dashboard addresses; keep checkout-only stubs as fallback. */
export function mergeCheckoutAddresses(
  localAddresses: SavedAddress[],
  apiAddresses: CheckoutUserAddress[],
): SavedAddress[] {
  if (localAddresses.length > 0) return localAddresses
  return apiAddresses.map(mapCheckoutAddressToSaved)
}
