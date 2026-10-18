import { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { useHydrated } from '@/hooks/useHydrated'
import { useCompareStore } from '@/stores/compareStore'

const SCROLL_SHOW_THRESHOLD = 320

function scrollToTop() {
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

export function BackToTop() {
  const { t } = useTranslation()

  return (
    <div className="absolute start-1/2 top-0 -translate-x-1/2 -translate-y-1/2 rtl:translate-x-1/2">
      <button
        type="button"
        onClick={scrollToTop}
        className="group inline-flex items-center gap-2 rounded-full border border-warm-muted bg-surface-muted px-6 py-3 text-sm font-medium text-warm shadow-sm transition-all duration-300 hover:-translate-y-0.5 hover:border-warm hover:bg-warm hover:text-warm-text hover:shadow-md focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-warm"
      >
        <ChevronUpIcon className="transition-transform duration-300 group-hover:-translate-y-0.5" />
        {t('common.backToTop')}
      </button>
    </div>
  )
}

/** Fixed control for long storefront pages (opposite side from AI assistant). */
export function FloatingBackToTop() {
  const { t } = useTranslation()
  const hydrated = useHydrated()
  const compareCount = useCompareStore((s) => (hydrated ? s.slugs.length : 0))
  const [visible, setVisible] = useState(false)

  useEffect(() => {
    const onScroll = () => {
      setVisible(window.scrollY > SCROLL_SHOW_THRESHOLD)
    }
    onScroll()
    window.addEventListener('scroll', onScroll, { passive: true })
    return () => window.removeEventListener('scroll', onScroll)
  }, [])

  if (!visible) return null

  const bottomClass =
    compareCount > 0 ? 'bottom-24 sm:bottom-28' : 'bottom-4 sm:bottom-6'

  return (
    <button
      type="button"
      onClick={scrollToTop}
      aria-label={t('common.backToTop')}
      title={t('common.backToTop')}
      className={`fixed start-4 z-[52] flex size-11 items-center justify-center rounded-full border border-border bg-surface/95 text-text shadow-md backdrop-blur-sm transition hover:border-text hover:bg-surface focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-warm sm:start-6 sm:size-12 ${bottomClass}`}
    >
      <ChevronUpIcon />
    </button>
  )
}

function ChevronUpIcon({ className = '' }: { className?: string }) {
  return (
    <svg
      width="16"
      height="16"
      viewBox="0 0 16 16"
      fill="none"
      aria-hidden
      className={className}
    >
      <path
        d="M4 10.5 8 6.5 12 10.5"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  )
}
