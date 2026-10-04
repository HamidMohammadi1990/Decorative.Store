import type { ReactNode } from 'react'

interface CheckoutFormSectionProps {
  title: string
  description?: string
  step?: number
  children: ReactNode
}

export function CheckoutFormSection({
  title,
  description,
  step,
  children,
}: CheckoutFormSectionProps) {
  return (
    <section className="rounded-sm border border-border bg-surface p-5 shadow-sm sm:p-6">
      <header className="mb-5 border-b border-border pb-4">
        <div className="flex items-start gap-3">
          {step != null && (
            <span className="flex size-8 shrink-0 items-center justify-center rounded-full bg-warm-soft text-sm font-semibold text-warm">
              {step}
            </span>
          )}
          <div>
            <h2 className="text-base font-semibold text-text sm:text-lg">{title}</h2>
            {description && (
              <p className="mt-1 text-sm text-text-muted">{description}</p>
            )}
          </div>
        </div>
      </header>
      {children}
    </section>
  )
}
