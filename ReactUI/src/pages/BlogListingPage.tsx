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
    <div className="bg-surface">
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
              <span className="text-text">{activeCategoryLabel}</span>
            </>
          )}
        </nav>

        <div className="max-w-3xl">
          <h1 className="text-2xl font-bold text-text md:text-3xl">
            {activeCategoryLabel ?? data.title}
          </h1>
          <p className="mt-3 text-sm leading-relaxed text-text-muted md:text-base">
            {data.description}
          </p>
        </div>

        <div className="mt-8">
          <BlogCategoryFilter
            categories={data.categories}
            activeCategory={categorySlug}
          />
        </div>

        <div className="mt-8 flex items-center justify-between gap-4 border-b border-border pb-4">
          <p className="text-sm text-text-muted">
            {t('blog.articleCount', { count: data.totalCount })}
          </p>
        </div>

        {data.posts.length === 0 ? (
          <div className="py-16 text-center">
            <p className="text-base font-medium text-text">{t('blog.emptyTitle')}</p>
            <p className="mt-2 text-sm text-text-muted">{t('blog.emptyMessage')}</p>
            <Link
              to="/blog"
              className="mt-6 inline-block text-sm font-semibold text-warm hover:underline"
            >
              {t('blog.viewAll')}
            </Link>
          </div>
        ) : (
          <div className="mt-8 grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
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
