import { useState } from 'react'
import { useTranslation } from 'react-i18next'
import { copyToClipboard } from '@/extensions/copyToClipboard'

interface CopyableShortLinkProps {
  path: string
  className?: string
}

export function CopyableShortLink({ path, className = '' }: CopyableShortLinkProps) {
  const { t } = useTranslation()
  const [copied, setCopied] = useState(false)

  const handleCopy = async () => {
    const fullUrl =
      typeof window !== 'undefined' ? `${window.location.origin}${path}` : path
    const ok = await copyToClipboard(fullUrl)
    if (!ok) return
    setCopied(true)
    window.setTimeout(() => setCopied(false), 2000)
  }

  return (
    <div className={`flex flex-wrap items-center gap-2 ${className}`}>
      <span className="text-xs font-medium text-text-muted">{t('common.shortLink')}</span>
      <div className="flex min-w-0 flex-1 items-center gap-2 rounded-lg border border-border bg-surface-muted/50 px-3 py-2">
        <code
          dir="ltr"
          className="min-w-0 flex-1 truncate text-xs text-text-muted"
          title={path}
        >
          {path}
        </code>
        <button
          type="button"
          onClick={() => void handleCopy()}
          className="shrink-0 rounded-md border border-border bg-surface px-2.5 py-1 text-xs font-medium text-text transition-colors hover:border-warm hover:text-warm"
        >
          {copied ? t('common.linkCopied') : t('common.copyLink')}
        </button>
      </div>
    </div>
  )
}
