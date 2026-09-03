import type { AboutPageContent } from '@/models/about/aboutPage.model'
import type { ContactPageContent } from '@/models/contact/contactPage.model'
import type { CmsContentPageContent } from '@/models/content/cmsContentPage.model'
import type { ProductListingResult } from '@/models/catalog/listing.model'
import type { CatalogSearchResponse } from '@/models/catalog/catalogSearch.model'
import type { RoomTypeItem } from '@/models/admin/roomType.model'
import type { ProductReviewItem } from '@/extensions/productReviews'
import type { ProductQuestionItem } from '@/extensions/productQuestions'
import type { StoryGroup } from '@/models/stories/story.model'
import type { BlogCategory } from '@/models/blog/blog.model'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import type { HomePage } from '@/models/home/homePage.model'
import type { ProductSummary } from '@/models/catalog/product.model'
import type { BlogListingResult, BlogPostDetail, BlogPostSummary } from '@/models/blog/blog.model'
import type { Locale } from '@/models/shared/locale.model'

export interface ShopLayoutLoaderData {
  locale: Locale
  fetchKey: string
  homePage: HomePage | null
  stories: StoryGroup[]
  blogNavCategories: BlogCategory[]
  error?: 'failed'
}

export interface HomePageLoaderData {
  locale: Locale
  featuredPosts: BlogPostSummary[]
}

export interface ProductDetailLoaderData {
  locale: Locale
  slug: string
  product: ProductDetail | null
  related: ProductSummary[]
  reviews: ProductReviewItem[]
  questions: ProductQuestionItem[]
  error?: 'not-found' | 'failed'
}

export interface BlogListingLoaderData {
  locale: Locale
  categorySlug?: string
  data: BlogListingResult | null
  error?: 'failed'
}

export interface BlogDetailLoaderData {
  locale: Locale
  slug: string
  post: BlogPostDetail | null
  related: BlogPostSummary[]
  categoryMap: Record<string, string>
  error?: 'not-found' | 'failed'
}

export interface AboutPageLoaderData {
  locale: Locale
  content: AboutPageContent | null
  error?: 'failed'
}

export interface ContactPageLoaderData {
  locale: Locale
  content: ContactPageContent | null
  error?: 'failed'
}

export interface CmsContentPageLoaderData {
  locale: Locale
  slug: string
  content: CmsContentPageContent | null
  notFound?: boolean
  error?: 'failed'
}

export interface ProductListingLoaderData {
  locale: Locale
  pathname: string
  search: string
  data: ProductListingResult | null
  error?: 'failed' | 'not-found'
}

export interface SearchPageLoaderData {
  locale: Locale
  query: string
  data: CatalogSearchResponse | null
  error?: 'failed'
}

export interface RoomLayoutLoaderData {
  locale: Locale
  languageId: number
  items: RoomTypeItem[]
  fromApi: boolean
  error?: 'failed'
}

export interface ComparePageLoaderData {
  locale: Locale
  slugs: string[]
  products: ProductDetail[]
  error?: 'failed'
}
