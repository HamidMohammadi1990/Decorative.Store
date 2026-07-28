import type { HomePage } from '@/models/home/homePage.model'

export function mapHomePage(raw: unknown): HomePage {
  return raw as HomePage
}
