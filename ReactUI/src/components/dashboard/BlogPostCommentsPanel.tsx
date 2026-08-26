import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { AdminBlogPostComment } from '@/models/admin/blog.model'
import type { AdminBlogPostListItem } from '@/models/admin/blog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { BlogCommentsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminBlogPostCommentService } from '@/services/adminBlogPostCommentService'
import { adminBlogPostService } from '@/services/adminBlogPostService'
import { useUserStore } from '@/stores/userStore'

function truncateText(text: string, max = 160) {
  const trimmed = text.trim()
  if (trimmed.length <= max) return trimmed
  return `${trimmed.slice(0, max).trimEnd()}…`
}

export function BlogPostCommentsPanel() {
  const { t } = useTranslation()
  const [searchParams, setSearchParams] = useSearchParams()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()

  const blogPostIdFromUrl = searchParams.get('blogPostId') ?? ''

  const [items, setItems] = useState<AdminBlogPostComment[]>([])
  const [posts, setPosts] = useState<AdminBlogPostListItem[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [filterBlogPostId, setFilterBlogPostId] = useState(blogPostIdFromUrl)
  const [filterApproved, setFilterApproved] = useState('')

  const selectedPost = useMemo(
    () => posts.find((p) => p.id === filterBlogPostId) ?? null,
    [posts, filterBlogPostId],
  )

  useEffect(() => {
    setFilterBlogPostId(blogPostIdFromUrl)
  }, [blogPostIdFromUrl])

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.blogPostComments.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const postResult = await adminBlogPostService.getAll(accessToken, locale, { pageSize: 200 })
      setPosts(postResult.items)

      const commentResult = await adminBlogPostCommentService.getAll(accessToken, locale, {
        pageSize: 200,
        blogPostId: filterBlogPostId || null,
        isApproved:
          filterApproved === 'approved' ? true : filterApproved === 'pending' ? false : null,
      })
      setItems(commentResult.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPostComments.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, filterApproved, filterBlogPostId, locale, t])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  useEffect(() => {
    if (!blogPostIdFromUrl && posts.length > 0 && !filterBlogPostId) {
      const firstId = posts[0].id
      setFilterBlogPostId(firstId)
      setSearchParams({ blogPostId: firstId })
    }
  }, [blogPostIdFromUrl, filterBlogPostId, posts, setSearchParams])

  const handlePostFilterChange = (id: string) => {
    setFilterBlogPostId(id)
    if (id) setSearchParams({ blogPostId: id })
    else setSearchParams({})
  }

  const handleApprove = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return

    setSaving(true)
    setError(null)
    try {
      await adminBlogPostCommentService.approve(accessToken, locale, id)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPostComments.approveFailed')))
    } finally {
      setSaving(false)
    }
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.blogPostComments.title')}
        description={t('dashboard.blogPostComments.description')}
        icon={<BlogCommentsIcon size={22} />}
        action={
          <Link
            to="/account/dashboard/blog-posts"
            className="inline-flex items-center justify-center rounded-sm border border-border-strong px-4 py-2 text-sm font-medium text-text transition-colors hover:bg-surface-muted"
          >
            {t('dashboard.blogPostComments.backToPosts')}
          </Link>
        }
      />

      <div className="mb-4 grid gap-4 sm:grid-cols-2">
        <AdminField label={t('dashboard.blogPostComments.fieldPost')}>
          <select
            value={filterBlogPostId}
            onChange={(e) => handlePostFilterChange(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.blogPostComments.allPosts')}</option>
            {posts.map((post) => (
              <option key={post.id} value={post.id}>
                {post.title}
              </option>
            ))}
          </select>
        </AdminField>

        <AdminField label={t('dashboard.blogPostComments.filterApproved')}>
          <select
            value={filterApproved}
            onChange={(e) => setFilterApproved(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.blogPostComments.allStatuses')}</option>
            <option value="approved">{t('dashboard.blogPostComments.statusApproved')}</option>
            <option value="pending">{t('dashboard.blogPostComments.statusPending')}</option>
          </select>
        </AdminField>
      </div>

      {selectedPost && (
        <p className="mb-4 text-xs text-text-muted">{selectedPost.title}</p>
      )}

      {loading || languageLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.blogPostComments.loading')} />
        </div>
      ) : posts.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogCommentsIcon size={28} />}
          title={t('dashboard.blogPostComments.noPostsTitle')}
          message={t('dashboard.blogPostComments.noPostsMessage')}
        />
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogCommentsIcon size={28} />}
          title={t('dashboard.blogPostComments.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.blogPostComments.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogCommentsIcon size={28} />}
          title={t('dashboard.blogPostComments.emptyTitle')}
          message={t('dashboard.blogPostComments.emptyMessage')}
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.blogPostComments.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            {items.map((item) => (
              <li
                key={item.id}
                className="flex flex-wrap items-start justify-between gap-3 px-4 py-3 sm:px-5"
              >
                <div className="min-w-0 flex-1">
                  <p className="text-sm text-text">{truncateText(item.content)}</p>
                  <p className="mt-1 text-xs text-text-muted">
                    {item.authorName || t('dashboard.blogPostComments.unknownAuthor')}
                    {!filterBlogPostId && item.blogPostTitle ? ` · ${item.blogPostTitle}` : ''}
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
                      ? t('dashboard.blogPostComments.statusApproved')
                      : t('dashboard.blogPostComments.statusPending')}
                  </span>
                  {!item.isApproved && (
                    <Button
                      variant="secondary"
                      className="py-1.5 text-xs"
                      onClick={() => void handleApprove(item.id)}
                      disabled={saving}
                    >
                      {t('dashboard.blogPostComments.approve')}
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
