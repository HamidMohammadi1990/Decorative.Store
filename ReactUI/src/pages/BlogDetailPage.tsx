import { Link, useParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { CopyableShortLink } from '@/components/ui/CopyableShortLink'
import { BlogAuthorCard } from '@/components/blog/BlogAuthorCard'
import { BlogComments } from '@/components/blog/BlogComments'
import { BlogDetailGallery } from '@/components/blog/BlogDetailGallery'
import { BlogLikeButton } from '@/components/blog/BlogLikeButton'
import { BlogRelatedSlider } from '@/components/blog/BlogRelatedSlider'
import { BlogShareBar } from '@/components/blog/BlogShareBar'
import { BlogTagList } from '@/components/blog/BlogTagList'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { useBlogPost } from '@/hooks/useBlogPost'
import { NotFoundPage } from '@/pages/NotFoundPage'
import { useSettingsStore } from '@/stores/settingsStore'

export function BlogDetailPage() {
  const { t } = useTranslation()
  const { slug: routeSlug } = useParams<{ slug: string }>()
  const locale = useSettingsStore((s) => s.locale)
  const { post, related, categoryMap, loading, error, reload } = useBlogPost()

  if (loading) {
    return <PageLoading />
  }

  if (error === 'not-found' || !post) {
    return <NotFoundPage />
  }

  if (error) {
    return (
      <Container className="py-20 text-center">
        <h1 className="text-xl font-semibold text-text">{t('common.error')}</h1>
      </Container>
    )
  }

  const postSlug = post.slug || routeSlug || ''
  const sharePath = `/blog/${postSlug}`
  const shareUrl = typeof window !== 'undefined' ? window.location.href : sharePath
  const categoryLabel = post.categoryLabel || post.categorySlug

  const getCategoryLabel = (slug: string) => categoryMap[slug] ?? slug

  return (
    <article className="bg-surface pb-16">
      <Container className="py-6 md:py-8">
        <nav
          aria-label="Breadcrumb"
          className="mb-6 flex flex-wrap items-center gap-1.5 text-xs text-text-muted"
        >
          <Link to="/" className="transition-colors hover:text-warm">
            {t('product.breadcrumbHome')}
          </Link>
          <span aria-hidden>/</span>
          <Link to="/blog" className="transition-colors hover:text-warm">
            {t('blog.title')}
          </Link>
          <span aria-hidden>/</span>
          <span className="line-clamp-1 text-text">{post.title}</span>
        </nav>

        <div className="grid gap-10 lg:grid-cols-[minmax(0,1fr)_18rem] lg:gap-12">
          <div className="min-w-0">
            <Link
              to={`/blog/category/${post.categorySlug}`}
              className="inline-flex rounded-full bg-warm-soft px-3 py-1 text-xs font-semibold text-warm"
            >
              {categoryLabel}
            </Link>

            <h1 className="mt-4 text-2xl font-bold leading-tight text-text md:text-4xl">
              {post.title}
            </h1>

            <div className="mt-4 flex flex-wrap items-center gap-x-4 gap-y-2 text-sm text-text-muted">
              <span>{post.author.name}</span>
              <span aria-hidden>•</span>
              <span>{formatBlogDate(post.publishedAt, locale)}</span>
              <span aria-hidden>•</span>
              <span>{t('blog.readTime', { count: post.readTimeMinutes })}</span>
            </div>

            <CopyableShortLink path={sharePath} className="mt-4" />

            <div className="mt-6 flex flex-wrap items-center gap-3">
              <BlogLikeButton postId={post.id} baseLikes={post.likes} />
              <span className="text-sm text-text-muted">
                {t('blog.commentCount', { count: post.commentCount })}
              </span>
            </div>

            <div className="mt-8">
              <BlogDetailGallery images={post.gallery} title={post.title} />
            </div>

            <div className="prose-blog mt-8 space-y-5">
              {post.content.map((paragraph) => (
                <p key={paragraph} className="text-base leading-relaxed text-text-muted md:text-lg">
                  {paragraph}
                </p>
              ))}
            </div>

            <div className="mt-8">
              <BlogTagList tags={post.tags} label={t('blog.tags')} />
            </div>

            <div className="mt-8">
              <BlogShareBar title={post.title} url={shareUrl} />
            </div>

            <div className="mt-10 lg:hidden">
              <BlogAuthorCard author={post.author} label={t('blog.aboutAuthor')} />
            </div>

            <div className="mt-10">
              <BlogComments
                postId={post.id}
                comments={post.comments}
                onCommentCreated={reload}
              />
            </div>

            <BlogRelatedSlider posts={related} getCategoryLabel={getCategoryLabel} />
          </div>

          <aside className="hidden lg:block">
            <div className="sticky top-24 space-y-6">
              <BlogAuthorCard author={post.author} label={t('blog.aboutAuthor')} />
              <BlogShareBar title={post.title} url={shareUrl} />
            </div>
          </aside>
        </div>
      </Container>
    </article>
  )
}
