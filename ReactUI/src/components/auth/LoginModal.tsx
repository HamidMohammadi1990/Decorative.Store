import { useEffect } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { AuthTab } from '@/components/auth/AuthTab'
import { LoginForm } from '@/components/auth/LoginForm'
import { SignupForm } from '@/components/auth/SignupForm'
import { Button } from '@/components/ui/Button'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'
import { useAuthModalStore } from '@/stores/authModalStore'
import { useUserStore } from '@/stores/userStore'

export function LoginModal() {
  const { t } = useTranslation()
  const isOpen = useAuthModalStore((s) => s.isOpen)
  const mode = useAuthModalStore((s) => s.mode)
  const closeModal = useAuthModalStore((s) => s.closeModal)
  const setMode = useAuthModalStore((s) => s.setMode)
  const clearAuthError = useUserStore((s) => s.clearAuthError)

  useEffect(() => {
    if (!isOpen) return

    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') closeModal()
    }

    document.addEventListener('keydown', handleKeyDown)
    return () => document.removeEventListener('keydown', handleKeyDown)
  }, [closeModal, isOpen])

  useEffect(() => {
    if (!isOpen) {
      clearAuthError()
    }
  }, [clearAuthError, isOpen])

  if (!isOpen) return null

  const handleAuthSuccess = () => {
    closeModal()
    window.location.reload()
  }

  return (
    <Portal>
      <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 sm:p-6">
        <button
          type="button"
          aria-label={t('common.close')}
          className="absolute inset-0 bg-black/40"
          onClick={closeModal}
        />
        <div
          role="dialog"
          aria-modal="true"
          aria-label={t('auth.modalTitle')}
          className="relative z-10 flex max-h-[min(90vh,44rem)] w-full max-w-md flex-col overflow-hidden rounded-sm bg-surface shadow-2xl"
        >
          <header className="flex items-start justify-between border-b border-border px-5 py-4">
            <div>
              <h2 className="text-lg font-semibold text-text">
                {mode === 'signin' ? t('auth.signInTitle') : t('auth.signUpTitle')}
              </h2>
              <p className="mt-1 text-sm text-text-muted">
                {mode === 'signin' ? t('auth.signInSubtitle') : t('auth.signUpSubtitle')}
              </p>
            </div>
            <button
              type="button"
              onClick={closeModal}
              aria-label={t('common.close')}
              className="flex size-9 shrink-0 items-center justify-center rounded-full text-text-muted hover:bg-surface-muted"
            >
              <CloseIcon />
            </button>
          </header>

          <div className="overflow-y-auto px-5 py-5">
            <div
              role="tablist"
              aria-label={t('auth.tabLabel')}
              className="mb-6 inline-flex w-full rounded-full border border-warm-muted bg-warm-soft/60 p-1"
            >
              <AuthTab
                active={mode === 'signin'}
                onClick={() => setMode('signin')}
                label={t('auth.signInTab')}
              />
              <AuthTab
                active={mode === 'signup'}
                onClick={() => setMode('signup')}
                label={t('auth.signUpTab')}
              />
            </div>

            {mode === 'signin' ? (
              <LoginForm
                onSuccess={handleAuthSuccess}
                onSwitchToSignUp={() => setMode('signup')}
              />
            ) : (
              <SignupForm
                onSuccess={handleAuthSuccess}
                onSwitchToSignIn={() => setMode('signin')}
              />
            )}

            <div className="mt-6 border-t border-border pt-4 text-center">
              <Link
                to={mode === 'signup' ? '/account?mode=signup' : '/account'}
                className="text-sm font-medium text-warm hover:underline"
                onClick={closeModal}
              >
                {mode === 'signup' ? t('auth.openFullSignUpPage') : t('auth.openFullSignInPage')}
              </Link>
            </div>
          </div>
        </div>
      </div>
    </Portal>
  )
}

interface AuthGateProps {
  message?: string
}

export function AuthGate({ message }: AuthGateProps) {
  const { t } = useTranslation()
  const openModal = useAuthModalStore((s) => s.openModal)

  return (
    <div className="flex min-h-[40vh] flex-col items-center justify-center px-4 py-16 text-center">
      <p className="max-w-md text-sm text-text-muted">{message ?? t('auth.gateMessage')}</p>
      <Button variant="warm" className="mt-5 min-w-40" onClick={() => openModal({ mode: 'signin' })}>
        {t('auth.signInButton')}
      </Button>
    </div>
  )
}
