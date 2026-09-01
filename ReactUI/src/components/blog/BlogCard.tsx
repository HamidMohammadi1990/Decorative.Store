import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { LocalImage } from '@/components/ui/LocalImage'
import { useSettingsStore } from '@/stores/settingsStore'
import type { BlogPostSummary } from '@/models/blog/blog.model'

interface BlogCardProps {
  post: BlogPostSummary
  categoryLabel?: string
  variant?: 'grid' | 'horizontal'
}

function BlogCardMeta({ post, locale }: { post: BlogPostSummary; locale: string }) {
  const { t } = useTranslation()

  return (
    <div className="flex flex-wrap items-center gap-x-3 gap-y-1 text-xs text-text-muted">
      <span className="inline-flex items-center gap-1">
        <CalendarIcon />
        {formatBlogDate(post.publishedAt, locale)}
      </span>
      <span className="inline-flex items-center gap-1">
        <ClockIcon />
        {t('blog.readTime', { count: post.readTimeMinutes })}
      </span>
      {post.commentCount > 0 && (
        <span className="inline-flex items-center gap-1">
          <CommentIcon />
          {post.commentCount}
        </span>
      )}
      {post.likes > 0 && (
        <span className="inline-flex items-center gap-1">
          <HeartIcon />
          {post.likes}
        </span>
      )}
    </div>
  )
}

export function BlogCard({ post, categoryLabel, variant = 'grid' }: BlogCardProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const articlePath = `/blog/${post.slug}`

  if (variant === 'horizontal') {
    return (
      <article className="group flex gap-4 overflow-hidden rounded-2xl border border-border/70 bg-surface p-3 shadow-sm ring-1 ring-black/[0.02] transition-all duration-300 hover:-translate-y-0.5 hover:border-warm/25 hover:shadow-md sm:gap-5 sm:p-4">
        <Link
          to={articlePath}
          className="relative size-28 shrink-0 overflow-hidden rounded-xl sm:size-36"
        >
          <LocalImage
            image={post.coverImage}
            className="size-full object-cover transition-transform duration-500 ease-out group-hover:scale-[1.05]"
          />
          <div
            aria-hidden
            className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/25 via-transparent to-transparent opacity-0 transition-opacity duration-300 group-hover:opacity-100"
          />
          {categoryLabel && (
            <span className="absolute start-2 top-2 rounded-md bg-surface/90 px-2 py-0.5 text-[10px] font-semibold text-warm shadow-sm backdrop-blur-sm">
              {categoryLabel}
            </span>
          )}
        </Link>

        <div className="flex min-w-0 flex-1 flex-col">
          <Link to={articlePath}>
            <h3 className="line-clamp-2 text-base font-semibold leading-snug text-text transition-colors group-hover:text-warm sm:text-lg">
              {post.title}
            </h3>
          </Link>
          <p className="mt-2 line-clamp-2 text-sm leading-relaxed text-text-muted">
            {post.excerpt}
          </p>
          <div className="mt-auto space-y-3 pt-4">
            <BlogCardMeta post={post} locale={locale} />
            <span className="inline-flex items-center gap-1 text-sm font-semibold text-warm transition-all group-hover:gap-2">
              {t('blog.readArticle')}
              <ChevronIcon expanded={false} className="text-warm rtl:rotate-90 ltr:-rotate-90" />
            </span>
          </div>
        </div>
      </article>
    )
  }

  return (
    <article className="group flex h-full flex-col overflow-hidden rounded-2xl border border-border/70 bg-surface shadow-sm ring-1 ring-black/[0.02] transition-all duration-300 hover:-translate-y-1 hover:border-warm/25 hover:shadow-lg">
      <Link to={articlePath} className="relative block overflow-hidden">
        <div className="aspect-[16/10] overflow-hidden bg-surface-muted">
          <LocalImage
            image={post.coverImage}
            className="size-full object-cover transition-transform duration-500 ease-out group-hover:scale-[1.05]"
          />
        </div>
        <div
          aria-hidden
          className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/35 via-black/5 to-transparent opacity-60 transition-opacity duration-300 group-hover:opacity-80"
        />
        {categoryLabel && (
          <span className="absolute start-3 top-3 rounded-full bg-surface/95 px-2.5 py-1 text-[11px] font-semibold text-warm shadow-sm backdrop-blur-sm">
            {categoryLabel}
          </span>
        )}
      </Link>

      <div className="flex flex-1 flex-col p-4 md:p-5">
        <Link to={articlePath}>
          <h3 className="line-clamp-2 text-base font-semibold leading-snug text-text transition-colors group-hover:text-warm md:text-lg">
            {post.title}
          </h3>
        </Link>
        <p className="mt-2 line-clamp-3 flex-1 text-sm leading-relaxed text-text-muted">
          {post.excerpt}
        </p>

        <div className="mt-4 space-y-3 border-t border-border/60 pt-4">
          <BlogCardMeta post={post} locale={locale} />
          <span className="inline-flex items-center gap-1 text-sm font-semibold text-warm transition-all group-hover:gap-2">
            {t('blog.readArticle')}
            <ChevronIcon expanded={false} className="text-warm rtl:rotate-90 ltr:-rotate-90" />
          </span>
        </div>
      </div>
    </article>
  )
}

function CalendarIcon() {
  return (
    <svg viewBox="0 0 16 16" aria-hidden className="size-3.5 shrink-0 opacity-70">
      <rect x="2" y="3" width="12" height="11" rx="1.5" stroke="currentColor" strokeWidth="1.2" fill="none" />
      <path d="M2 6.5h12M5.5 1.5v2M10.5 1.5v2" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
    </svg>
  )
}

function ClockIcon() {
  return (
    <svg viewBox="0 0 16 16" aria-hidden className="size-3.5 shrink-0 opacity-70">
      <circle cx="8" cy="8" r="5.5" stroke="currentColor" strokeWidth="1.2" fill="none" />
      <path d="M8 5v3.5l2 1.5" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" strokeLinejoin="round" />
    </svg>
  )
}

function CommentIcon() {
  return (
    <svg viewBox="0 0 16 16" aria-hidden className="size-3.5 shrink-0 opacity-70">
      <path
        d="M3 4.5a1.5 1.5 0 011.5-1.5h7A1.5 1.5 0 0113 4.5v4a1.5 1.5 0 01-1.5 1.5H7l-2.5 2v-2H4.5A1.5 1.5 0 013 8.5v-4z"
        stroke="currentColor"
        strokeWidth="1.2"
        fill="none"
        strokeLinejoin="round"
      />
    </svg>
  )
}

function HeartIcon() {
  return (
    <svg viewBox="0 0 16 16" aria-hidden className="size-3.5 shrink-0 opacity-70">
      <path
        d="M8 13.5s-4.5-2.8-4.5-6.2C3.5 5.4 5.2 4 7 4c1 0 1.8.5 2.3 1.2C9.8 4.5 10.6 4 11.5 4 13.3 4 15 5.4 15 7.3 15 10.7 8 13.5 8 13.5z"
        stroke="currentColor"
        strokeWidth="1.2"
        fill="none"
        strokeLinejoin="round"
      />
    </svg>
  )
}
