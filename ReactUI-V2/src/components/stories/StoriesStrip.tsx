import { StoriesCarousel } from '@/components/stories/StoriesCarousel'
import { useStories } from '@/hooks/useStories'

export function StoriesStrip() {
  const { stories, loading } = useStories()

  if (!loading && stories.length === 0) return null

  if (loading) {
    return <StoriesStripSkeleton />
  }

  return <StoriesCarousel stories={stories} />
}

function StoriesStripSkeleton() {
  return (
    <section className="border-b border-border/70 bg-surface">
      <div className="mx-auto max-w-7xl px-5 py-2.5 sm:px-8 sm:py-3 md:px-12 lg:px-16 xl:px-20">
        <div className="flex justify-center gap-3 overflow-hidden sm:gap-4 md:gap-5">
          {Array.from({ length: 12 }, (_, i) => (
            <div key={i} className="flex w-[4.5rem] shrink-0 flex-col items-center gap-1.5 sm:w-[4.875rem]">
              <div className="size-[4.125rem] animate-pulse rounded-full bg-surface-muted ring-2 ring-border/50 sm:size-[4.5rem]" />
              <div className="h-2 w-10 animate-pulse rounded bg-surface-muted" />
            </div>
          ))}
        </div>
      </div>
    </section>
  )
}
