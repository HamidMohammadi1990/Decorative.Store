import type {
  BlogAuthor,
  BlogCategory,
  BlogComment,
  BlogPostDetail,
  BlogPostSummary,
} from '@/models/blog/blog.model'
import type { ImageAsset } from '@/models/shared/image.model'
import {
  readNumberField,
  readOptionalStringField,
  readRecord,
  readStringField,
} from '@/services/api/apiNormalize'
import { API_BASE_URL } from '@/config/api'

function resolveBlogImageUrl(url: string): string {
  if (!url) return ''
  if (url.startsWith('http://') || url.startsWith('https://')) return url
  if (url.startsWith('/')) return `${API_BASE_URL}${url}`
  return `${API_BASE_URL}/${url}`
}

function toImageAsset(src: string, alt: string): ImageAsset {
  return { src: resolveBlogImageUrl(src), alt }
}

function readCoverImage(record: Record<string, unknown>, fallback: ImageAsset, alt: string): ImageAsset {
  const coverImageUrl = readOptionalStringField(record, 'coverImageUrl', 'CoverImageUrl')
  if (coverImageUrl) return toImageAsset(coverImageUrl, alt)
  return { ...fallback, alt: alt || fallback.alt }
}

function readGalleryImages(record: Record<string, unknown>, fallback: ImageAsset, alt: string): ImageAsset[] {
  const images = record.images ?? record.Images
  if (!Array.isArray(images) || images.length === 0) {
    return [fallback]
  }

  const gallery = images
    .map((item) => {
      const imageRecord = readRecord(item)
      if (!imageRecord) return null
      const imageUrl = readStringField(imageRecord, 'imageUrl', 'ImageUrl')
      if (!imageUrl) return null
      const title = readStringField(imageRecord, 'title', 'Title')
      return toImageAsset(imageUrl, title || alt)
    })
    .filter((item): item is ImageAsset => item !== null)

  return gallery.length > 0 ? gallery : [fallback]
}

function readBooleanField(record: Record<string, unknown>, ...keys: string[]) {
  for (const key of keys) {
    const value = record[key]
    if (typeof value === 'boolean') return value
  }
  return false
}

function readStringListField(record: Record<string, unknown>, ...keys: string[]) {
  for (const key of keys) {
    const value = record[key]
    if (!Array.isArray(value)) continue
    return value.filter((item): item is string => typeof item === 'string' && item.length > 0)
  }
  return []
}

export function parseBlogContent(content: string): string[] {
  return content
    .split(/\n{2,}/)
    .map((paragraph) => paragraph.trim())
    .filter((paragraph) => paragraph.length > 0)
}

export function parseSeoKeywords(keywords: string): string[] {
  return keywords
    .split(',')
    .map((keyword) => keyword.trim())
    .filter((keyword) => keyword.length > 0)
}

function readAuthorName(record: Record<string, unknown>) {
  const firstName = readStringField(record, 'userFirstName', 'UserFirstName')
  const lastName = readStringField(record, 'userLastName', 'UserLastName')
  return [firstName, lastName].filter(Boolean).join(' ')
}

export function normalizeBlogPostCategory(data: unknown): BlogCategory {
  const record = readRecord(data) ?? {}

  return {
    id: readStringField(record, 'id', 'Id'),
    slug: readStringField(record, 'slug', 'Slug'),
    label: readStringField(record, 'title', 'Title'),
    count: readNumberField(record, 'postCount', 'PostCount'),
  }
}

export function readBlogPostCategoryId(data: unknown): string {
  const record = readRecord(data) ?? {}
  return readStringField(record, 'categoryId', 'CategoryId')
}

export function normalizeBlogPostSummary(
  data: unknown,
  fallbackCover: ImageAsset,
  categorySlug = '',
): BlogPostSummary {
  const record = readRecord(data) ?? {}
  const metaDescription = readStringField(record, 'metaDescription', 'MetaDescription')
  const publishedAt =
    readOptionalStringField(record, 'publishedOnUtc', 'PublishedOnUtc') ??
    readOptionalStringField(record, 'createdOnUtc', 'CreatedOnUtc') ??
    new Date().toISOString()

  const seoKeywords = readStringField(record, 'seoKeywords', 'SeoKeywords')
  const featured = readBooleanField(record, 'isFeatured', 'IsFeatured')
  const title = readStringField(record, 'title', 'Title')
  const coverImage = readCoverImage(record, fallbackCover, title)

  return {
    id: readStringField(record, 'id', 'Id'),
    slug: readStringField(record, 'slug', 'Slug'),
    title,
    excerpt: metaDescription,
    coverImage,
    categorySlug,
    categoryLabel: readStringField(record, 'categoryTitle', 'CategoryTitle'),
    authorId: readStringField(record, 'userId', 'UserId'),
    publishedAt,
    readTimeMinutes: readNumberField(record, 'readingTimeInMinutes', 'ReadingTimeInMinutes'),
    likes: readNumberField(record, 'likeCount', 'LikeCount'),
    commentCount: readNumberField(record, 'commentCount', 'CommentCount'),
    featured,
    tags: parseSeoKeywords(seoKeywords),
  }
}

export function normalizeBlogPostDetail(
  data: unknown,
  fallbackCover: ImageAsset,
  categorySlug = '',
  categoryLabel = '',
): BlogPostDetail {
  const record = readRecord(data) ?? {}
  const summary = normalizeBlogPostSummary(data, fallbackCover, categorySlug)
  const content = parseBlogContent(readStringField(record, 'content', 'Content'))
  const coverImage = summary.coverImage
  const gallery = readGalleryImages(record, coverImage, summary.title)
  const authorName = readAuthorName(record)
  const author: BlogAuthor = {
    id: summary.authorId,
    name: authorName || summary.title,
    role: '',
    avatar: coverImage,
    bio: '',
  }

  return {
    ...summary,
    categoryId: readStringField(record, 'categoryId', 'CategoryId'),
    categoryLabel: categoryLabel || readStringField(record, 'categoryTitle', 'CategoryTitle'),
    content: content.length > 0 ? content : [summary.excerpt].filter(Boolean),
    gallery,
    author,
    comments: [],
  }
}

export function normalizeBlogComment(data: unknown): BlogComment {
  const record = readRecord(data) ?? {}
  const firstName = readStringField(record, 'createdByUserFirstName', 'CreatedByUserFirstName')
  const lastName = readStringField(record, 'createdByUserLastName', 'CreatedByUserLastName')
  const authorName = [firstName, lastName].filter(Boolean).join(' ')

  return {
    id: readStringField(record, 'id', 'Id'),
    authorName,
    date:
      readOptionalStringField(record, 'approvedOnUtc', 'ApprovedOnUtc') ??
      readOptionalStringField(record, 'createdOnUtc', 'CreatedOnUtc') ??
      new Date().toISOString(),
    text: readStringField(record, 'content', 'Content'),
    likes: 0,
  }
}

function readCategoryLabels(data: unknown): Record<string, string> {
  const record = readRecord(data) ?? {}
  const labels = record.categoryLabels ?? record.CategoryLabels
  if (!labels || typeof labels !== 'object') return {}

  return Object.fromEntries(
    Object.entries(labels as Record<string, unknown>)
      .filter(([, value]) => typeof value === 'string' && value.length > 0)
      .map(([key, value]) => [key, String(value)]),
  )
}

export function normalizeBlogPostDetailPage(
  data: unknown,
  requestSlug = '',
): {
  post: BlogPostDetail | null
  related: BlogPostSummary[]
  categoryMap: Record<string, string>
} {
  const record = readRecord(data) ?? {}
  const notFound = Boolean(record.notFound ?? record.NotFound)
  if (notFound) {
    return { post: null, related: [], categoryMap: readCategoryLabels(data) }
  }

  const postRecord = readRecord(record.post ?? record.Post)
  if (!postRecord) {
    return { post: null, related: [], categoryMap: readCategoryLabels(data) }
  }

  const categorySlug = readStringField(postRecord, 'categorySlug', 'CategorySlug')
  const categoryLabel = readStringField(postRecord, 'categoryTitle', 'CategoryTitle')
  const tagTitles = readStringListField(postRecord, 'tagTitles', 'TagTitles')
  const post = normalizeBlogPostDetail(postRecord, DEFAULT_DETAIL_COVER, categorySlug, categoryLabel)
  const imagesFromDetail = postRecord.images ?? postRecord.Images
  const postWithImages =
    Array.isArray(imagesFromDetail) && imagesFromDetail.length > 0
      ? {
          ...post,
          coverImage: readGalleryImages(postRecord, post.coverImage, post.title)[0] ?? post.coverImage,
          gallery: readGalleryImages(postRecord, post.coverImage, post.title),
        }
      : post
  const resolvedSlug = post.slug || requestSlug.trim()
  const tags = tagTitles.length > 0 ? tagTitles : post.tags

  const commentsRaw = record.comments ?? record.Comments
  const comments = (Array.isArray(commentsRaw) ? commentsRaw : []).map(normalizeBlogComment)

  const commentCount =
    readNumberField(postRecord, 'commentCount', 'CommentCount') || comments.length

  const relatedRaw = record.relatedPosts ?? record.RelatedPosts
  const related = (Array.isArray(relatedRaw) ? relatedRaw : []).map((item: unknown) => {
    const relatedRecord = readRecord(item) ?? {}
    const relatedCategorySlug = readStringField(relatedRecord, 'categorySlug', 'CategorySlug')
    return normalizeBlogPostSummary(item, DEFAULT_DETAIL_COVER, relatedCategorySlug)
  })

  return {
    post: {
      ...postWithImages,
      slug: resolvedSlug,
      tags,
      comments,
      commentCount,
    },
    related,
    categoryMap: readCategoryLabels(data),
  }
}

const DEFAULT_DETAIL_COVER = {
  src: '/images/home/living-room.jpg',
  alt: '',
}
