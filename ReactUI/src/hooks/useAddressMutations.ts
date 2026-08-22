import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { SavedAddressInput } from '@/models/address/savedAddress.model'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { ApiError } from '@/services/api/apiTypes'
import { userAddressService } from '@/services/userAddressService'
import { useAddressStore } from '@/stores/addressStore'
import { useUserStore } from '@/stores/userStore'

function resolveMutationError(error: unknown, fallback: string) {
  if (error instanceof ApiError && error.messages.length > 0) {
    return error.messages[0].message
  }

  return fallback
}

export function useAddressMutations() {
  const { t } = useTranslation()
  const { locale } = useLocaleSettings()
  const accessToken = useUserStore((state) => state.accessToken)
  const loadAddresses = useAddressStore((state) => state.loadAddresses)
  const setDefaultAddress = useAddressStore((state) => state.setDefaultAddress)
  const [isSaving, setIsSaving] = useState(false)
  const [mutationError, setMutationError] = useState<string | null>(null)

  const saveAddress = useCallback(
    async (
      input: SavedAddressInput,
      options: { editingId?: string | null; makeDefault?: boolean } = {},
    ) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        setMutationError(t('address.errors.saveFailed'))
        return false
      }

      setIsSaving(true)
      setMutationError(null)

      try {
        let savedId: string | undefined

        if (options.editingId) {
          const updated = await userAddressService.updateAddress(
            accessToken,
            locale,
            options.editingId,
            input,
          )
          savedId = updated.id
        } else {
          const created = await userAddressService.createAddress(accessToken, locale, input)
          savedId = created.id
        }

        await loadAddresses(accessToken, locale)

        if (options.makeDefault && savedId) {
          setDefaultAddress(savedId)
        }

        return true
      } catch (error) {
        const message = resolveMutationError(error, t('address.errors.saveFailed'))
        setMutationError(message)
        return false
      } finally {
        setIsSaving(false)
      }
    },
    [accessToken, locale, loadAddresses, setDefaultAddress, t],
  )

  const deleteAddress = useCallback(
    async (id: string) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        setMutationError(t('address.errors.deleteFailed'))
        return false
      }

      setIsSaving(true)
      setMutationError(null)

      try {
        await userAddressService.deleteAddress(accessToken, id)
        await loadAddresses(accessToken, locale)

        const { defaultAddressId, addresses } = useAddressStore.getState()
        if (defaultAddressId === id && addresses.length > 0) {
          setDefaultAddress(addresses[0].id)
        }

        return true
      } catch (error) {
        const message = resolveMutationError(error, t('address.errors.deleteFailed'))
        setMutationError(message)
        return false
      } finally {
        setIsSaving(false)
      }
    },
    [accessToken, locale, loadAddresses, setDefaultAddress, t],
  )

  const clearMutationError = useCallback(() => setMutationError(null), [])

  return {
    isSaving,
    mutationError,
    saveAddress,
    deleteAddress,
    clearMutationError,
  }
}
