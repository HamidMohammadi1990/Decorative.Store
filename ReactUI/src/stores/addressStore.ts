import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { SavedAddress, SavedAddressInput } from '@/models/address/savedAddress.model'

interface AddressState {
  addresses: SavedAddress[]
  isModalOpen: boolean
  openModal: () => void
  closeModal: () => void
  addAddress: (input: SavedAddressInput) => SavedAddress
  updateAddress: (id: string, input: Partial<SavedAddressInput>) => void
  removeAddress: (id: string) => void
  setDefaultAddress: (id: string) => void
  getDefaultAddress: () => SavedAddress | undefined
}

function createId() {
  return `addr-${Date.now().toString(36)}`
}

export const useAddressStore = create<AddressState>()(
  persist(
    (set, get) => ({
      addresses: [],
      isModalOpen: false,

      openModal: () => set({ isModalOpen: true }),
      closeModal: () => set({ isModalOpen: false }),

      addAddress: (input) => {
        const shouldBeDefault = input.isDefault ?? get().addresses.length === 0
        const newAddress: SavedAddress = {
          id: createId(),
          label: input.label,
          firstName: input.firstName,
          lastName: input.lastName,
          address: input.address,
          apartment: input.apartment,
          city: input.city,
          postcode: input.postcode,
          phone: input.phone,
          isDefault: shouldBeDefault,
        }

        set((state) => ({
          addresses: shouldBeDefault
            ? [...state.addresses.map((a) => ({ ...a, isDefault: false })), newAddress]
            : [...state.addresses, newAddress],
        }))

        return newAddress
      },

      updateAddress: (id, input) =>
        set((state) => {
          const makeDefault = input.isDefault === true
          return {
            addresses: state.addresses.map((address) => {
              if (address.id === id) {
                return {
                  ...address,
                  ...input,
                  isDefault: makeDefault ? true : input.isDefault === false ? false : address.isDefault,
                }
              }
              if (makeDefault) return { ...address, isDefault: false }
              return address
            }),
          }
        }),

      removeAddress: (id) =>
        set((state) => {
          const remaining = state.addresses.filter((a) => a.id !== id)
          const removedWasDefault = state.addresses.find((a) => a.id === id)?.isDefault
          if (removedWasDefault && remaining.length > 0) {
            return {
              addresses: remaining.map((a, i) => ({ ...a, isDefault: i === 0 })),
            }
          }
          return { addresses: remaining }
        }),

      setDefaultAddress: (id) =>
        set((state) => ({
          addresses: state.addresses.map((a) => ({
            ...a,
            isDefault: a.id === id,
          })),
        })),

      getDefaultAddress: () => get().addresses.find((a) => a.isDefault),
    }),
    {
      name: 'westelm-addresses',
      partialize: (state) => ({ addresses: state.addresses }),
    },
  ),
)
