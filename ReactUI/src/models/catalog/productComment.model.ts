export interface ProductComment {
  id: string
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
}

export interface CreateProductCommentInput {
  productId: string
  commentTopicId: string
  description: string
  commentRate: number
  qualityRating: number
  affordableRating: number
}
