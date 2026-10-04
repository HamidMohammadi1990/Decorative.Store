interface BlogTagListProps {
  tags: string[]
  label: string
}

export function BlogTagList({ tags, label }: BlogTagListProps) {
  if (tags.length === 0) return null

  return (
    <div className="flex flex-wrap items-center gap-2">
      <span className="text-sm font-medium text-text-muted">{label}:</span>
      {tags.map((tag) => (
        <span
          key={tag}
          className="rounded-full border border-border bg-surface-muted px-3 py-1 text-xs font-medium text-text"
        >
          {tag}
        </span>
      ))}
    </div>
  )
}
