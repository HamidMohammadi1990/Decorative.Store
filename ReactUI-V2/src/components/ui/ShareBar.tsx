import { useState, type ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import { buildSocialShareUrls } from '@/seo/socialShareUrls'

interface ShareBarProps {
  heading: string
  title: string
  url: string
  shareActionLabel: string
  className?: string
}

export function ShareBar({
  heading,
  title,
  url,
  shareActionLabel,
  className = '',
}: ShareBarProps) {
  const { t } = useTranslation()
  const [copied, setCopied] = useState(false)
  const shareUrls = buildSocialShareUrls(title, url)

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

  return (
    <div className={`rounded-2xl border border-border/60 bg-surface p-4 ring-1 ring-black/[0.02] ${className}`}>
      <p className="text-sm font-semibold text-text">{heading}</p>

      <div className="mt-3 flex flex-wrap items-center gap-2">
        <button
          type="button"
          onClick={() => void shareNative()}
          className="inline-flex items-center gap-2 rounded-full border border-border/70 bg-surface px-4 py-2 text-sm font-medium text-text transition-colors hover:border-warm/35 hover:text-warm"
        >
          <ShareIcon />
          {shareActionLabel}
        </button>

        <button
          type="button"
          onClick={() => void copyLink()}
          className="inline-flex items-center gap-2 rounded-full border border-border/70 bg-surface px-4 py-2 text-sm font-medium text-text transition-colors hover:border-warm/35 hover:text-warm"
        >
          <LinkIcon />
          {copied ? t('common.linkCopied') : t('common.copyLink')}
        </button>

        <SocialShareLink href={shareUrls.whatsApp} label={t('share.whatsApp')} icon={<WhatsAppIcon />} />
        <SocialShareLink href={shareUrls.telegram} label={t('share.telegram')} icon={<TelegramIcon />} />
        <SocialShareLink href={shareUrls.facebook} label={t('share.facebook')} icon={<FacebookIcon />} />
        <SocialShareLink href={shareUrls.twitter} label={t('blog.shareTwitter')} icon={<TwitterIcon />} />
        <SocialShareLink href={shareUrls.linkedIn} label={t('blog.shareLinkedIn')} icon={<LinkedInIcon />} />
        <SocialShareLink href={shareUrls.pinterest} label={t('share.pinterest')} icon={<PinterestIcon />} />
      </div>
    </div>
  )
}

function SocialShareLink({
  href,
  label,
  icon,
}: {
  href: string
  label: string
  icon: ReactNode
}) {
  return (
    <a
      href={href}
      target="_blank"
      rel="noopener noreferrer"
      className="inline-flex size-10 items-center justify-center rounded-full border border-border/70 text-text-muted transition-colors hover:border-warm/35 hover:text-warm"
      aria-label={label}
      title={label}
    >
      {icon}
    </a>
  )
}

function ShareIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" aria-hidden>
      <path
        d="M6 9l4-2M6 7l4 2M10 3a1 1 0 1 0 0-2 1 1 0 0 0 0 2ZM4 9a1 1 0 1 0 0-2 1 1 0 0 0 0 2Zm6 6a1 1 0 1 0 0-2 1 1 0 0 0 0 2Z"
        stroke="currentColor"
        strokeWidth="1.2"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
      <path d="M5.2 8.2 8.8 6.8M5.2 7.8 8.8 9.2" stroke="currentColor" strokeWidth="1.2" />
    </svg>
  )
}

function LinkIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" aria-hidden>
      <path
        d="M6.2 9.8a3 3 0 0 0 4.2 0l2-2a3 3 0 0 0-4.2-4.2l-.8.8M9.8 6.2a3 3 0 0 0-4.2 0l-2 2a3 3 0 0 0 4.2 4.2l.8-.8"
        stroke="currentColor"
        strokeWidth="1.2"
        strokeLinecap="round"
      />
    </svg>
  )
}

function TwitterIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor" aria-hidden>
      <path d="M9.4 7.2 13.7 2h-1l-3.8 4.5L6.2 2H2l4.5 6.6L14 14h1l4-4.7L9.8 14H14L9.4 7.2Zm-1.3 1.5-.5-.7L3.6 2.8h1.5l3 4.2.5.7 4.4 6.2H10l-3.9-5.4Z" />
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

function FacebookIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor" aria-hidden>
      <path d="M9.5 3h2.1V.1C11.2 0 10.2 0 9.1 0 6.9 0 5.4 1.4 5.4 4v2.2H3.3V9h2.1v7h2.9V9h2.8l.5-2.8H8.3V4.3c0-.8.2-1.3 1.2-1.3Z" />
    </svg>
  )
}

function WhatsAppIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor" aria-hidden>
      <path d="M8 1.3a6.5 6.5 0 0 0-5.6 9.8L1.3 14l3.1-.9A6.5 6.5 0 1 0 8 1.3Zm0 1.2a5.3 5.3 0 0 1 0 10.6 5.2 5.2 0 0 1-2.6-.7l-.2-.1-1.8.5.5-1.8-.1-.2a5.3 5.3 0 0 1 2.2-7.3A5.2 5.2 0 0 1 8 2.5Zm-1.6 2.4c-.1 0-.3 0-.4.2-.2.2-.6.6-.6 1.4 0 .8.6 1.6.7 1.7.1.2 1.2 1.9 3 2.6 1.5.6 1.8.5 2.1.5.3 0 1-.4 1.1-.8.1-.4.1-.8.1-.8s-.1-.1-.4-.2c-.3-.1-1.1-.6-1.3-.6-.1 0-.2-.1-.3.1-.1.2-.5.6-.6.7-.1.1-.2.1-.4 0-.2-.1-.8-.3-1.5-1-.6-.6-1-1.3-1.1-1.5-.1-.2 0-.3.1-.4.1-.1.2-.3.3-.4.1-.1.1-.2.2-.3.1-.1 0-.2 0-.3 0-.1-.4-1.1-.6-1.5-.2-.3-.3-.3-.4-.3Z" />
    </svg>
  )
}

function TelegramIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor" aria-hidden>
      <path d="M8 1.3a6.7 6.7 0 1 0 0 13.4A6.7 6.7 0 0 0 8 1.3Zm2.9 4.6-1 4.7c-.1.6-.4.7-.8.4l-2.2-1.6-1.1 1c-.1.1-.2.2-.4.2l.2-2.7 4.1-3.7c.2-.2 0-.3-.2-.1l-5 3.1-2.2-.7c-.5-.1-.5-.5.1-.8l8.5-3.3c.4-.2.8.1.7.7Z" />
    </svg>
  )
}

function PinterestIcon() {
  return (
    <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor" aria-hidden>
      <path d="M8 1.3a6.7 6.7 0 0 0-2.7 12.9c0-.6 0-1.3.2-2 .2-.8 1.2-5.1 1.2-5.1s-.3-.6-.3-1.5c0-1.4.8-2.4 1.9-2.4.9 0 1.3.7 1.3 1.5 0 .9-.6 2.3-.9 3.5-.3 1 .5 1.9 1.5 1.9 1.8 0 3.1-1.9 3.1-4.7 0-2.5-1.8-4.2-4.3-4.2-2.9 0-4.6 2.2-4.6 4.4 0 .9.3 1.8 1 2.1.1 0 .1 0 .1-.1l.4-1.5c0-.1 0-.1-.1-.2-.2-.3-.4-.8-.4-1.3 0-1.6 1.2-3.1 3.2-3.1 1.7 0 2.8 1.1 2.8 2.8 0 1.7-1.1 3.2-2.7 3.2-.5 0-1-.3-1.2-.7 0 0-.3 1.1-.3 1.4-.1.4-.3.9-.5 1.2A6.7 6.7 0 1 0 8 1.3Z" />
    </svg>
  )
}
