import type { ApiMessage } from '@/services/api/apiTypes'

type ApiEnvelope<T> = {
  isSuccess: boolean
  data: T | undefined
  messages: ApiMessage[]
}

function readRecord(value: unknown): Record<string, unknown> | null {
  return value && typeof value === 'object' ? (value as Record<string, unknown>) : null
}

export { readRecord }

function readMessages(raw: unknown): ApiMessage[] {
  if (!Array.isArray(raw)) return []

  return raw
    .map((item) => readRecord(item))
    .filter((item): item is Record<string, unknown> => item !== null)
    .map((item) => ({
      code: String(item.code ?? item.Code ?? ''),
      message: String(item.message ?? item.Message ?? ''),
    }))
    .filter((item) => item.code.length > 0 || item.message.length > 0)
}

export function normalizeApiEnvelope<T>(body: unknown): ApiEnvelope<T> {
  const record = readRecord(body)
  if (!record) {
    return { isSuccess: false, data: undefined, messages: [] }
  }

  const isSuccess = Boolean(record.isSuccess ?? record.IsSuccess)
  const data = (record.data ?? record.Data) as T | undefined
  const messages = readMessages(record.messages ?? record.Messages)

  return { isSuccess, data, messages }
}

export function readStringField(record: Record<string, unknown>, ...keys: string[]) {
  for (const key of keys) {
    const value = record[key]
    if (typeof value === 'string' && value.length > 0) return value
    if (typeof value === 'number' && Number.isFinite(value)) return String(value)
  }
  return ''
}

export function readOptionalStringField(record: Record<string, unknown>, ...keys: string[]) {
  for (const key of keys) {
    const value = record[key]
    if (typeof value === 'string') return value
    if (value === null) return null
  }
  return undefined
}

export function readNumberField(record: Record<string, unknown>, ...keys: string[]) {
  for (const key of keys) {
    const value = record[key]
    if (typeof value === 'number' && Number.isFinite(value)) return value
    if (typeof value === 'string' && value.trim().length > 0) {
      const parsed = Number(value)
      if (Number.isFinite(parsed)) return parsed
    }
  }
  return 0
}

export function readOptionalNumberField(
  record: Record<string, unknown>,
  ...keys: string[]
): number | undefined {
  for (const key of keys) {
    const value = record[key]
    if (value === null || value === undefined) continue
    if (typeof value === 'number' && Number.isFinite(value)) return value
    if (typeof value === 'string' && value.trim().length > 0) {
      const parsed = Number(value)
      if (Number.isFinite(parsed)) return parsed
    }
  }
  return undefined
}

export function readBooleanField(record: Record<string, unknown>, ...keys: string[]) {
  for (const key of keys) {
    const value = record[key]
    if (typeof value === 'boolean') return value
  }
  return false
}
