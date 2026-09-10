export interface SavedAddress {
  id: string
  label: string
  firstName: string
  lastName: string
  address: string
  apartment: string
  postcode: string
  phone: string
  latitude: number | null
  longitude: number | null
  isDefault: boolean
}

export type SavedAddressInput = Omit<SavedAddress, 'id' | 'isDefault'> & {
  isDefault?: boolean
}
