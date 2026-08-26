import { useCallback, useEffect, useState } from 'react'
import { Link, useNavigate, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { AdminBlogPostCategory } from '@/models/admin/blog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
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
import { slugifyTitle } from '@/services/admin/adminCatalogNormalize'
import { useUserStore } from '@/stores/userStore'

export function BlogPostFormPanel() {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const [searchParams] = useSearchParams()
  const postId = searchParams.get('id') ?? undefined
  const isEdit = Boolean(postId)

  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const [categories, setCategories] = useState<AdminBlogPostCategory[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [publishing, setPublishing] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const [code, setCode] = useState('')
  const [title, setTitle] = useState('')
  const [slug, setSlug] = useState('')
  const [categoryId, setCategoryId] = useState('')
  const [metaDescription, setMetaDescription] = useState('')
  const [seoKeywords, setSeoKeywords] = useState('')
  const [content, setContent] = useState('')
  const [readingTime, setReadingTime] = useState('5')
  const [isFeatured, setIsFeatured] = useState(false)
  const [isPublished, setIsPublished] = useState(false)
  const [slugTouched, setSlugTouched] = useState(false)

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.blogPosts.authRequired'))
      setLoading(false)
      return
    }

    setLoading(true)
    setError(null)
    try {
      const categoryResult = await adminBlogPostCategoryService.getAll(accessToken, locale, {
        pageSize: 100,
        languageId: languageId ?? undefined,
      })
      setCategories(categoryResult.items)

      if (isEdit && postId) {
        const post = await adminBlogPostService.get(accessToken, locale, postId)
        if (!post) {
          setError(t('dashboard.blogPosts.notFound'))
          return
        }
        setCode(post.code)
        setTitle(post.title)
        setSlug(post.slug)
        setCategoryId(post.categoryId)
        setMetaDescription(post.metaDescription)
        setSeoKeywords(post.seoKeywords)
        setContent(post.content)
        setReadingTime(String(post.readingTimeInMinutes || 5))
        setIsFeatured(post.isFeatured)
        setIsPublished(post.isPublished)
        setSlugTouched(true)
      } else if (categoryResult.items[0]) {
        setCategoryId(categoryResult.items[0].id)
      }
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPosts.loadFailed')))
    } finally {
      setLoading(false)
    }
  }, [accessToken, isEdit, languageId, locale, postId, t])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  const handleTitleChange = (value: string) => {
    setTitle(value)
    if (!slugTouched) setSlug(slugifyTitle(value))
    if (!code && !isEdit) setCode(slugifyTitle(value))
  }

  const handleSave = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || languageId == null) {
      setError(t('dashboard.blogPosts.saveFailed'))
      return
    }

    const parsedReadingTime = Number(readingTime)
    if (
      !code.trim() ||
      !title.trim() ||
      !slug.trim() ||
      !categoryId ||
      !content.trim() ||
      !Number.isFinite(parsedReadingTime)
    ) {
      setError(t('dashboard.blogPosts.validationRequired'))
      return
    }

    setSaving(true)
    setError(null)
    try {
      const payload = {
        languageId,
        code: code.trim(),
        categoryId,
        title: title.trim(),
        slug: slug.trim(),
        metaDescription: metaDescription.trim(),
        seoKeywords: seoKeywords.trim(),
        content: content.trim(),
        readingTimeInMinutes: parsedReadingTime,
        isFeatured,
      }

      if (isEdit && postId) {
        await adminBlogPostService.update(accessToken, locale, { id: postId, ...payload })
        await load()
      } else {
        const newId = await adminBlogPostService.create(accessToken, locale, payload)
        navigate(`/account/dashboard/blog-posts/edit?id=${encodeURIComponent(newId)}`, {
          replace: true,
        })
      }
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPosts.saveFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handlePublish = async () => {
    if (!accessToken || accessToken === 'mock-access-token' || !postId) return

    setPublishing(true)
    setError(null)
    try {
      await adminBlogPostService.publish(accessToken, locale, postId)
      setIsPublished(true)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.blogPosts.publishFailed')))
    } finally {
      setPublishing(false)
    }
  }

  if (loading || languageLoading) {
    return (
      <div className="flex justify-center py-16">
        <InlineLoading label={t('dashboard.blogPosts.loading')} />
      </div>
    )
  }

  return (
    <div>
      <DashboardPageHeader
        title={
          isEdit ? t('dashboard.blogPosts.editTitle') : t('dashboard.blogPosts.createTitle')
        }
        description={
          isEdit
            ? t('dashboard.blogPosts.editDescription')
            : t('dashboard.blogPosts.createDescription')
        }
        icon={<BlogPostsIcon size={22} />}
      />

      {categories.length === 0 ? (
        <div className="rounded-sm border border-border bg-surface-muted/20 p-6 text-center">
          <p className="text-sm text-text-muted">{t('dashboard.blogPosts.noCategoriesMessage')}</p>
          <Link
            to="/account/dashboard/blog-categories"
            className="mt-4 inline-flex items-center justify-center rounded-sm bg-warm px-4 py-2 text-sm font-medium text-warm-text"
          >
            {t('dashboard.blogPosts.goToCategories')}
          </Link>
        </div>
      ) : (
        <div className="space-y-5 rounded-sm border border-border bg-surface-muted/20 p-5 shadow-sm sm:p-6">
          <AdminField label={t('dashboard.blogPosts.fieldCategory')}>
            <select
              value={categoryId}
              onChange={(e) => setCategoryId(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.blogPosts.selectCategory')}</option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.title}
                </option>
              ))}
            </select>
          </AdminField>

          <AdminField label={t('dashboard.blogPosts.fieldCode')}>
            <input
              value={code}
              onChange={(e) => setCode(e.target.value)}
              className={adminInputClass}
              dir="ltr"
            />
          </AdminField>

          <AdminField label={t('dashboard.blogPosts.fieldTitle')}>
            <input
              value={title}
              onChange={(e) => handleTitleChange(e.target.value)}
              className={adminInputClass}
              placeholder={t('dashboard.blogPosts.titlePlaceholder')}
            />
          </AdminField>

          <AdminField label={t('dashboard.blogPosts.fieldSlug')}>
            <input
              value={slug}
              onChange={(e) => {
                setSlugTouched(true)
                setSlug(e.target.value)
              }}
              className={adminInputClass}
              dir="ltr"
            />
          </AdminField>

          <AdminField label={t('dashboard.blogPosts.fieldMetaDescription')}>
            <textarea
              value={metaDescription}
              onChange={(e) => setMetaDescription(e.target.value)}
              className={`${adminInputClass} min-h-[4rem] resize-y`}
            />
          </AdminField>

          <AdminField label={t('dashboard.blogPosts.fieldSeoKeywords')}>
            <input
              value={seoKeywords}
              onChange={(e) => setSeoKeywords(e.target.value)}
              className={adminInputClass}
              dir="ltr"
            />
          </AdminField>

          <AdminField label={t('dashboard.blogPosts.fieldContent')}>
            <textarea
              value={content}
              onChange={(e) => setContent(e.target.value)}
              className={`${adminInputClass} min-h-[12rem] resize-y`}
              placeholder={t('dashboard.blogPosts.contentPlaceholder')}
            />
          </AdminField>

          <AdminField label={t('dashboard.blogPosts.fieldReadingTime')}>
            <input
              type="number"
              min="1"
              value={readingTime}
              onChange={(e) => setReadingTime(e.target.value)}
              className={adminInputClass}
              dir="ltr"
            />
          </AdminField>

          <label className="flex items-center gap-2 text-sm text-text">
            <input
              type="checkbox"
              checked={isFeatured}
              onChange={(e) => setIsFeatured(e.target.checked)}
              className="size-4 rounded border-border text-warm focus:ring-warm"
            />
            {t('dashboard.blogPosts.fieldFeatured')}
          </label>

          {isEdit && isPublished && (
            <p className="text-xs text-warm">{t('dashboard.blogPosts.publishedHint')}</p>
          )}

          {error && <p className="text-sm text-sale">{error}</p>}

          <div className="flex flex-wrap gap-3">
            <Button variant="warm" onClick={() => void handleSave()} disabled={saving || publishing}>
              {saving ? (
                <InlineLoading label={t('dashboard.blogPosts.saving')} />
              ) : (
                t('dashboard.blogPosts.save')
              )}
            </Button>
            {isEdit && !isPublished && (
              <Button
                variant="secondary"
                onClick={() => void handlePublish()}
                disabled={saving || publishing}
              >
                {publishing ? (
                  <InlineLoading label={t('dashboard.blogPosts.publishing')} />
                ) : (
                  t('dashboard.blogPosts.publish')
                )}
              </Button>
            )}
            <Link
              to="/account/dashboard/blog-posts"
              className="inline-flex items-center justify-center rounded-sm border border-border-strong px-4 py-2 text-sm font-medium text-text transition-colors hover:bg-surface-muted"
            >
              {t('dashboard.blogPosts.cancel')}
            </Link>
          </div>
        </div>
      )}
    </div>
  )
}
