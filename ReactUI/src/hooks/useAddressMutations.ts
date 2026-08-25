import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { SavedAddress, SavedAddressInput } from '@/models/address/savedAddress.model'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { ApiError } from '@/services/api/apiTypes'
import { savedAddressToInput } from '@/services/mappers/userAddressMapper'
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
        const payload: SavedAddressInput = {
          ...input,
          isDefault: options.makeDefault ?? input.isDefault ?? false,
        }

        if (options.editingId) {
          await userAddressService.updateAddress(accessToken, locale, options.editingId, payload)
        } else {
          await userAddressService.createAddress(accessToken, locale, payload)
        }

        await loadAddresses(accessToken, locale)
        return true
      } catch (error) {
        const message = resolveMutationError(error, t('address.errors.saveFailed'))
        setMutationError(message)
        return false
      } finally {
        setIsSaving(false)
      }
    },
    [accessToken, locale, loadAddresses, t],
  )

  const setAsDefault = useCallback(
    async (address: SavedAddress) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        setMutationError(t('address.errors.saveFailed'))
        return false
      }

      setIsSaving(true)
      setMutationError(null)

      try {
        await userAddressService.updateAddress(
          accessToken,
          locale,
          address.id,
          savedAddressToInput(address, true),
        )
        await loadAddresses(accessToken, locale)
        return true
      } catch (error) {
        const message = resolveMutationError(error, t('address.errors.saveFailed'))
        setMutationError(message)
        return false
      } finally {
        setIsSaving(false)
      }
    },
    [accessToken, locale, loadAddresses, t],
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
        return true
      } catch (error) {
        const message = resolveMutationError(error, t('address.errors.deleteFailed'))
        setMutationError(message)
        return false
      } finally {
        setIsSaving(false)
      }
    },
    [accessToken, locale, loadAddresses, t],
  )

  const clearMutationError = useCallback(() => setMutationError(null), [])

  return {
    isSaving,
    mutationError,
    saveAddress,
    setAsDefault,
    deleteAddress,
    clearMutationError,
  }
}
