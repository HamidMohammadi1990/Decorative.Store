import type { ProductDetail } from '@/models/catalog/productDetail.model'
import type { Locale } from '@/models/shared/locale.model'
import { catalogProductService } from '@/services/catalogProductService'
import { commentTopicService } from '@/services/commentTopicService'
import { productCommentService } from '@/services/productCommentService'
import { useUserStore } from '@/stores/userStore'

export async function resolveReviewProductId(
  product: ProductDetail,
  locale: Locale,
): Promise<string> {
  if (product.id) return product.id

  const catalogProduct = await catalogProductService.getProduct(product.slug, locale)
  return catalogProduct.id
}

export async function resolveDefaultCommentTopicId(
  locale: Locale,
  cachedTopicId: string | null,
): Promise<string | null> {
  if (cachedTopicId) return cachedTopicId

  try {
    return await commentTopicService.getDefaultTopicId(locale)
  } catch {
    return null
  }
}

export async function submitProductReview(input: {
  product: ProductDetail
  locale: Locale
  description: string
  cachedCommentTopicId: string | null
}) {
  const accessToken = useUserStore.getState().accessToken
  if (!accessToken) {
    throw new Error('missing-access-token')
  }

  const [productId, commentTopicId] = await Promise.all([
    resolveReviewProductId(input.product, input.locale),
    resolveDefaultCommentTopicId(input.locale, input.cachedCommentTopicId),
  ])

  if (!productId) {
    throw new Error('missing-product-id')
  }

  if (!commentTopicId) {
    throw new Error('missing-comment-topic')
  }

  await productCommentService.create(
    {
      productId,
      commentTopicId,
      description: input.description,
      commentRate: 5,
      qualityRating: 5,
      affordableRating: 5,
    },
    input.locale,
    accessToken,
  )
}
