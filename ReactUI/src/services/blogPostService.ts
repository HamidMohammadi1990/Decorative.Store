import type { BlogPostDetail, BlogPostSummary } from '@/models/blog/blog.model'
import type { PagedRequest, PagedResult } from '@/models/shared/paged.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet, apiPost } from '@/services/api/apiClient'
import {
  normalizeBlogPostDetailPage,
  normalizeBlogPostSummary,
  readBlogPostCategoryId,
} from '@/services/mappers/blogResponseNormalize'

const BLOG_POST_SEARCH_PATH = '/api/v1/blog-post/search'
const BLOG_POST_DETAIL_PATH = '/api/v1/blog-post/detail'

const DEFAULT_COVER = {
  src: '/images/home/living-room.jpg',
  alt: '',
}

interface BlogPostSearchOptions {
  slug?: string
  categoryId?: string
  categorySlugById?: Record<string, string>
  isFeatured?: boolean
  pagination?: PagedRequest
}

interface BlogPostDetailPageResult {
  post: BlogPostDetail | null
  related: BlogPostSummary[]
  categoryMap: Record<string, string>
}

function searchCacheKey(locale: Locale, options: BlogPostSearchOptions) {
  const pagination = options.pagination ?? { pageNumber: 1, pageSize: 12 }
  return `${locale}:${options.slug ?? 'all'}:${options.categoryId ?? 'all'}:${options.isFeatured ?? 'all'}:${pagination.pageNumber}:${pagination.pageSize}`
}

const searchCache = new Map<string, PagedResult<BlogPostSummary>>()
const searchRequests = new Map<string, Promise<PagedResult<BlogPostSummary>>>()
const detailCache = new Map<string, BlogPostDetailPageResult>()
const detailRequests = new Map<string, Promise<BlogPostDetailPageResult>>()

function detailCacheKey(locale: Locale, slug: string) {
  return `${locale}:${slug}`
}

function mapSearchResult(
  result: {
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
  },
  options: BlogPostSearchOptions,
  pagination: PagedRequest,
): PagedResult<BlogPostSummary> {
  const categorySlugById = options.categorySlugById ?? {}
  const items = (result.items ?? result.Items ?? []).map((item) => {
    const categoryId = readBlogPostCategoryId(item)
    const categorySlug = categorySlugById[categoryId] ?? ''
    return normalizeBlogPostSummary(item, DEFAULT_COVER, categorySlug)
  })

  return {
    items,
    pageNumber: result.pageNumber ?? result.PageNumber ?? pagination.pageNumber,
    pageSize: result.pageSize ?? result.PageSize ?? pagination.pageSize,
    totalCount: result.totalCount ?? result.TotalCount ?? items.length,
    totalPages: result.totalPages ?? result.TotalPages ?? 1,
  }
}

export const blogPostService = {
  async getDetail(slug: string, locale: Locale, force = false): Promise<BlogPostDetailPageResult> {
    const key = detailCacheKey(locale, slug)

    if (!force) {
      const cached = detailCache.get(key)
      if (cached) return cached

      const inFlight = detailRequests.get(key)
      if (inFlight) return inFlight
    } else {
      detailCache.delete(key)
    }

    const request = (async () => {
      const query = `?slug=${encodeURIComponent(slug)}`
      const data = await apiGet<unknown>(`${BLOG_POST_DETAIL_PATH}${query}`, locale)
      return normalizeBlogPostDetailPage(data)
    })()

    detailRequests.set(key, request)

    try {
      const result = await request
      detailCache.set(key, result)
      return result
    } catch (error) {
      detailCache.delete(key)
      throw error
    } finally {
      detailRequests.delete(key)
    }
  },

  async search(
    locale: Locale,
    options: BlogPostSearchOptions = {},
    force = false,
  ): Promise<PagedResult<BlogPostSummary>> {
    const pagination = options.pagination ?? { pageNumber: 1, pageSize: 12 }
    const key = searchCacheKey(locale, { ...options, pagination })

    if (!force) {
      const cached = searchCache.get(key)
      if (cached) return cached

      const inFlight = searchRequests.get(key)
      if (inFlight) return inFlight
    } else {
      searchCache.delete(key)
    }

    const body: Record<string, unknown> = { pagination }
    if (options.categoryId) {
      body.categoryId = options.categoryId
    }
    if (options.slug) {
      body.slug = options.slug
    }
    if (options.isFeatured !== undefined) {
      body.isFeatured = options.isFeatured
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
    }>(BLOG_POST_SEARCH_PATH, body, { locale }).then((result) =>
      mapSearchResult(result, options, pagination),
    )

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
}
