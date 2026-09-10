import { useState } from 'react'
import { useTranslation } from 'react-i18next'
import { AuthField } from '@/components/auth/AuthField'
import { Button } from '@/components/ui/Button'
import { openLoginModal } from '@/stores/authModalStore'

interface CheckoutDiscountFieldProps {
  appliedCode: string | null
  isDiscountApplied?: boolean
  applying: boolean
  errorKey: string | null
  canApply: boolean
  onApply: (code: string) => Promise<boolean>
  onRemove: () => Promise<boolean>
  compact?: boolean
}

export function CheckoutDiscountField({
  appliedCode,
  isDiscountApplied = false,
  applying,
  errorKey,
  canApply,
  onApply,
  onRemove,
  compact = false,
}: CheckoutDiscountFieldProps) {
  const { t } = useTranslation()
  const [code, setCode] = useState(appliedCode ?? '')
  const [localError, setLocalError] = useState<string | null>(null)

  const handleApply = async () => {
    const trimmed = code.trim()
    if (!trimmed) {
      setLocalError(t('checkout.discountRequired'))
      return
    }

    if (!canApply) {
      openLoginModal({
        onSuccess: () => {
          void onApply(trimmed)
        },
      })
      return
    }

    setLocalError(null)
    const success = await onApply(trimmed)
    if (success) {
      setCode(trimmed)
    }
  }

  const handleRemove = async () => {
    const success = await onRemove()
    if (success) {
      setCode('')
      setLocalError(null)
    }
  }

  const displayError = localError ?? (errorKey ? t(errorKey, { defaultValue: t('checkout.discountErrors.generic') }) : null)

  if (appliedCode || isDiscountApplied) {
    return (
      <div
        className={`rounded-sm border border-accent/30 bg-accent/5 ${
          compact ? 'px-4 py-3' : 'px-5 py-4'
        }`}
      >
        <div className="flex flex-wrap items-center justify-between gap-2">
          <div>
            <p className="text-xs font-medium uppercase tracking-wide text-text-muted">
              {t('checkout.discountApplied')}
            </p>
            <p className="mt-0.5 font-mono text-sm font-semibold text-accent" dir="ltr">
              {appliedCode ?? t('checkout.discountActive')}
            </p>
          </div>
          <Button
            type="button"
            variant="ghost"
            className="text-xs text-text-muted hover:text-sale"
            disabled={applying}
            onClick={() => void handleRemove()}
          >
            {t('checkout.removeDiscount')}
          </Button>
        </div>
      </div>
    )
  }

  return (
    <div className={compact ? 'px-4 py-3' : 'px-5 py-4'}>
      <p className={`font-medium text-text ${compact ? 'text-xs' : 'text-sm'}`}>
        {t('checkout.discountTitle')}
      </p>
      {!canApply && (
        <p className="mt-1 text-xs text-text-muted">{t('checkout.discountSignInHint')}</p>
      )}
      <div className="mt-2 flex flex-col gap-2 sm:flex-row sm:items-start">
        <div className="min-w-0 flex-1">
          <AuthField
            name="discountCode"
            label={t('checkout.discountCodeLabel')}
            placeholder={t('checkout.discountCodePlaceholder')}
            value={code}
            onChange={(e) => {
              setCode(e.target.value)
              if (localError) setLocalError(null)
            }}
            error={displayError ?? undefined}
            autoComplete="off"
            dir="ltr"
          />
        </div>
        <Button
          type="button"
          variant="secondary"
          className={`shrink-0 sm:mt-6 ${compact ? 'py-2 text-xs' : ''}`}
          disabled={applying}
          onClick={() => void handleApply()}
        >
          {applying ? t('checkout.applyingDiscount') : t('checkout.applyDiscount')}
        </Button>
      </div>
    </div>
  )
}
