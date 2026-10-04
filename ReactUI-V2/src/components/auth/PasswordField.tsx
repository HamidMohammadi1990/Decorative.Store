import { useState } from 'react'
import type { InputHTMLAttributes } from 'react'
import { FieldError, getAuthInputClassName } from '@/components/auth/AuthField'

interface PasswordFieldProps extends Omit<InputHTMLAttributes<HTMLInputElement>, 'type'> {
  label: string
  error?: string
  showLabel: string
  hideLabel: string
}

export function PasswordField({
  label,
  error,
  showLabel,
  hideLabel,
  id,
  className = '',
  ...props
}: PasswordFieldProps) {
  const [visible, setVisible] = useState(false)
  const fieldId = id ?? props.name
  const errorId = fieldId ? `${fieldId}-error` : undefined

  return (
    <div className={className}>
      <label htmlFor={fieldId} className="mb-1.5 block text-sm font-medium text-text">
        {label}
      </label>
      <div className="relative">
        <input
          id={fieldId}
          type={visible ? 'text' : 'password'}
          autoComplete={props.autoComplete ?? 'current-password'}
          aria-invalid={error ? true : undefined}
          aria-describedby={error ? errorId : undefined}
          className={`${getAuthInputClassName(Boolean(error))} py-2.5 pe-10 ps-3`}
          {...props}
        />
        <button
          type="button"
          onClick={() => setVisible((v) => !v)}
          aria-label={visible ? hideLabel : showLabel}
          className="absolute end-2 top-1/2 -translate-y-1/2 rounded-sm p-1 text-text-muted transition-colors hover:text-warm"
        >
          {visible ? <EyeOffIcon /> : <EyeIcon />}
        </button>
      </div>
      {error && <FieldError id={errorId} message={error} />}
    </div>
  )
}

function EyeIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" aria-hidden>
      <path
        d="M2.5 12C4.5 7.5 8 5 12 5s7.5 2.5 9.5 7c-2 4.5-5.5 7-9.5 7s-7.5-2.5-9.5-7Z"
        stroke="currentColor"
        strokeWidth="1.5"
      />
      <circle cx="12" cy="12" r="2.75" stroke="currentColor" strokeWidth="1.5" />
    </svg>
  )
}

function EyeOffIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" aria-hidden>
      <path
        d="M3 3 21 21M9.9 9.9A3 3 0 0 0 12 15a3 3 0 0 0 2.1-.9M7.2 7.2C5.4 8.4 3.9 10.1 2.5 12c2 4.5 5.5 7 9.5 7 1.6 0 3.1-.4 4.4-1.1M10.7 5.3A9.8 9.8 0 0 1 12 5c4 0 7.5 2.5 9.5 7a10.8 10.8 0 0 1-4.1 4.6"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
      />
    </svg>
  )
}
