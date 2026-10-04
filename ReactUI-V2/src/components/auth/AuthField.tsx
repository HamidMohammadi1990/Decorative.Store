import type { InputHTMLAttributes } from 'react'

interface AuthFieldProps extends InputHTMLAttributes<HTMLInputElement> {
  label: string
  error?: string
}

export function getAuthInputClassName(hasError: boolean) {
  return `w-full rounded-sm border bg-surface px-3 py-2.5 text-sm text-text outline-none transition-colors placeholder:text-text-muted/70 ${
    hasError
      ? 'border-sale focus:border-sale focus:ring-2 focus:ring-sale/15'
      : 'border-border focus:border-warm focus:ring-2 focus:ring-warm/15'
  }`
}

export function AuthField({
  label,
  error,
  id,
  className = '',
  ...props
}: AuthFieldProps) {
  const fieldId = id ?? props.name
  const errorId = fieldId ? `${fieldId}-error` : undefined

  return (
    <div className={className}>
      <label htmlFor={fieldId} className="mb-1.5 block text-sm font-medium text-text">
        {label}
      </label>
      <input
        id={fieldId}
        aria-invalid={error ? true : undefined}
        aria-describedby={error ? errorId : undefined}
        className={getAuthInputClassName(Boolean(error))}
        {...props}
      />
      {error && <FieldError id={errorId} message={error} />}
    </div>
  )
}

export function AuthCheckbox({
  label,
  id,
  className = '',
  ...props
}: InputHTMLAttributes<HTMLInputElement> & { label: string }) {
  const fieldId = id ?? props.name

  return (
    <label
      htmlFor={fieldId}
      className={`flex cursor-pointer items-center gap-2.5 text-sm text-text-muted ${className}`}
    >
      <input
        id={fieldId}
        type="checkbox"
        className="size-4 rounded-sm border-border text-warm accent-[#9a7448]"
        {...props}
      />
      {label}
    </label>
  )
}

export function FieldError({ id, message }: { id?: string; message: string }) {
  return (
    <p id={id} role="alert" className="mt-1.5 flex items-start gap-1.5 text-xs leading-relaxed text-sale">
      <ErrorIcon />
      <span>{message}</span>
    </p>
  )
}

function ErrorIcon() {
  return (
    <svg
      width="14"
      height="14"
      viewBox="0 0 14 14"
      fill="none"
      aria-hidden
      className="mt-0.5 shrink-0"
    >
      <circle cx="7" cy="7" r="5.25" stroke="currentColor" strokeWidth="1.2" />
      <path d="M7 4.5V7.5M7 9.2h.01" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
    </svg>
  )
}
