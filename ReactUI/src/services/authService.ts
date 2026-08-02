import type { DashboardUser } from '@/models/dashboard/dashboard.model'
import { apiGetAuth, apiPost } from '@/services/api/apiClient'

const SIGN_IN_PATH = '/api/v1/account/sign-in'
const SIGN_OUT_PATH = '/api/v1/account/sign-out'
const CURRENT_USER_PATH = '/api/v1/account/me'
const REFRESH_TOKEN_PATH = '/api/v1/account/refresh-token'

export interface SignInResponse {
  accessToken: string
  refreshToken: string
  tokenType: string
  expiresIn: number
  sessionId: string
}

export interface CurrentUserResponse {
  id: string
  userName: string
  firstName?: string | null
  lastName?: string | null
  email?: string | null
  phoneNumber?: string | null
}

export interface RefreshTokenResponse {
  accessToken: string
  refreshToken: string
  tokenType: string
  expiresIn: number
  sessionId: string
}

export function mapCurrentUserToDashboardUser(user: CurrentUserResponse): DashboardUser {
  const firstName = user.firstName?.trim() || user.userName
  const lastName = user.lastName?.trim() || ''

  return {
    id: user.id,
    firstName,
    lastName,
    email: user.email?.trim() || user.userName,
    memberSince: new Date().toISOString().slice(0, 10),
  }
}

export const authService = {
  async signIn(userName: string, password: string): Promise<SignInResponse> {
    return apiPost<SignInResponse>(SIGN_IN_PATH, { userName, password })
  },

  async getCurrentUser(accessToken: string): Promise<CurrentUserResponse> {
    return apiGetAuth<CurrentUserResponse>(CURRENT_USER_PATH, accessToken)
  },

  async refreshToken(token: string, refreshToken: string): Promise<RefreshTokenResponse> {
    return apiPost<RefreshTokenResponse>(REFRESH_TOKEN_PATH, { token, refreshToken })
  },

  async signOut(accessToken: string): Promise<boolean> {
    return apiGetAuth<boolean>(SIGN_OUT_PATH, accessToken)
  },
}
