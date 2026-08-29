import type {
  CreateProductCommentInput,
  ProductComment,
  VoteProductCommentResult,
} from '@/models/catalog/productComment.model'
import type { DashboardReview } from '@/models/dashboard/dashboard.model'
import type { PagedRequest, PagedResult } from '@/models/shared/paged.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet, apiPost } from '@/services/api/apiClient'
import {
  normalizeProductCommentPaged,
  normalizeVoteProductCommentResult,
} from '@/services/mappers/productCommentNormalize'
import { mapMyProductCommentsResponse } from '@/services/mappers/reviewMapper'

const PRODUCT_COMMENT_SEARCH_PATH = '/api/v1/product-comment/search'
const PRODUCT_COMMENT_MY_PATH = '/api/v1/product-comment/my'
const PRODUCT_COMMENT_CREATE_PATH = '/api/v1/product-comment/create'
const PRODUCT_COMMENT_VOTE_PATH = '/api/v1/product-comment/vote'

export const productCommentService = {
  async getMy(accessToken: string, locale: Locale): Promise<DashboardReview[]> {
    const result = await apiGet<unknown>(PRODUCT_COMMENT_MY_PATH, locale, accessToken)
    return mapMyProductCommentsResponse(result)
  },

  async search(
    productId: string,
    locale: Locale,
    pagination: PagedRequest = { pageNumber: 1, pageSize: 50 },
  ): Promise<PagedResult<ProductComment>> {
    const data = await apiPost<unknown>(
      PRODUCT_COMMENT_SEARCH_PATH,
      { productId, pagination },
      { locale },
    )

    return normalizeProductCommentPaged(data)
  },

  async create(
    input: CreateProductCommentInput,
    locale: Locale,
    accessToken: string,
  ) {
    return apiPost<{ id: string }>(PRODUCT_COMMENT_CREATE_PATH, input, {
      locale,
      accessToken,
    })
  },

  async vote(
    commentId: string,
    isHelpful: boolean,
    locale: Locale,
    accessToken: string,
  ): Promise<VoteProductCommentResult> {
    const data = await apiPost<unknown>(
      PRODUCT_COMMENT_VOTE_PATH,
      { id: commentId, isHelpful },
      { locale, accessToken },
    )

    return normalizeVoteProductCommentResult(data)
  },
}
