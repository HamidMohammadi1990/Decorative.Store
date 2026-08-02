import type { CategoryTreeItem } from '@/models/catalog/categoryTree.model'
import type { NavLinkGroup } from '@/models/shared/link.model'

function slugToNavId(slug: string): string {
  const parts = slug.split('/').filter(Boolean)
  return parts[parts.length - 1] ?? slug
}

function toHref(slug: string): string {
  return `/${slug.replace(/^\/+/, '')}`
}

export function mapCategoryTreeToNav(items: CategoryTreeItem[]): NavLinkGroup[] {
  return items.map((category) => {
    const navItem: NavLinkGroup = {
      id: slugToNavId(category.slug),
      label: category.title,
      href: toHref(category.slug),
    }

    if (category.subCategories.length > 0) {
      navItem.columns = [
        {
          title: category.title,
          links: category.subCategories.map((subCategory) => ({
            label: subCategory.title,
            href: toHref(subCategory.slug),
          })),
        },
      ]
    }

    return navItem
  })
}
