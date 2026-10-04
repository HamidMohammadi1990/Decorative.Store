import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useConfirm } from '@/hooks/useConfirm'
import { PriceDisplay } from '@/components/ui/PriceDisplay'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useCartMutations } from '@/hooks/useCartMutations'
import { useCartStore } from '@/stores/cartStore'
import { CartEmptyState } from '@/components/cart/CartEmptyState'
import { CartLineItem } from '@/components/cart/CartLineItem'
import { Button } from '@/components/ui/Button'
import { CloseIcon } from '@/components/ui/CloseIcon'

export function CartDrawer() {
  const { t } = useTranslation()
  const confirm = useConfirm()
  const { currency } = useLocaleSettings()
  const lines = useCartStore((s) => s.lines)
  const isOpen = useCartStore((s) => s.isOpen)
  const closeCart = useCartStore((s) => s.closeCart)
  const { removeLine, updateQuantity } = useCartMutations()

  const handleRemoveLine = async (lineId: string, title: string) => {
    if (!(await confirm({ message: t('cartPage.removeConfirm', { title }) }))) return
    await removeLine(lineId)
  }

  if (!isOpen) return null

  const itemCount = lines.reduce((sum, line) => sum + line.quantity, 0)
  const subtotal = lines.reduce(
    (sum, line) => sum + line.unitPrice.amount * line.quantity,
    0,
  )

  return (
    <>
      <button
        type="button"
        className="fixed inset-0 z-40 bg-black/40 backdrop-blur-[1px]"
        aria-label={t('common.close')}
        onClick={closeCart}
      />

      <aside
        className="fixed inset-y-0 end-0 z-50 flex w-full max-w-md flex-col bg-surface shadow-2xl"
        role="dialog"
        aria-modal="true"
        aria-label={t('common.cart')}
      >
        <header className="flex items-start justify-between gap-4 border-b border-border px-5 py-4">
          <div>
            <h2 className="text-lg font-bold text-text">{t('common.cart')}</h2>
            {lines.length > 0 && (
              <p className="mt-0.5 text-xs text-text-muted">
                {t('common.cartItemCount', { count: itemCount })}
              </p>
            )}
          </div>
          <button
            type="button"
            onClick={closeCart}
            aria-label={t('common.close')}
            className="flex size-9 shrink-0 items-center justify-center rounded-full border border-border text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
          >
            <CloseIcon />
          </button>
        </header>

        <div className="flex min-h-0 flex-1 flex-col overflow-y-auto bg-surface-muted/30">
          {lines.length === 0 ? (
            <CartEmptyState onContinue={closeCart} />
          ) : (
            <ul className="space-y-3 px-4 py-4">
              {lines.map((line) => (
                <CartLineItem
                  key={line.lineId}
                  line={line}
                  currency={currency}
                  onRemove={() => void handleRemoveLine(line.lineId, line.title)}
                  onUpdateQuantity={(quantity) => void updateQuantity(line.lineId, quantity)}
                />
              ))}
            </ul>
          )}
        </div>

        {lines.length > 0 && currency && (
          <footer className="border-t border-border bg-surface px-5 py-4">
            <div className="mb-4 flex items-center justify-between gap-4">
              <span className="text-sm text-text-muted">{t('common.subtotal')}</span>
              <PriceDisplay
                money={{ amount: subtotal, currencyCode: currency.code }}
                currency={currency}
                className="text-lg font-bold text-text"
                iconSize={16}
              />
            </div>

            <Link to="/checkout" onClick={closeCart} className="block">
              <Button variant="warm" className="w-full py-3 font-semibold">
                {t('common.checkout')}
              </Button>
            </Link>

            <Link
              to="/account/dashboard/cart"
              onClick={closeCart}
              className="mt-2 block w-full py-2 text-center text-sm font-medium text-warm transition-colors hover:text-warm-hover"
            >
              {t('cartPage.viewFullCart')}
            </Link>

            {lines.length > 0 && (
              <Link
                to="/room-layout"
                onClick={closeCart}
                className="mt-2 block w-full py-2 text-center text-sm font-medium text-warm transition-colors hover:text-warm-hover"
              >
                {t('roomLayout.openFromCart')}
              </Link>
            )}

            <button
              type="button"
              onClick={closeCart}
              className="mt-3 w-full py-2 text-center text-sm font-medium text-text-muted transition-colors hover:text-text"
            >
              {t('common.continueShopping')}
            </button>
          </footer>
        )}
      </aside>
    </>
  )
}
