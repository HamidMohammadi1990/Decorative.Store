import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import type { AdminBlogPostListItem } from '@/models/admin/blog.model'
import type { AdminBlogPostTag } from '@/models/admin/blog.model'
import type { AdminTag } from '@/models/admin/blog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { BlogPostTagsIcon, EditIcon, DeleteIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminGridActions,
  AdminGridIconButton,
} from '@/components/dashboard/admin/AdminGridActions'
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
import { adminBlogPostService } from '@/services/adminBlogPostService'
import { adminBlogPostTagService } from '@/services/adminBlogPostTagService'
import { adminTagService } from '@/services/adminTagService'
import { useUserStore } from '@/stores/userStore'

type Mode = 'list' | 'create' | 'edit'

export function BlogPostTagsPanel() {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const [searchParams, setSearchParams] = useSearchParams()
  const accessToken = useUserStore((s) => s.accessToken)
  const { locale, loading: languageLoading } = useCurrentLanguageId()

  const blogPostIdFromUrl = searchParams.get('blogPostId') ?? ''

  const [mode, setMode] = useState<Mode>('list')
  const [items, setItems] = useState<AdminBlogPostTag[]>([])
  const [posts, setPosts] = useState<AdminBlogPostListItem[]>([])
  const [tags, setTags] = useState<AdminTag[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [editingId, setEditingId] = useState<string | null>(null)
  const [filterBlogPostId, setFilterBlogPostId] = useState(blogPostIdFromUrl)

  const [blogPostId, setBlogPostId] = useState(blogPostIdFromUrl)
  const [tagId, setTagId] = useState('')

  const selectedPost = useMemo(
    () => posts.find((p) => p.id === filterBlogPostId) ?? null,
    [posts, filterBlogPostId],
  )

  useEffect(() => {
    setFilterBlogPostId(blogPostIdFromUrl)
    setBlogPostId(blogPostIdFromUrl)
  }, [blogPostIdFromUrl])

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.blogPostTags.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const [linkResult, postResult, tagResult] = await Promise.all([
        filterBlogPostId
          ? adminBlogPostTagService.getAll(accessToken, locale, {
              pageSize: 200,
              blogPostId: filterBlogPostId,
            })
          : Promise.resolve({ items: [], totalCount: 0, pageNumber: 1, pageSize: 200 }),
        adminBlogPostService.getAll(accessToken, locale, { pageSize: 200 }),
        adminTagService.getAll(accessToken, locale, { pageSize: 200 }),
      ])
      setItems(linkResult.items)
      setPosts(postResult.items)
      setTags(tagResult.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPostTags.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [accessToken, filterBlogPostId, locale, t])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  useEffect(() => {
    if (!blogPostIdFromUrl && posts.length > 0 && !filterBlogPostId) {
      const firstId = posts[0].id
      setFilterBlogPostId(firstId)
      setBlogPostId(firstId)
      setSearchParams({ blogPostId: firstId })
    }
  }, [blogPostIdFromUrl, filterBlogPostId, posts, setSearchParams])

  const handlePostFilterChange = (id: string) => {
    setFilterBlogPostId(id)
    setBlogPostId(id)
    if (id) setSearchParams({ blogPostId: id })
    else setSearchParams({})
  }

  const resetForm = () => {
    setBlogPostId((filterBlogPostId || posts[0]?.id) ?? '')
    setTagId(tags[0]?.id ?? '')
    setEditingId(null)
    setError(null)
  }

  const openCreate = () => {
    resetForm()
    setBlogPostId((filterBlogPostId || posts[0]?.id) ?? '')
    setTagId(tags[0]?.id ?? '')
    setMode('create')
  }

  const openEdit = (item: AdminBlogPostTag) => {
    setEditingId(item.id)
    setBlogPostId(item.blogPostId)
    setTagId(item.tagId)
    setError(null)
    setMode('edit')
  }

  const backToList = () => {
    resetForm()
    setMode('list')
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.blogPostTags.saveFailed'))
      return
    }

    if (!blogPostId || !tagId) {
      setError(t('dashboard.blogPostTags.validationRequired'))
      return
    }

    setSaving(true)
    setError(null)
    try {
      if (mode === 'edit' && editingId) {
        await adminBlogPostTagService.update(accessToken, locale, {
          id: editingId,
          blogPostId,
          tagId,
        })
      } else {
        await adminBlogPostTagService.create(accessToken, locale, { blogPostId, tagId })
      }
      await load()
      backToList()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPostTags.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return
    if (!(await confirm({ message: t('dashboard.blogPostTags.deleteConfirm') }))) return

    setSaving(true)
    setError(null)
    try {
      await adminBlogPostTagService.delete(accessToken, id)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPostTags.deleteFailed')))
    } finally {
      setSaving(false)
    }
  }

  if (mode === 'create' || mode === 'edit') {
    return (
      <div>
        <DashboardPageHeader
          title={
            mode === 'edit'
              ? t('dashboard.blogPostTags.editTitle')
              : t('dashboard.blogPostTags.createTitle')
          }
          description={
            mode === 'edit'
              ? t('dashboard.blogPostTags.editDescription')
              : t('dashboard.blogPostTags.createDescription')
          }
          icon={<BlogPostTagsIcon size={22} />}
        />

        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.blogPostTags.fieldPost')}>
            <select
              value={blogPostId}
              onChange={(e) => setBlogPostId(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.blogPostTags.selectPost')}</option>
              {posts.map((post) => (
                <option key={post.id} value={post.id}>
                  {post.title}
                </option>
              ))}
            </select>
          </AdminField>

          <AdminField label={t('dashboard.blogPostTags.fieldTag')}>
            <select
              value={tagId}
              onChange={(e) => setTagId(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.blogPostTags.selectTag')}</option>
              {tags.map((tag) => (
                <option key={tag.id} value={tag.id}>
                  {tag.title}
                </option>
              ))}
            </select>
          </AdminField>

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving}>
              {saving ? (
                <InlineLoading label={t('dashboard.blogPostTags.saving')} />
              ) : (
                t('dashboard.blogPostTags.save')
              )}
            </Button>
            <Button variant="secondary" onClick={backToList} disabled={saving}>
              {t('dashboard.blogPostTags.cancel')}
            </Button>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={t('dashboard.blogPostTags.title')}
        description={t('dashboard.blogPostTags.description')}
        icon={<BlogPostTagsIcon size={22} />}
        action={
          <div className="flex flex-wrap gap-2">
            <Link
              to="/account/dashboard/blog-posts"
              className="inline-flex items-center justify-center rounded-sm border border-border-strong px-4 py-2 text-sm font-medium text-text transition-colors hover:bg-surface-muted"
            >
              {t('dashboard.blogPostTags.backToPosts')}
            </Link>
            <Button
              variant="warm"
              onClick={openCreate}
              disabled={posts.length === 0 || tags.length === 0 || !filterBlogPostId}
            >
              {t('dashboard.blogPostTags.add')}
            </Button>
          </div>
        }
      />

      <div className="mb-4">
        <AdminField label={t('dashboard.blogPostTags.fieldPost')}>
          <select
            value={filterBlogPostId}
            onChange={(e) => handlePostFilterChange(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.blogPostTags.allPosts')}</option>
            {posts.map((post) => (
              <option key={post.id} value={post.id}>
                {post.title}
              </option>
            ))}
          </select>
        </AdminField>
        {selectedPost && (
          <p className="mt-1 text-xs text-text-muted">{selectedPost.title}</p>
        )}
      </div>

      {loading || languageLoading ? (
        <div className="flex justify-center py-16">
          <InlineLoading label={t('dashboard.blogPostTags.loading')} />
        </div>
      ) : posts.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogPostTagsIcon size={28} />}
          title={t('dashboard.blogPostTags.noPostsTitle')}
          message={t('dashboard.blogPostTags.noPostsMessage')}
        />
      ) : tags.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogPostTagsIcon size={28} />}
          title={t('dashboard.blogPostTags.noTagsTitle')}
          message={t('dashboard.blogPostTags.noTagsMessage')}
          action={
            <Link
              to="/account/dashboard/blog-tags"
              className="inline-flex items-center justify-center rounded-sm bg-warm px-4 py-2 text-sm font-medium text-warm-text"
            >
              {t('dashboard.blogPostTags.goToTags')}
            </Link>
          }
        />
      ) : !filterBlogPostId ? (
        <DashboardEmptyState
          icon={<BlogPostTagsIcon size={28} />}
          title={t('dashboard.blogPostTags.noPostTitle')}
          message={t('dashboard.blogPostTags.noPostMessage')}
        />
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogPostTagsIcon size={28} />}
          title={t('dashboard.blogPostTags.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.blogPostTags.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<BlogPostTagsIcon size={28} />}
          title={t('dashboard.blogPostTags.emptyTitle')}
          message={t('dashboard.blogPostTags.emptyMessage')}
          action={
            <Button variant="warm" onClick={openCreate}>
              {t('dashboard.blogPostTags.add')}
            </Button>
          }
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.blogPostTags.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            <AdminListGridHeader />
            {items.map((item, index) => (
              <li
                key={item.id}
                className="flex flex-wrap items-center gap-3 px-4 py-3 sm:px-5"
              >
                <AdminRowNumber value={index + 1} />
                <p className="min-w-0 flex-1 text-sm font-semibold text-text">{item.tagTitle}</p>
                <div className="flex flex-wrap items-center gap-2">
                <AdminGridActions>
                  <AdminGridIconButton
                    label={t('dashboard.blogPostTags.edit')}
                    icon={<EditIcon size={15} />}
                    onClick={() => openEdit(item)}
                    disabled={saving}
                  />
                  <AdminGridIconButton
                    label={t('dashboard.blogPostTags.delete')}
                    icon={<DeleteIcon size={15} />}
                    tone="danger"
                    onClick={() => void handleDelete(item.id)}
                    disabled={saving}
                  />
                </AdminGridActions>
                </div>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  )
}
