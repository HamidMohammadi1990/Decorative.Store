import { create } from 'zustand'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import type { Locale } from '@/models/shared/locale.model'
import { userAddressService } from '@/services/userAddressService'

interface AddressState {
  addresses: SavedAddress[]
  isLoading: boolean
  isModalOpen: boolean
  openModal: () => void
  closeModal: () => void
  setAddresses: (addresses: SavedAddress[]) => void
  clearAddresses: () => void
  loadAddresses: (accessToken: string, locale: Locale) => Promise<void>
  getDefaultAddress: () => SavedAddress | undefined
}

function withFallbackDefault(addresses: SavedAddress[]): SavedAddress[] {
  if (addresses.length === 0) return []
  if (addresses.some((address) => address.isDefault)) return addresses

  return addresses.map((address, index) => ({
    ...address,
    isDefault: index === 0,
  }))
}

export const useAddressStore = create<AddressState>()((set, get) => ({
  addresses: [],
  isLoading: false,
  isModalOpen: false,

  openModal: () => set({ isModalOpen: true }),
  closeModal: () => set({ isModalOpen: false }),

  setAddresses: (addresses) => set({ addresses: withFallbackDefault(addresses) }),

  clearAddresses: () => set({ addresses: [], isLoading: false }),

  loadAddresses: async (accessToken, locale) => {
    set({ isLoading: true })
    try {
      const addresses = await userAddressService.getMyAddresses(accessToken, locale)
      set({
        addresses: withFallbackDefault(addresses),
        isLoading: false,
      })
    } catch {
      set({ isLoading: false })
    }
  },

  getDefaultAddress: () => get().addresses.find((address) => address.isDefault),
}))
