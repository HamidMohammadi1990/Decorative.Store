import type { LoaderFunctionArgs } from 'react-router-dom'
import { mapProductCommentToReview } from '@/extensions/productReviews'
import { mapProductQuestionToItem } from '@/extensions/productQuestions'
import { catalogService } from '@/services/catalogService'
import { productCommentService } from '@/services/productCommentService'
import { productQuestionService } from '@/services/productQuestionService'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { ProductDetailLoaderData } from '@/routes/loaders/types'

export async function productDetailLoader({
  request,
  params,
}: LoaderFunctionArgs): Promise<ProductDetailLoaderData> {
  const locale = resolveLocale(request)
  const slug = params.slug?.trim()

  if (!slug) {
    return {
      locale,
      slug: '',
      product: null,
      related: [],
      reviews: [],
      questions: [],
      error: 'not-found',
    }
  }

  try {
    const [product, related] = await Promise.all([
      catalogService.getProduct(slug, locale),
      catalogService.getRelatedProducts(slug, locale),
    ])

    if (!product) {
      return {
        locale,
        slug,
        product: null,
        related: [],
        reviews: [],
        questions: [],
        error: 'not-found',
      }
    }

    const [commentsResult, questionsResult] = await Promise.all([
      productCommentService.search(product.id, locale, { pageNumber: 1, pageSize: 50 }),
      productQuestionService.search(product.id, locale, { pageNumber: 1, pageSize: 50 }),
    ])

    const reviews = commentsResult.items.map((item) => mapProductCommentToReview(item, locale))
    const questions = questionsResult.items.map((item) => mapProductQuestionToItem(item, locale))

    return { locale, slug, product, related, reviews, questions }
  } catch {
    return {
      locale,
      slug,
      product: null,
      related: [],
      reviews: [],
      questions: [],
      error: 'failed',
    }
  }
}
