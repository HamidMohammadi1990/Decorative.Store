import { useCallback, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { ApiError } from '@/services/api/apiTypes'
import { blogPostLikeService } from '@/services/blogPostLikeService'
import { useBlogInteractionStore } from '@/stores/blogInteractionStore'
import { useSettingsStore } from '@/stores/settingsStore'
import { useAccessToken } from '@/stores/userStore'

interface BlogLikeButtonProps {
  postId: string
  baseLikes: number
  size?: 'sm' | 'md'
}

export function BlogLikeButton({ postId, baseLikes, size = 'md' }: BlogLikeButtonProps) {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const accessToken = useAccessToken()
  const liked = useBlogInteractionStore((s) => s.likedPostIds.includes(postId))
  const markPostLiked = useBlogInteractionStore((s) => s.markPostLiked)
  const [pending, setPending] = useState(false)
  const [extraLike, setExtraLike] = useState(0)
  const count = baseLikes + extraLike

  const submitLike = useCallback(async () => {
    setPending(true)

    try {
      await blogPostLikeService.create(postId, locale, accessToken)
      markPostLiked(postId)
      setExtraLike(1)
    } catch (error) {
      if (error instanceof ApiError && error.hasCode('DuplicateLike')) {
        markPostLiked(postId)
        return
      }
    } finally {
      setPending(false)
    }
  }, [accessToken, locale, markPostLiked, postId])

  const handleClick = () => {
    if (liked || pending) return
    void submitLike()
  }

  const dim = size === 'sm' ? 'size-9 text-sm' : 'h-11 px-4 text-sm'

  return (
    <button
      type="button"
      onClick={handleClick}
      disabled={liked || pending}
      aria-pressed={liked}
      aria-label={t('blog.likeArticle')}
      className={`inline-flex items-center justify-center gap-2 rounded-full border transition-colors ${dim} ${
        liked
          ? 'border-warm bg-warm-soft text-warm'
          : 'border-border bg-surface text-text-muted hover:border-warm hover:text-warm disabled:cursor-default'
      }`}
    >
      <HeartIcon filled={liked} />
      <span className="font-semibold tabular-nums">{count}</span>
    </button>
  )
}

function HeartIcon({ filled }: { filled: boolean }) {
  return (
    <svg width="18" height="18" viewBox="0 0 18 18" fill={filled ? 'currentColor' : 'none'} aria-hidden>
      <path
        d="M9 15.2S3.5 11.4 3.5 7.4A3.1 3.1 0 0 1 9 5.3a3.1 3.1 0 0 1 5.5 2.1C14.5 11.4 9 15.2 9 15.2Z"
        stroke="currentColor"
        strokeWidth="1.4"
        strokeLinejoin="round"
      />
    </svg>
  )
}
