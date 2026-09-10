import { useEffect, useMemo, useState, type FormEvent, type ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import type { CartLine } from '@/models/cart/cartLine.model'
import type { CurrencyConfig } from '@/models/shared/currency.model'
import type { CheckoutTotals } from '@/extensions/calculateCheckoutTotals'
import type { CheckoutPropertyValues } from '@/components/checkout/CheckoutProductOptionsSection'
import { AuthField } from '@/components/auth/AuthField'
import { CheckoutEmptyState } from '@/components/checkout/CheckoutEmptyState'
import { CheckoutFormSection } from '@/components/checkout/CheckoutFormSection'
import {
  CheckoutFulfillmentSection,
  type CheckoutAddressFields,
} from '@/components/checkout/CheckoutFulfillmentSection'
import { CheckoutDiscountField } from '@/components/checkout/CheckoutDiscountField'
import { CheckoutOrderSummary } from '@/components/checkout/CheckoutOrderSummary'
import { CheckoutPaymentPanel } from '@/components/checkout/CheckoutPaymentPanel'
import { CheckoutProductOptionsSection } from '@/components/checkout/CheckoutProductOptionsSection'
import { CheckoutReviewPanel } from '@/components/checkout/CheckoutReviewPanel'
import {
  CheckoutSteps,
  type CheckoutFlowStep,
} from '@/components/checkout/CheckoutSteps'
import { Button } from '@/components/ui/Button'
import { Container } from '@/components/ui/Container'
import { InlineLoading } from '@/components/ui/Spinner'
import {
  applyCartDiscountToTotals,
  calculateCheckoutTotals,
  type DeliveryMethod,
  type FulfillmentType,
} from '@/extensions/calculateCheckoutTotals'
import { useCheckoutDiscount } from '@/hooks/useCheckoutDiscount'
import type { ServerCartSummary } from '@/services/cartService'
import { mergeCheckoutAddresses } from '@/extensions/mapCheckoutAddress'
import { validateCheckoutProperties } from '@/extensions/validateCheckoutProperties'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { useCheckoutData } from '@/hooks/useCheckoutData'
import { useShopPageMeta } from '@/hooks/useShopPageMeta'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import type { SavedAddress } from '@/models/address/savedAddress.model'
import { useAddressStore } from '@/stores/addressStore'
import { useCartStore } from '@/stores/cartStore'
import { useUserStore } from '@/stores/userStore'

interface ContactFormValues {
  email: string
}

const emptyAddressFields: CheckoutAddressFields = {
  firstName: '',
  lastName: '',
  address: '',
  apartment: '',
  city: '',
  postcode: '',
  phone: '',
}

function createOrderRef() {
  return `WE-${Date.now().toString(36).toUpperCase()}`
}

export function CheckoutPage() {
  const { t } = useTranslation()

  useShopPageMeta({
    title: t('checkout.title', { defaultValue: 'Checkout' }),
    noindex: true,
    path: '/checkout',
  })

  const { currency } = useLocaleSettings()
  const lines = useCartStore((s) => s.lines)
  const closeCart = useCartStore((s) => s.closeCart)
  const localAddresses = useAddressStore((s) => s.addresses)
  const user = useUserStore((s) => s.user)
  const { sessions, addresses: apiAddresses, loading, error } = useCheckoutData(lines)
  const {
    summary: discountSummary,
    appliedCode,
    applying: discountApplying,
    errorKey: discountErrorKey,
    applyDiscount,
    removeDiscount,
    canApply: canApplyDiscount,
  } = useCheckoutDiscount()
  const checkoutAddresses = useMemo(
    () => mergeCheckoutAddresses(localAddresses, apiAddresses),
    [apiAddresses, localAddresses],
  )

  const [step, setStep] = useState<CheckoutFlowStep>('details')
  const [fulfillment, setFulfillment] = useState<FulfillmentType>('delivery')
  const [delivery, setDelivery] = useState<DeliveryMethod>('standard')
  const [propertyValues, setPropertyValues] = useState<CheckoutPropertyValues>({})
  const [propertyErrors, setPropertyErrors] = useState<Record<string, Record<string, string>>>({})
  const [contactForm, setContactForm] = useState<ContactFormValues>({ email: user?.email ?? '' })
  const [addressFields, setAddressFields] = useState<CheckoutAddressFields>(emptyAddressFields)
  const [selectedAddress, setSelectedAddress] = useState<SavedAddress | null>(null)
  const [contactErrors, setContactErrors] = useState<Partial<Record<keyof ContactFormValues, string>>>({})
  const [addressErrors, setAddressErrors] = useState<Partial<Record<keyof CheckoutAddressFields, string>>>({})
  const [orderRef, setOrderRef] = useState<string | null>(null)
  const [processing, setProcessing] = useState(false)

  const selectedAddressId = selectedAddress?.id ?? null

  useEffect(() => {
    closeCart()
  }, [closeCart])

  useEffect(() => {
    if (user?.email && !contactForm.email) {
      setContactForm((prev) => ({ ...prev, email: user.email }))
    }
  }, [contactForm.email, user?.email])

  const baseTotals = useMemo(
    () => calculateCheckoutTotals(lines, fulfillment, delivery),
    [lines, fulfillment, delivery],
  )

  const totals = useMemo(
    () => applyCartDiscountToTotals(baseTotals, discountSummary),
    [baseTotals, discountSummary],
  )

  if (lines.length === 0) {
    return <CheckoutEmptyState />
  }

  if (loading) {
    return (
      <div className="flex min-h-[50vh] items-center justify-center bg-surface-muted">
        <InlineLoading label={t('checkout.loadingOptions')} />
      </div>
    )
  }

  if (error) {
    return (
      <div className="flex min-h-[50vh] items-center justify-center bg-surface-muted px-4">
        <p className="text-center text-sm text-sale">{t(error)}</p>
      </div>
    )
  }

  if (!currency) return null

  const updatePropertyValue = (productId: string, propertyId: string, value: unknown) => {
    setPropertyValues((prev) => ({
      ...prev,
      [productId]: {
        ...prev[productId],
        [propertyId]: value,
      },
    }))
    setPropertyErrors((prev) => {
      const productErrors = prev[productId]
      if (!productErrors?.[propertyId]) return prev
      const nextProductErrors = { ...productErrors }
      delete nextProductErrors[propertyId]
      return {
        ...prev,
        [productId]: nextProductErrors,
      }
    })
  }

  const updateContactField = (field: keyof ContactFormValues, value: string) => {
    setContactForm((prev) => ({ ...prev, [field]: value }))
    if (contactErrors[field]) {
      setContactErrors((prev) => {
        const next = { ...prev }
        delete next[field]
        return next
      })
    }
  }

  const validateDetails = () => {
    const nextContact: Partial<Record<keyof ContactFormValues, string>> = {}

    if (!contactForm.email.trim()) {
      nextContact.email = t('checkout.validation.required')
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(contactForm.email)) {
      nextContact.email = t('checkout.validation.emailInvalid')
    }

    let nextAddress: Partial<Record<keyof CheckoutAddressFields, string>> = {}

    if (fulfillment === 'delivery') {
      const hasSelectedAddress =
        checkoutAddresses.length > 0 &&
        selectedAddress != null &&
        addressFields.firstName.trim() &&
        addressFields.address.trim()

      if (!hasSelectedAddress) {
        nextAddress = { firstName: t('checkout.noAddressSelected') }
      }
    }

    const nextPropertyErrors = validateCheckoutProperties(sessions, propertyValues)
    setPropertyErrors(nextPropertyErrors)

    setContactErrors(nextContact)
    setAddressErrors(nextAddress)

    if (
      Object.keys(nextContact).length > 0 ||
      Object.keys(nextAddress).length > 0 ||
      Object.keys(nextPropertyErrors).length > 0
    ) {
      return false
    }

    return true
  }

  const goToReview = (e?: FormEvent) => {
    e?.preventDefault()
    if (!validateDetails()) return
    setOrderRef(createOrderRef())
    setStep('review')
    window.scrollTo({ top: 0, behavior: 'smooth' })
  }

  const goToPayment = () => {
    setStep('payment')
    window.scrollTo({ top: 0, behavior: 'smooth' })
  }

  const handlePayment = async () => {
    setProcessing(true)
    await new Promise((resolve) => setTimeout(resolve, 1200))
    setProcessing(false)
  }

  const standardTotals = calculateCheckoutTotals(lines, 'delivery', 'standard')
  const expressTotals = calculateCheckoutTotals(lines, 'delivery', 'express')
  const pickupTotals = calculateCheckoutTotals(lines, 'pickup')

  return (
    <div className="bg-surface-muted py-8 sm:py-10">
      <Container>
        <header className="mb-6">
          <h1 className="text-2xl font-semibold text-text md:text-3xl">
            {t('checkout.title')}
          </h1>
          <p className="mt-1 text-sm text-text-muted">{t('checkout.subtitle')}</p>
        </header>

        <CheckoutSteps currentStep={step} />

        {step === 'details' && (
          <form id="checkout-form" onSubmit={goToReview}>
            <div className="grid gap-8 lg:grid-cols-[minmax(0,1fr)_22rem] xl:grid-cols-[minmax(0,1fr)_24rem] xl:gap-10">
              <div className="space-y-6">
                <CheckoutFormSection
                  step={1}
                  title={t('checkout.contactTitle')}
                  description={t('checkout.contactDescription')}
                >
                  <AuthField
                    name="email"
                    type="email"
                    autoComplete="email"
                    label={t('checkout.emailLabel')}
                    placeholder={t('checkout.emailPlaceholder')}
                    value={contactForm.email}
                    onChange={(e) => updateContactField('email', e.target.value)}
                    error={contactErrors.email}
                  />
                </CheckoutFormSection>

                <CheckoutFulfillmentSection
                  fulfillment={fulfillment}
                  onFulfillmentChange={setFulfillment}
                  selectedAddressId={selectedAddressId}
                  onSelectedAddressChange={setSelectedAddress}
                  onAddressFieldsChange={setAddressFields}
                  addressErrors={addressErrors}
                  addressesOverride={checkoutAddresses}
                />

                {sessions
                  .filter((session) => session.data.properties.length > 0)
                  .map((session) => (
                  <CheckoutFormSection
                    key={session.productId}
                    step={3}
                    title={t('checkout.productOptionsTitle')}
                    description={t('checkout.productOptionsDescription')}
                  >
                    <CheckoutProductOptionsSection
                      productTitle={session.productTitle}
                      propertyGroups={session.data.properties}
                      values={propertyValues[session.productId] ?? {}}
                      errors={Object.fromEntries(
                        Object.entries(propertyErrors[session.productId] ?? {}).map(
                          ([propertyId, code]) => [
                            propertyId,
                            code === 'required' ? t('checkout.validation.required') : code,
                          ],
                        ),
                      )}
                      onChange={(propertyId, value) =>
                        updatePropertyValue(session.productId, propertyId, value)
                      }
                    />
                  </CheckoutFormSection>
                ))}

                {fulfillment === 'delivery' && (
                  <CheckoutFormSection
                    step={4}
                    title={t('checkout.deliveryTitle')}
                    description={t('checkout.deliveryDescription')}
                  >
                    <div className="grid gap-3 sm:grid-cols-2">
                      <DeliveryOption
                        id="standard"
                        name="delivery"
                        checked={delivery === 'standard'}
                        title={t('checkout.deliveryStandard')}
                        description={
                          standardTotals.shippingIsFree ? (
                            t('checkout.deliveryStandardFree')
                          ) : (
                            <PriceDisplay
                              money={{
                                amount: standardTotals.shipping,
                                currencyCode: currency.code,
                              }}
                              currency={currency}
                            />
                          )
                        }
                        eta={t('checkout.deliveryStandardEta')}
                        onChange={() => setDelivery('standard')}
                      />
                      <DeliveryOption
                        id="express"
                        name="delivery"
                        checked={delivery === 'express'}
                        title={t('checkout.deliveryExpress')}
                        description={
                          <PriceDisplay
                            money={{
                              amount: expressTotals.shipping,
                              currencyCode: currency.code,
                            }}
                            currency={currency}
                          />
                        }
                        eta={t('checkout.deliveryExpressEta')}
                        onChange={() => setDelivery('express')}
                      />
                    </div>
                  </CheckoutFormSection>
                )}

                {fulfillment === 'pickup' && (
                  <CheckoutFormSection
                    step={4}
                    title={t('checkout.pickupReadyTitle')}
                    description={t('checkout.pickupReadyDescription')}
                  >
                    <p className="flex flex-wrap items-center gap-1 text-sm text-text-muted">
                      <span>{t('checkout.pickupReadyEta')}</span>
                      <PriceDisplay
                        money={{ amount: pickupTotals.total, currencyCode: currency.code }}
                        currency={currency}
                      />
                    </p>
                  </CheckoutFormSection>
                )}

                <div className="lg:hidden">
                  <SidebarSummary
                    lines={lines}
                    totals={totals}
                    currency={currency}
                    summary={discountSummary}
                    discountProps={{
                      appliedCode,
                      applying: discountApplying,
                      errorKey: discountErrorKey,
                      canApply: canApplyDiscount,
                      onApply: applyDiscount,
                      onRemove: removeDiscount,
                      isDiscountApplied: discountSummary?.isDiscountApplied ?? false,
                    }}
                    actionLabel={t('checkout.reviewOrder')}
                    actionHint={t('checkout.reviewOrderHint')}
                    onAction={() => goToReview()}
                  />
                  <TrustList />
                </div>
              </div>

              <div className="hidden lg:block">
                <div className="sticky top-24">
                  <SidebarSummary
                    lines={lines}
                    totals={totals}
                    currency={currency}
                    summary={discountSummary}
                    discountProps={{
                      appliedCode,
                      applying: discountApplying,
                      errorKey: discountErrorKey,
                      canApply: canApplyDiscount,
                      onApply: applyDiscount,
                      onRemove: removeDiscount,
                      isDiscountApplied: discountSummary?.isDiscountApplied ?? false,
                    }}
                    actionLabel={t('checkout.reviewOrder')}
                    actionHint={t('checkout.reviewOrderHint')}
                    onAction={() => goToReview()}
                  />
                  <TrustList />
                </div>
              </div>
            </div>
          </form>
        )}

        {step === 'review' && orderRef && (
          <div className="grid gap-8 lg:grid-cols-[minmax(0,1fr)_22rem] xl:gap-10">
            <CheckoutReviewPanel
              email={contactForm.email}
              fulfillment={fulfillment}
              delivery={delivery}
              selectedAddress={selectedAddress}
              lines={lines}
              totals={totals}
              currency={currency}
              orderRef={orderRef}
              onEditDetails={() => {
                setStep('details')
                window.scrollTo({ top: 0, behavior: 'smooth' })
              }}
            />

            <div className="lg:sticky lg:top-24 lg:self-start">
              <div className="rounded-sm border border-border bg-surface p-5 shadow-sm">
                <p className="text-sm text-text-muted">{t('checkout.totalDue')}</p>
                <PriceDisplay
                  money={{ amount: totals.total, currencyCode: currency.code }}
                  currency={currency}
                  className="mt-1 text-2xl font-semibold text-text"
                  iconSize={18}
                />
                <Button
                  type="button"
                  variant="warm"
                  className="mt-4 w-full py-3 font-semibold"
                  onClick={goToPayment}
                >
                  {t('checkout.proceedToPayment')}
                </Button>
                <button
                  type="button"
                  onClick={() => setStep('details')}
                  className="mt-3 w-full text-center text-sm font-medium text-warm hover:underline"
                >
                  {t('checkout.backToDetails')}
                </button>
              </div>
              <TrustList />
            </div>
          </div>
        )}

        {step === 'payment' && orderRef && (
          <div className="grid gap-8 lg:grid-cols-[minmax(0,1fr)_22rem] xl:gap-10">
            <CheckoutPaymentPanel
              orderRef={orderRef}
              totals={totals}
              currency={currency}
              paying={processing}
              onPay={handlePayment}
            />

            <div className="lg:sticky lg:top-24 lg:self-start">
              <CheckoutOrderSummary
                lines={lines}
                totals={totals}
                currency={currency}
                summary={discountSummary}
                discountSlot={
                  <CheckoutDiscountField
                    appliedCode={appliedCode}
                    isDiscountApplied={discountSummary?.isDiscountApplied ?? false}
                    applying={discountApplying}
                    errorKey={discountErrorKey}
                    canApply={canApplyDiscount}
                    onApply={applyDiscount}
                    onRemove={removeDiscount}
                    compact
                  />
                }
                compact
              />
              <button
                type="button"
                onClick={() => setStep('review')}
                className="mt-4 w-full text-center text-sm font-medium text-warm hover:underline"
              >
                {t('checkout.backToReview')}
              </button>
            </div>
          </div>
        )}
      </Container>
    </div>
  )
}

interface CheckoutDiscountProps {
  appliedCode: string | null
  isDiscountApplied: boolean
  applying: boolean
  errorKey: string | null
  canApply: boolean
  onApply: (code: string) => Promise<boolean>
  onRemove: () => Promise<boolean>
}

function SidebarSummary({
  lines,
  totals,
  currency,
  summary,
  discountProps,
  actionLabel,
  actionHint,
  onAction,
}: {
  lines: CartLine[]
  totals: CheckoutTotals
  currency: CurrencyConfig
  summary?: ServerCartSummary | null
  discountProps: CheckoutDiscountProps
  actionLabel: string
  actionHint: string
  onAction: () => void
}) {
  return (
    <CheckoutOrderSummary
      lines={lines}
      totals={totals}
      currency={currency}
      summary={summary}
      discountSlot={<CheckoutDiscountField {...discountProps} />}
      showAction
      actionLabel={actionLabel}
      actionHint={actionHint}
      onAction={onAction}
    />
  )
}

function TrustList() {
  const { t } = useTranslation()
  return (
    <ul className="mt-4 space-y-2 rounded-sm border border-warm-muted/80 bg-warm-soft px-4 py-3 text-xs text-text-muted">
      <li className="flex items-start gap-2">
        <span className="text-warm">✓</span>
        {t('checkout.trustFreeReturns')}
      </li>
      <li className="flex items-start gap-2">
        <span className="text-warm">✓</span>
        {t('checkout.trustDelivery')}
      </li>
      <li className="flex items-start gap-2">
        <span className="text-warm">✓</span>
        {t('checkout.trustSupport')}
      </li>
    </ul>
  )
}

function DeliveryOption({
  id,
  name,
  checked,
  title,
  description,
  eta,
  onChange,
}: {
  id: string
  name: string
  checked: boolean
  title: string
  description: ReactNode
  eta: string
  onChange: () => void
}) {
  return (
    <label
      htmlFor={id}
      className={`flex cursor-pointer flex-col rounded-sm border p-4 transition-colors ${
        checked
          ? 'border-warm bg-warm-soft ring-1 ring-warm/30'
          : 'border-border bg-surface hover:border-warm-muted'
      }`}
    >
      <div className="flex items-start gap-3">
        <input
          id={id}
          type="radio"
          name={name}
          checked={checked}
          onChange={onChange}
          className="mt-0.5 size-4 accent-[#9a7448]"
        />
        <div>
          <p className="text-sm font-semibold text-text">{title}</p>
          <p className="mt-1 text-sm text-warm">{description}</p>
          <p className="mt-1 text-xs text-text-muted">{eta}</p>
        </div>
      </div>
    </label>
  )
}
