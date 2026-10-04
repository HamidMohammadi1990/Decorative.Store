import { useCallback, useEffect, useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { DeleteIcon, ProfileCompletionIcon } from '@/components/dashboard/DashboardIcons'
import { AdminDataGrid } from '@/components/dashboard/admin/AdminDataGrid'
import {
  AdminGridActions,
  AdminGridIconButton,
} from '@/components/dashboard/admin/AdminGridActions'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { AdminLargeModal } from '@/components/dashboard/admin/AdminLargeModal'
import { useConfirm } from '@/hooks/useConfirm'
import { useAdminPagedList } from '@/hooks/useAdminPagedList'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { getProfileCompletionConfig } from '@/services/profileCompletionService'
import {
  adminProfileCompletionService,
  type AdminProfileCompletionUserState,
} from '@/services/profileCompletionApiService'
import type { ProfileAnswerValue, ProfileCompletionQuestion } from '@/models/profile/profileCompletion.model'
import { useUserStore } from '@/stores/userStore'

type RewardFilter = 'all' | 'claimed' | 'not_claimed'

function parseAnswers(answersJson: string): Record<string, ProfileAnswerValue> {
  try {
    const parsed = JSON.parse(answersJson) as Record<string, ProfileAnswerValue>
    return parsed && typeof parsed === 'object' ? parsed : {}
  } catch {
    return {}
  }
}

function formatAnswerValue(question: ProfileCompletionQuestion | undefined, value: ProfileAnswerValue): string {
  if (value == null || value === '') return '—'

  if (Array.isArray(value)) {
    if (!question?.options?.length) return value.join(', ')
    return value
      .map((item) => question.options?.find((option) => option.value === item)?.label ?? item)
      .join('، ')
  }

  if (question?.type === 'single_select' && question.options?.length) {
    return question.options.find((option) => option.value === value)?.label ?? String(value)
  }

  if (question?.type === 'date' && value) {
    const date = new Date(String(value))
    if (!Number.isNaN(date.getTime())) {
      return date.toLocaleDateString('fa-IR')
    }
  }

  return String(value)
}

function displayUserName(item: AdminProfileCompletionUserState): string {
  const fullName = [item.firstName, item.lastName].filter(Boolean).join(' ').trim()
  if (fullName) return fullName
  if (item.userName) return item.userName
  return item.email ?? `#${item.userId}`
}

function buildAnswerRows(
  item: AdminProfileCompletionUserState,
  questions: ProfileCompletionQuestion[],
  questionMap: Map<string, ProfileCompletionQuestion>,
) {
  const answers = parseAnswers(item.answersJson)
  const knownQuestions = [...questions].sort((a, b) => a.order - b.order)

  const rows = knownQuestions.map((question) => ({
    question,
    value: answers[question.id],
  }))

  for (const [questionId, value] of Object.entries(answers)) {
    if (!questionMap.has(questionId)) {
      rows.push({
        question: {
          id: questionId,
          type: 'text',
          label: questionId,
          required: false,
          order: 999,
        },
        value,
      })
    }
  }

  return rows
}

export function ProfileCompletionAnswersPanel() {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()

  const [userSearch, setUserSearch] = useState('')
  const [appliedUserSearch, setAppliedUserSearch] = useState<string | null>(null)
  const [rewardFilter, setRewardFilter] = useState<RewardFilter>('all')
  const [appliedRewardFilter, setAppliedRewardFilter] = useState<RewardFilter>('all')
  const [configLoading, setConfigLoading] = useState(true)
  const [configError, setConfigError] = useState<string | null>(null)
  const [adminConfig, setAdminConfig] = useState<Awaited<
    ReturnType<typeof adminProfileCompletionService.getConfig>
  > | null>(null)
  const [viewItem, setViewItem] = useState<AdminProfileCompletionUserState | null>(null)
  const [actionError, setActionError] = useState<string | null>(null)
  const [actionLoading, setActionLoading] = useState(false)

  const canLoad =
    !languageLoading &&
    Boolean(accessToken) &&
    accessToken !== 'mock-access-token'

  const localizedConfig = useMemo(
    () => (adminConfig ? getProfileCompletionConfig(locale, adminConfig) : null),
    [adminConfig, locale],
  )

  const questionMap = useMemo(() => {
    const map = new Map<string, ProfileCompletionQuestion>()
    for (const question of localizedConfig?.questions ?? []) {
      map.set(question.id, question)
    }
    return map
  }, [localizedConfig])

  useEffect(() => {
    if (!canLoad || !accessToken) {
      setConfigLoading(false)
      return
    }

    let cancelled = false
    const loadConfig = async () => {
      setConfigLoading(true)
      setConfigError(null)
      try {
        const remote = await adminProfileCompletionService.getConfig(accessToken)
        if (!cancelled) setAdminConfig(remote)
      } catch (err) {
        if (!cancelled) {
          setConfigError(
            resolveAdminMutationError(err, t('dashboard.profileCompletionAnswers.loadConfigFailed')),
          )
        }
      } finally {
        if (!cancelled) setConfigLoading(false)
      }
    }

    void loadConfig()
    return () => {
      cancelled = true
    }
  }, [accessToken, canLoad, t])

  const rewardClaimedParam =
    appliedRewardFilter === 'claimed' ? true : appliedRewardFilter === 'not_claimed' ? false : null

  const fetchPage = useCallback(
    async (pageNumber: number, pageSize: number) => {
      if (!accessToken || accessToken === 'mock-access-token') {
        throw new Error(t('dashboard.profileCompletionAnswers.authRequired'))
      }
      return adminProfileCompletionService.getUserStates(accessToken, {
        pageNumber,
        pageSize,
        userSearch: appliedUserSearch,
        rewardClaimed: rewardClaimedParam,
      })
    },
    [accessToken, appliedUserSearch, rewardClaimedParam, t],
  )

  const {
    items,
    loading: listLoading,
    error: listError,
    pageNumber,
    pageSize,
    totalCount,
    totalPages,
    goToPage,
    reload,
  } = useAdminPagedList<AdminProfileCompletionUserState>({
    fetchPage,
    initialPageSize: 20,
    enabled: canLoad && !configLoading,
  })

  const viewAnswers = useMemo(() => {
    if (!viewItem) return []
    return buildAnswerRows(viewItem, localizedConfig?.questions ?? [], questionMap)
  }, [localizedConfig?.questions, questionMap, viewItem])

  const applyFilters = () => {
    setAppliedUserSearch(userSearch.trim() || null)
    setAppliedRewardFilter(rewardFilter)
    setViewItem(null)
  }

  const clearFilters = () => {
    setUserSearch('')
    setAppliedUserSearch(null)
    setRewardFilter('all')
    setAppliedRewardFilter('all')
    setViewItem(null)
  }

  const handleDelete = async (item: AdminProfileCompletionUserState) => {
    if (!accessToken) return
    const accepted = await confirm({
      title: t('dashboard.profileCompletionAnswers.deleteConfirmTitle'),
      message: t('dashboard.profileCompletionAnswers.deleteConfirm', { user: displayUserName(item) }),
      confirmLabel: t('dashboard.profileCompletionAnswers.delete'),
      cancelLabel: t('dashboard.profileCompletionAnswers.cancel'),
    })
    if (!accepted) return

    setActionLoading(true)
    setActionError(null)
    try {
      await adminProfileCompletionService.deleteUserState(accessToken, item.id)
      if (viewItem?.id === item.id) setViewItem(null)
      await reload()
    } catch (err) {
      setActionError(resolveAdminMutationError(err, t('dashboard.profileCompletionAnswers.deleteFailed')))
    } finally {
      setActionLoading(false)
    }
  }

  const handleResetReward = async (item: AdminProfileCompletionUserState) => {
    if (!accessToken) return
    const accepted = await confirm({
      title: t('dashboard.profileCompletionAnswers.resetRewardConfirmTitle'),
      message: t('dashboard.profileCompletionAnswers.resetRewardConfirm', { user: displayUserName(item) }),
      confirmLabel: t('dashboard.profileCompletionAnswers.resetReward'),
      cancelLabel: t('dashboard.profileCompletionAnswers.cancel'),
    })
    if (!accepted) return

    setActionLoading(true)
    setActionError(null)
    try {
      await adminProfileCompletionService.updateUserState(accessToken, {
        id: item.id,
        clearRewardClaim: true,
      })
      await reload()
    } catch (err) {
      setActionError(resolveAdminMutationError(err, t('dashboard.profileCompletionAnswers.resetRewardFailed')))
    } finally {
      setActionLoading(false)
    }
  }

  const listErrorMessage = listError
    ? resolveAdminMutationError(listError, t('dashboard.profileCompletionAnswers.loadFailed'))
    : null

  if (!canLoad) {
    return (
      <DashboardEmptyState
        icon={<ProfileCompletionIcon size={28} />}
        title={t('dashboard.profileCompletionAnswers.emptyTitle')}
        message={t('dashboard.profileCompletionAnswers.authRequired')}
      />
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.profileCompletionAnswers.title')}
        description={t('dashboard.profileCompletionAnswers.description')}
        icon={<ProfileCompletionIcon size={22} />}
      />

      <div className="mb-5 flex flex-col gap-3 lg:flex-row lg:items-end">
        <div className="flex-1">
          <AdminField label={t('dashboard.profileCompletionAnswers.fieldUserSearch')}>
            <input
              type="search"
              value={userSearch}
              onChange={(event) => setUserSearch(event.target.value)}
              placeholder={t('dashboard.profileCompletionAnswers.fieldUserSearchPlaceholder')}
              className={adminInputClass}
              onKeyDown={(event) => {
                if (event.key === 'Enter') applyFilters()
              }}
            />
          </AdminField>
        </div>
        <div className="w-full lg:w-52">
          <AdminField label={t('dashboard.profileCompletionAnswers.fieldRewardFilter')}>
            <select
              value={rewardFilter}
              onChange={(event) => setRewardFilter(event.target.value as RewardFilter)}
              className={adminInputClass}
            >
              <option value="all">{t('dashboard.profileCompletionAnswers.rewardFilterAll')}</option>
              <option value="claimed">{t('dashboard.profileCompletionAnswers.rewardFilterClaimed')}</option>
              <option value="not_claimed">{t('dashboard.profileCompletionAnswers.rewardFilterNotClaimed')}</option>
            </select>
          </AdminField>
        </div>
        <div className="flex flex-wrap gap-2">
          <Button variant="secondary" onClick={applyFilters}>
            {t('dashboard.profileCompletionAnswers.search')}
          </Button>
          {(appliedUserSearch || appliedRewardFilter !== 'all') && (
            <Button variant="secondary" onClick={clearFilters}>
              {t('dashboard.profileCompletionAnswers.clearSearch')}
            </Button>
          )}
        </div>
      </div>

      {configError && (
        <p className="mb-4 text-sm text-red-600" role="alert">
          {configError}
        </p>
      )}

      {actionError && (
        <p className="mb-4 text-sm text-red-600" role="alert">
          {actionError}
        </p>
      )}

      {configLoading || listLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.profileCompletionAnswers.loading')} />
        </div>
      ) : listErrorMessage ? (
        <DashboardEmptyState
          icon={<ProfileCompletionIcon size={28} />}
          title={t('dashboard.profileCompletionAnswers.loadFailedTitle')}
          message={listErrorMessage}
          action={
            <Button variant="secondary" onClick={() => reload()}>
              {t('dashboard.profileCompletionAnswers.retry')}
            </Button>
          }
        />
      ) : (items ?? []).length === 0 ? (
        <DashboardEmptyState
          icon={<ProfileCompletionIcon size={28} />}
          title={t('dashboard.profileCompletionAnswers.emptyTitle')}
          message={t('dashboard.profileCompletionAnswers.emptyMessage')}
        />
      ) : (
        <>
          <AdminDataGrid
            className="w-full"
            rows={items ?? []}
            rowKey={(item) => String(item.id)}
            loading={listLoading || actionLoading}
            loadingLabel={t('dashboard.profileCompletionAnswers.loading')}
            pagination={{
              pageNumber,
              pageSize,
              totalCount,
              totalPages,
              onPageChange: goToPage,
            }}
            columns={[
              {
                id: 'user',
                header: t('dashboard.profileCompletionAnswers.colUser'),
                cell: (item) => (
                  <div>
                    <p className="font-medium text-text">{displayUserName(item)}</p>
                    {item.email && (
                      <p className="text-xs text-text-muted" dir="ltr">
                        {item.email}
                      </p>
                    )}
                  </div>
                ),
              },
              {
                id: 'updated',
                header: t('dashboard.profileCompletionAnswers.colUpdated'),
                cell: (item) =>
                  item.updatedOnUtc
                    ? new Date(item.updatedOnUtc).toLocaleString(locale === 'fa' ? 'fa-IR' : 'en-US')
                    : '—',
              },
              {
                id: 'reward',
                header: t('dashboard.profileCompletionAnswers.colReward'),
                align: 'center',
                cell: (item) => (
                  <span
                    className={`inline-flex rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                      item.rewardClaimedOnUtc
                        ? 'bg-emerald-500/10 text-emerald-700'
                        : 'bg-surface-muted text-text-muted'
                    }`}
                  >
                    {item.rewardClaimedOnUtc
                      ? t('dashboard.profileCompletionAnswers.rewardClaimed')
                      : t('dashboard.profileCompletionAnswers.rewardNotClaimed')}
                  </span>
                ),
              },
              {
                id: 'actions',
                header: t('dashboard.profileCompletionAnswers.colActions'),
                align: 'right',
                cell: (item) => (
                  <AdminGridActions>
                    <Button
                      variant="secondary"
                      className="!px-2 !py-1 text-xs"
                      onClick={() => setViewItem(item)}
                    >
                      {t('dashboard.profileCompletionAnswers.viewAnswers')}
                    </Button>
                    {item.rewardClaimedOnUtc && (
                      <Button
                        variant="secondary"
                        className="!px-2 !py-1 text-xs"
                        disabled={actionLoading}
                        onClick={() => void handleResetReward(item)}
                      >
                        {t('dashboard.profileCompletionAnswers.resetReward')}
                      </Button>
                    )}
                    <AdminGridIconButton
                      label={t('dashboard.profileCompletionAnswers.delete')}
                      icon={<DeleteIcon size={16} />}
                      onClick={() => void handleDelete(item)}
                      disabled={actionLoading}
                      tone="danger"
                    />
                  </AdminGridActions>
                ),
              },
            ]}
          />

          <AdminLargeModal
            open={viewItem != null}
            title={
              viewItem
                ? t('dashboard.profileCompletionAnswers.detailTitleFor', {
                    user: displayUserName(viewItem),
                  })
                : t('dashboard.profileCompletionAnswers.detailTitle')
            }
            description={
              viewItem?.email
                ? viewItem.email
                : undefined
            }
            onClose={() => setViewItem(null)}
          >
            {viewItem && (
              <div className="space-y-4">
                <div className="rounded-lg bg-surface-muted/60 p-3 text-sm">
                  <p className="font-medium text-text">{displayUserName(viewItem)}</p>
                  {viewItem.email && (
                    <p className="text-text-muted" dir="ltr">
                      {viewItem.email}
                    </p>
                  )}
                  <p className="mt-2 text-xs text-text-muted">
                    {t('dashboard.profileCompletionAnswers.updatedAt', {
                      date: new Date(viewItem.updatedOnUtc).toLocaleString(
                        locale === 'fa' ? 'fa-IR' : 'en-US',
                      ),
                    })}
                  </p>
                  {viewItem.rewardClaimedOnUtc && (
                    <p className="mt-1 text-xs text-emerald-700">
                      {t('dashboard.profileCompletionAnswers.claimedAt', {
                        date: new Date(viewItem.rewardClaimedOnUtc).toLocaleString(
                          locale === 'fa' ? 'fa-IR' : 'en-US',
                        ),
                      })}
                    </p>
                  )}
                </div>

                {viewAnswers.length === 0 ? (
                  <p className="text-sm text-text-muted">
                    {t('dashboard.profileCompletionAnswers.noAnswers')}
                  </p>
                ) : (
                  <dl className="space-y-3">
                    {viewAnswers.map(({ question, value }) => (
                      <div key={question.id} className="border-b border-border/70 pb-3 last:border-0">
                        <dt className="text-xs font-semibold uppercase tracking-wide text-text-muted">
                          {question.label}
                        </dt>
                        <dd className="mt-1 text-sm text-text">
                          {formatAnswerValue(question, value ?? '')}
                        </dd>
                      </div>
                    ))}
                  </dl>
                )}
              </div>
            )}
          </AdminLargeModal>
        </>
      )}
    </div>
  )
}
