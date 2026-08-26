import { useCallback, useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { AdminBlogPostCategory } from '@/models/admin/blog.model'
import type { AdminBlogPostListItem } from '@/models/admin/blog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { BlogPostsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminBlogPostCategoryService } from '@/services/adminBlogPostCategoryService'
import { adminBlogPostService } from '@/services/adminBlogPostService'
import { useUserStore } from '@/stores/userStore'

export function BlogPostsPanel() {
  const { t } = useTranslation()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [items, setItems] = useState<AdminBlogPostListItem[]>([])
  const [categories, setCategories] = useState<AdminBlogPostCategory[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [filterCategoryId, setFilterCategoryId] = useState('')
  const [filterPublished, setFilterPublished] = useState('')

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.blogPosts.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const [postResult, categoryResult] = await Promise.all([
        adminBlogPostService.getAll(accessToken, locale, {
          pageSize: 200,
          categoryId: filterCategoryId || null,
          isPublished:
            filterPublished === 'published'
              ? true
              : filterPublished === 'draft'
                ? false
                : null,
        }),
        adminBlogPostCategoryService.getAll(accessToken, locale, {
          pageSize: 100,
          languageId: languageId ?? undefined,
        }),
      ])
      setItems(postResult.items)
      setCategories(categoryResult.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPosts.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, filterCategoryId, filterPublished, languageId, locale, t])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.blogPosts.title')}
        description={t('dashboard.blogPosts.description')}
        icon={<BlogPostsIcon size={22} />}
        action={
          <Link
            to="/account/dashboard/blog-posts/new"
            className="inline-flex items-center justify-center rounded-sm bg-warm px-4 py-2 text-sm font-medium text-warm-text transition-colors hover:bg-warm-hover"
          >
            {t('dashboard.blogPosts.add')}
          </Link>
        }
      />

      <div className="mb-4 grid gap-4 sm:grid-cols-2">
        <AdminField label={t('dashboard.blogPosts.filterCategory')}>
          <select
            value={filterCategoryId}
            onChange={(e) => setFilterCategoryId(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.blogPosts.allCategories')}</option>
            {categories.map((category) => (
              <option key={category.id} value={category.id}>
                {category.title}
              </option>
            ))}
          </select>
        </AdminField>

        <AdminField label={t('dashboard.blogPosts.filterPublished')}>
          <select
            value={filterPublished}
            onChange={(e) => setFilterPublished(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.blogPosts.allStatuses')}</option>
            <option value="published">{t('dashboard.blogPosts.statusPublished')}</option>
            <option value="draft">{t('dashboard.blogPosts.statusDraft')}</option>
          </select>
        </AdminField>
      </div>

      {loading || languageLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.blogPosts.loading')} />
        </div>
      ) : categories.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogPostsIcon size={28} />}
          title={t('dashboard.blogPosts.noCategoriesTitle')}
          message={t('dashboard.blogPosts.noCategoriesMessage')}
          action={
            <Link
              to="/account/dashboard/blog-categories"
              className="inline-flex items-center justify-center rounded-sm bg-warm px-4 py-2 text-sm font-medium text-warm-text transition-colors hover:bg-warm-hover"
            >
              {t('dashboard.blogPosts.goToCategories')}
            </Link>
          }
        />
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogPostsIcon size={28} />}
          title={t('dashboard.blogPosts.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.blogPosts.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogPostsIcon size={28} />}
          title={t('dashboard.blogPosts.emptyTitle')}
          message={t('dashboard.blogPosts.emptyMessage')}
          action={
            <Link
              to="/account/dashboard/blog-posts/new"
              className="inline-flex items-center justify-center rounded-sm bg-warm px-4 py-2 text-sm font-medium text-warm-text transition-colors hover:bg-warm-hover"
            >
              {t('dashboard.blogPosts.add')}
            </Link>
          }
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.blogPosts.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            {items.map((item) => (
              <li
                key={item.id}
                className="flex flex-wrap items-center justify-between gap-3 px-4 py-3 sm:px-5"
              >
                <div className="min-w-0">
                  <p className="truncate text-sm font-semibold text-text">{item.title}</p>
                  <p className="mt-0.5 text-xs text-text-muted">
                    {item.categoryTitle}
                    {item.authorName ? ` · ${item.authorName}` : ''}
                  </p>
                </div>
                <div className="flex flex-wrap items-center gap-2">
                  <span
                    className={`rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                      item.isPublished
                        ? 'bg-warm-soft text-warm'
                        : 'bg-surface-muted text-text-muted'
                    }`}
                  >
                    {item.isPublished
                      ? t('dashboard.blogPosts.statusPublished')
                      : t('dashboard.blogPosts.statusDraft')}
                  </span>
                  <Link
                    to={`/account/dashboard/blog-posts/edit?id=${encodeURIComponent(item.id)}`}
                    className="inline-flex items-center justify-center rounded-sm border border-border-strong px-3 py-1.5 text-xs font-medium text-text transition-colors hover:bg-surface-muted"
                  >
                    {t('dashboard.blogPosts.edit')}
                  </Link>
                  <Link
                    to={`/account/dashboard/blog-post-tags?blogPostId=${encodeURIComponent(item.id)}`}
                    className="inline-flex items-center justify-center rounded-sm border border-border-strong px-3 py-1.5 text-xs font-medium text-text transition-colors hover:bg-surface-muted"
                  >
                    {t('dashboard.blogPosts.tags')}
                  </Link>
                  <Link
                    to={`/account/dashboard/blog-post-comments?blogPostId=${encodeURIComponent(item.id)}`}
                    className="inline-flex items-center justify-center rounded-sm border border-border-strong px-3 py-1.5 text-xs font-medium text-text transition-colors hover:bg-surface-muted"
                  >
                    {t('dashboard.blogPosts.comments')}
                  </Link>
                </div>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  )
}
