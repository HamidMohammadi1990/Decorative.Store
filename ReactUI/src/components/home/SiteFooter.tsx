import type { SiteFooter as SiteFooterModel } from '@/models/home/siteFooter.model'
import { Container } from '@/components/ui/Container'
import { TextLink } from '@/components/ui/TextLink'
import { Button } from '@/components/ui/Button'
import { BackToTop } from '@/components/ui/BackToTop'

interface SiteFooterProps {
  data: SiteFooterModel
}

export function SiteFooter({ data }: SiteFooterProps) {
  return (
    <footer className="border-t border-border bg-surface-muted">
      <Container className="py-12">
        <div className="mb-10 max-w-md">
          <h2 className="text-sm font-semibold">{data.newsletterTitle}</h2>
          <form
            className="mt-3 flex gap-2"
            onSubmit={(e) => e.preventDefault()}
          >
            <input
              type="email"
              placeholder={data.newsletterPlaceholder}
              className="flex-1 rounded-sm border border-border bg-surface px-3 py-2 text-sm"
            />
            <Button type="submit" variant="warm">{data.newsletterButton}</Button>
          </form>
        </div>

        <div className="grid gap-8 sm:grid-cols-2 lg:grid-cols-4">
          {data.columns.map((col) => (
            <div key={col.title}>
              <h3 className="text-xs font-semibold uppercase tracking-wider">
                {col.title}
              </h3>
              <ul className="mt-3 space-y-2 text-sm text-text-muted">
                {col.links.map((link) => (
                  <li key={link.href}>
                    <TextLink link={link} className="hover:text-text" />
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>

        <div className="relative mt-10 border-t border-border pt-8 text-xs text-text-muted">
          <BackToTop />
          <div className="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
            <p>{data.copyright}</p>
            <ul className="flex flex-wrap gap-4">
              {data.legalLinks.map((link) => (
                <li key={link.href}>
                  <TextLink link={link} />
                </li>
              ))}
            </ul>
          </div>
        </div>
      </Container>
    </footer>
  )
}
