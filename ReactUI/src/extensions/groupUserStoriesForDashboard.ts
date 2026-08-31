import type { UserStoryDraft } from '@/models/stories/story.model'

const BATCH_WINDOW_MS = 2 * 60 * 1000

export interface DashboardStoryGroup {
  id: string
  title: string
  caption: string
  slides: UserStoryDraft[]
  isActive: boolean
  productSlugs: string[]
  createdAt: string
}

function withinBatch(createdAtA: string, createdAtB: string) {
  const a = new Date(createdAtA).getTime()
  const b = new Date(createdAtB).getTime()
  if (Number.isNaN(a) || Number.isNaN(b)) return false
  return Math.abs(a - b) <= BATCH_WINDOW_MS
}

function matchesBatch(story: UserStoryDraft, group: DashboardStoryGroup) {
  const productA = story.productSlugs[0] ?? ''
  const productB = group.productSlugs[0] ?? ''
  if (story.title.trim() !== group.title.trim()) return false
  if (story.caption.trim() !== group.caption.trim()) return false
  if (productA !== productB) return false
  if (story.isActive !== group.isActive) return false

  return group.slides.some((slide) => withinBatch(story.createdAt, slide.createdAt))
}

/** Groups stories published together (same metadata + close created time) into one dashboard card. */
export function groupUserStoriesForDashboard(stories: UserStoryDraft[]): DashboardStoryGroup[] {
  const sorted = [...stories].sort((a, b) => b.createdAt.localeCompare(a.createdAt))
  const groups: DashboardStoryGroup[] = []

  for (const story of sorted) {
    const existing = groups.find((group) => matchesBatch(story, group))

    if (existing) {
      existing.slides.push(story)
      existing.slides.sort((a, b) => a.createdAt.localeCompare(b.createdAt))
      continue
    }

    groups.push({
      id: story.id,
      title: story.title,
      caption: story.caption,
      slides: [story],
      isActive: story.isActive,
      productSlugs: story.productSlugs,
      createdAt: story.createdAt,
    })
  }

  return groups
}
