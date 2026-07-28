import { useState, type MouseEvent } from 'react'
import { useTranslation } from 'react-i18next'
import { CompareIcon } from '@/components/compare/CompareIcon'
import { useCompareStore } from '@/stores/compareStore'

interface CompareButtonProps {
  slug: string
  variant?: 'icon' | 'pill' | 'compact' | 'card'
  className?: string
}

export function CompareButton({ slug, variant = 'pill', className = '' }: CompareButtonProps) {
  const { t } = useTranslation()
  const toggle = useCompareStore((s) => s.toggle)
  const isInCompare = useCompareStore((s) => s.isInCompare(slug))
  const [notice, setNotice] = useState<string | null>(null)

  const handleClick = (e: MouseEvent) => {
    e.preventDefault()
    e.stopPropagation()
    const result = toggle(slug)
    if (result === 'full') {
      setNotice(t('compare.maxReached'))
      window.setTimeout(() => setNotice(null), 2200)
    }
  }

  const label = isInCompare ? t('compare.inCompare') : t('compare.add')

  if (variant === 'icon') {
    return (
      <button
        type="button"
        onClick={handleClick}
        aria-pressed={isInCompare}
        aria-label={isInCompare ? t('compare.remove') : t('compare.add')}
        title={notice ?? label}
        className={`flex size-10 items-center justify-center rounded-full border p-0 leading-none transition-all ${
          isInCompare
            ? 'border-warm bg-warm text-warm-text shadow-md'
            : 'border-border/80 bg-surface/90 text-text-muted shadow-sm backdrop-blur-sm hover:border-warm hover:bg-surface hover:text-warm hover:shadow-md'
        } ${className}`}
      >
        <span className="flex size-4 items-center justify-center" aria-hidden>
          <CompareIcon size={16} className="block" />
        </span>
      </button>
    )
  }

  if (variant === 'card') {
    return (
      <div className={className}>
        <button
          type="button"
          onClick={handleClick}
          aria-pressed={isInCompare}
          className={`inline-flex w-full items-center justify-center gap-2 rounded-sm border px-3 py-2.5 text-sm font-medium transition-all ${
            isInCompare
              ? 'border-warm bg-warm-soft text-warm'
              : 'border-border bg-surface text-text-muted hover:border-warm hover:bg-warm-soft/40 hover:text-warm'
          }`}
        >
          <CompareIcon size={16} />
          <span>{label}</span>
        </button>
        {notice && (
          <p className="mt-1.5 text-center text-xs text-sale" role="status">
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
        onClick={handleClick}
        aria-pressed={isInCompare}
        className={`inline-flex items-center gap-2 rounded-sm border px-3 py-2 text-xs font-semibold transition-all ${
          isInCompare
            ? 'border-warm bg-warm-soft text-warm'
            : 'border-border bg-surface text-text-muted hover:border-warm hover:text-warm'
        } ${className}`}
      >
        <CompareIcon size={14} />
        {label}
      </button>
    )
  }

  return (
    <button
      type="button"
      onClick={handleClick}
      aria-pressed={isInCompare}
      className={`inline-flex w-full items-center justify-center gap-2 rounded-sm border px-4 py-2.5 text-sm font-medium transition-all ${
        isInCompare
          ? 'border-warm bg-warm text-warm-text shadow-sm'
          : 'border-border bg-surface text-text hover:border-warm hover:text-warm'
      } ${className}`}
    >
      <CompareIcon size={16} />
      {label}
    </button>
  )
}
