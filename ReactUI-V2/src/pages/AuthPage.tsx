import { useEffect, useState } from 'react'
import { Navigate, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { AuthTab } from '@/components/auth/AuthTab'
import { LoginForm } from '@/components/auth/LoginForm'
import { SignupForm } from '@/components/auth/SignupForm'
import { resolveReturnUrl } from '@/extensions/resolveReturnUrl'
import { useShopPageMeta } from '@/hooks/useShopPageMeta'
import { useUserStore } from '@/stores/userStore'

type AuthMode = 'signin' | 'signup'

export function AuthPage() {
  const { t } = useTranslation()
  const user = useUserStore((s) => s.user)
  const [searchParams, setSearchParams] = useSearchParams()
  const returnUrl = resolveReturnUrl(searchParams.get('returnUrl'))
  const initialMode = searchParams.get('mode') === 'signup' ? 'signup' : 'signin'
  const [mode, setMode] = useState<AuthMode>(initialMode)

  useShopPageMeta({
    title: mode === 'signup' ? t('auth.signUpTitle') : t('auth.signInTitle'),
    noindex: true,
    path: '/account',
  })

  useEffect(() => {
    setMode(searchParams.get('mode') === 'signup' ? 'signup' : 'signin')
  }, [searchParams])

  if (user) {
    return <Navigate to={returnUrl ?? '/account/dashboard/wallet'} replace />
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
              <LoginForm
                returnUrl={returnUrl}
                onSwitchToSignUp={() => switchMode('signup')}
              />
            ) : (
              <SignupForm
                returnUrl={returnUrl}
                onSwitchToSignIn={() => switchMode('signin')}
              />
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
