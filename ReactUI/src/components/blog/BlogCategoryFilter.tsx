import { Link, useLocation } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useHorizontalDragScroll } from '@/hooks/useHorizontalDragScroll'
import type { BlogCategory } from '@/models/blog/blog.model'

interface BlogCategoryFilterProps {
  categories: BlogCategory[]
  activeCategory?: string
}

export function BlogCategoryFilter({ categories, activeCategory }: BlogCategoryFilterProps) {
  const { t } = useTranslation()
  const location = useLocation()
  const drag = useHorizontalDragScroll<HTMLDivElement>()

  const isAllActive = !activeCategory && location.pathname === '/blog'

  return (
    <div
      ref={drag.ref}
      role="navigation"
      aria-label={t('blog.navLabel')}
      className={`-mx-1 flex gap-2 overflow-x-auto px-1 pb-0.5 touch-pan-y select-none ${
        drag.isGrabbing ? 'cursor-grabbing snap-none' : 'cursor-grab snap-x snap-mandatory'
      }`}
      onPointerDown={drag.onPointerDown}
      onPointerMove={drag.onPointerMove}
      onPointerUp={drag.onPointerUp}
      onLostPointerCapture={drag.onLostPointerCapture}
      onClickCapture={drag.onClickCapture}
      onDragStart={(e) => e.preventDefault()}
    >
      <CategoryPill
        to="/blog"
        label={t('blog.allCategories')}
        count={categories.reduce((sum, item) => sum + (item.count ?? 0), 0)}
        active={isAllActive}
      />
      {categories.map((category) => (
        <CategoryPill
          key={category.id}
          to={`/blog/category/${category.slug}`}
          label={category.label}
          count={category.count}
          active={activeCategory === category.slug}
        />
      ))}
    </div>
  )
}

function CategoryPill({
  to,
  label,
  count,
  active,
}: {
  to: string
  label: string
  count?: number
  active: boolean
}) {
  return (
    <Link
      to={to}
      className={`inline-flex shrink-0 snap-start items-center gap-2 rounded-full border px-4 py-2 text-sm font-medium transition-all duration-200 ${
        active
          ? 'border-warm bg-warm text-warm-text shadow-sm shadow-warm/20'
          : 'border-border/80 bg-surface text-text-muted hover:border-warm/40 hover:bg-surface-muted hover:text-warm'
      }`}
    >
      <span>{label}</span>
      {count != null && (
        <span
          className={`rounded-full px-2 py-0.5 text-xs tabular-nums ${
            active ? 'bg-warm-text/15 text-warm-text' : 'bg-surface-muted text-text-muted'
          }`}
        >
          {count}
        </span>
      )}
    </Link>
  )
}
