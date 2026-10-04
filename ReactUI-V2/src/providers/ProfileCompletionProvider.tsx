import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from 'react'
import { useTranslation } from 'react-i18next'
import { calculateProfileProgress } from '@/extensions/calculateProfileCompletion'
import type { ProfileCompletionConfig } from '@/models/profile/profileCompletion.model'
import { getProfileCompletionConfig } from '@/services/profileCompletionService'
import { profileCompletionApiService } from '@/services/profileCompletionApiService'
import {
  isProfileCompletionSynced,
  syncProfileCompletion,
} from '@/services/profileCompletionSync'
import { useProfileCompletionAdminStore } from '@/stores/profileCompletionAdminStore'
import { useProfileCompletionStore } from '@/stores/profileCompletionStore'
import { useSettingsStore } from '@/stores/settingsStore'
import { useUserStore } from '@/stores/userStore'

interface ProfileCompletionContextValue {
  config: ProfileCompletionConfig | null
  configLoaded: boolean
  answers: Record<string, string | string[]>
  progress: ReturnType<typeof calculateProfileProgress>
  rewardClaimed: boolean
  canClaimReward: boolean
  loading: boolean
  saving: boolean
  syncError: string | null
  setAnswer: (questionId: string, value: string | string[]) => void
  saveAnswers: () => Promise<boolean>
  claimReward: () => Promise<boolean>
  rewardClaimedLabel: string | null
}

const ProfileCompletionContext = createContext<ProfileCompletionContextValue | null>(null)

export function ProfileCompletionProvider({ children }: { children: ReactNode }) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const accessToken = useUserStore((s) => s.accessToken)
  const adminConfig = useProfileCompletionAdminStore((s) => s.config)
  const answers = useProfileCompletionStore((s) => s.answers)
  const rewardClaimedAt = useProfileCompletionStore((s) => s.rewardClaimedAt)
  const setAnswer = useProfileCompletionStore((s) => s.setAnswer)
  const setRewardClaimedAt = useProfileCompletionStore((s) => s.setRewardClaimedAt)

  const [authReady, setAuthReady] = useState(() => useUserStore.persist.hasHydrated())
  const [loading, setLoading] = useState(() => !isProfileCompletionSynced(accessToken))
  const [saving, setSaving] = useState(false)
  const [syncError, setSyncError] = useState<string | null>(null)

  useEffect(() => {
    if (useUserStore.persist.hasHydrated()) {
      setAuthReady(true)
      return
    }

    return useUserStore.persist.onFinishHydration(() => {
      setAuthReady(true)
    })
  }, [])

  useEffect(() => {
    if (!authReady) return

    if (isProfileCompletionSynced(accessToken)) {
      setLoading(false)
      setSyncError(null)
      return
    }

    let cancelled = false
    setLoading(true)
    setSyncError(null)

    void syncProfileCompletion(accessToken)
      .then(() => {
        if (!cancelled) {
          setSyncError(null)
          setLoading(false)
        }
      })
      .catch(() => {
        if (!cancelled) {
          setSyncError('syncFailed')
          setLoading(false)
        }
      })

    return () => {
      cancelled = true
    }
  }, [accessToken, authReady])

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
      setSyncError('saveFailed')
      return false
    } finally {
      setSaving(false)
    }
  }, [accessToken, answers])

  const claimReward = useCallback(async () => {
    if (!canClaimReward || !accessToken) return false

    setSyncError(null)
    try {
      const claimedAt = await profileCompletionApiService.claimReward(accessToken)
      setRewardClaimedAt(claimedAt)
      profileCompletionApiService.invalidateMyState(accessToken)
      return true
    } catch {
      setSyncError('claimFailed')
      return false
    }
  }, [accessToken, canClaimReward, setRewardClaimedAt])

  const syncErrorMessage = syncError
    ? t(`dashboard.profileCompletion.${syncError}`)
    : null

  const value = useMemo(
    (): ProfileCompletionContextValue => ({
      config,
      configLoaded: adminConfig !== null,
      answers,
      progress,
      rewardClaimed,
      canClaimReward,
      loading,
      saving,
      syncError: syncErrorMessage,
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
    }),
    [
      adminConfig,
      answers,
      canClaimReward,
      claimReward,
      config,
      loading,
      locale,
      progress,
      rewardClaimed,
      rewardClaimedAt,
      saveAnswers,
      saving,
      setAnswer,
      syncErrorMessage,
      t,
    ],
  )

  return (
    <ProfileCompletionContext.Provider value={value}>{children}</ProfileCompletionContext.Provider>
  )
}

export function useProfileCompletion() {
  const context = useContext(ProfileCompletionContext)
  if (!context) {
    throw new Error('useProfileCompletion must be used within ProfileCompletionProvider')
  }
  return context
}
