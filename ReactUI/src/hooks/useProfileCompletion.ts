import { useCallback, useMemo } from 'react'
import { useTranslation } from 'react-i18next'
import { calculateProfileProgress } from '@/extensions/calculateProfileCompletion'
import type { DashboardCoupon } from '@/models/dashboard/dashboard.model'
import type { ProfileCompletionReward } from '@/models/profile/profileCompletion.model'
import { getProfileCompletionConfig } from '@/services/profileCompletionService'
import { useEarnedCouponsStore } from '@/stores/earnedCouponsStore'
import { useProfileCompletionAdminStore } from '@/stores/profileCompletionAdminStore'
import { useProfileCompletionStore } from '@/stores/profileCompletionStore'
import { useSettingsStore } from '@/stores/settingsStore'

function buildRewardCoupon(
  reward: ProfileCompletionReward,
  campaignId: string,
): DashboardCoupon {
  const expires = new Date()
  expires.setDate(expires.getDate() + reward.validDays)

  const discountLabel =
    reward.type === 'percent_discount'
      ? `${reward.value}%`
      : `${reward.value.toLocaleString()} Toman`

  return {
    id: `earned-${campaignId}`,
    code: reward.code,
    description: reward.description,
    discount: discountLabel,
    expiresAt: expires.toISOString().slice(0, 10),
    status: 'active',
  }
}

export function useProfileCompletion() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const adminConfig = useProfileCompletionAdminStore((s) => s.config)
  const answers = useProfileCompletionStore((s) => s.answers)
  const rewardClaimedAt = useProfileCompletionStore((s) => s.rewardClaimedAt)
  const setAnswer = useProfileCompletionStore((s) => s.setAnswer)
  const markClaimed = useProfileCompletionStore((s) => s.claimReward)
  const addEarnedCoupon = useEarnedCouponsStore((s) => s.addCoupon)
  const hasEarnedCoupon = useEarnedCouponsStore((s) => s.hasCoupon)

  const config = useMemo(
    () => getProfileCompletionConfig(locale, adminConfig),
    [locale, adminConfig],
  )
  const progress = useMemo(
    () => calculateProfileProgress(config.questions, answers),
    [config.questions, answers],
  )

  const rewardClaimed = rewardClaimedAt !== null
  const canClaimReward = progress.isComplete && !rewardClaimed

  const claimReward = useCallback(() => {
    if (!canClaimReward) return null

    const coupon = buildRewardCoupon(config.campaign.reward, config.campaign.id)
    if (!hasEarnedCoupon(coupon.id)) {
      addEarnedCoupon(coupon)
    }
    markClaimed()
    return coupon
  }, [
    addEarnedCoupon,
    canClaimReward,
    config.campaign.id,
    config.campaign.reward,
    hasEarnedCoupon,
    markClaimed,
  ])

  return {
    config,
    answers,
    progress,
    rewardClaimed,
    canClaimReward,
    setAnswer,
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
