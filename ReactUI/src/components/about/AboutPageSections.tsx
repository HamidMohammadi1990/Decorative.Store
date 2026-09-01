import { Link } from 'react-router-dom'
import type {
  AboutHero,
  AboutPageContent,
  AboutStory,
  AboutStat,
  AboutTimelineItem,
  AboutValue,
} from '@/models/about/aboutPage.model'
import { Button } from '@/components/ui/Button'
import { Container } from '@/components/ui/Container'
import { LocalImage } from '@/components/ui/LocalImage'

const VALUE_ICONS: Record<string, string> = {
  quality: '✦',
  design: '◈',
  sustainability: '❋',
  service: '◎',
}

export function AboutHeroSection({ data }: { data: AboutHero }) {
  return (
    <section className="relative overflow-hidden bg-surface-muted">
      <div className="grid min-h-[28rem] lg:grid-cols-2 lg:min-h-[34rem]">
        <div className="flex items-center">
          <Container className="py-14 lg:py-20">
            {data.eyebrow && (
              <p className="mb-3 text-xs font-semibold uppercase tracking-[0.2em] text-warm">
                {data.eyebrow}
              </p>
            )}
            <h1 className="font-display text-4xl font-semibold leading-tight tracking-tight text-text md:text-5xl">
              {data.title}
            </h1>
            {data.subtitle && (
              <p className="mt-5 max-w-xl text-base leading-relaxed text-text-muted md:text-lg">
                {data.subtitle}
              </p>
            )}
            {data.cta.label && (
              <div className="mt-8">
                <Link to={data.cta.href} className="inline-block">
                  <Button variant="primary">{data.cta.label}</Button>
                </Link>
              </div>
            )}
          </Container>
        </div>
        <div className="relative min-h-[18rem] lg:min-h-full">
          <LocalImage
            image={data.image}
            loading="eager"
            className="absolute inset-0 size-full object-cover"
          />
          <div className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/20 via-transparent to-transparent lg:bg-gradient-to-l lg:from-black/10 lg:via-transparent lg:to-transparent" />
        </div>
      </div>
    </section>
  )
}

export function AboutStorySection({ data }: { data: AboutStory }) {
  const hasText = Boolean(data.heading?.trim() || data.lead?.trim() || data.paragraphs.length > 0)
  if (!hasText && !data.image.src) return null

  return (
    <section className="border-b border-border bg-surface py-16 md:py-20">
      <Container>
        <div className={`grid items-center gap-10 ${hasText ? 'lg:grid-cols-[1.1fr_0.9fr] lg:gap-16' : ''}`}>
          {hasText && (
            <div>
              {data.heading && (
                <h2 className="font-display text-3xl font-semibold tracking-tight text-text md:text-4xl">
                  {data.heading}
                </h2>
              )}
              {data.lead && (
                <p className="mt-5 text-lg leading-relaxed text-text-muted">{data.lead}</p>
              )}
              <div className="mt-6 space-y-4 text-sm leading-relaxed text-text-muted md:text-base">
                {data.paragraphs.map((paragraph) => (
                  <p key={paragraph.slice(0, 24)}>{paragraph}</p>
                ))}
              </div>
            </div>
          )}
          {data.image.src && (
            <div className="rounded-sm border border-border bg-surface-muted/60 p-8 md:p-10">
              <LocalImage
                image={data.image}
                className="mx-auto h-auto max-h-72 w-full max-w-sm object-contain"
              />
            </div>
          )}
        </div>
      </Container>
    </section>
  )
}

export function AboutStatsSection({
  heading,
  items,
}: {
  heading: string
  items: AboutStat[]
}) {
  return (
    <section className="border-b border-border bg-surface-muted py-14 md:py-16">
      <Container>
        <h2 className="text-center font-display text-2xl font-semibold tracking-tight text-text md:text-3xl">
          {heading}
        </h2>
        <dl className="mt-10 grid gap-6 sm:grid-cols-2 lg:grid-cols-4">
          {items.map((item) => (
            <div
              key={`${item.value}-${item.label}`}
              className="rounded-sm border border-border bg-surface px-5 py-6 text-center shadow-sm"
            >
              <dt className="font-display text-3xl font-semibold text-warm md:text-4xl">{item.value}</dt>
              <dd className="mt-2 text-sm leading-relaxed text-text-muted">{item.label}</dd>
            </div>
          ))}
        </dl>
      </Container>
    </section>
  )
}

export function AboutValuesSection({
  heading,
  items,
}: {
  heading: string
  items: AboutValue[]
}) {
  return (
    <section className="border-b border-border bg-surface py-16 md:py-20">
      <Container>
        <h2 className="text-center font-display text-2xl font-semibold tracking-tight text-text md:text-3xl">
          {heading}
        </h2>
        <div className="mt-10 grid gap-5 md:grid-cols-2">
          {items.map((item) => (
            <article
              key={item.id}
              className="rounded-sm border border-border bg-surface-muted/40 p-6 md:p-7"
            >
              <span className="inline-flex size-10 items-center justify-center rounded-full bg-warm-soft text-lg text-warm">
                {VALUE_ICONS[item.id] ?? '◆'}
              </span>
              <h3 className="mt-4 text-lg font-semibold text-text">{item.title}</h3>
              <p className="mt-2 text-sm leading-relaxed text-text-muted md:text-base">
                {item.description}
              </p>
            </article>
          ))}
        </div>
      </Container>
    </section>
  )
}

export function AboutTimelineSection({
  heading,
  items,
}: {
  heading: string
  items: AboutTimelineItem[]
}) {
  return (
    <section className="border-b border-border bg-surface-muted py-16 md:py-20">
      <Container className="max-w-3xl">
        <h2 className="text-center font-display text-2xl font-semibold tracking-tight text-text md:text-3xl">
          {heading}
        </h2>
        <ol className="mt-10 space-y-0">
          {items.map((item, index) => (
            <li key={`${item.year}-${index}`} className="flex gap-5 md:gap-8">
              <div className="relative flex w-10 shrink-0 justify-center md:w-12">
                <span
                  aria-hidden
                  className="relative z-10 mt-1.5 size-3 shrink-0 rounded-full border-2 border-surface bg-warm"
                />
                {index < items.length - 1 && (
                  <span
                    aria-hidden
                    className="absolute top-4 bottom-0 w-px bg-warm/30"
                  />
                )}
              </div>
              <div className={`min-w-0 flex-1 ${index < items.length - 1 ? 'pb-8' : ''}`}>
                <p className="text-sm font-semibold tracking-wide text-warm">{item.year}</p>
                <p className="mt-1.5 text-sm leading-relaxed text-text-muted md:text-base">{item.text}</p>
              </div>
            </li>
          ))}
        </ol>
      </Container>
    </section>
  )
}

export function AboutPageSections({ content }: { content: AboutPageContent }) {
  return (
    <>
      <AboutHeroSection data={content.hero} />
      <AboutStorySection data={content.story} />
      {content.stats.items.length > 0 && (
        <AboutStatsSection heading={content.stats.heading} items={content.stats.items} />
      )}
      {content.values.items.length > 0 && (
        <AboutValuesSection heading={content.values.heading} items={content.values.items} />
      )}
      {content.timeline.items.length > 0 && (
        <AboutTimelineSection heading={content.timeline.heading} items={content.timeline.items} />
      )}
    </>
  )
}
