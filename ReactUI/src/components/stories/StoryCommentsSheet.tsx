import { useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import type { StoryComment } from '@/models/stories/story.model'
import { formatRelativeDate } from '@/extensions/formatRelativeDate'
import {
  getDisplayedCommentLikes,
  useStoryInteractionStore,
} from '@/stores/storyInteractionStore'
import { useSettingsStore } from '@/stores/settingsStore'
import { useUserStore } from '@/stores/userStore'
import { CloseIcon } from '@/components/ui/CloseIcon'
import { Portal } from '@/components/ui/Portal'

const EMPTY_COMMENTS: StoryComment[] = []

interface StoryCommentsSheetProps {
  storyId: string
  baseComments: StoryComment[]
  isOpen: boolean
  onClose: () => void
}

export function StoryCommentsSheet({
  storyId,
  baseComments,
  isOpen,
  onClose,
}: StoryCommentsSheetProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const user = useUserStore((s) => s.user)
  const [text, setText] = useState('')
  const addComment = useStoryInteractionStore((s) => s.addComment)
  const userComments = useStoryInteractionStore(
    (s) => s.userComments[storyId] ?? EMPTY_COMMENTS,
  )

  const allComments = useMemo(
    () => [...userComments, ...baseComments],
    [baseComments, userComments],
  )

  if (!isOpen) return null

  const authorName = user
    ? `${user.firstName}${user.lastName ? ` ${user.lastName}` : ''}`
    : t('stories.guestAuthor')

  const handleSubmit = () => {
    const trimmed = text.trim()
    if (!trimmed) return
    addComment(storyId, trimmed, authorName)
    setText('')
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
            {allComments.length === 0 ? (
              <li className="px-5 py-10 text-center text-sm text-text-muted">
                {t('stories.noComments')}
              </li>
            ) : (
              allComments.map((comment) => (
                <CommentRow key={comment.id} comment={comment} locale={locale} />
              ))
            )}
          </ul>

          <div className="border-t border-border bg-surface p-4">
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
                disabled={!text.trim()}
                className={`shrink-0 text-sm font-semibold transition-colors ${
                  text.trim() ? 'text-warm hover:text-warm-hover' : 'text-text-muted'
                }`}
              >
                {t('stories.postComment')}
              </button>
            </div>
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
  const { t } = useTranslation()
  const toggleLike = useStoryInteractionStore((s) => s.toggleCommentLike)
  const liked = useStoryInteractionStore((s) => s.isCommentLiked(comment.id))
  const likes = getDisplayedCommentLikes(comment.likes, comment.id)

  return (
    <li className="border-b border-border/70 px-5 py-4 last:border-b-0">
      <p className="text-sm leading-relaxed text-text">{comment.text}</p>
      <div className="mt-3 flex items-center justify-between gap-3">
        <div className="flex min-w-0 items-center gap-2">
          <span className="flex size-7 shrink-0 items-center justify-center rounded-full bg-warm-soft text-[10px] font-semibold text-warm">
            {comment.authorName.charAt(0).toUpperCase()}
          </span>
          <span className="truncate text-xs font-medium text-text">{comment.authorName}</span>
          <span className="text-xs text-text-muted">
            {formatRelativeDate(comment.date, locale)}
          </span>
        </div>
        <button
          type="button"
          onClick={() => toggleLike(comment.id)}
          aria-pressed={liked}
          aria-label={t('stories.likeComment')}
          className={`inline-flex items-center gap-1 text-xs transition-colors ${
            liked ? 'text-warm' : 'text-text-muted hover:text-warm'
          }`}
        >
          <HeartIcon filled={liked} />
          <span className="tabular-nums">{likes}</span>
        </button>
      </div>
    </li>
  )
}

function HeartIcon({ filled }: { filled: boolean }) {
  return (
    <svg width="14" height="14" viewBox="0 0 14 14" fill={filled ? 'currentColor' : 'none'} aria-hidden>
      <path
        d="M7 12.2S2.8 9.2 2.8 5.8A2.6 2.6 0 0 1 7 4.2a2.6 2.6 0 0 1 4.2 1.6C11.2 9.2 7 12.2 7 12.2Z"
        stroke="currentColor"
        strokeWidth="1.2"
      />
    </svg>
  )
}
