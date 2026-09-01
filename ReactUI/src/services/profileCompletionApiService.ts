import { apiDelete, apiPost, apiPut } from '@/services/api/apiClient'
import { normalizeAdminPaged } from '@/services/admin/adminCatalogNormalize'
import type { AdminProfileCompletionConfig } from '@/models/profile/profileCompletionAdmin.model'

const ADMIN_BASE = '/api/v1/admin/profile-completion'

function parseAdminConfig(configJson: string): AdminProfileCompletionConfig | null {
  try {
    const parsed = JSON.parse(configJson) as AdminProfileCompletionConfig
    if (!parsed || typeof parsed !== 'object' || !Array.isArray(parsed.questions)) return null
    return parsed
  } catch {
    return null
  }
}

function readConfigJson(data: { configJson?: string; ConfigJson?: string } | null): string | null {
  if (!data) return null
  return data.configJson ?? data.ConfigJson ?? null
}

export const adminProfileCompletionService = {
  async getConfig(accessToken: string): Promise<AdminProfileCompletionConfig | null> {
    const data = await apiPost<{ configJson?: string; ConfigJson?: string }>(
      `${ADMIN_BASE}/get-config`,
      {},
      { accessToken },
    )
    const configJson = readConfigJson(data)
    if (!configJson) return null
    return parseAdminConfig(configJson)
  },

  async saveConfig(accessToken: string, config: AdminProfileCompletionConfig): Promise<void> {
    await apiPut(
      `${ADMIN_BASE}/update-config`,
      { configJson: JSON.stringify(config) },
      { accessToken },
    )
  },

  async getUserStates(
    accessToken: string,
    options: {
      pageNumber?: number
      pageSize?: number
      userSearch?: string | null
      rewardClaimed?: boolean | null
    } = {},
  ) {
    const data = await apiPost<unknown>(
      `${ADMIN_BASE}/user-states/get-all`,
      {
        userSearch: options.userSearch ?? null,
        rewardClaimed: options.rewardClaimed ?? null,
        pagination: {
          pageNumber: options.pageNumber ?? 1,
          pageSize: options.pageSize ?? 20,
        },
      },
      { accessToken },
    )

    return normalizeAdminPaged(data, normalizeUserState)
  },

  async getUserState(accessToken: string, id: number): Promise<AdminProfileCompletionUserState | null> {
    const data = await apiPost<unknown>(
      `${ADMIN_BASE}/user-states/get`,
      { id },
      { accessToken },
    )
    return normalizeUserState(data)
  },

  async updateUserState(
    accessToken: string,
    payload: { id: number; answersJson?: string; clearRewardClaim?: boolean },
  ): Promise<void> {
    await apiPut(
      `${ADMIN_BASE}/user-states/update`,
      {
        id: payload.id,
        answersJson: payload.answersJson ?? null,
        clearRewardClaim: payload.clearRewardClaim ?? false,
      },
      { accessToken },
    )
  },

  async deleteUserState(accessToken: string, id: number): Promise<void> {
    await apiDelete(`${ADMIN_BASE}/user-states/delete`, accessToken, { id })
  },
}

export interface AdminProfileCompletionUserState {
  id: number
  userId: number
  userName: string
  firstName: string | null
  lastName: string | null
  email: string | null
  answersJson: string
  updatedOnUtc: string
  rewardClaimedOnUtc: string | null
}

function normalizeUserState(data: unknown): AdminProfileCompletionUserState | null {
  if (!data || typeof data !== 'object') return null
  const record = data as Record<string, unknown>

  const id = Number(record.id ?? record.Id ?? 0)
  if (!id) return null

  return {
    id,
    userId: Number(record.userId ?? record.UserId ?? 0),
    userName: String(record.userName ?? record.UserName ?? ''),
    firstName:
      typeof record.firstName === 'string'
        ? record.firstName
        : typeof record.FirstName === 'string'
          ? record.FirstName
          : null,
    lastName:
      typeof record.lastName === 'string'
        ? record.lastName
        : typeof record.LastName === 'string'
          ? record.LastName
          : null,
    email:
      typeof record.email === 'string'
        ? record.email
        : typeof record.Email === 'string'
          ? record.Email
          : null,
    answersJson: String(record.answersJson ?? record.AnswersJson ?? '{}'),
    updatedOnUtc: String(record.updatedOnUtc ?? record.UpdatedOnUtc ?? ''),
    rewardClaimedOnUtc:
      record.rewardClaimedOnUtc != null || record.RewardClaimedOnUtc != null
        ? String(record.rewardClaimedOnUtc ?? record.RewardClaimedOnUtc)
        : null,
  }
}

export const profileCompletionApiService = {
  async getConfig(): Promise<AdminProfileCompletionConfig | null> {
    const data = await apiPost<{ configJson?: string; ConfigJson?: string }>(
      '/api/v1/profile-completion/config',
      {},
    )
    const configJson = readConfigJson(data)
    if (!configJson) return null
    return parseAdminConfig(configJson)
  },

  async getMyState(accessToken: string): Promise<{
    answers: Record<string, string | string[]>
    rewardClaimedAt: string | null
  }> {
    const data = await apiPost<{
      answersJson?: string
      AnswersJson?: string
      rewardClaimedOnUtc?: string | null
      RewardClaimedOnUtc?: string | null
    }>('/api/v1/profile-completion/my-state', {}, { accessToken })

    const answersJson = data.answersJson ?? data.AnswersJson ?? '{}'
    const rewardClaimedOnUtc = data.rewardClaimedOnUtc ?? data.RewardClaimedOnUtc ?? null

    try {
      const answers = JSON.parse(answersJson) as Record<string, string | string[]>
      return {
        answers: answers && typeof answers === 'object' ? answers : {},
        rewardClaimedAt: rewardClaimedOnUtc,
      }
    } catch {
      return { answers: {}, rewardClaimedAt: null }
    }
  },

  async saveAnswers(
    accessToken: string,
    answers: Record<string, string | string[]>,
  ): Promise<void> {
    await apiPut(
      '/api/v1/profile-completion/my-answers',
      { answersJson: JSON.stringify(answers) },
      { accessToken },
    )
  },

  async claimReward(accessToken: string): Promise<string> {
    const data = await apiPost<{ claimedOnUtc?: string; ClaimedOnUtc?: string }>(
      '/api/v1/profile-completion/claim-reward',
      {},
      { accessToken },
    )
    return data.claimedOnUtc ?? data.ClaimedOnUtc ?? new Date().toISOString()
  },
}
