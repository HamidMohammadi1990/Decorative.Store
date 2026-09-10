import type { ProductComment } from '@/models/catalog/productComment.model'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import type { Locale } from '@/models/shared/locale.model'

export interface ProductReviewItem {
  id: string
  parentId?: string | null
  commentTopicId: string
  author: string
  authorAvatarUrl?: string
  date: string
  createdOnUtc?: string
  rating: number
  text: string
  isBuyer: boolean
  variant?: string
  helpful: number
  notHelpful: number
  userVoteHelpful?: boolean | null
  replies: ProductReviewItem[]
  replyToAuthor?: string
  isStandaloneReply?: boolean
}

export function formatCommentAuthor(comment: ProductComment): string {
  const fullName = [comment.userFirstName, comment.userLastName].filter(Boolean).join(' ').trim()
  return fullName || comment.userName
}

export function mapProductCommentToReview(
  comment: ProductComment,
  locale: string,
): ProductReviewItem {
  const dateLocale = locale === 'fa' ? 'fa' : 'en'

  return {
    id: comment.id,
    parentId: comment.parentId ?? null,
    commentTopicId: comment.commentTopicId,
    author: formatCommentAuthor(comment),
    authorAvatarUrl: comment.userAvatarUrl,
    date: comment.createdOnUtc
      ? formatBlogDate(comment.createdOnUtc, dateLocale as Locale)
      : '',
    createdOnUtc: comment.createdOnUtc,
    rating: comment.commentRate,
    text: comment.description,
    isBuyer: comment.isBuyer ?? false,
    variant: comment.commentTopicTitle || undefined,
    helpful: comment.helpfulCount ?? 0,
    notHelpful: comment.notHelpfulCount ?? 0,
    replies: [],
  }
}

function getReviewSortTime(review: ProductReviewItem): number {
  if (review.createdOnUtc) {
    const parsed = Date.parse(review.createdOnUtc)
    if (!Number.isNaN(parsed)) return parsed
  }

  const parsed = Date.parse(review.date)
  return Number.isNaN(parsed) ? 0 : parsed
}

export function isMainReview(review: ProductReviewItem): boolean {
  return !review.parentId && !review.isStandaloneReply
}

export function buildReviewTree(items: ProductReviewItem[]): ProductReviewItem[] {
  const nodes = new Map<string, ProductReviewItem>()

  for (const item of items) {
    nodes.set(item.id, { ...item, replies: [] })
  }

  const roots: ProductReviewItem[] = []

  for (const item of items) {
    const node = nodes.get(item.id)
    if (!node) continue

    if (item.parentId && nodes.has(item.parentId)) {
      const parent = nodes.get(item.parentId)
      if (parent) {
        node.replyToAuthor = parent.author
        parent.replies.push(node)
      }
    } else if (!item.parentId) {
      roots.push(node)
    } else {
      node.isStandaloneReply = true
      roots.push(node)
    }
  }

  for (const root of roots) {
    root.replies = sortReviewReplies(root.replies)
  }

  return roots
}

function sortReviewReplies(replies: ProductReviewItem[]): ProductReviewItem[] {
  return [...replies].sort((a, b) => {
    const aTime = getReviewSortTime(a)
    const bTime = getReviewSortTime(b)
    if (aTime !== bTime) return aTime - bTime
    return a.id.localeCompare(b.id)
  })
}

export function sortReviewRoots(roots: ProductReviewItem[], sort: 'newest' | 'buyers' | 'useful') {
  const mainReviews = roots.filter(isMainReview)
  const orphanReplies = roots.filter((review) => review.isStandaloneReply)

  let list = [...mainReviews]

  if (sort === 'buyers') {
    list = list.filter((review) => review.isBuyer)
  }

  if (sort === 'useful') {
    list.sort((a, b) => b.helpful - a.helpful)
  } else if (sort === 'newest') {
    list.sort((a, b) => getReviewSortTime(b) - getReviewSortTime(a))
  }

  orphanReplies.sort((a, b) => getReviewSortTime(b) - getReviewSortTime(a))

  return [...list, ...orphanReplies]
}

export function applyVoteToReviewTree(
  roots: ProductReviewItem[],
  commentId: string,
  vote: { helpfulCount: number; notHelpfulCount: number; userVoteHelpful?: boolean | null },
): ProductReviewItem[] {
  const updateNode = (node: ProductReviewItem): ProductReviewItem => {
    const replies = node.replies.map(updateNode)
    if (node.id !== commentId) {
      return { ...node, replies }
    }

    return {
      ...node,
      helpful: vote.helpfulCount,
      notHelpful: vote.notHelpfulCount,
      userVoteHelpful: vote.userVoteHelpful,
      replies,
    }
  }

  return roots.map(updateNode)
}

export function computeSatisfactionPercent(reviews: ProductReviewItem[]): number | null {
  const roots = reviews.filter((review) => !review.parentId)
  if (roots.length === 0) return null

  const positiveCount = roots.filter((review) => review.rating >= 4).length
  return Math.round((positiveCount / roots.length) * 100)
}

export function formatProductRatingDisplay(ratingValue: number): string {
  return ratingValue >= 4.95 ? '5' : ratingValue.toFixed(1)
}

export function computeReviewStatsFromItems(
  reviews: ProductReviewItem[],
): { ratingDisplay: string; ratingValue: number; reviewCount: number } {
  const roots = reviews.filter((review) => !review.parentId)
  if (roots.length === 0) {
    return { ratingDisplay: '—', ratingValue: 0, reviewCount: 0 }
  }

  const ratingValue = roots.reduce((sum, review) => sum + review.rating, 0) / roots.length

  return {
    ratingDisplay: formatProductRatingDisplay(ratingValue),
    ratingValue,
    reviewCount: roots.length,
  }
}

