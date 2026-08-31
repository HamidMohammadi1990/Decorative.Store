import { useCallback, useEffect, useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { AdminLargeModal } from '@/components/dashboard/admin/AdminLargeModal'
import { resolveAdminMutationError } from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import type { DashboardStoryGroup } from '@/extensions/groupUserStoriesForDashboard'
import {
  adminUserStoryCommentService,
  type AdminUserStoryComment,
} from '@/services/adminUserStoryCommentService'
import { useUserStore } from '@/stores/userStore'

function truncateText(text: string, max = 320) {
  const trimmed = text.trim()
  if (trimmed.length <= max) return trimmed
  return `${trimmed.slice(0, max).trimEnd()}…`
}

export function StoryGroupCommentsModal({
  open,
  group,
  onClose,
}: {
  open: boolean
  group: DashboardStoryGroup | null
  onClose: () => void
}) {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()

  const [items, setItems] = useState<AdminUserStoryComment[]>([])
  const [loading, setLoading] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [filterApproved, setFilterApproved] = useState('')

  const slideIds = useMemo(() => group?.slides.map((slide) => slide.id) ?? [], [group])

  const load = useCallback(async () => {
    if (!open || !group || !accessToken || accessToken === 'mock-access-token') {
      setItems([])
      return
    }

    if (slideIds.length === 0) {
      setItems([])
      return
    }

    setLoading(true)
    setError(null)

    try {
      const isApproved =
        filterApproved === 'approved' ? true : filterApproved === 'pending' ? false : null

      const results = await Promise.all(
        slideIds.map((userStoryId) =>
          adminUserStoryCommentService.getAll(accessToken, locale, {
            pageSize: 100,
            userStoryId,
            isApproved,
          }),
        ),
      )

      const merged = results
        .flatMap((result) => result.items)
        .sort((a, b) => b.createdOnUtc.localeCompare(a.createdOnUtc))

      setItems(merged)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.userStoryComments.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, filterApproved, group, locale, open, slideIds, t])

  useEffect(() => {
    if (!open) {
      setItems([])
      setError(null)
      setFilterApproved('')
      return
    }

    if (!languageLoading) void load()
  }, [languageLoading, load, open])

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

  if (!group) return null

  return (
    <AdminLargeModal
      open={open}
      title={t('dashboard.stories.commentsModalTitle')}
      description={t('dashboard.stories.commentsModalDescription', { title: group.title })}
      onClose={onClose}
    >
      {error && (
        <p className="mb-4 rounded-xl border border-danger/30 bg-danger/5 px-4 py-3 text-sm text-danger">
          {error}
        </p>
      )}

      <div className="mb-4">
        <label className="block text-sm font-medium text-text">
          {t('dashboard.userStoryComments.filterApproved')}
        </label>
        <select
          value={filterApproved}
          onChange={(e) => setFilterApproved(e.target.value)}
          className="mt-2 w-full rounded-lg border border-border bg-surface px-3 py-2.5 text-sm text-text outline-none focus:border-warm"
        >
          <option value="">{t('dashboard.userStoryComments.allStatuses')}</option>
          <option value="approved">{t('dashboard.userStoryComments.statusApproved')}</option>
          <option value="pending">{t('dashboard.userStoryComments.statusPending')}</option>
        </select>
      </div>

      {loading ? (
        <InlineLoading label={t('dashboard.userStoryComments.loading')} />
      ) : items.length === 0 ? (
        <p className="rounded-xl border border-border bg-surface-muted/30 px-4 py-8 text-center text-sm text-text-muted">
          {t('dashboard.stories.commentsEmpty')}
        </p>
      ) : (
        <div className="space-y-3">
          <p className="text-sm text-text-muted">
            {t('dashboard.userStoryComments.itemCount', { count: items.length })}
          </p>
          {items.map((item) => (
            <article
              key={item.id}
              className="rounded-2xl border border-border bg-surface p-4 shadow-sm"
            >
              <div className="flex flex-wrap items-start justify-between gap-3">
                <div className="min-w-0">
                  <p className="text-sm font-semibold text-text">
                    {item.authorName || t('dashboard.userStoryComments.unknownAuthor')}
                  </p>
                  {slideIds.length > 1 && item.userStoryTitle && (
                    <p className="mt-1 text-xs text-text-muted">{item.userStoryTitle}</p>
                  )}
                </div>
                <span
                  className={`rounded-full px-2.5 py-1 text-xs font-medium ${
                    item.isApproved ? 'bg-success/10 text-success' : 'bg-warm-soft text-warm'
                  }`}
                >
                  {item.isApproved
                    ? t('dashboard.userStoryComments.statusApproved')
                    : t('dashboard.userStoryComments.statusPending')}
                </span>
              </div>
              <p className="mt-3 text-sm leading-relaxed text-text">{truncateText(item.content)}</p>
              {!item.isApproved && (
                <div className="mt-4">
                  <Button
                    type="button"
                    size="sm"
                    disabled={saving}
                    onClick={() => void handleApprove(item.id)}
                  >
                    {t('dashboard.userStoryComments.approve')}
                  </Button>
                </div>
              )}
            </article>
          ))}
        </div>
      )}
    </AdminLargeModal>
  )
}
