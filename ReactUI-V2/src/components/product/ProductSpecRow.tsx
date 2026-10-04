interface ProductSpecRowProps {
  label: string
  value: string
}

export function ProductSpecRow({ label, value }: ProductSpecRowProps) {
  return (
    <div className="flex items-start gap-6 border-b border-border/70 py-4 text-sm last:border-b-0 sm:gap-10">
      <dt className="w-[7.5rem] shrink-0 text-text-muted sm:w-[8.5rem]">{label}</dt>
      <dd className="min-w-0 font-semibold leading-relaxed text-text">{value}</dd>
    </div>
  )
}
