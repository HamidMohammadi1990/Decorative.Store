import type { CategoryTreeItem } from '@/models/catalog/categoryTree.model'
import type { CategoryNav } from '@/models/home/categoryNav.model'
import type { NavLinkGroup } from '@/models/shared/link.model'

function slugToNavId(slug: string): string {
  const parts = slug.split('/').filter(Boolean)
  return parts[parts.length - 1] ?? slug
}

function slugLeaf(slug: string): string {
  const parts = slug.split('/').filter(Boolean)
  return parts[parts.length - 1] ?? slug
}

function toHref(slug: string): string {
  return `/${slug.replace(/^\/+/, '')}`
}

function toSubCategoryHref(categorySlug: string, subCategorySlug: string): string {
  const normalizedSub = subCategorySlug.replace(/^\/+/, '')
  if (normalizedSub.includes('/')) return toHref(normalizedSub)
  const normalizedCategory = categorySlug.replace(/^\/+/, '')
  if (!normalizedCategory) return toHref(normalizedSub)
  return `/${normalizedCategory}/${slugLeaf(normalizedSub)}`
}

export function mapCategoryTreeToNav(items: CategoryTreeItem[]): NavLinkGroup[] {
  return items.map((category) => {
    const subCategories = category.subCategories ?? []
    const navItem: NavLinkGroup = {
      id: slugToNavId(category.slug),
      label: category.title,
      href: toHref(category.slug),
    }

    if (subCategories.length > 0) {
      navItem.columns = [
        {
          title: category.title,
          links: subCategories.map((subCategory) => ({
            label: subCategory.title,
            href: toSubCategoryHref(category.slug, subCategory.slug),
          })),
        },
      ]
    }

    return navItem
  })
}

export function mapCategoryTreeToCategoryNav(items: CategoryTreeItem[]): CategoryNav {
  return {
    items: items.map((category) => ({
      label: category.title,
      href: toHref(category.slug),
    })),
  }
}
