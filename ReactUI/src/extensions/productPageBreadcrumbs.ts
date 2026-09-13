import type { TFunction } from 'i18next'
import type { PageBreadcrumbItem } from '@/components/ui/PageBreadcrumbs'
import type { ProductSummary } from '@/models/catalog/product.model'

function slugLeaf(slug: string): string {
  const parts = slug.split('/').filter(Boolean)
  return parts[parts.length - 1] ?? slug
}

function categoryLabel(t: TFunction, slug: string): string {
  const leaf = slugLeaf(slug)
  return t(`product.categories.${leaf}`, {
    defaultValue: leaf.replace(/-/g, ' '),
  })
}

export function buildProductPageBreadcrumbs(
  product: ProductSummary,
  t: TFunction,
): PageBreadcrumbItem[] {
  const items: PageBreadcrumbItem[] = [
    {
      label: t('product.breadcrumbHome'),
      href: '/',
      home: true,
    },
  ]

  const slugs = product.categorySlugs.filter(Boolean)
  let path = ''

  for (const slug of slugs) {
    const segment = slugLeaf(slug)
    path = path ? `${path}/${segment}` : segment.replace(/^\/+/, '')

    items.push({
      label: categoryLabel(t, slug),
      href: `/${path}`,
    })
  }

  items.push({ label: product.title })

  return items
}

export function buildProductBreadcrumbJsonLdItems(
  product: ProductSummary,
  t: TFunction,
): { name: string; path?: string }[] {
  return buildProductPageBreadcrumbs(product, t)
    .filter((item) => item.label)
    .map((item) => ({
      name: item.label,
      path: item.href,
    }))
}
