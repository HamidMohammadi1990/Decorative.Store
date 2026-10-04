import type { Locale } from '@/models/shared/locale.model'
import { blogPostCommentService } from '@/services/blogPostCommentService'
import { useUserStore } from '@/stores/userStore'

export async function submitBlogComment(input: {
  blogPostId: string
  content: string
  locale: Locale
}) {
  let accessToken = useUserStore.getState().accessToken
  if (!accessToken) {
    throw new Error('missing-access-token')
  }

  const refreshedToken = await useUserStore.getState().refreshAccessToken()
  if (refreshedToken) {
    accessToken = refreshedToken
  }

  return blogPostCommentService.create(
    {
      blogPostId: input.blogPostId,
      content: input.content,
    },
    input.locale,
    accessToken,
  )
}
