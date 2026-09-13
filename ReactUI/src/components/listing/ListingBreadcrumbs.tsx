import type { ListingBreadcrumb } from '@/models/catalog/listing.model'
import { PageBreadcrumbs, type PageBreadcrumbItem } from '@/components/ui/PageBreadcrumbs'

interface ListingBreadcrumbsProps {
  items: ListingBreadcrumb[]
}

export function ListingBreadcrumbs({ items }: ListingBreadcrumbsProps) {
  const mapped: PageBreadcrumbItem[] = items.map((item, index) => ({
    label: item.label,
    href: index < items.length - 1 ? item.href : undefined,
    home: index === 0 && item.href === '/',
  }))

  return <PageBreadcrumbs items={mapped} />
}
