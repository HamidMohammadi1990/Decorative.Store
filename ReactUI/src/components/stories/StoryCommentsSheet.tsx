import { useCallback, useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { StoryComment } from '@/models/stories/story.model'
import { formatRelativeDate } from '@/extensions/formatRelativeDate'
import { useSettingsStore } from '@/stores/settingsStore'
import { useAccessToken, useUserStore } from '@/stores/userStore'
import { openLoginModal } from '@/stores/authModalStore'
import { userStoryCommentService } from '@/services/userStoryCommentService'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'

interface StoryCommentsSheetProps {
  storyId: string
  commentCount: number
  isOpen: boolean
  onClose: () => void
  onCommentSubmitted?: () => void
}

export function StoryCommentsSheet({
  storyId,
  commentCount,
  isOpen,
  onClose,
  onCommentSubmitted,
}: StoryCommentsSheetProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const accessToken = useAccessToken()
  const user = useUserStore((s) => s.user)
  const [text, setText] = useState('')
  const [comments, setComments] = useState<StoryComment[]>([])
  const [loading, setLoading] = useState(false)
  const [submitting, setSubmitting] = useState(false)
  const [submitError, setSubmitError] = useState<string | null>(null)
  const [submitSuccess, setSubmitSuccess] = useState(false)

  const loadComments = useCallback(async () => {
    setLoading(true)
    try {
      const result = await userStoryCommentService.search(locale, storyId)
      setComments(result.items)
    } catch {
      setComments([])
    } finally {
      setLoading(false)
    }
  }, [locale, storyId])

  useEffect(() => {
    if (!isOpen) return
    void loadComments()
  }, [isOpen, loadComments])

  if (!isOpen) return null

  const authorName = user
    ? `${user.firstName}${user.lastName ? ` ${user.lastName}` : ''}`
    : t('stories.guestAuthor')

  const submitComment = async (content: string) => {
    if (!accessToken) return

    setSubmitting(true)
    setSubmitError(null)
    setSubmitSuccess(false)
    try {
      await userStoryCommentService.create(storyId, content, locale, accessToken)
      setText('')
      setSubmitSuccess(true)
      onCommentSubmitted?.()
    } catch {
      setSubmitError(t('stories.commentSubmitFailed'))
    } finally {
      setSubmitting(false)
    }
  }

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

  return (
    <Portal>
      <div className="fixed inset-0 z-[80] flex items-end justify-center sm:items-center">
        <button
          type="button"
          aria-label={t('common.close')}
          className="absolute inset-0 bg-black/50 backdrop-blur-[2px]"
          onClick={onClose}
        />

        <div
          role="dialog"
          aria-modal
          aria-labelledby="story-comments-title"
          className="relative z-10 flex max-h-[min(85vh,40rem)] w-full max-w-lg flex-col overflow-hidden rounded-t-2xl border border-border bg-surface shadow-2xl sm:rounded-2xl"
        >
          <header className="flex items-center justify-between border-b border-border px-5 py-4">
            <h2 id="story-comments-title" className="text-base font-bold text-text">
              {t('stories.commentsTitle')}
              <span className="ms-2 text-sm font-medium text-text-muted tabular-nums">
                ({commentCount})
              </span>
            </h2>
            <button
              type="button"
              onClick={onClose}
              aria-label={t('common.close')}
              className="flex size-9 items-center justify-center rounded-full text-text-muted transition-colors hover:bg-surface-muted hover:text-text"
            >
              <CloseIcon />
            </button>
          </header>

          <ul className="flex-1 space-y-0 overflow-y-auto">
            {loading ? (
              <li className="px-5 py-10 text-center text-sm text-text-muted">
                {t('common.loading')}
              </li>
            ) : comments.length === 0 ? (
              <li className="px-5 py-10 text-center text-sm text-text-muted">
                {t('stories.noComments')}
              </li>
            ) : (
              comments.map((comment) => (
                <CommentRow key={comment.id} comment={comment} locale={locale} />
              ))
            )}
          </ul>

          <div className="border-t border-border bg-surface p-4">
            {submitSuccess ? (
              <div className="rounded-xl border border-warm/30 bg-warm-soft/40 px-4 py-3 text-sm text-text">
                <p>{t('stories.commentPending')}</p>
                <button
                  type="button"
                  className="mt-2 text-sm font-semibold text-warm hover:text-warm-hover"
                  onClick={() => {
                    setSubmitSuccess(false)
                    setSubmitError(null)
                  }}
                >
                  {t('stories.writeAnotherComment')}
                </button>
              </div>
            ) : (
              <>
                {!accessToken && (
                  <p className="mb-2 text-xs text-text-muted">{t('stories.signInToComment')}</p>
                )}
                {submitError && (
                  <p className="mb-2 text-xs text-danger">{submitError}</p>
                )}
                <div className="flex items-center gap-3 rounded-full border border-border bg-surface-muted/60 px-4 py-2.5">
                  <span className="flex size-8 shrink-0 items-center justify-center rounded-full bg-warm-soft text-xs font-semibold text-warm">
                    {authorName.charAt(0).toUpperCase()}
                  </span>
                  <input
                    type="text"
                    value={text}
                    onChange={(e) => setText(e.target.value)}
                    onKeyDown={(e) => {
                      if (e.key === 'Enter') handleSubmit()
                    }}
                    placeholder={t('stories.commentPlaceholder')}
                    className="min-w-0 flex-1 bg-transparent text-sm text-text outline-none placeholder:text-text-muted"
                  />
                  <button
                    type="button"
                    onClick={handleSubmit}
                    disabled={!text.trim() || submitting}
                    className={`shrink-0 text-sm font-semibold transition-colors ${
                      text.trim() && !submitting
                        ? 'text-warm hover:text-warm-hover'
                        : 'text-text-muted'
                    }`}
                  >
                    {t('stories.postComment')}
                  </button>
                </div>
              </>
            )}
          </div>
        </div>
      </div>
    </Portal>
  )
}

function CommentRow({
  comment,
  locale,
}: {
  comment: StoryComment
  locale: ReturnType<typeof useSettingsStore.getState>['locale']
}) {
  return (
    <li className="border-b border-border/70 px-5 py-4 last:border-b-0">
      <p className="text-sm leading-relaxed text-text">{comment.text}</p>
      <div className="mt-3 flex items-center gap-2">
        <span className="flex size-7 shrink-0 items-center justify-center rounded-full bg-warm-soft text-[10px] font-semibold text-warm">
          {comment.authorName.charAt(0).toUpperCase()}
        </span>
        <span className="truncate text-xs font-medium text-text">{comment.authorName}</span>
        <span className="text-xs text-text-muted">
          {formatRelativeDate(comment.date, locale)}
        </span>
      </div>
    </li>
  )
}
