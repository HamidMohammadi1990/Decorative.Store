import { Link, useLocation, useMatch } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { BlogCategory } from '@/models/blog/blog.model'
import { JournalIcon } from '@/components/ui/NavCategoryIcons'
import { ScrollArrowButton } from '@/components/ui/ScrollArrowButton'
import { InlineLoading } from '@/components/ui/Spinner'
import { useHorizontalScrollArrows } from '@/hooks/useHorizontalScrollArrows'

interface BlogMainNavProps {
  categories: BlogCategory[]
  loading?: boolean
}

export function BlogMainNav({ categories, loading = false }: BlogMainNavProps) {
  const { t } = useTranslation()
  const location = useLocation()
  const categoryMatch = useMatch('/blog/category/:categorySlug')
  const activeCategory = categoryMatch?.params.categorySlug
  const isAllActive = location.pathname === '/blog'

  const {
    scrollRef,
    canScrollPrev,
    canScrollNext,
    hasOverflow,
    scrollPrev,
    scrollNext,
  } = useHorizontalScrollArrows()

  if (loading) {
    return (
      <nav
        className="hidden min-h-10 items-center justify-center lg:flex"
        aria-label={t('blog.navLabel')}
      >
        <InlineLoading />
      </nav>
    )
  }

  return (
    <nav
      className="relative hidden min-w-0 items-center gap-2 lg:flex"
      aria-label={t('blog.navLabel')}
    >
      {hasOverflow && (
        <ScrollArrowButton
          direction="prev"
          disabled={!canScrollPrev}
          label={t('common.scrollNavPrev')}
          onClick={scrollPrev}
        />
      )}

      <div
        ref={scrollRef}
        className="main-nav-scroll min-w-0 flex-1 overflow-x-auto"
      >
        <ul className="flex w-max flex-nowrap items-center gap-x-4 xl:gap-x-5">
          <li className="shrink-0">
            <Link
              to="/blog"
              className={`inline-flex items-center gap-1.5 py-2 text-sm font-medium tracking-wide whitespace-nowrap transition-colors ${
                isAllActive
                  ? 'font-semibold text-warm'
                  : 'text-text hover:text-warm'
              }`}
            >
              <JournalIcon className={isAllActive ? 'text-warm' : 'text-text-muted'} />
              {t('blog.allCategories')}
            </Link>
          </li>

          {categories.map((category) => {
            const isActive = activeCategory === category.slug
            const label = t(`blog.categories.${category.slug}`, {
              defaultValue: category.label,
            })

            return (
              <li key={category.id} className="shrink-0">
                <Link
                  to={`/blog/category/${category.slug}`}
                  className={`inline-block py-2 text-sm font-medium tracking-wide whitespace-nowrap transition-colors ${
                    isActive
                      ? 'font-semibold text-warm'
                      : 'text-text hover:text-warm'
                  }`}
                >
                  {label}
                </Link>
              </li>
            )
          })}

          <li className="h-5 w-px shrink-0 bg-border" aria-hidden />

          <li className="shrink-0">
            <Link
              to="/"
              className="inline-block py-2 text-sm font-medium tracking-wide whitespace-nowrap text-text-muted transition-colors hover:text-text"
            >
              {t('blog.backToShop')}
            </Link>
          </li>
        </ul>
      </div>

      {hasOverflow && (
        <ScrollArrowButton
          direction="next"
          disabled={!canScrollNext}
          label={t('common.scrollNavNext')}
          onClick={scrollNext}
        />
      )}
    </nav>
  )
}

interface BlogMobileNavProps {
  categories: BlogCategory[]
  loading?: boolean
  onClose: () => void
}

export function BlogMobileNav({
  categories,
  loading = false,
  onClose,
}: BlogMobileNavProps) {
  const { t } = useTranslation()
  const location = useLocation()
  const categoryMatch = useMatch('/blog/category/:categorySlug')
  const activeCategory = categoryMatch?.params.categorySlug
  const isAllActive = location.pathname === '/blog'

  if (loading) {
    return <InlineLoading className="px-4 py-8" />
  }

  return (
    <ul className="px-2">
      <li className="border-b border-border">
        <Link
          to="/blog"
          className={`flex items-center gap-2 px-2 py-2.5 text-base font-semibold transition-colors ${
            isAllActive ? 'text-warm' : 'text-text hover:text-warm'
          }`}
          onClick={onClose}
        >
          <JournalIcon className="text-warm" />
          {t('blog.allCategories')}
        </Link>
      </li>

      {categories.map((category) => {
        const isActive = activeCategory === category.slug
        const label = t(`blog.categories.${category.slug}`, {
          defaultValue: category.label,
        })

        return (
          <li key={category.id} className="border-b border-border">
            <Link
              to={`/blog/category/${category.slug}`}
              className={`block px-2 py-2.5 text-base font-semibold transition-colors ${
                isActive ? 'text-warm' : 'text-text hover:text-warm'
              }`}
              onClick={onClose}
            >
              {label}
            </Link>
          </li>
        )
      })}

      <li className="border-b border-border">
        <Link
          to="/"
          className="block px-2 py-2.5 text-base font-medium text-text-muted transition-colors hover:text-text"
          onClick={onClose}
        >
          {t('blog.backToShop')}
        </Link>
      </li>
    </ul>
  )
}
