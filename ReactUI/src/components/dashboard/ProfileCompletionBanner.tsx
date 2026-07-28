import { Link, useLocation } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { ProfileCompletionIcon } from '@/components/dashboard/DashboardIcons'
import { useProfileCompletion } from '@/hooks/useProfileCompletion'

export function ProfileCompletionBanner() {
  const { t } = useTranslation()
  const location = useLocation()
  const { progress, rewardClaimed, canClaimReward, config } = useProfileCompletion()

  if (rewardClaimed || location.pathname.includes('/profile')) return null

  const reward = config.campaign.reward

  return (
    <div className="mb-5 overflow-hidden rounded-sm border border-warm/25 bg-gradient-to-r from-warm-soft/80 via-surface to-surface p-4 shadow-sm sm:p-5">
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div className="flex min-w-0 items-start gap-3">
          <span className="flex size-10 shrink-0 items-center justify-center rounded-sm bg-warm text-warm-text shadow-sm">
            <ProfileCompletionIcon size={18} />
          </span>
          <div className="min-w-0">
            <p className="text-sm font-semibold text-text">
              {canClaimReward
                ? t('dashboard.profileCompletion.bannerClaimTitle', { percent: reward.value })
                : t('dashboard.profileCompletion.bannerTitle', { percent: reward.value })}
            </p>
            <p className="mt-1 text-xs leading-relaxed text-text-muted sm:text-sm">
              {canClaimReward
                ? t('dashboard.profileCompletion.bannerClaimMessage')
                : t('dashboard.profileCompletion.bannerMessage', {
                    answered: progress.answeredCount,
                    total: progress.totalCount,
                  })}
            </p>
            {!canClaimReward && (
              <div className="mt-3 h-1.5 max-w-xs overflow-hidden rounded-full bg-surface-muted">
                <div
                  className="h-full rounded-full bg-warm transition-all duration-500"
                  style={{ width: `${progress.percent}%` }}
                />
              </div>
            )}
          </div>
        </div>
        <Link
          to="/account/dashboard/profile"
          className="inline-flex shrink-0 items-center justify-center rounded-sm bg-warm px-4 py-2 text-xs font-semibold text-warm-text shadow-sm transition-colors hover:bg-warm-hover sm:text-sm"
        >
          {canClaimReward
            ? t('dashboard.profileCompletion.claimReward')
            : t('dashboard.profileCompletion.bannerCta')}
        </Link>
      </div>
    </div>
  )
}
