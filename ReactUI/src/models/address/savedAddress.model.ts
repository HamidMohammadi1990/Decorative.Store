export interface SavedAddress {
  id: string
  label: string
  firstName: string
  lastName: string
  address: string
  apartment: string
  city: string
  postcode: string
  phone: string
  isDefault: boolean
}

export type SavedAddressInput = Omit<SavedAddress, 'id' | 'isDefault'> & {
  isDefault?: boolean
}
