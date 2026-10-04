import { useTranslation } from 'react-i18next'
import type { AdminBlogPostListItem } from '@/models/admin/blog.model'
import { BlogPostImagesSection } from '@/components/dashboard/BlogPostImagesSection'
import { AdminLargeModal } from '@/components/dashboard/admin/AdminLargeModal'

export type BlogPostManageTab = 'images'

export function BlogPostManageModal({
  open,
  tab,
  post,
  onClose,
}: {
  open: boolean
  tab: BlogPostManageTab
  post: AdminBlogPostListItem | null
  onClose: () => void
}) {
  const { t } = useTranslation()

  if (!post) return null

  return (
    <AdminLargeModal
      open={open}
      title={t('dashboard.blogPosts.images.title')}
      description={`${t('dashboard.blogPosts.images.description')} · ${post.title}`}
      onClose={onClose}
    >
      {tab === 'images' && (
        <BlogPostImagesSection blogPostId={post.id} blogPostTitle={post.title} />
      )}
    </AdminLargeModal>
  )
}
