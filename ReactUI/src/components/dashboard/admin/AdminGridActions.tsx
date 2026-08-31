import { useEffect, useId, useRef, useState, type ReactNode } from 'react'
import { Link } from 'react-router-dom'
import { ChevronIcon } from '@/components/ui/ChevronIcon'

const iconButtonClass =
  'inline-flex size-8 shrink-0 items-center justify-center rounded-sm border border-border transition-colors disabled:cursor-not-allowed disabled:opacity-40'

export function AdminGridActions({ children }: { children: ReactNode }) {
  return <div className="flex flex-wrap items-center justify-end gap-1.5">{children}</div>
}

export function AdminGridActionButton({
  label,
  icon,
  onClick,
  disabled = false,
}: {
  label: string
  icon: ReactNode
  onClick: () => void
  disabled?: boolean
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      disabled={disabled}
      className="inline-flex items-center gap-1.5 rounded-sm border border-warm/35 bg-warm-soft/70 px-2.5 py-1.5 text-xs font-semibold text-warm shadow-sm transition-colors hover:border-warm/55 hover:bg-warm-soft disabled:cursor-not-allowed disabled:opacity-50"
    >
      {icon}
      <span>{label}</span>
    </button>
  )
}

export function AdminGridActionLink({
  label,
  icon,
  to,
}: {
  label: string
  icon: ReactNode
  to: string
}) {
  return (
    <Link
      to={to}
      className="inline-flex items-center gap-1.5 rounded-sm border border-warm/35 bg-warm-soft/70 px-2.5 py-1.5 text-xs font-semibold text-warm shadow-sm transition-colors hover:border-warm/55 hover:bg-warm-soft"
    >
      {icon}
      <span>{label}</span>
    </Link>
  )
}

export interface AdminGridMenuItem {
  id: string
  label: string
  icon?: ReactNode
  onClick?: () => void
  disabled?: boolean
}

export function AdminGridActionMenu({
  label,
  icon,
  items,
  disabled = false,
}: {
  label: string
  icon?: ReactNode
  items: AdminGridMenuItem[]
  disabled?: boolean
}) {
  const [open, setOpen] = useState(false)
  const containerRef = useRef<HTMLDivElement>(null)
  const menuId = useId()

  useEffect(() => {
    if (!open) return

    const handlePointerDown = (event: MouseEvent) => {
      if (!containerRef.current?.contains(event.target as Node)) {
        setOpen(false)
      }
    }

    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') setOpen(false)
    }

    document.addEventListener('mousedown', handlePointerDown)
    document.addEventListener('keydown', handleKeyDown)
    return () => {
      document.removeEventListener('mousedown', handlePointerDown)
      document.removeEventListener('keydown', handleKeyDown)
    }
  }, [open])

  return (
    <div ref={containerRef} className="relative">
      <button
        type="button"
        disabled={disabled}
        aria-haspopup="menu"
        aria-expanded={open}
        aria-controls={menuId}
        onClick={() => setOpen((value) => !value)}
        className="inline-flex items-center gap-1.5 rounded-sm border border-warm/35 bg-warm-soft/70 px-2.5 py-1.5 text-xs font-semibold text-warm shadow-sm transition-colors hover:border-warm/55 hover:bg-warm-soft disabled:cursor-not-allowed disabled:opacity-50"
      >
        {icon}
        <span>{label}</span>
        <ChevronIcon expanded={open} className="text-warm" />
      </button>

      {open && (
        <div
          id={menuId}
          role="menu"
          className="absolute end-0 top-[calc(100%+0.25rem)] z-30 min-w-[11rem] overflow-hidden rounded-sm border border-border bg-surface py-1 shadow-lg"
        >
          {items.map((item) => (
            <button
              key={item.id}
              type="button"
              role="menuitem"
              disabled={item.disabled}
              onClick={() => {
                item.onClick?.()
                setOpen(false)
              }}
              className="flex w-full items-center gap-2 px-3 py-2 text-start text-xs font-medium text-text transition-colors hover:bg-surface-muted disabled:cursor-not-allowed disabled:opacity-40"
            >
              {item.icon ? <span className="shrink-0 text-text-muted">{item.icon}</span> : null}
              <span>{item.label}</span>
            </button>
          ))}
        </div>
      )}
    </div>
  )
}

export function AdminGridIconButton({
  label,
  icon,
  onClick,
  disabled = false,
  tone = 'default',
}: {
  label: string
  icon: ReactNode
  onClick: () => void
  disabled?: boolean
  tone?: 'default' | 'danger'
}) {
  const toneClasses =
    tone === 'danger'
      ? 'text-text-muted hover:border-red-200 hover:bg-red-50 hover:text-red-600'
      : 'text-text-muted hover:border-border-strong hover:bg-surface-muted hover:text-text'

  return (
    <button
      type="button"
      aria-label={label}
      title={label}
      onClick={onClick}
      disabled={disabled}
      className={`${iconButtonClass} ${toneClasses}`}
    >
      {icon}
    </button>
  )
}

export function AdminGridIconLink({
  label,
  icon,
  to,
}: {
  label: string
  icon: ReactNode
  to: string
}) {
  return (
    <Link
      to={to}
      aria-label={label}
      title={label}
      className={`${iconButtonClass} text-text-muted hover:border-border-strong hover:bg-surface-muted hover:text-text`}
    >
      {icon}
    </Link>
  )
}
