import type { TFunction } from 'i18next'

export interface LoginFormValues {
  userName: string
  password: string
}

export interface SignupFormValues {
  firstName: string
  lastName: string
  userName: string
  password: string
}

export type LoginField = keyof LoginFormValues
export type SignupField = keyof SignupFormValues

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
const MOBILE_PATTERN = /^(\+98|0098|98|0)?9\d{9}$/
const PASSWORD_MIN_LENGTH = 5

function isBlank(value: string) {
  return value.trim().length === 0
}

export function isValidAuthUserName(value: string) {
  const trimmed = value.trim()
  return EMAIL_PATTERN.test(trimmed) || MOBILE_PATTERN.test(trimmed)
}

function validateUserName(value: string, t: TFunction) {
  const trimmed = value.trim()
  if (isBlank(trimmed)) return t('auth.validation.required')
  if (!isValidAuthUserName(trimmed)) return t('auth.validation.userNameInvalid')
  return undefined
}

function validatePassword(value: string, t: TFunction) {
  if (isBlank(value)) return t('auth.validation.required')
  if (value.length < PASSWORD_MIN_LENGTH) {
    return t('auth.validation.passwordMin', { count: PASSWORD_MIN_LENGTH })
  }
  return undefined
}

function validateName(value: string, t: TFunction) {
  const trimmed = value.trim()
  if (isBlank(trimmed)) return t('auth.validation.required')
  if (trimmed.length < 2) return t('auth.validation.nameMin')
  return undefined
}

export function validateLoginField(
  field: LoginField,
  values: LoginFormValues,
  t: TFunction,
): string | undefined {
  switch (field) {
    case 'userName':
      return validateUserName(values.userName, t)
    case 'password':
      return validatePassword(values.password, t)
    default:
      return undefined
  }
}

export function validateLoginForm(values: LoginFormValues, t: TFunction) {
  const errors: Partial<Record<LoginField, string>> = {}

  for (const field of ['userName', 'password'] as const) {
    const message = validateLoginField(field, values, t)
    if (message) errors[field] = message
  }

  return errors
}

export function validateSignupField(
  field: SignupField,
  values: SignupFormValues,
  t: TFunction,
): string | undefined {
  switch (field) {
    case 'firstName':
      return validateName(values.firstName, t)
    case 'lastName':
      return validateName(values.lastName, t)
    case 'userName':
      return validateUserName(values.userName, t)
    case 'password':
      return validatePassword(values.password, t)
    default:
      return undefined
  }
}

export function validateSignupForm(values: SignupFormValues, t: TFunction) {
  const errors: Partial<Record<SignupField, string>> = {}

  for (const field of ['firstName', 'lastName', 'userName', 'password'] as const) {
    const message = validateSignupField(field, values, t)
    if (message) errors[field] = message
  }

  return errors
}

export function hasErrors(errors: object) {
  return Object.keys(errors).length > 0
}
