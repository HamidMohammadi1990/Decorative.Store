import type { DashboardUser } from '@/models/dashboard/dashboard.model'
import type { Locale } from '@/models/shared/locale.model'
import { resolveUserImageUrl } from '@/utils/resolveUserImageUrl'
import { apiGetAuth, apiPost, apiPostForm } from '@/services/api/apiClient'
import {
  readNumberField,
  readOptionalStringField,
  readStringField,
} from '@/services/api/apiNormalize'

const SIGN_IN_PATH = '/api/v1/account/sign-in'
const SIGN_UP_PATH = '/api/v1/account/sign-up'
const SIGN_OUT_PATH = '/api/v1/account/sign-out'
const CURRENT_USER_PATH = '/api/v1/account/me'
const PROFILE_IMAGE_PATH = '/api/v1/account/profile-image'
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
  profileImageUrl?: string | null
}

export interface RefreshTokenResponse {
  accessToken: string
  refreshToken: string
  tokenType: string
  expiresIn: number
  sessionId: string
}

function normalizeSignInResponse(data: unknown): SignInResponse {
  const record = data && typeof data === 'object' ? (data as Record<string, unknown>) : {}

  return {
    accessToken: readStringField(record, 'accessToken', 'AccessToken'),
    refreshToken: readStringField(record, 'refreshToken', 'RefreshToken'),
    tokenType: readStringField(record, 'tokenType', 'TokenType') || 'Bearer',
    expiresIn: readNumberField(record, 'expiresIn', 'ExpiresIn'),
    sessionId: readStringField(record, 'sessionId', 'SessionId'),
  }
}

function normalizeCurrentUserResponse(data: unknown): CurrentUserResponse | null {
  if (!data || typeof data !== 'object') return null

  const record = data as Record<string, unknown>
  const id = readStringField(record, 'id', 'Id')
  const userName = readStringField(record, 'userName', 'UserName')

  if (!id && !userName) return null

  return {
    id,
    userName,
    firstName: readOptionalStringField(record, 'firstName', 'FirstName'),
    lastName: readOptionalStringField(record, 'lastName', 'LastName'),
    email: readOptionalStringField(record, 'email', 'Email'),
    phoneNumber: readOptionalStringField(record, 'phoneNumber', 'PhoneNumber'),
    profileImageUrl: readOptionalStringField(record, 'profileImageUrl', 'ProfileImageUrl'),
  }
}

export function mapCurrentUserToDashboardUser(user: CurrentUserResponse): DashboardUser {
  const userName = user.userName.trim()
  const firstName = user.firstName?.trim() || userName.split('@')[0] || userName
  const lastName = user.lastName?.trim() || ''

  return {
    id: user.id,
    firstName,
    lastName,
    email: user.email?.trim() || (userName.includes('@') ? userName : user.phoneNumber?.trim() || userName),
    memberSince: new Date().toISOString().slice(0, 10),
    profileImageUrl: user.profileImageUrl
      ? resolveUserImageUrl(user.profileImageUrl)
      : undefined,
  }
}

export function createFallbackDashboardUser(userName: string, firstName?: string, lastName?: string): DashboardUser {
  const normalizedUserName = userName.trim()
  const resolvedFirstName = firstName?.trim() || normalizedUserName.split('@')[0] || normalizedUserName

  return {
    id: normalizedUserName,
    firstName: resolvedFirstName,
    lastName: lastName?.trim() || '',
    email: normalizedUserName.includes('@') ? normalizedUserName : normalizedUserName,
    memberSince: new Date().toISOString().slice(0, 10),
  }
}

export interface SignUpInput {
  userName: string
  password: string
  firstName: string
  lastName: string
}

export const authService = {
  async signIn(userName: string, password: string): Promise<SignInResponse> {
    const data = await apiPost<unknown>(SIGN_IN_PATH, { userName, password })
    const tokens = normalizeSignInResponse(data)

    if (!tokens.accessToken) {
      throw new Error('Sign-in response did not include an access token.')
    }

    return tokens
  },

  async signUp(input: SignUpInput): Promise<SignInResponse> {
    const data = await apiPost<unknown>(SIGN_UP_PATH, {
      userName: input.userName.trim(),
      password: input.password,
      firstName: input.firstName.trim(),
      lastName: input.lastName.trim(),
    })
    const tokens = normalizeSignInResponse(data)

    if (!tokens.accessToken) {
      throw new Error('Sign-up response did not include an access token.')
    }

    return tokens
  },

  async getCurrentUser(accessToken: string): Promise<CurrentUserResponse | null> {
    const data = await apiGetAuth<unknown>(CURRENT_USER_PATH, accessToken)
    return normalizeCurrentUserResponse(data)
  },

  async refreshToken(token: string, refreshToken: string): Promise<RefreshTokenResponse> {
    const data = await apiPost<unknown>(REFRESH_TOKEN_PATH, { token, refreshToken })
    return normalizeSignInResponse(data)
  },

  async signOut(accessToken: string): Promise<boolean> {
    return apiGetAuth<boolean>(SIGN_OUT_PATH, accessToken)
  },

  async uploadProfileImage(
    accessToken: string,
    locale: Locale,
    file: File,
  ): Promise<{ imageFileName: string; imageUrl: string }> {
    const formData = new FormData()
    formData.append('image', file)

    const data = await apiPostForm<unknown>(PROFILE_IMAGE_PATH, formData, {
      locale,
      accessToken,
    })

    const record = data && typeof data === 'object' ? (data as Record<string, unknown>) : {}
    const imageFileName = readStringField(record, 'imageFileName', 'ImageFileName')
    const imageUrl = readStringField(record, 'imageUrl', 'ImageUrl')
    if (!imageFileName || !imageUrl) {
      throw new Error('Profile image upload response was incomplete.')
    }

    return {
      imageFileName,
      imageUrl: resolveUserImageUrl(imageUrl),
    }
  },
}
