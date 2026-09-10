type UserAvatarSize = 'xs' | 'sm' | 'md' | 'lg' | 'xl'

const SIZE_CLASS: Record<UserAvatarSize, string> = {
  xs: 'size-7 text-[10px]',
  sm: 'size-8 text-xs',
  md: 'size-10 text-sm',
  lg: 'size-11 text-base',
  xl: 'size-16 text-base',
}

function buildInitials(name: string) {
  return name
    .split(/\s+/)
    .filter(Boolean)
    .slice(0, 2)
    .map((part) => part[0])
    .join('')
    .toUpperCase()
}

type UserAvatarProps = {
  name: string
  imageUrl?: string | null
  size?: UserAvatarSize
  className?: string
  title?: string
}

export function UserAvatar({ name, imageUrl, size = 'md', className = '', title }: UserAvatarProps) {
  const dim = SIZE_CLASS[size]
  const initials = buildInitials(name.trim()) || '?'

  if (imageUrl) {
    return (
      <img
        src={imageUrl}
        alt=""
        title={title}
        className={`shrink-0 rounded-full object-cover ring-1 ring-black/5 ${dim} ${className}`}
      />
    )
  }

  return (
    <span
      aria-hidden
      title={title}
      className={`inline-flex shrink-0 items-center justify-center rounded-full bg-warm-soft font-bold text-warm ${dim} ${className}`}
    >
      {initials}
    </span>
  )
}
