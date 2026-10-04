interface StarRatingInputProps {
  label: string
  value: number
  onChange: (value: number) => void
  disabled?: boolean
}

export function StarRatingInput({ label, value, onChange, disabled = false }: StarRatingInputProps) {
  return (
    <div>
      <p className="text-sm font-medium text-text">{label}</p>
      <div className="mt-2 flex gap-1" role="radiogroup" aria-label={label}>
        {Array.from({ length: 5 }).map((_, index) => {
          const starValue = index + 1
          const filled = starValue <= value

          return (
            <button
              key={starValue}
              type="button"
              disabled={disabled}
              onClick={() => onChange(starValue)}
              className="rounded p-0.5 transition-colors hover:scale-105 disabled:opacity-50"
              aria-label={`${starValue} stars`}
              aria-checked={filled}
              role="radio"
            >
              <StarIcon filled={filled} />
            </button>
          )
        })}
      </div>
    </div>
  )
}

function StarIcon({ filled }: { filled: boolean }) {
  return (
    <svg
      width={22}
      height={22}
      viewBox="0 0 12 12"
      className={filled ? 'text-amber-400' : 'text-border-strong'}
      fill="currentColor"
      aria-hidden
    >
      <path d="M6 1.2l1.4 2.9 3.1.5-2.2 2.2.5 3.1L6 8.4 3.2 10l.5-3.1-2.2-2.2 3.1-.5L6 1.2z" />
    </svg>
  )
}
