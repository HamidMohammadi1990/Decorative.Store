interface AuthTabProps {
  active: boolean
  onClick: () => void
  label: string
}

export function AuthTab({ active, onClick, label }: AuthTabProps) {
  return (
    <button
      type="button"
      role="tab"
      aria-selected={active}
      onClick={onClick}
      className={`flex-1 rounded-full px-4 py-2.5 text-sm font-semibold transition-all duration-200 ${
        active
          ? 'bg-warm text-warm-text shadow-sm'
          : 'text-text-muted hover:text-warm'
      }`}
    >
      {label}
    </button>
  )
}
