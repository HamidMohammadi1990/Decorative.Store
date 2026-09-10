import { AdminField, adminInputClass } from '@/components/dashboard/admin/adminFormShared'

type CmsAdminDescriptionFieldProps = {
  label: string
  hint: string
  value: string
  onChange: (value: string) => void
  placeholder?: string
}

export function CmsAdminDescriptionField({
  label,
  hint,
  value,
  onChange,
  placeholder,
}: CmsAdminDescriptionFieldProps) {
  return (
    <AdminField label={label}>
      <p className="mb-2 text-xs leading-relaxed text-text-muted">{hint}</p>
      <textarea
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className={adminInputClass}
        rows={3}
        placeholder={placeholder}
      />
    </AdminField>
  )
}

export function CmsAdminDescriptionNote({ text }: { text?: string | null }) {
  if (!text?.trim()) return null

  return (
    <p className="mt-1 text-xs leading-relaxed text-text-muted">{text}</p>
  )
}
