import type { BlogComment } from '@/models/blog/blog.model'
import type { PagedRequest, PagedResult } from '@/models/shared/paged.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiPost } from '@/services/api/apiClient'
import { normalizeBlogComment } from '@/services/mappers/blogResponseNormalize'

const BLOG_POST_COMMENT_SEARCH_PATH = '/api/v1/blog-post-comment/search'
const BLOG_POST_COMMENT_CREATE_PATH = '/api/v1/blog-post-comment/create'

interface CreateBlogPostCommentInput {
  blogPostId: string
  content: string
  parentId?: string | null
}

export const blogPostCommentService = {
  async search(
    locale: Locale,
    blogPostId: string,
    pagination: PagedRequest = { pageNumber: 1, pageSize: 50 },
  ): Promise<PagedResult<BlogComment>> {
    const result = await apiPost<{
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
      BLOG_POST_COMMENT_SEARCH_PATH,
      { blogPostId, pagination },
      { locale },
    )

    const items = (result.items ?? result.Items ?? []).map(normalizeBlogComment)

    return {
      items,
      pageNumber: result.pageNumber ?? result.PageNumber ?? pagination.pageNumber,
      pageSize: result.pageSize ?? result.PageSize ?? pagination.pageSize,
      totalCount: result.totalCount ?? result.TotalCount ?? items.length,
      totalPages: result.totalPages ?? result.TotalPages ?? 1,
    }
  },

  async create(
    input: CreateBlogPostCommentInput,
    locale: Locale,
    accessToken: string,
  ): Promise<{ id: string }> {
    const result = await apiPost<{ id?: string; Id?: string }>(
      BLOG_POST_COMMENT_CREATE_PATH,
      {
        blogPostId: input.blogPostId,
        content: input.content,
        parentId: input.parentId ?? null,
      },
      { locale, accessToken },
    )

    return { id: result.id ?? result.Id ?? '' }
  },
}
