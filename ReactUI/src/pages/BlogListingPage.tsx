import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { BlogCard } from '@/components/blog/BlogCard'
import { BlogCategoryFilter } from '@/components/blog/BlogCategoryFilter'
import { BlogFeaturedSlider } from '@/components/blog/BlogFeaturedSlider'
import { Container } from '@/components/ui/Container'
import { PageLoading } from '@/components/ui/Spinner'
import { useBlogListing } from '@/hooks/useBlogListing'

export function BlogListingPage() {
  const { t } = useTranslation()
  const { data, loading, error, categorySlug } = useBlogListing()

  if (loading) {
    return <PageLoading />
  }

  if (error || !data) {
    return (
      <Container className="py-20 text-center">
        <h1 className="text-xl font-semibold text-text">{t('common.error')}</h1>
      </Container>
    )
  }

  const categoryMap = Object.fromEntries(
    data.categories.map((category) => [category.slug, category.label]),
  )

  const getCategoryLabel = (slug: string) => categoryMap[slug] ?? slug

  const activeCategoryLabel = categorySlug ? categoryMap[categorySlug] : undefined
  const showFeatured = !categorySlug

  return (
    <div className="min-h-screen bg-gradient-to-b from-surface-muted/40 via-surface to-surface">
      {showFeatured && (
        <BlogFeaturedSlider
          posts={data.featuredPosts}
          getCategoryLabel={getCategoryLabel}
        />
      )}

      <Container className="py-8 md:py-12">
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
          {activeCategoryLabel && (
            <>
              <span aria-hidden>/</span>
              <span className="font-medium text-text">{activeCategoryLabel}</span>
            </>
          )}
        </nav>

        <header className="max-w-3xl">
          <p className="text-xs font-semibold uppercase tracking-widest text-warm">
            {t('blog.title')}
          </p>
          <h1 className="mt-2 text-2xl font-bold tracking-tight text-text md:text-4xl">
            {activeCategoryLabel ?? t('blog.listingTitle')}
          </h1>
          {!activeCategoryLabel && (
            <p className="mt-3 text-sm leading-relaxed text-text-muted md:text-base">
              {t('blog.listingDescription')}
            </p>
          )}
        </header>

        <div className="mt-8 rounded-2xl border border-border/60 bg-surface/80 p-3 shadow-sm backdrop-blur-sm md:p-4">
          <BlogCategoryFilter
            categories={data.categories}
            activeCategory={categorySlug}
          />
        </div>

        <div className="mt-8 flex items-center justify-between gap-4 border-b border-border/70 pb-4">
          <p className="inline-flex items-center rounded-full bg-surface-muted px-3 py-1 text-sm text-text-muted">
            {t('blog.articleCount', { count: data.totalCount })}
          </p>
        </div>

        {data.posts.length === 0 ? (
          <div className="mt-8 rounded-2xl border border-dashed border-border bg-surface px-6 py-16 text-center shadow-sm">
            <div className="mx-auto flex size-14 items-center justify-center rounded-full bg-surface-muted text-warm">
              <EmptyArticlesIcon />
            </div>
            <p className="mt-5 text-lg font-semibold text-text">{t('blog.emptyTitle')}</p>
            <p className="mx-auto mt-2 max-w-md text-sm leading-relaxed text-text-muted">
              {t('blog.emptyMessage')}
            </p>
            <Link
              to="/blog"
              className="mt-6 inline-flex items-center justify-center rounded-full bg-warm px-5 py-2.5 text-sm font-semibold text-warm-text transition-opacity hover:opacity-90"
            >
              {t('blog.viewAll')}
            </Link>
          </div>
        ) : (
          <div className="mt-8 grid gap-5 sm:grid-cols-2 lg:grid-cols-3 lg:gap-6 xl:gap-7">
            {data.posts.map((post) => (
              <BlogCard
                key={post.id}
                post={post}
                categoryLabel={getCategoryLabel(post.categorySlug)}
              />
            ))}
          </div>
        )}
      </Container>
    </div>
  )
}

function EmptyArticlesIcon() {
  return (
    <svg viewBox="0 0 24 24" aria-hidden className="size-7">
      <path
        d="M6 4h9l3 3v13H6V4z"
        stroke="currentColor"
        strokeWidth="1.5"
        fill="none"
        strokeLinejoin="round"
      />
      <path d="M15 4v3h3M8 10h8M8 14h8M8 18h5" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </svg>
  )
}
