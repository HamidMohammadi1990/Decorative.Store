import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { AuthCheckbox, AuthField } from '@/components/auth/AuthField'
import { PasswordField } from '@/components/auth/PasswordField'
import { Button } from '@/components/ui/Button'
import {
  hasErrors,
  validateSignupField,
  validateSignupForm,
  type SignupField,
  type SignupFormValues,
} from '@/extensions/validateAuthForm'
import { useUserStore } from '@/stores/userStore'

interface SignupFormProps {
  returnUrl?: string | null
  onSuccess?: () => void
  onSwitchToSignIn: () => void
}

export function SignupForm({
  returnUrl = null,
  onSuccess,
  onSwitchToSignIn,
}: SignupFormProps) {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const login = useUserStore((s) => s.login)
  const loginDemo = useUserStore((s) => s.loginDemo)
  const authLoading = useUserStore((s) => s.authLoading)
  const authError = useUserStore((s) => s.authError)
  const useMockAuth = import.meta.env.VITE_AUTH_USE_MOCK === 'true'
  const [values, setValues] = useState<SignupFormValues>({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
  })
  const [errors, setErrors] = useState<Partial<Record<SignupField, string>>>({})
  const [touched, setTouched] = useState<Partial<Record<SignupField, boolean>>>({})

  const updateField = (field: SignupField, value: string) => {
    setValues((prev) => ({ ...prev, [field]: value }))
    if (touched[field]) {
      const nextErrors = { ...errors }
      const message = validateSignupField(field, { ...values, [field]: value }, t)
      if (message) nextErrors[field] = message
      else delete nextErrors[field]
      setErrors(nextErrors)
    }
  }

  const handleBlur = (field: SignupField) => {
    setTouched((prev) => ({ ...prev, [field]: true }))
    const message = validateSignupField(field, values, t)
    setErrors((prev) => {
      const next = { ...prev }
      if (message) next[field] = message
      else delete next[field]
      return next
    })
  }

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault()
    const nextErrors = validateSignupForm(values, t)
    setErrors(nextErrors)
    setTouched({
      firstName: true,
      lastName: true,
      email: true,
      password: true,
    })
    if (hasErrors(nextErrors)) return

    try {
      if (useMockAuth) {
        loginDemo({
          email: values.email,
          firstName: values.firstName,
          lastName: values.lastName,
        })
      } else {
        await login({ email: values.email, password: values.password })
      }

      if (onSuccess) {
        onSuccess()
      } else {
        navigate(returnUrl ?? '/account/dashboard/wallet')
      }
    } catch {
      // Error state is stored in userStore.authError.
    }
  }

  return (
    <form className="space-y-5" noValidate onSubmit={handleSubmit}>
      <div className="grid gap-5 sm:grid-cols-2">
        <AuthField
          label={t('auth.firstNameLabel')}
          name="firstName"
          autoComplete="given-name"
          placeholder={t('auth.firstNamePlaceholder')}
          value={values.firstName}
          onChange={(e) => updateField('firstName', e.target.value)}
          onBlur={() => handleBlur('firstName')}
          error={touched.firstName ? errors.firstName : undefined}
        />
        <AuthField
          label={t('auth.lastNameLabel')}
          name="lastName"
          autoComplete="family-name"
          placeholder={t('auth.lastNamePlaceholder')}
          value={values.lastName}
          onChange={(e) => updateField('lastName', e.target.value)}
          onBlur={() => handleBlur('lastName')}
          error={touched.lastName ? errors.lastName : undefined}
        />
      </div>

      <AuthField
        label={t('auth.emailLabel')}
        name="email"
        type="email"
        autoComplete="email"
        placeholder={t('auth.emailPlaceholder')}
        value={values.email}
        onChange={(e) => updateField('email', e.target.value)}
        onBlur={() => handleBlur('email')}
        error={touched.email ? errors.email : undefined}
      />

      <PasswordField
        label={t('auth.passwordLabel')}
        name="password"
        autoComplete="new-password"
        placeholder={t('auth.createPasswordPlaceholder')}
        showLabel={t('auth.showPassword')}
        hideLabel={t('auth.hidePassword')}
        value={values.password}
        onChange={(e) => updateField('password', e.target.value)}
        onBlur={() => handleBlur('password')}
        error={touched.password ? errors.password : undefined}
      />

      <AuthCheckbox label={t('auth.marketingOptIn')} name="marketing" />

      <Button type="submit" variant="warm" className="w-full py-2.5" disabled={authLoading}>
        {authLoading ? t('auth.signingUp') : t('auth.signUpButton')}
      </Button>

      {authError && (
        <p className="text-center text-sm text-sale" role="alert">
          {t(authError)}
        </p>
      )}

      <p className="text-center text-sm text-text-muted">
        {t('auth.hasAccount')}{' '}
        <button
          type="button"
          className="font-semibold text-warm hover:underline"
          onClick={onSwitchToSignIn}
        >
          {t('auth.signInLink')}
        </button>
      </p>
    </form>
  )
}
