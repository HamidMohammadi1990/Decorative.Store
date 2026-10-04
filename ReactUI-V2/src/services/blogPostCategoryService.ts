import type { BlogCategory } from '@/models/blog/blog.model'
import type { PagedRequest, PagedResult } from '@/models/shared/paged.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { normalizeBlogPostCategory } from '@/services/mappers/blogResponseNormalize'

const BLOG_POST_CATEGORY_SEARCH_PATH = '/api/v1/blog-post-category/search'

const searchCache = new Map<string, PagedResult<BlogCategory>>()
const searchRequests = new Map<string, Promise<PagedResult<BlogCategory>>>()

function cacheKey(locale: Locale, pagination: PagedRequest) {
  return `${locale}:${pagination.pageNumber}:${pagination.pageSize}`
}

export const blogPostCategoryService = {
  async search(
    locale: Locale,
    pagination: PagedRequest = { pageNumber: 1, pageSize: 50 },
    force = false,
  ): Promise<PagedResult<BlogCategory>> {
    const key = cacheKey(locale, pagination)

    if (!force) {
      const cached = searchCache.get(key)
      if (cached) return cached

      const inFlight = searchRequests.get(key)
      if (inFlight) return inFlight
    } else {
      searchCache.delete(key)
    }

    const request = apiPost<{
      items?: unknown[]
      Items?: unknown[]
      pageNumber?: number
      PageNumber?: number
      pageSize?: number
      PageSize?: number
      totalCount?: number
      TotalCount?: number
      totalPages?: number
      TotalPages?: number
    }>(
      BLOG_POST_CATEGORY_SEARCH_PATH,
      { pagination },
      { locale },
    ).then((result) => {
      const items = (result.items ?? result.Items ?? [])
        .map(normalizeBlogPostCategory)
        .filter((category) => category.slug)

      return {
        items,
        pageNumber: result.pageNumber ?? result.PageNumber ?? pagination.pageNumber,
        pageSize: result.pageSize ?? result.PageSize ?? pagination.pageSize,
        totalCount: result.totalCount ?? result.TotalCount ?? items.length,
        totalPages: result.totalPages ?? result.TotalPages ?? 1,
      } satisfies PagedResult<BlogCategory>
    })

    searchRequests.set(key, request)

    try {
      const result = await request
      searchCache.set(key, result)
      return result
    } catch (error) {
      searchCache.delete(key)
      throw error
    } finally {
      searchRequests.delete(key)
    }
  },

  async getCategories(locale: Locale, force = false): Promise<BlogCategory[]> {
    const result = await this.search(locale, { pageNumber: 1, pageSize: 50 }, force)
    return result.items
  },
}
