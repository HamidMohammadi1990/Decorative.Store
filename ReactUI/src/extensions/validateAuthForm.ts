import type { TFunction } from 'i18next'

export interface LoginFormValues {
  email: string
  password: string
}

export interface SignupFormValues {
  firstName: string
  lastName: string
  email: string
  password: string
}

export type LoginField = keyof LoginFormValues
export type SignupField = keyof SignupFormValues

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

function isBlank(value: string) {
  return value.trim().length === 0
}

function validateEmail(value: string, t: TFunction) {
  const trimmed = value.trim()
  if (isBlank(trimmed)) return t('auth.validation.required')
  if (!EMAIL_PATTERN.test(trimmed)) return t('auth.validation.emailInvalid')
  return undefined
}

function validatePassword(value: string, t: TFunction, minLength: number) {
  if (isBlank(value)) return t('auth.validation.required')
  if (value.length < minLength) {
    return t('auth.validation.passwordMin', { count: minLength })
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
    case 'email':
      return validateEmail(values.email, t)
    case 'password':
      return validatePassword(values.password, t, 6)
    default:
      return undefined
  }
}

export function validateLoginForm(values: LoginFormValues, t: TFunction) {
  const errors: Partial<Record<LoginField, string>> = {}

  for (const field of ['email', 'password'] as const) {
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
    case 'email':
      return validateEmail(values.email, t)
    case 'password':
      return validatePassword(values.password, t, 8)
    default:
      return undefined
  }
}

export function validateSignupForm(values: SignupFormValues, t: TFunction) {
  const errors: Partial<Record<SignupField, string>> = {}

  for (const field of ['firstName', 'lastName', 'email', 'password'] as const) {
    const message = validateSignupField(field, values, t)
    if (message) errors[field] = message
  }

  return errors
}

export function hasErrors(errors: object) {
  return Object.keys(errors).length > 0
}
