import { useCallback, useEffect, useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { calculateProfileProgress } from '@/extensions/calculateProfileCompletion'
import type { ProfileCompletionConfig } from '@/models/profile/profileCompletion.model'
import { getProfileCompletionConfig } from '@/services/profileCompletionService'
import { profileCompletionApiService } from '@/services/profileCompletionApiService'
import { useProfileCompletionAdminStore } from '@/stores/profileCompletionAdminStore'
import { useProfileCompletionStore } from '@/stores/profileCompletionStore'
import { useSettingsStore } from '@/stores/settingsStore'
import { useUserStore } from '@/stores/userStore'

export function useProfileCompletion() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const accessToken = useUserStore((s) => s.accessToken)
  const adminConfig = useProfileCompletionAdminStore((s) => s.config)
  const setAdminConfig = useProfileCompletionAdminStore((s) => s.setConfigFromServer)
  const clearAdminConfig = useProfileCompletionAdminStore((s) => s.clearConfig)
  const answers = useProfileCompletionStore((s) => s.answers)
  const rewardClaimedAt = useProfileCompletionStore((s) => s.rewardClaimedAt)
  const setAnswer = useProfileCompletionStore((s) => s.setAnswer)
  const setAnswers = useProfileCompletionStore((s) => s.setAnswers)
  const setRewardClaimedAt = useProfileCompletionStore((s) => s.setRewardClaimedAt)
  const resetUserState = useProfileCompletionStore((s) => s.reset)

  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [syncError, setSyncError] = useState<string | null>(null)

  useEffect(() => {
    let cancelled = false

    const load = async () => {
      setLoading(true)
      setSyncError(null)
      clearAdminConfig()

      try {
        const remoteConfig = await profileCompletionApiService.getConfig()
        if (cancelled) return

        if (!remoteConfig) {
          setSyncError(t('dashboard.profileCompletion.syncFailed'))
          return
        }

        setAdminConfig(remoteConfig)

        if (accessToken) {
          const state = await profileCompletionApiService.getMyState(accessToken)
          if (!cancelled) {
            setAnswers(state.answers)
            setRewardClaimedAt(state.rewardClaimedAt)
          }
        } else if (!cancelled) {
          resetUserState()
        }
      } catch {
        if (!cancelled) setSyncError(t('dashboard.profileCompletion.syncFailed'))
      } finally {
        if (!cancelled) setLoading(false)
      }
    }

    void load()
    return () => {
      cancelled = true
    }
  }, [
    accessToken,
    clearAdminConfig,
    resetUserState,
    setAdminConfig,
    setAnswers,
    setRewardClaimedAt,
    t,
  ])

  const config = useMemo<ProfileCompletionConfig | null>(() => {
    if (!adminConfig) return null
    return getProfileCompletionConfig(locale, adminConfig)
  }, [locale, adminConfig])

  const progress = useMemo(
    () =>
      config
        ? calculateProfileProgress(config.questions, answers)
        : { percent: 0, answeredCount: 0, totalCount: 0, isComplete: false },
    [config, answers],
  )

  const rewardClaimed = rewardClaimedAt !== null
  const canClaimReward = Boolean(config && progress.isComplete && !rewardClaimed && accessToken)

  const saveAnswers = useCallback(async () => {
    if (!accessToken) return false

    setSaving(true)
    setSyncError(null)
    try {
      await profileCompletionApiService.saveAnswers(accessToken, answers)
      return true
    } catch {
      setSyncError(t('dashboard.profileCompletion.saveFailed'))
      return false
    } finally {
      setSaving(false)
    }
  }, [accessToken, answers, t])

  const claimReward = useCallback(async () => {
    if (!canClaimReward || !accessToken) return false

    setSyncError(null)
    try {
      const claimedAt = await profileCompletionApiService.claimReward(accessToken)
      setRewardClaimedAt(claimedAt)
      return true
    } catch {
      setSyncError(t('dashboard.profileCompletion.claimFailed'))
      return false
    }
  }, [accessToken, canClaimReward, setRewardClaimedAt, t])

  return {
    config,
    configLoaded: adminConfig !== null,
    answers,
    progress,
    rewardClaimed,
    canClaimReward,
    loading,
    saving,
    syncError,
    setAnswer,
    saveAnswers,
    claimReward,
    rewardClaimedLabel: rewardClaimedAt
      ? t('dashboard.profileCompletion.claimedOn', {
          date: new Date(rewardClaimedAt).toLocaleDateString(
            locale === 'fa' ? 'fa-IR' : 'en-US',
          ),
        })
      : null,
  }
}
