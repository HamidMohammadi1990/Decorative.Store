import { profileCompletionApiService } from '@/services/profileCompletionApiService'
import { useProfileCompletionAdminStore } from '@/stores/profileCompletionAdminStore'
import { useProfileCompletionStore } from '@/stores/profileCompletionStore'

let syncPromise: Promise<void> | null = null
let syncedTokenKey: string | null = null

export function isProfileCompletionSynced(accessToken: string | null | undefined): boolean {
  const tokenKey = accessToken ?? ''
  return syncedTokenKey === tokenKey && Boolean(useProfileCompletionAdminStore.getState().config)
}

export function resetProfileCompletionSync() {
  syncedTokenKey = null
  syncPromise = null
}

/** Loads config + my-state once per access token (deduped across components and Strict Mode). */
export async function syncProfileCompletion(accessToken: string | null | undefined): Promise<void> {
  const tokenKey = accessToken ?? ''

  if (isProfileCompletionSynced(accessToken)) {
    return
  }

  if (syncPromise) {
    return syncPromise
  }

  syncPromise = (async () => {
    if (syncedTokenKey !== tokenKey) {
      useProfileCompletionAdminStore.getState().clearConfig()
      useProfileCompletionStore.getState().reset()
    }

    const remoteConfig = await profileCompletionApiService.getConfig()
    if (!remoteConfig) {
      throw new Error('profile-completion-config-missing')
    }

    useProfileCompletionAdminStore.getState().setConfigFromServer(remoteConfig)

    if (accessToken) {
      const state = await profileCompletionApiService.getMyState(accessToken)
      useProfileCompletionStore.getState().setAnswers(state.answers)
      useProfileCompletionStore.getState().setRewardClaimedAt(state.rewardClaimedAt)
    } else {
      useProfileCompletionStore.getState().reset()
    }

    syncedTokenKey = tokenKey
  })().finally(() => {
    syncPromise = null
  })

  return syncPromise
}
