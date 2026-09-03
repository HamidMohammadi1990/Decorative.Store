import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { BlogCard } from '@/components/blog/BlogCard'
import { ChevronIcon } from '@/components/ui/ChevronIcon'
import { Container } from '@/components/ui/Container'
import { useHorizontalDragScroll } from '@/hooks/useHorizontalDragScroll'
import { useHomeFeaturedPosts } from '@/hooks/useHomeFeaturedPosts'

export function BlogHomeSection() {
  const { t } = useTranslation()
  const drag = useHorizontalDragScroll<HTMLDivElement>()
  const posts = useHomeFeaturedPosts()

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

          <Link
            to="/blog"
            className="inline-flex shrink-0 items-center gap-1 text-sm font-semibold text-warm transition-colors hover:underline"
          >
            {t('home.viewJournal')}
            <ChevronIcon expanded={false} className="text-warm rtl:rotate-90 ltr:-rotate-90" />
          </Link>
        </div>

        <div className="mt-8 hidden gap-6 md:grid md:grid-cols-2 lg:grid-cols-3">
          {posts.map((post) => (
            <BlogCard
              key={post.id}
              post={post}
              categoryLabel={post.categoryLabel ?? post.categorySlug}
            />
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
              <BlogCard
                post={post}
                categoryLabel={post.categoryLabel ?? post.categorySlug}
              />
            </div>
          ))}
        </div>
      </Container>
    </section>
  )
}
