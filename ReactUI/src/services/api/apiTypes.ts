export interface ApiMessage {
  code: string
  message: string
}

export interface ApiResult<T> {
  isSuccess: boolean
  statusCode: number
  data?: T
  messages?: ApiMessage[]
}

export class ApiError extends Error {
  readonly statusCode: number
  readonly messages: ApiMessage[]

  constructor(statusCode: number, messages: ApiMessage[] = []) {
    const primary = messages[0]
    super(primary?.message ?? `API request failed with status ${statusCode}`)
    this.name = 'ApiError'
    this.statusCode = statusCode
    this.messages = messages
  }

  hasCode(code: string) {
    return this.messages.some((message) => message.code === code)
  }
}
