import { useEffect, useState, type FormEvent } from 'react'
import { Navigate, useNavigate, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { AuthCheckbox, AuthField } from '@/components/auth/AuthField'
import { PasswordField } from '@/components/auth/PasswordField'
import { Button } from '@/components/ui/Button'
import {
  hasErrors,
  validateLoginField,
  validateLoginForm,
  validateSignupField,
  validateSignupForm,
  type LoginField,
  type LoginFormValues,
  type SignupField,
  type SignupFormValues,
} from '@/extensions/validateAuthForm'
import { useUserStore } from '@/stores/userStore'

type AuthMode = 'signin' | 'signup'

export function AuthPage() {
  const { t } = useTranslation()
  const user = useUserStore((s) => s.user)
  const [searchParams, setSearchParams] = useSearchParams()
  const initialMode = searchParams.get('mode') === 'signup' ? 'signup' : 'signin'
  const [mode, setMode] = useState<AuthMode>(initialMode)

  useEffect(() => {
    setMode(searchParams.get('mode') === 'signup' ? 'signup' : 'signin')
  }, [searchParams])

  if (user) {
    return <Navigate to="/account/dashboard/wallet" replace />
  }

  const switchMode = (next: AuthMode) => {
    setMode(next)
    setSearchParams(next === 'signup' ? { mode: 'signup' } : {}, { replace: true })
  }

  return (
    <div className="flex flex-1">
      <aside className="relative hidden overflow-hidden bg-warm lg:flex lg:w-[44%] xl:w-[42%]">
        <div
          className="absolute inset-0 opacity-20"
          style={{
            backgroundImage:
              'radial-gradient(circle at 20% 80%, #fffaf5 0%, transparent 45%), radial-gradient(circle at 80% 20%, #826038 0%, transparent 40%)',
          }}
        />
        <div className="relative flex flex-col justify-between p-10 xl:p-14">
          <div>
            <p className="text-xs font-semibold tracking-[0.25em] text-warm-text/80 uppercase">
              {t('common.brandName')}
            </p>
            <h1 className="mt-6 max-w-sm text-3xl leading-tight font-semibold text-warm-text xl:text-4xl">
              {mode === 'signin' ? t('auth.heroSignInTitle') : t('auth.heroSignUpTitle')}
            </h1>
            <p className="mt-4 max-w-sm text-sm leading-relaxed text-warm-text/85">
              {mode === 'signin' ? t('auth.heroSignInText') : t('auth.heroSignUpText')}
            </p>
          </div>

          <ul className="space-y-3 text-sm text-warm-text/90">
            <BenefitItem text={t('auth.benefitOrders')} />
            <BenefitItem text={t('auth.benefitWishlist')} />
            <BenefitItem text={t('auth.benefitOffers')} />
          </ul>
        </div>
      </aside>

      <div className="flex flex-1 items-center justify-center px-4 py-10 sm:px-6 lg:px-10">
        <div className="w-full max-w-md">
          <div className="mb-8 text-center lg:text-start">
            <h2 className="text-2xl font-semibold text-text">
              {mode === 'signin' ? t('auth.signInTitle') : t('auth.signUpTitle')}
            </h2>
            <p className="mt-2 text-sm text-text-muted">
              {mode === 'signin' ? t('auth.signInSubtitle') : t('auth.signUpSubtitle')}
            </p>
          </div>

          <div
            role="tablist"
            aria-label={t('auth.tabLabel')}
            className="mb-8 inline-flex w-full rounded-full border border-warm-muted bg-warm-soft/60 p-1"
          >
            <AuthTab
              active={mode === 'signin'}
              onClick={() => switchMode('signin')}
              label={t('auth.signInTab')}
            />
            <AuthTab
              active={mode === 'signup'}
              onClick={() => switchMode('signup')}
              label={t('auth.signUpTab')}
            />
          </div>

          <div className="rounded-lg border border-border bg-surface p-6 shadow-sm sm:p-8">
            {mode === 'signin' ? (
              <LoginForm onSwitchToSignUp={() => switchMode('signup')} />
            ) : (
              <SignupForm onSwitchToSignIn={() => switchMode('signin')} />
            )}
          </div>

          <p className="mt-6 text-center text-xs leading-relaxed text-text-muted lg:text-start">
            {t('auth.termsPrefix')}{' '}
            <a href="/terms" className="font-medium text-warm hover:underline">
              {t('auth.termsLink')}
            </a>{' '}
            {t('auth.termsAnd')}{' '}
            <a href="/privacy" className="font-medium text-warm hover:underline">
              {t('auth.privacyLink')}
            </a>
            .
          </p>
        </div>
      </div>
    </div>
  )
}

function AuthTab({
  active,
  onClick,
  label,
}: {
  active: boolean
  onClick: () => void
  label: string
}) {
  return (
    <button
      type="button"
      role="tab"
      aria-selected={active}
      onClick={onClick}
      className={`flex-1 rounded-full px-4 py-2.5 text-sm font-semibold transition-all duration-200 ${
        active
          ? 'bg-warm text-warm-text shadow-sm'
          : 'text-text-muted hover:text-warm'
      }`}
    >
      {label}
    </button>
  )
}

function BenefitItem({ text }: { text: string }) {
  return (
    <li className="flex items-center gap-2.5">
      <span className="flex size-5 shrink-0 items-center justify-center rounded-full bg-warm-text/15">
        <svg width="10" height="10" viewBox="0 0 10 10" aria-hidden>
          <path
            d="M2 5.2 4.1 7.3 8 3.4"
            stroke="currentColor"
            strokeWidth="1.4"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
        </svg>
      </span>
      {text}
    </li>
  )
}

function LoginForm({ onSwitchToSignUp }: { onSwitchToSignUp: () => void }) {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const login = useUserStore((s) => s.login)
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

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault()
    const nextErrors = validateLoginForm(values, t)
    setErrors(nextErrors)
    setTouched({ email: true, password: true })
    if (hasErrors(nextErrors)) return
    login({ email: values.email })
    navigate('/account/dashboard/wallet')
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
        onChange={(e) => updateField('email', e.target.value)}
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
        onChange={(e) => updateField('password', e.target.value)}
        onBlur={() => handleBlur('password')}
        error={touched.password ? errors.password : undefined}
      />

      <div className="flex flex-wrap items-center justify-between gap-3">
        <AuthCheckbox label={t('auth.rememberMe')} name="remember" />
        <a href="/account/forgot-password" className="text-sm font-medium text-warm hover:underline">
          {t('auth.forgotPassword')}
        </a>
      </div>

      <Button type="submit" variant="warm" className="w-full py-2.5">
        {t('auth.signInButton')}
      </Button>

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

function SignupForm({ onSwitchToSignIn }: { onSwitchToSignIn: () => void }) {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const login = useUserStore((s) => s.login)
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

  const handleSubmit = (e: FormEvent) => {
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
    login({
      email: values.email,
      firstName: values.firstName,
      lastName: values.lastName,
    })
    navigate('/account/dashboard/wallet')
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

      <Button type="submit" variant="warm" className="w-full py-2.5">
        {t('auth.signUpButton')}
      </Button>

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
