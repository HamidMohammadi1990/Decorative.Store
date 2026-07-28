import { LocalImage } from '@/components/ui/LocalImage'
import type { BlogAuthor } from '@/models/blog/blog.model'

interface BlogAuthorCardProps {
  author: BlogAuthor
  label: string
}

export function BlogAuthorCard({ author, label }: BlogAuthorCardProps) {
  return (
    <aside className="rounded-2xl border border-border bg-surface-muted/50 p-5">
      <p className="text-xs font-semibold uppercase tracking-wider text-text-muted">{label}</p>

      <div className="mt-4 flex items-start gap-4">
        <div className="size-16 shrink-0 overflow-hidden rounded-full border-2 border-surface shadow-sm">
          <LocalImage image={author.avatar} className="size-full object-cover" />
        </div>

        <div className="min-w-0">
          <h3 className="text-base font-bold text-text">{author.name}</h3>
          <p className="mt-0.5 text-sm font-medium text-warm">{author.role}</p>
          <p className="mt-3 text-sm leading-relaxed text-text-muted">{author.bio}</p>
        </div>
      </div>
    </aside>
  )
}
