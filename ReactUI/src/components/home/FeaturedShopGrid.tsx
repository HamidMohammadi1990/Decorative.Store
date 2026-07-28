import { Link } from 'react-router-dom'
import type { FeaturedShopGrid as FeaturedShopGridModel } from '@/models/home/featuredShop.model'
import { Container } from '@/components/ui/Container'
import { LocalImage } from '@/components/ui/LocalImage'

interface FeaturedShopGridProps {
  data: FeaturedShopGridModel
}

export function FeaturedShopGrid({ data }: FeaturedShopGridProps) {
  return (
    <section className="py-10">
      <Container>
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {data.sections.map((section) => (
            <article
              key={section.id}
              className="overflow-hidden rounded-sm"
            >
              <Link
                to={section.link.href}
                className="group relative block aspect-[5/4] overflow-hidden"
              >
                <LocalImage
                  image={section.image}
                  className="size-full object-cover transition-transform duration-500 group-hover:scale-105"
                />
                <div className="absolute inset-0 bg-gradient-to-t from-black/70 via-black/20 to-black/5 transition-opacity group-hover:from-black/75" />
                <div className="absolute inset-x-0 bottom-0 flex flex-col items-center px-4 pb-6 pt-16 text-center">
                  <h3 className="text-lg font-semibold text-white md:text-xl">
                    {section.title}
                  </h3>
                  {section.subtitle && (
                    <p className="mt-1 text-sm text-white/90">{section.subtitle}</p>
                  )}
                  <span className="mt-4 inline-flex items-center justify-center rounded-sm bg-warm px-5 py-2 text-sm font-medium text-warm-text shadow-sm transition-colors group-hover:bg-warm-hover">
                    {section.link.label}
                  </span>
                </div>
              </Link>
            </article>
          ))}
        </div>
      </Container>
    </section>
  )
}
