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

  return {
    id: readStringField(record, 'id', 'Id'),
    slug: readStringField(record, 'slug', 'Slug'),
    title: readStringField(record, 'title', 'Title'),
    excerpt: metaDescription,
    coverImage: fallbackCover,
    categorySlug,
    categoryLabel: readStringField(record, 'categoryTitle', 'CategoryTitle'),
    authorId: readStringField(record, 'userId', 'UserId'),
    publishedAt,
    readTimeMinutes: readNumberField(record, 'readingTimeInMinutes', 'ReadingTimeInMinutes'),
    likes: 0,
    commentCount: 0,
    featured: false,
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
  const coverImage = { ...fallbackCover, alt: summary.title }
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
    gallery: [coverImage],
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

export function normalizeBlogPostDetailPage(data: unknown): {
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
  const post = normalizeBlogPostDetail(postRecord, DEFAULT_DETAIL_COVER, categorySlug, categoryLabel)

  const comments = (Array.isArray(record.comments ?? record.Comments)
    ? (record.comments ?? record.Comments)
    : []
  ).map(normalizeBlogComment)

  const commentCount =
    readNumberField(postRecord, 'commentCount', 'CommentCount') || comments.length

  const related = (Array.isArray(record.relatedPosts ?? record.RelatedPosts)
    ? (record.relatedPosts ?? record.RelatedPosts)
    : []
  ).map((item) => {
    const relatedRecord = readRecord(item) ?? {}
    const relatedCategorySlug = readStringField(relatedRecord, 'categorySlug', 'CategorySlug')
    return normalizeBlogPostSummary(item, DEFAULT_DETAIL_COVER, relatedCategorySlug)
  })

  return {
    post: {
      ...post,
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
