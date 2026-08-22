import type { MouseEvent } from 'react'
import { useTranslation } from 'react-i18next'
import { Button } from '@/components/ui/Button'
import { useAddToBag, type AddToBagItem } from '@/hooks/useAddToBag'

interface AddToBagButtonProps {
  item: AddToBagItem
  className?: string
  variant?: 'primary' | 'secondary' | 'ghost' | 'warm'
  stopPropagation?: boolean
}

export function AddToBagButton({
  item,
  className = '',
  variant = 'warm',
  stopPropagation = true,
}: AddToBagButtonProps) {
  const { t } = useTranslation()
  const { addToBag, status } = useAddToBag()

  const handleClick = (event: MouseEvent<HTMLButtonElement>) => {
    if (stopPropagation) {
      event.preventDefault()
      event.stopPropagation()
    }
    void addToBag(item)
  }

  const label =
    status === 'adding'
      ? t('cart.adding')
      : status === 'added'
        ? t('cart.added')
        : item.inStock === false
          ? t('cart.viewProductToOrder')
          : t('listing.addToBag')

  return (
    <Button
      type="button"
      variant={variant}
      className={className}
      onClick={handleClick}
      disabled={status === 'adding'}
      aria-live="polite"
      aria-busy={status === 'adding'}
    >
      {label}
    </Button>
  )
}
