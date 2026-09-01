import { Link } from 'react-router-dom'
import type { ReactNode } from 'react'
import type {
  ContentBodyBlock,
  ContentCta,
  ContentHero,
  ContentStep,
  CmsContentPageContent,
} from '@/models/content/cmsContentPage.model'
import { Container } from '@/components/ui/Container'

function isExternalHref(href: string) {
  return href.startsWith('http') || href.startsWith('mailto:') || href.startsWith('tel:')
}

function CtaLink({
  href,
  className,
  children,
}: {
  href: string
  className: string
  children: ReactNode
}) {
  if (isExternalHref(href)) {
    return (
      <a href={href} className={className}>
        {children}
      </a>
    )
  }
  return (
    <Link to={href} className={className}>
      {children}
    </Link>
  )
}

export function ContentHeroSection({ data }: { data: ContentHero }) {
  return (
    <section className="border-b border-border bg-surface-muted py-14 md:py-20">
      <Container className="max-w-3xl text-center">
        {data.eyebrow && (
          <p className="text-xs font-semibold uppercase tracking-[0.2em] text-warm">{data.eyebrow}</p>
        )}
        <h1 className="mt-3 font-display text-4xl font-semibold tracking-tight text-text md:text-5xl">
          {data.title}
        </h1>
        {data.subtitle && (
          <p className="mt-5 text-base leading-relaxed text-text-muted md:text-lg">{data.subtitle}</p>
        )}
      </Container>
    </section>
  )
}

export function ContentBodyBlockSection({
  block,
  index,
}: {
  block: ContentBodyBlock
  index: number
}) {
  const muted = index % 2 === 1
  return (
    <section className={muted ? 'border-b border-border bg-surface-muted/50 py-12 md:py-14' : 'border-b border-border bg-surface py-12 md:py-14'}>
      <Container className="max-w-3xl">
        {block.heading && (
          <h2 className="font-display text-2xl font-semibold tracking-tight text-text md:text-3xl">
            {block.heading}
          </h2>
        )}
        {block.lead && (
          <p className="mt-4 text-base leading-relaxed text-text-muted md:text-lg">{block.lead}</p>
        )}
        {block.paragraphs.length > 0 && (
          <div className="mt-5 space-y-4 text-sm leading-relaxed text-text-muted md:text-base">
            {block.paragraphs.map((paragraph) => (
              <p key={paragraph.slice(0, 32)}>{paragraph}</p>
            ))}
          </div>
        )}
      </Container>
    </section>
  )
}

export function ContentStepsSection({
  heading,
  items,
}: {
  heading: string
  items: ContentStep[]
}) {
  if (items.length === 0) return null

  return (
    <section className="border-b border-border bg-surface py-14 md:py-16">
      <Container>
        <h2 className="text-center font-display text-2xl font-semibold tracking-tight text-text md:text-3xl">
          {heading}
        </h2>
        <ol className="mx-auto mt-10 grid max-w-4xl gap-5 md:grid-cols-3">
          {items.map((item, index) => (
            <li
              key={`${item.title}-${index}`}
              className="relative rounded-sm border border-border bg-surface-muted/40 p-6 text-center shadow-sm transition-shadow hover:shadow-md"
            >
              <span className="inline-flex size-10 items-center justify-center rounded-full bg-warm-soft text-sm font-bold text-warm">
                {index + 1}
              </span>
              <h3 className="mt-4 text-base font-semibold text-text">{item.title}</h3>
              <p className="mt-2 text-sm leading-relaxed text-text-muted">{item.description}</p>
            </li>
          ))}
        </ol>
      </Container>
    </section>
  )
}

export function ContentCtaSection({ data }: { data: ContentCta }) {
  return (
    <section className="bg-warm py-14 text-white md:py-16">
      <Container className="max-w-2xl text-center">
        <h2 className="font-display text-2xl font-semibold md:text-3xl">{data.title}</h2>
        {data.subtitle && (
          <p className="mt-3 text-base leading-relaxed text-white/85">{data.subtitle}</p>
        )}
        <div className="mt-8 flex flex-col items-center justify-center gap-3 sm:flex-row">
          <CtaLink
            href={data.primaryCta.href}
            className="inline-flex min-w-[11rem] items-center justify-center rounded-sm bg-white px-5 py-2.5 text-sm font-semibold text-warm transition-colors hover:bg-white/90"
          >
            {data.primaryCta.label}
          </CtaLink>
          {data.secondaryCta?.label && (
            <CtaLink
              href={data.secondaryCta.href}
              className="inline-flex min-w-[11rem] items-center justify-center rounded-sm border border-white/40 px-5 py-2.5 text-sm font-semibold text-white transition-colors hover:bg-white/10"
            >
              {data.secondaryCta.label}
            </CtaLink>
          )}
        </div>
      </Container>
    </section>
  )
}

export function CmsContentPageSections({ content }: { content: CmsContentPageContent }) {
  return (
    <article className="bg-surface">
      <ContentHeroSection data={content.hero} />
      {content.blocks.map((block, index) => (
        <ContentBodyBlockSection key={`${block.heading}-${index}`} block={block} index={index} />
      ))}
      {content.steps.items.length > 0 && (
        <ContentStepsSection heading={content.steps.heading} items={content.steps.items} />
      )}
      {content.cta?.title && <ContentCtaSection data={content.cta} />}
    </article>
  )
}
