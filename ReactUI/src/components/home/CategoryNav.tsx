import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { CategoryNav as CategoryNavModel } from '@/models/home/categoryNav.model'
import { Container } from '@/components/ui/Container'
import { ScrollArrowButton } from '@/components/ui/ScrollArrowButton'
import { useCategoryNavScroll } from '@/hooks/useCategoryNavScroll'

interface CategoryNavProps {
  data: CategoryNavModel
}

export function CategoryNav({ data }: CategoryNavProps) {
  const { t } = useTranslation()
  const {
    scrollRef,
    isGrabbing,
    canScrollPrev,
    canScrollNext,
    scrollPrev,
    scrollNext,
    onMouseDown,
    onClickCapture,
  } = useCategoryNavScroll()

  return (
    <section className="min-w-0 bg-warm-soft py-6">
      <Container className="relative min-w-0">
        <p className="mb-5 text-center text-sm font-semibold tracking-[0.18em] text-warm uppercase sm:text-base">
          {t('common.shopByCategory')}
        </p>

        <div className="flex items-center gap-2 sm:gap-3">
          <ScrollArrowButton
            direction="prev"
            disabled={!canScrollPrev}
            label={t('common.scrollPrev')}
            onClick={scrollPrev}
            className="border-warm-muted/80 bg-surface text-warm shadow-sm hover:border-warm hover:bg-warm hover:text-warm-text"
          />

          <div className="relative min-w-0 flex-1">
            {canScrollPrev && (
              <div
                aria-hidden
                className="pointer-events-none absolute inset-y-0 start-0 z-[1] w-8 bg-gradient-to-r from-warm-soft to-transparent"
              />
            )}
            {canScrollNext && (
              <div
                aria-hidden
                className="pointer-events-none absolute inset-y-0 end-0 z-[1] w-8 bg-gradient-to-l from-warm-soft to-transparent"
              />
            )}

            <div
              ref={scrollRef}
              role="region"
              aria-label={t('common.shopByCategory')}
              tabIndex={0}
              onMouseDown={onMouseDown}
              onClickCapture={onClickCapture}
              className={`category-nav-scroll relative z-[2] py-1 ${
                isGrabbing ? 'category-nav-scroll--grabbing cursor-grabbing' : 'cursor-grab'
              }`}
            >
              <div className="flex w-max gap-2.5 px-1">
                {data.items.map((item) => (
                  <Link
                    key={item.href}
                    to={item.href}
                    draggable={false}
                    className="shrink-0 rounded-full bg-surface px-5 py-2.5 text-sm font-medium whitespace-nowrap text-text shadow-sm ring-1 ring-warm-muted/70 transition-all duration-200 hover:-translate-y-0.5 hover:bg-warm hover:text-warm-text hover:ring-warm hover:shadow-none active:translate-y-0"
                  >
                    {item.label}
                  </Link>
                ))}
              </div>
            </div>
          </div>

          <ScrollArrowButton
            direction="next"
            disabled={!canScrollNext}
            label={t('common.scrollNext')}
            onClick={scrollNext}
            className="border-warm-muted/80 bg-surface text-warm shadow-sm hover:border-warm hover:bg-warm hover:text-warm-text"
          />
        </div>
      </Container>
    </section>
  )
}
