export interface ProductQuestion {
  id: string
  productId: string
  userId: string
  question: string
  answer?: string
  createdOnUtc: string
  userName: string
  userFirstName?: string
  userLastName?: string
  answeredByFirstName?: string
  answeredByLastName?: string
  answeredByUserName?: string
}

export interface CreateProductQuestionInput {
  productId: string
  question: string
}
