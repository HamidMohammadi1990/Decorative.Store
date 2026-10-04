import type { SavedAddress, SavedAddressInput } from '@/models/address/savedAddress.model'
import type { PagedRequest } from '@/models/shared/paged.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import {
  buildCreateUserAddressPayload,
  buildSavedAddressFromInput,
  buildUpdateUserAddressPayload,
  normalizeUserAddress,
} from '@/services/mappers/userAddressMapper'

const USER_ADDRESS_MY_PATH = '/api/v1/user-address/my'
const USER_ADDRESS_CREATE_PATH = '/api/v1/user-address/create'
const USER_ADDRESS_UPDATE_PATH = '/api/v1/user-address/update'
const USER_ADDRESS_DELETE_PATH = '/api/v1/user-address/delete'

export const userAddressService = {
  async getMyAddresses(
    accessToken: string,
    locale: Locale,
    pagination: PagedRequest = { pageNumber: 1, pageSize: 50 },
  ): Promise<SavedAddress[]> {
    const result = await apiPost<{
      items?: unknown[]
      Items?: unknown[]
    }>(
      USER_ADDRESS_MY_PATH,
      {
        isActive: true,
        pagination,
      },
      { locale, accessToken },
    )

    const items = result.items ?? result.Items ?? []
    return items.map(normalizeUserAddress).filter((address) => address.id)
  },

  async createAddress(
    accessToken: string,
    locale: Locale,
    input: SavedAddressInput,
  ): Promise<SavedAddress> {
    const payload = buildCreateUserAddressPayload(input)

    const result = await apiPost<{ id?: string | number; Id?: string | number }>(
      USER_ADDRESS_CREATE_PATH,
      payload,
      { locale, accessToken },
    )

    const createdId = String(result.id ?? result.Id ?? '').trim()
    if (!createdId) {
      throw new Error('address_create_failed')
    }

    try {
      const addresses = await this.getMyAddresses(accessToken, locale)
      const created = addresses.find((address) => address.id === createdId)
      if (created) return created
    } catch {
      // Create already succeeded; list refresh happens in the mutation hook.
    }

    return buildSavedAddressFromInput(createdId, input)
  },

  async updateAddress(
    accessToken: string,
    locale: Locale,
    id: string,
    input: SavedAddressInput,
    options: { isActive?: boolean } = {},
  ): Promise<SavedAddress> {
    const payload = buildUpdateUserAddressPayload(id, input, options.isActive ?? true)

    await apiPut(USER_ADDRESS_UPDATE_PATH, payload, { locale, accessToken })

    try {
      const addresses = await this.getMyAddresses(accessToken, locale)
      const updated = addresses.find((address) => address.id === id)
      if (updated) return updated
    } catch {
      // Update already succeeded; list refresh happens in the mutation hook.
    }

    return buildSavedAddressFromInput(id, input)
  },

  async deleteAddress(accessToken: string, id: string) {
    await apiDelete(USER_ADDRESS_DELETE_PATH, accessToken, { id })
  },
}
