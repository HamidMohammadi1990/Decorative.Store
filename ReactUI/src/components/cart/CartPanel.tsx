import { Link } from 'react-router-dom'
import { useMemo } from 'react'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { CartEmptyState } from '@/components/cart/CartEmptyState'
import { CartOrderSummary } from '@/components/cart/CartOrderSummary'
import { CartPageLineItem } from '@/components/cart/CartPageLineItem'
import { CartIcon } from '@/components/ui/HeaderIcons'
import { Button } from '@/components/ui/Button'
import { Container } from '@/components/ui/Container'
import { InlineLoading } from '@/components/ui/Spinner'
import { calculateCheckoutTotals } from '@/extensions/calculateCheckoutTotals'
import { useCartPage } from '@/hooks/useCartPage'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useCartStore } from '@/stores/cartStore'

interface CartPanelProps {
  embedded?: boolean
}

export function CartPanel({ embedded = false }: CartPanelProps) {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const { currency } = useLocaleSettings()
  const closeCart = useCartStore((s) => s.closeCart)
  const {
    lines,
    summary,
    trackingCode,
    loading,
    mutating,
    error,
    refresh,
    removeLine,
    updateQuantity,
    clearCart,
  } = useCartPage()

  const totals = useMemo(
    () => calculateCheckoutTotals(lines, 'delivery', 'standard'),
    [lines],
  )

  const itemCount = totals.itemCount

  const handleRemoveLine = async (lineId: string, title: string) => {
    if (!(await confirm({ message: t('cartPage.removeConfirm', { title }) }))) return
    await removeLine(lineId)
  }

  const handleClearCart = async () => {
    if (!(await confirm({ message: t('cartPage.clearConfirm') }))) return
    await clearCart()
  }

  if (loading) {
    return embedded ? (
      <InlineLoading label={t('cartPage.loading')} />
    ) : (
      <div className="flex min-h-[50vh] items-center justify-center bg-surface-muted/20 py-16">
        <InlineLoading label={t('cartPage.loading')} />
      </div>
    )
  }

  if (lines.length === 0) {
    if (embedded) {
      return (
        <DashboardEmptyState
          icon={<CartIcon size={28} />}
          title={t('common.emptyCartTitle')}
          message={t('common.emptyCartMessage')}
          action={
            <Link
              to="/"
              className="inline-flex items-center justify-center rounded-sm bg-warm px-4 py-2 text-sm font-medium text-warm-text transition-colors hover:bg-warm-hover"
            >
              {t('common.continueShopping')}
            </Link>
          }
        />
      )
    }

    return (
      <div className="flex-1 bg-gradient-to-b from-surface-muted/40 to-surface py-10 md:py-16">
        <Container className="max-w-lg">
          <nav aria-label={t('cartPage.breadcrumbLabel')} className="mb-8 text-sm text-text-muted">
            <ol className="flex flex-wrap items-center gap-2">
              <li>
                <Link to="/" className="transition-colors hover:text-warm">
                  {t('cartPage.breadcrumbHome')}
                </Link>
              </li>
              <li aria-hidden>/</li>
              <li className="font-medium text-text">{t('cartPage.title')}</li>
            </ol>
          </nav>
          <div className="overflow-hidden rounded-sm border border-border bg-surface shadow-sm">
            <CartEmptyState onContinue={closeCart} />
          </div>
        </Container>
      </div>
    )
  }

  if (!currency) return null

  const content = (
    <>
      {!embedded && (
        <header className="mb-8 flex flex-wrap items-end justify-between gap-4 border-b border-border pb-6">
          <div>
            <p className="text-xs font-semibold uppercase tracking-[0.18em] text-warm">
              {t('cartPage.eyebrow')}
            </p>
            <h1 className="mt-2 text-2xl font-semibold text-text md:text-3xl">{t('cartPage.title')}</h1>
            <p className="mt-2 text-sm text-text-muted">
              {t('common.cartItemCount', { count: itemCount })}
            </p>
          </div>

          <Button
            variant="ghost"
            className="text-sale hover:bg-sale/10 hover:text-sale"
            disabled={mutating}
            onClick={() => void clearCart()}
          >
            {t('cartPage.clearCart')}
          </Button>
        </header>
      )}

      {embedded && (
        <DashboardPageHeader
          title={t('dashboard.cart.title')}
          description={t('dashboard.cart.description', { count: itemCount })}
          icon={<CartIcon size={22} />}
          action={
            <Button
              variant="ghost"
              className="text-sale hover:bg-sale/10 hover:text-sale"
              disabled={mutating}
              onClick={() => void handleClearCart()}
            >
              {t('cartPage.clearCart')}
            </Button>
          }
        />
      )}

      {error && (
        <div className="mb-6 flex flex-wrap items-center justify-between gap-3 rounded-sm border border-sale/30 bg-sale/5 px-4 py-3 text-sm text-sale">
          <span>{t(`cartPage.${error}`)}</span>
          <Button variant="secondary" className="px-3 py-1.5 text-xs" onClick={() => void refresh()}>
            {t('common.retry')}
          </Button>
        </div>
      )}

      <div
        className={`grid gap-6 ${
          embedded
            ? 'lg:grid lg:grid-cols-[minmax(0,1fr)_minmax(0,17rem)] lg:items-start xl:grid-cols-[minmax(0,1fr)_18rem]'
            : 'lg:grid-cols-[minmax(0,1fr)_22rem] xl:grid-cols-[minmax(0,1fr)_24rem]'
        }`}
      >
        <section className={embedded ? 'space-y-3' : 'overflow-hidden rounded-sm border border-border bg-surface shadow-sm'}>
          {!embedded && (
            <div className="hidden border-b border-border bg-surface-muted/50 px-6 py-3 text-[11px] font-semibold uppercase tracking-[0.12em] text-text-muted lg:grid lg:grid-cols-[minmax(0,1fr)_7rem_9rem_8rem_3rem] lg:gap-4">
              <span>{t('cartPage.colProduct')}</span>
              <span className="text-end">{t('cartPage.colUnitPrice')}</span>
              <span className="text-center">{t('common.quantity')}</span>
              <span className="text-end">{t('cartPage.colTotal')}</span>
              <span className="sr-only">{t('cartPage.colRemove')}</span>
            </div>
          )}

          <div className={mutating ? 'pointer-events-none opacity-70' : undefined}>
            {lines.map((line) => (
              <CartPageLineItem
                key={line.lineId}
                line={line}
                currency={currency}
                embedded={embedded}
                disabled={mutating}
                onRemove={() => void handleRemoveLine(line.lineId, line.title)}
                onUpdateQuantity={(quantity) => void updateQuantity(line.lineId, quantity)}
              />
            ))}
          </div>
        </section>

        <aside className={embedded ? 'lg:sticky lg:top-24 lg:self-start' : undefined}>
          <CartOrderSummary
            embedded={embedded}
            totals={totals}
            currency={currency}
            summary={summary}
            trackingCode={trackingCode}
            checkoutDisabled={mutating}
          />
        </aside>
      </div>
    </>
  )

  if (embedded) {
    return <div>{content}</div>
  }

  return (
    <div className="flex-1 bg-gradient-to-b from-warm-soft/20 via-surface to-surface pb-16 pt-8 md:pb-24 md:pt-10">
      <Container>
        <nav aria-label={t('cartPage.breadcrumbLabel')} className="mb-6 text-sm text-text-muted">
          <ol className="flex flex-wrap items-center gap-2">
            <li>
              <Link to="/" className="transition-colors hover:text-warm">
                {t('cartPage.breadcrumbHome')}
              </Link>
            </li>
            <li aria-hidden>/</li>
            <li className="font-medium text-text">{t('cartPage.title')}</li>
          </ol>
        </nav>
        {content}
      </Container>
    </div>
  )
}
