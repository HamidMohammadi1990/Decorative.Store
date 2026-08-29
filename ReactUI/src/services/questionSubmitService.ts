import type { ProductDetail } from '@/models/catalog/productDetail.model'
import type { Locale } from '@/models/shared/locale.model'
import { productQuestionService } from '@/services/productQuestionService'
import { resolveReviewProductId } from '@/services/reviewSubmitService'
import { useUserStore } from '@/stores/userStore'

export async function submitProductQuestion(input: {
  product: ProductDetail
  locale: Locale
  question: string
}) {
  let accessToken = useUserStore.getState().accessToken
  if (!accessToken) {
    throw new Error('missing-access-token')
  }

  const refreshedToken = await useUserStore.getState().refreshAccessToken()
  if (refreshedToken) {
    accessToken = refreshedToken
  }

  const productId = await resolveReviewProductId(input.product, input.locale)
  if (!productId) {
    throw new Error('missing-product-id')
  }

  const trimmed = input.question.trim()
  if (!trimmed) {
    throw new Error('missing-question')
  }

  await productQuestionService.create(
    { productId, question: trimmed },
    input.locale,
    accessToken,
  )
}
