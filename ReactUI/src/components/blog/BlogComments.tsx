import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { formatBlogDate } from '@/extensions/formatBlogDate'
import type { BlogComment } from '@/models/blog/blog.model'
import { submitBlogComment } from '@/services/blogCommentSubmitService'
import { openLoginModal } from '@/stores/authModalStore'
import { useBlogInteractionStore } from '@/stores/blogInteractionStore'
import { useSettingsStore } from '@/stores/settingsStore'
import { useAccessToken, useUserStore } from '@/stores/userStore'

interface BlogCommentsProps {
  postId: string
  comments: BlogComment[]
  onCommentCreated?: () => Promise<void> | void
}

export function BlogComments({ postId, comments, onCommentCreated }: BlogCommentsProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const accessToken = useAccessToken()
  const user = useUserStore((s) => s.user)
  const [text, setText] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const [submitError, setSubmitError] = useState<string | null>(null)
  const [submitSuccess, setSubmitSuccess] = useState(false)

  const submitComment = useCallback(
    async (content: string) => {
      setSubmitting(true)
      setSubmitError(null)
      setSubmitSuccess(false)

      try {
        await submitBlogComment({
          blogPostId: postId,
          content,
          locale,
        })
        setText('')
        setSubmitSuccess(true)
        await onCommentCreated?.()
      } catch {
        setSubmitError(t('blog.commentSubmitFailed'))
      } finally {
        setSubmitting(false)
      }
    },
    [locale, onCommentCreated, postId, t],
  )

  const handleSubmit = () => {
    const trimmed = text.trim()
    if (!trimmed || submitting) return

    if (!accessToken) {
      openLoginModal({
        onSuccess: () => {
          void submitComment(trimmed)
        },
      })
      return
    }

    void submitComment(trimmed)
  }

  const authorLabel = user
    ? [user.firstName, user.lastName].filter(Boolean).join(' ') || user.email
    : null

  return (
    <section className="rounded-2xl border border-border bg-surface p-5 md:p-6">
      <div className="flex items-center justify-between gap-3">
        <h2 className="text-lg font-bold text-text">{t('blog.commentsTitle')}</h2>
        <span className="rounded-full bg-surface-muted px-3 py-1 text-xs font-medium text-text-muted">
          {t('blog.commentCount', { count: comments.length })}
        </span>
      </div>

      <div className="mt-5 rounded-xl border border-border bg-surface-muted/40 p-4">
        {submitSuccess ? (
          <CommentSubmitSuccess
            onWriteAnother={() => {
              setSubmitSuccess(false)
              setSubmitError(null)
            }}
          />
        ) : (
          <>
            <label htmlFor="blog-comment" className="text-sm font-medium text-text">
              {t('blog.addComment')}
            </label>
            {!accessToken && (
              <p className="mt-2 text-xs text-text-muted">{t('blog.signInToComment')}</p>
            )}
            {authorLabel && (
              <p className="mt-2 text-xs text-text-muted">
                {t('blog.commentAsUser', { name: authorLabel })}
              </p>
            )}
            <textarea
              id="blog-comment"
              value={text}
              onChange={(e) => setText(e.target.value)}
              placeholder={t('blog.commentPlaceholder')}
              rows={4}
              disabled={submitting}
              className="mt-2 block w-full resize-none rounded-lg border border-border bg-surface px-3 py-3 text-sm leading-relaxed text-text outline-none transition-colors placeholder:text-text-muted focus:border-warm disabled:cursor-not-allowed disabled:opacity-70"
            />
            {submitError && <CommentSubmitError message={submitError} />}
            <button
              type="button"
              onClick={handleSubmit}
              disabled={!text.trim() || submitting}
              className={`mt-3 rounded-lg px-5 py-2.5 text-sm font-semibold transition-colors ${
                text.trim() && !submitting
                  ? 'bg-warm text-warm-text hover:bg-warm-hover'
                  : 'cursor-not-allowed bg-border text-text-muted'
              }`}
            >
              {submitting ? t('common.loading') : t('blog.postComment')}
            </button>
          </>
        )}
      </div>

      <ul className="mt-6 space-y-4">
        {comments.map((comment) => (
          <CommentItem key={comment.id} comment={comment} locale={locale} />
        ))}
      </ul>
    </section>
  )
}

function CommentSubmitSuccess({ onWriteAnother }: { onWriteAnother: () => void }) {
  const { t } = useTranslation()

  return (
    <div
      className="flex gap-4 rounded-xl border border-accent/20 bg-accent/5 p-4 sm:p-5"
      role="status"
      aria-live="polite"
    >
      <span
        className="flex size-11 shrink-0 items-center justify-center rounded-full bg-accent/15 text-accent"
        aria-hidden
      >
        <CheckCircleIcon />
      </span>
      <div className="min-w-0 flex-1">
        <p className="text-base font-semibold text-text">{t('blog.commentSubmitSuccessTitle')}</p>
        <p className="mt-1.5 text-sm leading-relaxed text-text-muted">
          {t('blog.commentSubmitSuccessMessage')}
        </p>
        <button
          type="button"
          onClick={onWriteAnother}
          className="mt-4 text-sm font-semibold text-warm transition-colors hover:text-warm-hover hover:underline"
        >
          {t('blog.commentSubmitAnother')}
        </button>
      </div>
    </div>
  )
}

function CommentSubmitError({ message }: { message: string }) {
  return (
    <div
      className="mt-3 flex gap-3 rounded-xl border border-sale/25 bg-sale/5 p-3"
      role="alert"
      aria-live="assertive"
    >
      <span
        className="flex size-9 shrink-0 items-center justify-center rounded-full bg-sale/10 text-sale"
        aria-hidden
      >
        <AlertIcon />
      </span>
      <p className="min-w-0 flex-1 text-sm leading-relaxed text-sale">{message}</p>
    </div>
  )
}

function CheckCircleIcon() {
  return (
    <svg width="22" height="22" viewBox="0 0 22 22" fill="none" aria-hidden>
      <circle cx="11" cy="11" r="10" stroke="currentColor" strokeWidth="1.5" />
      <path
        d="M6.5 11.2l3 3 5.5-6"
        stroke="currentColor"
        strokeWidth="1.5"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  )
}

function AlertIcon() {
  return (
    <svg width="18" height="18" viewBox="0 0 18 18" fill="none" aria-hidden>
      <circle cx="9" cy="9" r="7.5" stroke="currentColor" strokeWidth="1.3" />
      <path d="M9 5.5v4.5" stroke="currentColor" strokeWidth="1.3" strokeLinecap="round" />
      <circle cx="9" cy="12.75" r="0.75" fill="currentColor" />
    </svg>
  )
}

function CommentItem({
  comment,
  locale,
}: {
  comment: BlogComment
  locale: 'en' | 'fa'
}) {
  const { t } = useTranslation()
  const liked = useBlogInteractionStore((s) => s.likedCommentIds.includes(comment.id))
  const toggleLike = useBlogInteractionStore((s) => s.toggleCommentLike)
  const likes = comment.likes + (liked ? 1 : 0)
  const authorName = comment.authorName.trim() || t('blog.guestAuthor')

  return (
    <li className="rounded-xl border border-border bg-surface-muted/30 p-4">
      <div className="flex items-start justify-between gap-3">
        <div className="flex items-center gap-3">
          <span className="flex size-10 shrink-0 items-center justify-center rounded-full bg-warm-soft text-sm font-bold text-warm">
            {authorName.charAt(0) || '?'}
          </span>
          <div>
            <p className="text-sm font-semibold text-text">{authorName}</p>
            <p className="text-xs text-text-muted">{formatBlogDate(comment.date, locale)}</p>
          </div>
        </div>

        <button
          type="button"
          onClick={() => toggleLike(comment.id)}
          aria-pressed={liked}
          aria-label={t('blog.likeComment')}
          className={`inline-flex items-center gap-1 rounded-full px-2.5 py-1 text-xs font-medium transition-colors ${
            liked ? 'bg-warm-soft text-warm' : 'text-text-muted hover:bg-surface-muted hover:text-text'
          }`}
        >
          <HeartIcon filled={liked} />
          {likes}
        </button>
      </div>
      <p className="mt-3 text-sm leading-relaxed text-text-muted">{comment.text}</p>
    </li>
  )
}

function HeartIcon({ filled }: { filled: boolean }) {
  return (
    <svg width="14" height="14" viewBox="0 0 14 14" fill={filled ? 'currentColor' : 'none'} aria-hidden>
      <path
        d="M7 11.8S3.5 9 3.5 5.8A2.4 2.4 0 0 1 7 4.2a2.4 2.4 0 0 1 3.5 1.6C10.5 9 7 11.8 7 11.8Z"
        stroke="currentColor"
        strokeWidth="1.2"
        strokeLinejoin="round"
      />
    </svg>
  )
}
