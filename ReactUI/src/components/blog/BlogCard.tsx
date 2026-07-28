import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { LocalImage } from '@/components/ui/LocalImage'
import { useSettingsStore } from '@/stores/settingsStore'
import type { BlogPostSummary } from '@/models/blog/blog.model'

interface BlogCardProps {
  post: BlogPostSummary
  categoryLabel?: string
  variant?: 'grid' | 'horizontal'
}

export function BlogCard({ post, categoryLabel, variant = 'grid' }: BlogCardProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)

  if (variant === 'horizontal') {
    return (
      <article className="group flex gap-4 rounded-xl border border-border bg-surface p-3 transition-shadow hover:shadow-md">
        <Link
          to={`/blog/${post.slug}`}
          className="size-28 shrink-0 overflow-hidden rounded-lg sm:size-32"
        >
          <LocalImage
            image={post.coverImage}
            className="size-full object-cover transition-transform duration-300 group-hover:scale-105"
          />
        </Link>

        <div className="flex min-w-0 flex-1 flex-col">
          {categoryLabel && (
            <span className="text-xs font-medium text-warm">{categoryLabel}</span>
          )}
          <Link to={`/blog/${post.slug}`}>
            <h3 className="mt-1 line-clamp-2 text-base font-semibold leading-snug text-text transition-colors group-hover:text-warm">
              {post.title}
            </h3>
          </Link>
          <p className="mt-2 line-clamp-2 text-sm leading-relaxed text-text-muted">
            {post.excerpt}
          </p>
          <div className="mt-auto flex flex-wrap items-center gap-3 pt-3 text-xs text-text-muted">
            <span>{formatBlogDate(post.publishedAt, locale)}</span>
            <span>{t('blog.readTime', { count: post.readTimeMinutes })}</span>
          </div>
        </div>
      </article>
    )
  }

  return (
    <article className="group flex h-full flex-col overflow-hidden rounded-xl border border-border bg-surface transition-shadow hover:shadow-md">
      <Link to={`/blog/${post.slug}`} className="block overflow-hidden">
        <div className="aspect-[4/3] overflow-hidden bg-surface-muted">
          <LocalImage
            image={post.coverImage}
            className="size-full object-cover transition-transform duration-500 group-hover:scale-105"
          />
        </div>
      </Link>

      <div className="flex flex-1 flex-col p-4">
        {categoryLabel && (
          <span className="text-xs font-medium text-warm">{categoryLabel}</span>
        )}
        <Link to={`/blog/${post.slug}`}>
          <h3 className="mt-1 line-clamp-2 text-base font-semibold leading-snug text-text transition-colors group-hover:text-warm">
            {post.title}
          </h3>
        </Link>
        <p className="mt-2 line-clamp-3 flex-1 text-sm leading-relaxed text-text-muted">
          {post.excerpt}
        </p>
        <div className="mt-4 flex items-center justify-between gap-2 text-xs text-text-muted">
          <span>{formatBlogDate(post.publishedAt, locale)}</span>
          <span>{t('blog.readTime', { count: post.readTimeMinutes })}</span>
        </div>
      </div>
    </article>
  )
}
