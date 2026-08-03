import { useState, type FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { AuthCheckbox, AuthField } from '@/components/auth/AuthField'
import { PasswordField } from '@/components/auth/PasswordField'
import { Button } from '@/components/ui/Button'
import {
  hasErrors,
  validateLoginField,
  validateLoginForm,
  type LoginField,
  type LoginFormValues,
} from '@/extensions/validateAuthForm'
import { useUserStore } from '@/stores/userStore'

interface LoginFormProps {
  returnUrl?: string | null
  onSuccess?: () => void
  onSwitchToSignUp: () => void
  forgotPasswordHref?: string
}

export function LoginForm({
  returnUrl = null,
  onSuccess,
  onSwitchToSignUp,
  forgotPasswordHref = '/account/forgot-password',
}: LoginFormProps) {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const login = useUserStore((s) => s.login)
  const authLoading = useUserStore((s) => s.authLoading)
  const authError = useUserStore((s) => s.authError)
  const clearAuthError = useUserStore((s) => s.clearAuthError)
  const [values, setValues] = useState<LoginFormValues>({ email: '', password: '' })
  const [errors, setErrors] = useState<Partial<Record<LoginField, string>>>({})
  const [touched, setTouched] = useState<Partial<Record<LoginField, boolean>>>({})

  const updateField = (field: LoginField, value: string) => {
    setValues((prev) => ({ ...prev, [field]: value }))
    if (touched[field]) {
      const nextErrors = { ...errors }
      const message = validateLoginField(field, { ...values, [field]: value }, t)
      if (message) nextErrors[field] = message
      else delete nextErrors[field]
      setErrors(nextErrors)
    }
  }

  const handleBlur = (field: LoginField) => {
    setTouched((prev) => ({ ...prev, [field]: true }))
    const message = validateLoginField(field, values, t)
    setErrors((prev) => {
      const next = { ...prev }
      if (message) next[field] = message
      else delete next[field]
      return next
    })
  }

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault()
    const nextErrors = validateLoginForm(values, t)
    setErrors(nextErrors)
    setTouched({ email: true, password: true })
    if (hasErrors(nextErrors)) return

    try {
      await login({ email: values.email, password: values.password })
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
      <AuthField
        label={t('auth.emailLabel')}
        name="email"
        type="email"
        autoComplete="email"
        placeholder={t('auth.emailPlaceholder')}
        value={values.email}
        onChange={(e) => {
          clearAuthError()
          updateField('email', e.target.value)
        }}
        onBlur={() => handleBlur('email')}
        error={touched.email ? errors.email : undefined}
      />

      <PasswordField
        label={t('auth.passwordLabel')}
        name="password"
        autoComplete="current-password"
        placeholder={t('auth.passwordPlaceholder')}
        showLabel={t('auth.showPassword')}
        hideLabel={t('auth.hidePassword')}
        value={values.password}
        onChange={(e) => {
          clearAuthError()
          updateField('password', e.target.value)
        }}
        onBlur={() => handleBlur('password')}
        error={touched.password ? errors.password : undefined}
      />

      <div className="flex flex-wrap items-center justify-between gap-3">
        <AuthCheckbox label={t('auth.rememberMe')} name="remember" />
        <Link to={forgotPasswordHref} className="text-sm font-medium text-warm hover:underline">
          {t('auth.forgotPassword')}
        </Link>
      </div>

      <Button type="submit" variant="warm" className="w-full py-2.5" disabled={authLoading}>
        {authLoading ? t('auth.signingIn') : t('auth.signInButton')}
      </Button>

      {authError && (
        <p className="text-center text-sm text-sale" role="alert">
          {t(authError)}
        </p>
      )}

      <p className="text-center text-sm text-text-muted">
        {t('auth.noAccount')}{' '}
        <button
          type="button"
          className="font-semibold text-warm hover:underline"
          onClick={onSwitchToSignUp}
        >
          {t('auth.createAccountLink')}
        </button>
      </p>
    </form>
  )
}
