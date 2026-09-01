import { Link } from 'react-router-dom'
import { useState } from 'react'
import { useTranslation } from 'react-i18next'
import { ProfileCompletionQuestionField } from '@/components/dashboard/ProfileCompletionQuestionField'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { isAnswerFilled } from '@/extensions/calculateProfileCompletion'
import { useProfileCompletion } from '@/hooks/useProfileCompletion'

export function ProfileCompletionUserSection() {
  const { t } = useTranslation()
  const {
    config,
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
    rewardClaimedLabel,
  } = useProfileCompletion()

  const [savedHint, setSavedHint] = useState(false)

  const handleSave = async () => {
    const ok = await saveAnswers()
    if (ok) setSavedHint(true)
  }

  if (loading) {
    return (
      <div className="flex justify-center py-12">
        <InlineLoading label={t('dashboard.profileCompletion.loading')} />
      </div>
    )
  }

  if (!config) {
    return (
      <div className="rounded-sm border border-sale/30 bg-sale/5 px-4 py-6 text-center text-sm text-sale">
        {syncError ?? t('dashboard.profileCompletion.syncFailed')}
      </div>
    )
  }

  const reward = config.campaign.reward

  return (
    <>
      <div className="mb-6 grid gap-4 lg:grid-cols-[minmax(0,1fr)_minmax(240px,280px)] lg:items-start">
        <section className="overflow-hidden rounded-sm border border-border bg-gradient-to-br from-warm-soft/50 via-surface to-surface p-5 shadow-sm sm:p-6">
          <div className="flex flex-col gap-5 sm:flex-row sm:items-center">
            <ProgressRing percent={progress.percent} />
            <div className="min-w-0 flex-1">
              <p className="text-xs font-semibold uppercase tracking-[0.14em] text-warm">
                {t('dashboard.profileCompletion.progressLabel')}
              </p>
              <p className="mt-1 text-2xl font-semibold text-text">
                {t('dashboard.profileCompletion.progressValue', {
                  percent: progress.percent,
                })}
              </p>
              <p className="mt-2 text-sm text-text-muted">
                {t('dashboard.profileCompletion.progressHint', {
                  answered: progress.answeredCount,
                  total: progress.totalCount,
                })}
              </p>
              <div className="mt-4 h-2 overflow-hidden rounded-full bg-surface-muted">
                <div
                  className="h-full rounded-full bg-gradient-to-r from-warm to-accent transition-all duration-500 ease-out"
                  style={{ width: `${progress.percent}%` }}
                />
              </div>
            </div>
          </div>
        </section>

        <aside
          className={`rounded-sm border p-5 shadow-sm transition-all duration-300 ${
            progress.isComplete
              ? 'border-warm/40 bg-gradient-to-br from-warm-soft/70 to-surface'
              : 'border-border bg-surface-muted/30'
          }`}
        >
          <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-text-muted">
            {t('dashboard.profileCompletion.rewardLabel')}
          </p>
          <p className="mt-2 text-3xl font-bold text-warm">
            {reward.type === 'percent_discount'
              ? t('dashboard.profileCompletion.rewardPercent', { value: reward.value })
              : reward.description}
          </p>
          <p className="mt-1 text-sm text-text">{reward.description}</p>
          <p className="mt-3 rounded-sm border border-border/70 bg-surface/70 px-3 py-2 font-mono text-sm font-semibold tracking-wider text-warm">
            {reward.code}
          </p>
          <p className="mt-3 text-xs leading-relaxed text-text-muted">
            {t('dashboard.profileCompletion.rewardExpiry', { days: reward.validDays })}
          </p>

          {rewardClaimed ? (
            <div className="mt-4 rounded-sm border border-warm/30 bg-warm-soft/60 px-3 py-3">
              <p className="text-sm font-semibold text-warm">
                {t('dashboard.profileCompletion.rewardClaimedTitle')}
              </p>
              {rewardClaimedLabel && (
                <p className="mt-1 text-xs text-text-muted">{rewardClaimedLabel}</p>
              )}
              <Link
                to="/account/dashboard/coupons"
                className="mt-3 inline-flex text-xs font-semibold text-warm hover:underline"
              >
                {t('dashboard.profileCompletion.viewCoupons')}
              </Link>
            </div>
          ) : canClaimReward ? (
            <Button variant="warm" className="mt-4 w-full" onClick={() => void claimReward()}>
              {t('dashboard.profileCompletion.claimReward')}
            </Button>
          ) : (
            <p className="mt-4 text-xs text-text-muted">
              {t('dashboard.profileCompletion.rewardLocked')}
            </p>
          )}
        </aside>
      </div>

      <div className="space-y-4">
        <div className="flex flex-wrap items-center justify-between gap-3">
          <h2 className="text-sm font-semibold uppercase tracking-[0.12em] text-text-muted">
            {t('dashboard.profileCompletion.questionsTitle')}
          </h2>
          <Button variant="secondary" className="text-xs" onClick={() => void handleSave()} disabled={saving}>
            {saving ? (
              <InlineLoading label={t('dashboard.profileCompletion.saving')} />
            ) : (
              t('dashboard.profileCompletion.saveAnswers')
            )}
          </Button>
        </div>

        {syncError && <p className="text-sm text-sale">{syncError}</p>}
        {savedHint && (
          <p className="text-sm text-warm">{t('dashboard.profileCompletion.savedHint')}</p>
        )}

        {config.questions.map((question) => (
          <ProfileCompletionQuestionField
            key={question.id}
            question={question}
            value={answers[question.id]}
            answered={isAnswerFilled(question, answers[question.id])}
            onChange={(value) => setAnswer(question.id, value)}
          />
        ))}
      </div>

      {canClaimReward && (
        <div className="sticky bottom-0 mt-6 -mx-5 border-t border-border/70 bg-surface/95 px-5 py-4 backdrop-blur-sm sm:-mx-6 sm:px-6 lg:-mx-8 lg:px-8">
          <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
            <p className="text-sm text-text">
              {t('dashboard.profileCompletion.readyToClaim')}
            </p>
            <Button variant="warm" className="sm:min-w-[12rem]" onClick={() => void claimReward()}>
              {t('dashboard.profileCompletion.claimReward')}
            </Button>
          </div>
        </div>
      )}
    </>
  )
}

function ProgressRing({ percent }: { percent: number }) {
  const size = 96
  const stroke = 8
  const radius = (size - stroke) / 2
  const circumference = 2 * Math.PI * radius
  const offset = circumference - (percent / 100) * circumference

  return (
    <div className="relative mx-auto shrink-0 sm:mx-0" style={{ width: size, height: size }}>
      <svg width={size} height={size} className="-rotate-90" aria-hidden>
        <circle
          cx={size / 2}
          cy={size / 2}
          r={radius}
          fill="none"
          stroke="currentColor"
          strokeWidth={stroke}
          className="text-surface-muted"
        />
        <circle
          cx={size / 2}
          cy={size / 2}
          r={radius}
          fill="none"
          stroke="currentColor"
          strokeWidth={stroke}
          strokeLinecap="round"
          strokeDasharray={circumference}
          strokeDashoffset={offset}
          className="text-warm transition-all duration-500 ease-out"
        />
      </svg>
      <span className="absolute inset-0 flex items-center justify-center text-lg font-bold text-text">
        {percent}%
      </span>
    </div>
  )
}
