export interface ProductComment {
  id: string
  parentId?: string | null
  productId: string
  userId: string
  description: string
  commentRate: number
  qualityRating: number
  affordableRating: number
  commentTopicId: string
  commentTopicTitle: string
  userName: string
  userFirstName?: string
  userLastName?: string
  createdOnUtc?: string
  isBuyer?: boolean
  helpfulCount?: number
  notHelpfulCount?: number
}

export interface CreateProductCommentInput {
  productId: string
  commentTopicId: string
  description: string
  commentRate: number
  qualityRating: number
  affordableRating: number
  parentId?: string | null
}

export interface VoteProductCommentResult {
  helpfulCount: number
  notHelpfulCount: number
  userVoteHelpful?: boolean | null
}
