import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { BlogCommentsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { AdminListGridHeader } from '@/components/dashboard/admin/AdminListGridHeader'
import { AdminRowNumber } from '@/components/dashboard/admin/AdminRowNumber'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import {
  adminUserStoryCommentService,
  type AdminUserStoryComment,
} from '@/services/adminUserStoryCommentService'
import { userStoryService } from '@/services/userStoryService'
import type { UserStoryDraft } from '@/models/stories/story.model'
import { useUserStore } from '@/stores/userStore'

function truncateText(text: string, max = 160) {
  const trimmed = text.trim()
  if (trimmed.length <= max) return trimmed
  return `${trimmed.slice(0, max).trimEnd()}…`
}

export function UserStoryCommentsPanel() {
  const { t } = useTranslation()
  const [searchParams, setSearchParams] = useSearchParams()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()

  const storyIdFromUrl = searchParams.get('userStoryId') ?? ''

  const [items, setItems] = useState<AdminUserStoryComment[]>([])
  const [stories, setStories] = useState<UserStoryDraft[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [filterStoryId, setFilterStoryId] = useState(storyIdFromUrl)
  const [filterApproved, setFilterApproved] = useState('')

  const selectedStory = useMemo(
    () => stories.find((story) => story.id === filterStoryId) ?? null,
    [stories, filterStoryId],
  )

  useEffect(() => {
    setFilterStoryId(storyIdFromUrl)
  }, [storyIdFromUrl])

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.userStoryComments.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const storyItems = await userStoryService.searchActive(locale, 200)
      setStories(storyItems)

      const commentResult = await adminUserStoryCommentService.getAll(accessToken, locale, {
        pageSize: 200,
        userStoryId: filterStoryId || null,
        isApproved:
          filterApproved === 'approved' ? true : filterApproved === 'pending' ? false : null,
      })
      setItems(commentResult.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.userStoryComments.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, filterApproved, filterStoryId, locale, t])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  useEffect(() => {
    if (!storyIdFromUrl && stories.length > 0 && !filterStoryId) {
      const firstId = stories[0].id
      setFilterStoryId(firstId)
      setSearchParams({ userStoryId: firstId })
    }
  }, [storyIdFromUrl, filterStoryId, stories, setSearchParams])

  const handleStoryFilterChange = (id: string) => {
    setFilterStoryId(id)
    if (id) setSearchParams({ userStoryId: id })
    else setSearchParams({})
  }

  const handleApprove = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return

    setSaving(true)
    setError(null)
    try {
      await adminUserStoryCommentService.approve(accessToken, locale, id)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.userStoryComments.approveFailed')))
    } finally {
      setSaving(false)
    }
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.userStoryComments.title')}
        description={t('dashboard.userStoryComments.description')}
        icon={<BlogCommentsIcon size={22} />}
        action={
          <Link
            to="/account/dashboard/stories"
            className="text-sm font-medium text-warm hover:text-warm-hover"
          >
            {t('dashboard.userStoryComments.backToStories')}
          </Link>
        }
      />

      {error && (
        <p className="mb-4 rounded-xl border border-danger/30 bg-danger/5 px-4 py-3 text-sm text-danger">
          {error}
        </p>
      )}

      <div className="mb-6 grid gap-4 sm:grid-cols-2">
        <AdminField label={t('dashboard.userStoryComments.fieldStory')}>
          <select
            value={filterStoryId}
            onChange={(e) => handleStoryFilterChange(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.userStoryComments.allStories')}</option>
            {stories.map((story) => (
              <option key={story.id} value={story.id}>
                {story.title}
              </option>
            ))}
          </select>
        </AdminField>

        <AdminField label={t('dashboard.userStoryComments.filterApproved')}>
          <select
            value={filterApproved}
            onChange={(e) => setFilterApproved(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.userStoryComments.allStatuses')}</option>
            <option value="approved">{t('dashboard.userStoryComments.statusApproved')}</option>
            <option value="pending">{t('dashboard.userStoryComments.statusPending')}</option>
          </select>
        </AdminField>
      </div>

      {selectedStory && (
        <p className="mb-4 text-sm text-text-muted">
          {t('dashboard.userStoryComments.selectedStory', { title: selectedStory.title })}
        </p>
      )}

      {loading ? (
        <InlineLoading label={t('dashboard.userStoryComments.loading')} />
      ) : stories.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogCommentsIcon size={28} />}
          title={t('dashboard.userStoryComments.noStoriesTitle')}
          message={t('dashboard.userStoryComments.noStoriesMessage')}
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogCommentsIcon size={28} />}
          title={t('dashboard.userStoryComments.emptyTitle')}
          message={t('dashboard.userStoryComments.emptyMessage')}
        />
      ) : (
        <div className="space-y-3">
          <p className="text-sm text-text-muted">
            {t('dashboard.userStoryComments.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            <AdminListGridHeader />
            {items.map((item, index) => (
              <li
                key={item.id}
                className="flex flex-wrap items-start gap-3 px-4 py-3 sm:px-5"
              >
                <AdminRowNumber value={index + 1} />
                <div className="min-w-0 flex-1">
                  <p className="text-sm font-semibold text-text">
                    {item.authorName || t('dashboard.userStoryComments.unknownAuthor')}
                  </p>
                  <p className="mt-0.5 text-xs text-text-muted">{item.userStoryTitle}</p>
                  <p className="mt-2 text-sm leading-relaxed text-text">
                    {truncateText(item.content)}
                  </p>
                </div>
                <div className="flex flex-wrap items-center gap-2">
                  <span
                    className={`rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                      item.isApproved
                        ? 'bg-warm-soft text-warm'
                        : 'bg-surface-muted text-text-muted'
                    }`}
                  >
                    {item.isApproved
                      ? t('dashboard.userStoryComments.statusApproved')
                      : t('dashboard.userStoryComments.statusPending')}
                  </span>
                  {!item.isApproved && (
                    <Button
                      type="button"
                      size="sm"
                      disabled={saving}
                      onClick={() => void handleApprove(item.id)}
                    >
                      {t('dashboard.userStoryComments.approve')}
                    </Button>
                  )}
                </div>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  )
}
