import { useEffect, useState } from 'react'
import { CatalogNavLink } from '@/components/routing/CatalogNavLink'
import { useTranslation } from 'react-i18next'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import { getBlogPostCoverSrc } from '@/extensions/blogPostCover'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { Container } from '@/components/ui/Container'
import { useHorizontalDragScroll } from '@/hooks/useHorizontalDragScroll'
import { useStorefrontLocale } from '@/hooks/useStorefrontLocale'
import type { BlogPostSummary } from '@/models/blog/blog.model'
import { blogService } from '@/services/blogService'

const JOURNAL_LIMIT = 3

function HomeJournalCard({ post }: { post: BlogPostSummary }) {
  const { t } = useTranslation()
  const locale = useStorefrontLocale()
  const coverSrc = getBlogPostCoverSrc(post)
  const articlePath = `/blog/${post.slug}`
  const categoryLabel = post.categoryLabel ?? post.categorySlug

  return (
    <article className="group flex h-full flex-col overflow-hidden rounded-2xl border border-border/70 bg-surface shadow-sm ring-1 ring-black/[0.02] transition-all duration-300 hover:-translate-y-1 hover:border-warm/25 hover:shadow-lg">
      <CatalogNavLink href={articlePath} className="relative block overflow-hidden">
        <div className="aspect-[16/10] overflow-hidden bg-surface-muted">
          <img
            src={coverSrc}
            alt={post.coverImage?.alt || post.title}
            loading="eager"
            decoding="async"
            className="size-full object-cover transition-transform duration-500 ease-out group-hover:scale-[1.05]"
          />
        </div>
        <div
          aria-hidden
          className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/35 via-black/5 to-transparent opacity-60 transition-opacity duration-300 group-hover:opacity-80"
        />
        {categoryLabel ? (
          <span className="absolute start-3 top-3 rounded-full bg-surface/95 px-2.5 py-1 text-[11px] font-semibold text-warm shadow-sm backdrop-blur-sm">
            {categoryLabel}
          </span>
        ) : null}
      </CatalogNavLink>

      <div className="flex flex-1 flex-col p-4 md:p-5">
        <CatalogNavLink href={articlePath}>
          <h3 className="line-clamp-2 text-base font-semibold leading-snug text-text transition-colors group-hover:text-warm md:text-lg">
            {post.title}
          </h3>
        </CatalogNavLink>
        <p className="mt-2 line-clamp-3 flex-1 text-sm leading-relaxed text-text-muted">
          {post.excerpt}
        </p>
        <div className="mt-4 flex flex-wrap items-center gap-x-3 gap-y-1 border-t border-border/60 pt-4 text-xs text-text-muted">
          <span>{formatBlogDate(post.publishedAt, locale)}</span>
          <span>{t('blog.readTime', { count: post.readTimeMinutes })}</span>
        </div>
      </div>
    </article>
  )
}

export function BlogHomeSection() {
  const { t } = useTranslation()
  const locale = useStorefrontLocale()
  const drag = useHorizontalDragScroll<HTMLDivElement>()
  const [posts, setPosts] = useState<BlogPostSummary[]>([])

  useEffect(() => {
    let cancelled = false

    void blogService.getListing(locale).then((listing) => {
      if (cancelled) return
      setPosts(listing.posts.slice(0, JOURNAL_LIMIT))
    })

    return () => {
      cancelled = true
    }
  }, [locale])

  if (posts.length === 0) return null

  return (
    <section className="border-t border-border bg-surface-muted/40 py-12 md:py-16">
      <Container>
        <div className="flex flex-wrap items-end justify-between gap-4">
          <div className="max-w-2xl">
            <h2 className="text-2xl font-bold text-text md:text-3xl">{t('home.journalTitle')}</h2>
            <p className="mt-2 text-sm leading-relaxed text-text-muted md:text-base">
              {t('home.journalSubtitle')}
            </p>
          </div>

          <CatalogNavLink
            href="/blog"
            className="inline-flex shrink-0 items-center gap-1 text-sm font-semibold text-warm transition-colors hover:underline"
          >
            {t('home.viewJournal')}
            <ChevronIcon expanded={false} className="text-warm rtl:rotate-90 ltr:-rotate-90" />
          </CatalogNavLink>
        </div>

        <div className="mt-8 hidden gap-6 md:grid md:grid-cols-2 lg:grid-cols-3">
          {posts.map((post) => (
            <HomeJournalCard key={post.id} post={post} />
          ))}
        </div>

        <div
          ref={drag.ref}
          className={`-mx-4 mt-8 flex gap-4 overflow-x-auto px-4 pb-1 md:hidden touch-pan-y select-none ${
            drag.isGrabbing ? 'cursor-grabbing snap-none' : 'cursor-grab snap-x snap-mandatory'
          }`}
          onPointerDown={drag.onPointerDown}
          onPointerMove={drag.onPointerMove}
          onPointerUp={drag.onPointerUp}
          onLostPointerCapture={drag.onLostPointerCapture}
          onClickCapture={drag.onClickCapture}
          onDragStart={(e) => e.preventDefault()}
        >
          {posts.map((post) => (
            <div
              key={post.id}
              className="w-[min(100%,18rem)] shrink-0 snap-start sm:w-[20rem]"
            >
              <HomeJournalCard post={post} />
            </div>
          ))}
        </div>
      </Container>
    </section>
  )
}
