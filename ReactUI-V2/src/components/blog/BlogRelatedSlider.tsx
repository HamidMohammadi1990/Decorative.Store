import { useTranslation } from 'react-i18next'
import { BlogCard } from '@/components/blog/BlogCard'
import { useHorizontalDragScroll } from '@/hooks/useHorizontalDragScroll'
import type { BlogPostSummary } from '@/models/blog/blog.model'

interface BlogRelatedSliderProps {
  posts: BlogPostSummary[]
  getCategoryLabel: (slug: string) => string
}

export function BlogRelatedSlider({ posts, getCategoryLabel }: BlogRelatedSliderProps) {
  const { t } = useTranslation()
  const drag = useHorizontalDragScroll<HTMLDivElement>()

  if (posts.length === 0) return null

  return (
    <section className="mt-12 border-t border-border pt-10">
      <h2 className="text-xl font-bold text-text">{t('blog.relatedTitle')}</h2>

      <div
        ref={drag.ref}
        className={`-mx-4 mt-6 flex gap-4 overflow-x-auto px-4 pb-2 touch-pan-y select-none ${
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
            <BlogCard post={post} categoryLabel={getCategoryLabel(post.categorySlug)} />
          </div>
        ))}
      </div>
    </section>
  )
}
