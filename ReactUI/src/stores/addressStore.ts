import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import type { Locale } from '@/models/shared/locale.model'
import { userAddressService } from '@/services/userAddressService'

interface AddressState {
  addresses: SavedAddress[]
  defaultAddressId: string | null
  isLoading: boolean
  isModalOpen: boolean
  openModal: () => void
  closeModal: () => void
  setAddresses: (addresses: SavedAddress[]) => void
  clearAddresses: () => void
  loadAddresses: (accessToken: string, locale: Locale) => Promise<void>
  setDefaultAddress: (id: string) => void
  getDefaultAddress: () => SavedAddress | undefined
}

function applyDefaultFlags(
  addresses: SavedAddress[],
  defaultAddressId: string | null,
): SavedAddress[] {
  if (addresses.length === 0) return []

  const effectiveDefaultId =
    defaultAddressId && addresses.some((address) => address.id === defaultAddressId)
      ? defaultAddressId
      : addresses[0].id

  return addresses.map((address) => ({
    ...address,
    isDefault: address.id === effectiveDefaultId,
  }))
}

export const useAddressStore = create<AddressState>()(
  persist(
    (set, get) => ({
      addresses: [],
      defaultAddressId: null,
      isLoading: false,
      isModalOpen: false,

      openModal: () => set({ isModalOpen: true }),
      closeModal: () => set({ isModalOpen: false }),

      setAddresses: (addresses) =>
        set((state) => ({
          addresses: applyDefaultFlags(addresses, state.defaultAddressId),
        })),

      clearAddresses: () => set({ addresses: [], isLoading: false }),

      loadAddresses: async (accessToken, locale) => {
        set({ isLoading: true })
        try {
          const addresses = await userAddressService.getMyAddresses(accessToken, locale)
          set((state) => ({
            addresses: applyDefaultFlags(addresses, state.defaultAddressId),
            isLoading: false,
          }))
        } catch {
          set({ isLoading: false })
        }
      },

      setDefaultAddress: (id) =>
        set((state) => ({
          defaultAddressId: id,
          addresses: applyDefaultFlags(state.addresses, id),
        })),

      getDefaultAddress: () => get().addresses.find((address) => address.isDefault),
    }),
    {
      name: 'westelm-addresses',
      partialize: (state) => ({ defaultAddressId: state.defaultAddressId }),
    },
  ),
)
