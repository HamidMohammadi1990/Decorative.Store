import { useTranslation } from 'react-i18next'

export function BackToTop() {
  const { t } = useTranslation()

  const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' })
  }

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
