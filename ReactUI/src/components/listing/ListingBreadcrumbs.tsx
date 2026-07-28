import { Link } from 'react-router-dom'
import type { ListingBreadcrumb } from '@/models/catalog/listing.model'

interface ListingBreadcrumbsProps {
  items: ListingBreadcrumb[]
}

export function ListingBreadcrumbs({ items }: ListingBreadcrumbsProps) {
  return (
    <nav aria-label="Breadcrumb" className="text-sm text-text-muted">
      <ol className="flex flex-wrap items-center gap-1.5">
        {items.map((item, index) => {
          const isLast = index === items.length - 1
          return (
            <li key={item.href} className="flex items-center gap-1.5">
              {index > 0 && <span aria-hidden className="text-border-strong">/</span>}
              {isLast ? (
                <span className="font-medium text-text">{item.label}</span>
              ) : (
                <Link to={item.href} className="transition-colors hover:text-warm">
                  {item.label}
                </Link>
              )}
            </li>
          )
        })}
      </ol>
    </nav>
  )
}
