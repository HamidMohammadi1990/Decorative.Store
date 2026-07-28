import { ChevronIcon } from '@/components/ui/ChevronIcon'

interface ScrollArrowButtonProps {
  direction: 'prev' | 'next'
  disabled?: boolean
  label: string
  onClick: () => void
  className?: string
}

export function ScrollArrowButton({
  direction,
  disabled = false,
  label,
  onClick,
  className = '',
}: ScrollArrowButtonProps) {
  const rotation =
    direction === 'prev'
      ? 'rotate-90 rtl:-rotate-90'
      : '-rotate-90 rtl:rotate-90'

  return (
    <button
      type="button"
      aria-label={label}
      disabled={disabled}
      onClick={onClick}
      className={`flex size-9 shrink-0 items-center justify-center rounded-full border border-border bg-surface text-text-muted transition-colors hover:border-accent hover:text-accent disabled:pointer-events-none disabled:opacity-30 ${className}`}
    >
      <ChevronIcon expanded={false} className={rotation} />
    </button>
  )
}
