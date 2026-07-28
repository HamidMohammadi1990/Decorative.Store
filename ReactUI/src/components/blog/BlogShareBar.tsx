import { useState } from 'react'
import { useTranslation } from 'react-i18next'

interface BlogShareBarProps {
  title: string
  url: string
}

export function BlogShareBar({ title, url }: BlogShareBarProps) {
  const { t } = useTranslation()
  const [copied, setCopied] = useState(false)

  const shareNative = async () => {
    if (navigator.share) {
      try {
        await navigator.share({ title, url })
        return
      } catch {
        // user cancelled or unsupported
      }
    }
    await copyLink()
  }

  const copyLink = async () => {
    try {
      await navigator.clipboard.writeText(url)
      setCopied(true)
      window.setTimeout(() => setCopied(false), 2000)
    } catch {
      setCopied(false)
    }
  }

  const encoded = encodeURIComponent(url)
  const encodedTitle = encodeURIComponent(title)

  return (
    <div className="rounded-2xl border border-border bg-surface p-4">
      <p className="text-sm font-semibold text-text">{t('blog.shareTitle')}</p>

      <div className="mt-3 flex flex-wrap items-center gap-2">
        <button
          type="button"
          onClick={() => void shareNative()}
          className="inline-flex items-center gap-2 rounded-full border border-border bg-surface px-4 py-2 text-sm font-medium text-text transition-colors hover:border-warm hover:text-warm"
        >
          <ShareIcon />
          {t('blog.share')}
        </button>

        <button
          type="button"
          onClick={() => void copyLink()}
          className="inline-flex items-center gap-2 rounded-full border border-border bg-surface px-4 py-2 text-sm font-medium text-text transition-colors hover:border-warm hover:text-warm"
        >
          <LinkIcon />
          {copied ? t('blog.linkCopied') : t('blog.copyLink')}
        </button>

        <a
          href={`https://twitter.com/intent/tweet?url=${encoded}&text=${encodedTitle}`}
          target="_blank"
          rel="noopener noreferrer"
          className="inline-flex size-10 items-center justify-center rounded-full border border-border text-text-muted transition-colors hover:border-warm hover:text-warm"
          aria-label={t('blog.shareTwitter')}
        >
          <TwitterIcon />
        </a>

        <a
          href={`https://www.linkedin.com/sharing/share-offsite/?url=${encoded}`}
          target="_blank"
          rel="noopener noreferrer"
          className="inline-flex size-10 items-center justify-center rounded-full border border-border text-text-muted transition-colors hover:border-warm hover:text-warm"
          aria-label={t('blog.shareLinkedIn')}
        >
          <LinkedInIcon />
        </a>
      </div>
    </div>
  )
}

function ShareIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" aria-hidden>
      <path d="M6 9l4-2M6 7l4 2M10 3a1 1 0 1 0 0-2 1 1 0 0 0 0 2ZM4 9a1 1 0 1 0 0-2 1 1 0 0 0 0 2Zm6 6a1 1 0 1 0 0-2 1 1 0 0 0 0 2Z" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" strokeLinejoin="round" />
      <path d="M5.2 8.2 8.8 6.8M5.2 7.8 8.8 9.2" stroke="currentColor" strokeWidth="1.2" />
    </svg>
  )
}

function LinkIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" aria-hidden>
      <path d="M6.2 9.8a3 3 0 0 0 4.2 0l2-2a3 3 0 0 0-4.2-4.2l-.8.8M9.8 6.2a3 3 0 0 0-4.2 0l-2 2a3 3 0 0 0 4.2 4.2l.8-.8" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
    </svg>
  )
}

function TwitterIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor" aria-hidden>
      <path d="M9.4 7.2 13.7 2h-1l-3.8 4.5L6.2 2H2l4.5 6.6L2 14h1l4-4.7L9.8 14H14L9.4 7.2Zm-1.3 1.5-.5-.7L3.6 2.8h1.5l3 4.2.5.7 4.4 6.2H10l-3.9-5.4Z" />
    </svg>
  )
}

function LinkedInIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor" aria-hidden>
      <path d="M3.5 6h2.2v7H3.5V6ZM4.6 3.5a1.3 1.3 0 1 0 0 2.6 1.3 1.3 0 0 0 0-2.6ZM8.4 6h2.1v.9h.1c.3-.6 1.1-1.2 2.2-1.2 2.4 0 2.8 1.5 2.8 3.5V13h-2.2V9.8c0-.8 0-1.8-1.1-1.8-1.1 0-1.3.9-1.3 1.7V13H8.4V6Z" />
    </svg>
  )
}
