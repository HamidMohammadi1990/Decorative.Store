import { useTranslation } from 'react-i18next'

export type CheckoutFlowStep = 'details' | 'review' | 'payment'

interface CheckoutStepsProps {
  currentStep: CheckoutFlowStep
}

const STEP_ORDER: CheckoutFlowStep[] = ['details', 'review', 'payment']

export function CheckoutSteps({ currentStep }: CheckoutStepsProps) {
  const { t } = useTranslation()
  const stepIndex = STEP_ORDER.indexOf(currentStep) + 1

  const meta = {
    details: {
      title: t('checkout.stepDetails'),
      description: t('checkout.stepDetailsHint'),
    },
    review: {
      title: t('checkout.stepReview'),
      description: t('checkout.stepReviewHint'),
    },
    payment: {
      title: t('checkout.stepPayment'),
      description: t('checkout.stepPaymentHint'),
    },
  }[currentStep]

  return (
    <div
      role="status"
      aria-live="polite"
      aria-label={t('checkout.currentStepAria', { step: meta.title })}
      className="mb-8 overflow-hidden rounded-sm border border-warm-muted/80 bg-surface shadow-sm"
    >
      <div className="flex items-stretch">
        <div className="flex w-1.5 shrink-0 bg-warm" aria-hidden />
        <div className="flex flex-1 flex-col gap-3 px-5 py-4 sm:flex-row sm:items-center sm:justify-between sm:px-6 sm:py-5">
          <div className="min-w-0">
            <p className="text-xs font-semibold uppercase tracking-[0.14em] text-warm">
              {t('checkout.currentStepLabel', {
                current: stepIndex,
                total: STEP_ORDER.length,
              })}
            </p>
            <h2 className="mt-1.5 text-lg font-semibold text-text sm:text-xl">
              {meta.title}
            </h2>
            <p className="mt-1 max-w-xl text-sm leading-relaxed text-text-muted">
              {meta.description}
            </p>
          </div>

          <div
            aria-hidden
            className="flex size-12 shrink-0 items-center justify-center self-start rounded-full bg-warm-soft text-sm font-semibold text-warm sm:self-center"
          >
            {stepIndex}/{STEP_ORDER.length}
          </div>
        </div>
      </div>
    </div>
  )
}
