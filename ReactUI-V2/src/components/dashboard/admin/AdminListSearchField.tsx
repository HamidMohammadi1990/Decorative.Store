import { AdminField, adminInputClass } from '@/components/dashboard/admin/adminFormShared'

type AdminListSearchFieldProps = {
  label: string
  placeholder: string
  value: string
  onChange: (value: string) => void
  onApply: () => void
}

export function AdminListSearchField({
  label,
  placeholder,
  value,
  onChange,
  onApply,
}: AdminListSearchFieldProps) {
  return (
    <AdminField label={label}>
      <input
        value={value}
        onChange={(e) => onChange(e.target.value)}
        onKeyDown={(e) => {
          if (e.key === 'Enter') onApply()
        }}
        onBlur={onApply}
        className={adminInputClass}
        placeholder={placeholder}
      />
    </AdminField>
  )
}
