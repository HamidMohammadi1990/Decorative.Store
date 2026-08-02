import type { ProductComment } from '@/models/catalog/productComment.model'

export interface ProductReviewItem {
  id: string
  author: string
  date: string
  rating: number
  text: string
  isBuyer: boolean
  variant?: string
  helpful: number
  notHelpful: number
}

export function formatCommentAuthor(comment: ProductComment): string {
  const fullName = [comment.userFirstName, comment.userLastName].filter(Boolean).join(' ').trim()
  return fullName || comment.userName
}

export function mapProductCommentToReview(
  comment: ProductComment,
  locale: string,
): ProductReviewItem {
  const dateLocale = locale === 'fa' ? 'fa-IR' : 'en-US'

  return {
    id: comment.id,
    author: formatCommentAuthor(comment),
    date: '',
    rating: comment.commentRate,
    text: comment.description,
    isBuyer: false,
    variant: comment.commentTopicTitle || undefined,
    helpful: 0,
    notHelpful: 0,
  }
}
