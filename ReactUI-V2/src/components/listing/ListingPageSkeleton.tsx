import { Container } from '@/components/ui/Container'

function Bone({ className = '' }: { className?: string }) {
  return <div className={`animate-pulse rounded-md bg-surface-muted ${className}`} aria-hidden />
}

function ProductCardSkeleton() {
  return (
    <div className="overflow-hidden rounded-xl border border-border/60 bg-surface shadow-sm">
      <Bone className="aspect-[5/6] w-full rounded-none" />
      <div className="space-y-2 p-2.5 sm:p-3">
        <Bone className="h-3.5 w-4/5" />
        <Bone className="h-3 w-1/2" />
        <Bone className="h-3 w-1/3" />
        <Bone className="mt-1 h-8 w-full rounded-lg" />
      </div>
    </div>
  )
}

function SidebarSkeleton() {
  return (
    <aside className="hidden w-60 shrink-0 space-y-3 lg:block xl:w-72">
      <Bone className="h-16 w-full rounded-xl" />
      {Array.from({ length: 4 }, (_, index) => (
        <div key={index} className="overflow-hidden rounded-xl border border-border/60 bg-surface shadow-sm">
          <div className="flex items-center gap-2.5 px-4 py-3.5">
            <Bone className="size-8 shrink-0 rounded-lg" />
            <Bone className="h-4 flex-1" />
          </div>
          <div className="space-y-2 border-t border-border/50 p-4">
            {Array.from({ length: 3 }, (_, row) => (
              <Bone key={row} className="h-9 w-full rounded-lg" />
            ))}
          </div>
        </div>
      ))}
    </aside>
  )
}

export function ListingPageSkeleton() {
  return (
    <div
      className="bg-gradient-to-b from-surface-muted/30 via-surface to-surface py-6 md:py-8"
      role="status"
      aria-live="polite"
      aria-busy="true"
    >
      <Container>
        <div className="mb-3 flex flex-wrap items-center gap-2">
          <Bone className="h-3 w-12" />
          <Bone className="h-3 w-3 rounded-full" />
          <Bone className="h-3 w-20" />
          <Bone className="h-3 w-3 rounded-full" />
          <Bone className="h-3 w-24" />
        </div>

        <header className="mb-5 md:mb-6">
          <Bone className="h-8 w-2/3 max-w-md sm:h-9" />
        </header>

        <div className="flex gap-5 lg:gap-7 xl:gap-8">
          <SidebarSkeleton />

          <div className="min-w-0 flex-1">
            <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
              <Bone className="h-8 w-24 rounded-full" />
              <Bone className="h-8 w-32 rounded-full" />
            </div>
            <Bone className="mb-4 h-px w-full" />

            <div className="grid grid-cols-2 gap-2.5 sm:gap-3 md:grid-cols-2 lg:grid-cols-3 lg:gap-4">
              {Array.from({ length: 12 }, (_, index) => (
                <ProductCardSkeleton key={index} />
              ))}
            </div>

            <div className="mt-10 border-t border-border/50 pt-8">
              <Bone className="mx-auto mb-5 h-4 w-56" />
              <div className="flex items-center justify-center gap-3">
                <Bone className="size-10 rounded-full" />
                <Bone className="h-10 w-48 rounded-full" />
                <Bone className="size-10 rounded-full" />
              </div>
            </div>
          </div>
        </div>
      </Container>
    </div>
  )
}
