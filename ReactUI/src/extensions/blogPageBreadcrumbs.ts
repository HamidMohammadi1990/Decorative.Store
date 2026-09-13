import type { TFunction } from 'i18next'
import type { PageBreadcrumbItem } from '@/components/ui/PageBreadcrumbs'
import type { BlogPostDetail } from '@/models/blog/blog.model'

export function buildBlogPageBreadcrumbs(
  post: Pick<BlogPostDetail, 'title' | 'categorySlug' | 'categoryLabel'>,
  t: TFunction,
  categoryMap: Record<string, string> = {},
): PageBreadcrumbItem[] {
  const items: PageBreadcrumbItem[] = [
    {
      label: t('product.breadcrumbHome'),
      href: '/',
      home: true,
    },
    {
      label: t('blog.title'),
      href: '/blog',
    },
  ]

  if (post.categorySlug) {
    items.push({
      label: post.categoryLabel || categoryMap[post.categorySlug] || post.categorySlug.replace(/-/g, ' '),
      href: `/blog/category/${post.categorySlug}`,
    })
  }

  items.push({ label: post.title })

  return items
}

export function buildBlogBreadcrumbJsonLdItems(
  post: Pick<BlogPostDetail, 'title' | 'categorySlug' | 'categoryLabel'>,
  t: TFunction,
  categoryMap: Record<string, string> = {},
): { name: string; path?: string }[] {
  return buildBlogPageBreadcrumbs(post, t, categoryMap)
    .filter((item) => item.label)
    .map((item) => ({
      name: item.label,
      path: item.href,
    }))
}
