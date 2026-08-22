import { useState, type MouseEvent } from 'react'
import { useTranslation } from 'react-i18next'
import { WishlistIcon } from '@/components/wishlist/WishlistIcon'
import { useLocaleSettings } from '@/hooks/useLocaleSettings'
import { useWishlistStore } from '@/stores/wishlistStore'
import { useIsAuthenticated, useUserStore } from '@/stores/userStore'

interface WishlistButtonProps {
  slug: string
  variant?: 'icon' | 'pill' | 'compact' | 'card'
  className?: string
}

export function WishlistButton({ slug, variant = 'pill', className = '' }: WishlistButtonProps) {
  const { t } = useTranslation()
  const { locale } = useLocaleSettings()
  const isAuthenticated = useIsAuthenticated()
  const accessToken = useUserStore((state) => state.accessToken)
  const toggleRemote = useWishlistStore((s) => s.toggleRemote)
  const isInWishlist = useWishlistStore((s) => s.isInWishlist(slug))
  const [notice, setNotice] = useState<string | null>(null)
  const [isSaving, setIsSaving] = useState(false)

  if (!isAuthenticated) return null

  const handleClick = async (e: MouseEvent) => {
    e.preventDefault()
    e.stopPropagation()
    if (!accessToken || accessToken === 'mock-access-token' || isSaving) return

    setIsSaving(true)
    try {
      const result = await toggleRemote(accessToken, locale, slug)
      if (result === 'added') {
        setNotice(t('wishlist.added'))
        window.setTimeout(() => setNotice(null), 1800)
      }
    } catch {
      // Keep current state when the API call fails.
    } finally {
      setIsSaving(false)
    }
  }

  const label = isInWishlist ? t('product.inWishlist') : t('product.wishlist')

  if (variant === 'icon') {
    return (
      <button
        type="button"
        onClick={(event) => void handleClick(event)}
        disabled={isSaving}
        aria-pressed={isInWishlist}
        aria-label={label}
        title={notice ?? label}
        className={`flex size-10 items-center justify-center rounded-full border p-0 leading-none transition-all ${
          isInWishlist
            ? 'border-warm bg-warm text-warm-text shadow-md'
            : 'border-border/80 bg-surface/90 text-text-muted shadow-sm backdrop-blur-sm hover:border-warm hover:bg-surface hover:text-warm hover:shadow-md'
        } ${className}`}
      >
        <span className="flex size-4 items-center justify-center" aria-hidden>
          <WishlistIcon size={16} filled={isInWishlist} />
        </span>
      </button>
    )
  }

  if (variant === 'card') {
    return (
      <div className={className}>
        <button
          type="button"
          onClick={(event) => void handleClick(event)}
          disabled={isSaving}
          aria-pressed={isInWishlist}
          className={`inline-flex w-full items-center justify-center gap-2 rounded-sm border px-3 py-2.5 text-sm font-medium transition-all ${
            isInWishlist
              ? 'border-warm bg-warm-soft text-warm'
              : 'border-border bg-surface text-text-muted hover:border-warm hover:bg-warm-soft/40 hover:text-warm'
          }`}
        >
          <WishlistIcon size={16} filled={isInWishlist} />
          <span>{label}</span>
        </button>
        {notice && (
          <p className="mt-1.5 text-center text-xs text-accent" role="status">
            {notice}
          </p>
        )}
      </div>
    )
  }

  if (variant === 'compact') {
    return (
      <button
        type="button"
        onClick={(event) => void handleClick(event)}
        disabled={isSaving}
        aria-pressed={isInWishlist}
        title={notice ?? label}
        className={`inline-flex items-center gap-2 rounded-sm border px-3 py-2 text-xs font-semibold transition-all ${
          isInWishlist
            ? 'border-warm bg-warm-soft text-warm'
            : 'border-border bg-surface text-text-muted hover:border-warm hover:text-warm'
        } ${className}`}
      >
        <WishlistIcon size={14} filled={isInWishlist} />
        {label}
      </button>
    )
  }

  return (
    <button
      type="button"
      onClick={(event) => void handleClick(event)}
      disabled={isSaving}
      aria-pressed={isInWishlist}
      className={`inline-flex w-full items-center justify-center gap-2 rounded-sm border px-4 py-2.5 text-sm font-medium transition-all ${
        isInWishlist
          ? 'border-warm bg-warm text-warm-text shadow-sm'
          : 'border-border bg-surface text-text hover:border-warm hover:text-warm'
      } ${className}`}
    >
      <WishlistIcon size={16} filled={isInWishlist} />
      {label}
    </button>
  )
}
