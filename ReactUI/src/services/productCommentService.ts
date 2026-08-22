import type {
  CreateProductCommentInput,
  ProductComment,
} from '@/models/catalog/productComment.model'
import type { DashboardReview } from '@/models/dashboard/dashboard.model'
import type { PagedRequest, PagedResult } from '@/models/shared/paged.model'
import type { Locale } from '@/models/shared/locale.model'
import { apiGet, apiPost } from '@/services/api/apiClient'
import { mapMyProductCommentsResponse } from '@/services/mappers/reviewMapper'

const PRODUCT_COMMENT_SEARCH_PATH = '/api/v1/product-comment/search'
const PRODUCT_COMMENT_MY_PATH = '/api/v1/product-comment/my'
const PRODUCT_COMMENT_CREATE_PATH = '/api/v1/product-comment/create'

export const productCommentService = {
  async getMy(accessToken: string, locale: Locale): Promise<DashboardReview[]> {
    const result = await apiGet<unknown>(PRODUCT_COMMENT_MY_PATH, locale, accessToken)
    return mapMyProductCommentsResponse(result)
  },

  async search(
    productId: string,
    locale: Locale,
    pagination: PagedRequest = { pageNumber: 1, pageSize: 50 },
  ) {
    return apiPost<PagedResult<ProductComment>>(
      PRODUCT_COMMENT_SEARCH_PATH,
      { productId, pagination },
      { locale },
    )
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
}
