import type { DesignServicesStrip as DesignServicesModel } from '@/models/home/designServices.model'
import { Container } from '@/components/ui/Container'
import { TextLink } from '@/components/ui/TextLink'

interface DesignServicesStripProps {
  data: DesignServicesModel
}

export function DesignServicesStrip({ data }: DesignServicesStripProps) {
  return (
    <section className="border-b border-border bg-surface-muted">
      <Container className="flex flex-col gap-3 py-4 md:flex-row md:items-center md:justify-between">
        <div className="flex flex-wrap items-center gap-3">
          <h2 className="text-sm font-semibold">{data.title}</h2>
          <TextLink link={data.cta} className="text-sm font-medium text-accent" />
        </div>
        <ul className="flex flex-wrap gap-x-4 gap-y-1 text-xs text-text-muted">
          {data.featuredLinks.map((link) => (
            <li key={link.href}>
              <TextLink link={link} className="hover:text-text" />
            </li>
          ))}
        </ul>
      </Container>
    </section>
  )
}
