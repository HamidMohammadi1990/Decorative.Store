import { useTranslation } from 'react-i18next'

type SpinnerSize = 'sm' | 'md' | 'lg'

const SIZE_PX: Record<SpinnerSize, number> = {
  sm: 18,
  md: 32,
  lg: 44,
}

interface SpinnerProps {
  size?: SpinnerSize
  className?: string
}

export function Spinner({ size = 'md', className = '' }: SpinnerProps) {
  const dim = SIZE_PX[size]
  const center = dim / 2
  const radius = center * 0.72
  const stroke = size === 'sm' ? 2 : 2.5
  const circumference = 2 * Math.PI * radius

  return (
    <svg
      width={dim}
      height={dim}
      viewBox={`0 0 ${dim} ${dim}`}
      role="status"
      aria-label="Loading"
      className={`loading-spinner shrink-0 ${className}`}
    >
      <circle
        cx={center}
        cy={center}
        r={radius}
        fill="none"
        stroke="currentColor"
        className="text-border"
        strokeWidth={stroke}
      />
      <circle
        cx={center}
        cy={center}
        r={radius}
        fill="none"
        stroke="currentColor"
        className="text-warm"
        strokeWidth={stroke}
        strokeLinecap="round"
        strokeDasharray={`${circumference * 0.24} ${circumference * 0.76}`}
      />
    </svg>
  )
}

interface PageLoadingProps {
  label?: string
  className?: string
  minHeight?: string
}

export function PageLoading({
  label,
  className = '',
  minHeight = 'min-h-[50vh]',
}: PageLoadingProps) {
  const { t } = useTranslation()
  const text = label ?? t('common.loading')

  return (
    <div
      className={`flex flex-col items-center justify-center gap-4 ${minHeight} ${className}`}
      role="status"
      aria-live="polite"
      aria-label={text}
    >
      <Spinner size="lg" />
      <p className="loading-label text-sm text-text-muted">{text}</p>
    </div>
  )
}

interface InlineLoadingProps {
  label?: string
  className?: string
}

export function InlineLoading({ label, className = '' }: InlineLoadingProps) {
  const { t } = useTranslation()
  const text = label ?? t('common.loading')

  return (
    <div
      className={`flex items-center justify-center gap-3 ${className}`}
      role="status"
      aria-live="polite"
      aria-label={text}
    >
      <Spinner size="sm" />
      <span className="text-sm text-text-muted">{text}</span>
    </div>
  )
}
