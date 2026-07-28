import { getBlogMock } from '@/data/mock'
import type { ImageAsset } from '@/models/shared/image.model'
import type {
  BlogComment,
  BlogListingResult,
  BlogPostDetail,
  BlogPostSummary,
} from '@/models/blog/blog.model'
import type { Locale } from '@/models/shared/locale.model'
import { mockFetch } from '@/services/api/mockClient'

interface BlogMockPost extends BlogPostSummary {
  content: string[]
  gallery: ImageAsset[]
  comments: BlogComment[]
  relatedSlugs: string[]
}

function toSummary(post: BlogMockPost): BlogPostSummary {
  const {
    id,
    slug,
    title,
    excerpt,
    coverImage,
    categorySlug,
    authorId,
    publishedAt,
    readTimeMinutes,
    likes,
    commentCount,
    featured,
    tags,
  } = post

  return {
    id,
    slug,
    title,
    excerpt,
    coverImage,
    categorySlug,
    authorId,
    publishedAt,
    readTimeMinutes,
    likes,
    commentCount,
    featured,
    tags,
  }
}

export const blogService = {
  async getNavCategories(locale: Locale) {
    return mockFetch(async () => {
      const data = getBlogMock(locale)

      return data.categories.map((category) => ({
        ...category,
        count: data.posts.filter((post) => post.categorySlug === category.slug).length,
      }))
    })
  },

  async getListing(locale: Locale, categorySlug?: string): Promise<BlogListingResult> {
    return mockFetch(async () => {
      const data = getBlogMock(locale)
      const categories = data.categories.map((category) => ({
        ...category,
        count: data.posts.filter((post) => post.categorySlug === category.slug).length,
      }))

      let posts = data.posts.map(toSummary)

      if (categorySlug) {
        posts = posts.filter((post) => post.categorySlug === categorySlug)
      }

      posts.sort(
        (a, b) =>
          new Date(b.publishedAt).getTime() - new Date(a.publishedAt).getTime(),
      )

      const featuredPosts = data.posts
        .filter((post) => post.featured)
        .map(toSummary)
        .slice(0, 5)

      return {
        title: locale === 'fa' ? 'مجله دیبا گالری' : 'Diba Gallery Journal',
        description:
          locale === 'fa'
            ? 'ایده‌های طراحی داخلی، نکات چیدمان و الهام فصلی از استودیوی دیبا گالری.'
            : 'Interior design ideas, styling tips, and seasonal inspiration from the Diba Gallery studio.',
        categories,
        featuredPosts,
        posts,
        totalCount: posts.length,
        activeCategory: categorySlug,
      }
    })
  },

  async getPost(slug: string, locale: Locale): Promise<BlogPostDetail | null> {
    return mockFetch(async () => {
      const data = getBlogMock(locale)
      const raw = data.posts.find((post) => post.slug === slug)
      if (!raw) return null

      const author = data.authors.find((item) => item.id === raw.authorId)
      if (!author) return null

      return {
        ...toSummary(raw),
        content: raw.content,
        gallery: raw.gallery.length > 0 ? raw.gallery : [raw.coverImage],
        author,
        comments: raw.comments,
        relatedSlugs: raw.relatedSlugs,
      }
    })
  },

  async getRelatedPosts(slug: string, locale: Locale): Promise<BlogPostSummary[]> {
    return mockFetch(async () => {
      const data = getBlogMock(locale)
      const current = data.posts.find((post) => post.slug === slug)
      if (!current) return []

      return current.relatedSlugs
        .map((relatedSlug) => data.posts.find((post) => post.slug === relatedSlug))
        .filter((post): post is BlogMockPost => Boolean(post))
        .map(toSummary)
    })
  },
}
