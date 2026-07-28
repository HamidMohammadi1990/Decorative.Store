import { Link } from 'react-router-dom'
import type { PromoTileStrip as PromoTileStripModel } from '@/models/home/promoTiles.model'
import { Container } from '@/components/ui/Container'
import { LocalImage } from '@/components/ui/LocalImage'
import { TextLink } from '@/components/ui/TextLink'

interface PromoTileStripProps {
  data: PromoTileStripModel
}

export function PromoTileStrip({ data }: PromoTileStripProps) {
  return (
    <section className="py-8">
      <Container>
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
          {data.tiles.map((tile) => (
            <Link
              key={tile.id}
              to={tile.link.href}
              className="group relative block overflow-hidden rounded-sm"
            >
              <LocalImage
                image={tile.image}
                className="aspect-[4/3] size-full object-cover transition-transform duration-500 group-hover:scale-105"
              />
              <div className="absolute inset-0 bg-gradient-to-t from-black/70 via-black/15 to-transparent transition-opacity group-hover:from-black/75" />
              <div className="absolute inset-x-0 bottom-0 flex flex-col items-center px-3 pb-5 pt-12 text-center">
                <h3 className="text-sm font-semibold text-white">{tile.title}</h3>
                {tile.subtitle && (
                  <p className="mt-1 text-xs text-white/90">{tile.subtitle}</p>
                )}
                <span className="mt-3 inline-flex items-center justify-center rounded-sm bg-warm px-4 py-1.5 text-xs font-medium text-warm-text shadow-sm transition-colors group-hover:bg-warm-hover">
                  {tile.link.label}
                </span>
              </div>
            </Link>
          ))}
        </div>
        {data.disclaimer && (
          <p className="mt-4 text-center text-xs text-text-muted">
            {data.disclaimer}{' '}
            {data.disclaimerLink && (
              <TextLink link={data.disclaimerLink} className="text-text" />
            )}
          </p>
        )}
      </Container>
    </section>
  )
}
