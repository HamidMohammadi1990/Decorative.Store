import type {
  BlogCategory,
  BlogListingResult,
  BlogPostDetail,
  BlogPostSummary,
} from '@/models/blog/blog.model'
import type { Locale } from '@/models/shared/locale.model'
import { blogPostCategoryService } from '@/services/blogPostCategoryService'
import { blogPostService } from '@/services/blogPostService'

function resolveCategoryId(categories: BlogCategory[], categorySlug?: string) {
  return categories.find((category) => category.slug === categorySlug)?.id
}

function buildCategorySlugMap(categories: BlogCategory[]) {
  return Object.fromEntries(categories.map((category) => [category.id, category.slug]))
}

export const blogService = {
  async getNavCategories(locale: Locale) {
    return blogPostCategoryService.getCategories(locale)
  },

  async getFeaturedPosts(locale: Locale, limit = 3): Promise<BlogPostSummary[]> {
    const postsResult = await blogPostService.search(locale, {
      pagination: { pageNumber: 1, pageSize: 50 },
    })

    return postsResult.items.slice(0, limit)
  },

  async getListing(locale: Locale, categorySlug?: string): Promise<BlogListingResult> {
    const categories = await blogPostCategoryService.getCategories(locale)
    const categoryId = resolveCategoryId(categories, categorySlug)
    const categorySlugById = buildCategorySlugMap(categories)

    const postsResult = await blogPostService.search(locale, {
      categoryId,
      categorySlugById,
      pagination: { pageNumber: 1, pageSize: 50 },
    })

    const posts = postsResult.items
    const featuredPosts = [...posts].slice(0, 5)

    return {
      categories,
      featuredPosts,
      posts,
      totalCount: postsResult.totalCount,
      activeCategory: categorySlug,
    }
  },

  async getPostDetail(
    slug: string,
    locale: Locale,
    force = false,
  ): Promise<{
    post: BlogPostDetail | null
    related: BlogPostSummary[]
    categoryMap: Record<string, string>
  }> {
    return blogPostService.getDetail(slug, locale, force)
  },
}
