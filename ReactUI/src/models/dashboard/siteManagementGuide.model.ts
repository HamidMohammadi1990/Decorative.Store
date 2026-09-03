export type GuideDiagramId = 'cms-hierarchy' | 'homepage-layout' | 'site-map'

export type GuideBlock =
  | { type: 'paragraph'; text: string }
  | { type: 'list'; items: string[] }
  | { type: 'ordered'; items: string[] }
  | { type: 'table'; headers: string[]; rows: string[][] }
  | { type: 'callout'; variant: 'info' | 'warning' | 'tip' | 'success'; title?: string; text: string }
  | { type: 'adminLinks'; links: { label: string; path: string }[] }
  | { type: 'storeLinks'; links: { label: string; path: string }[] }
  | { type: 'diagram'; id: GuideDiagramId }

export interface GuideSection {
  id: string
  title: string
  summary?: string
  incomplete?: boolean
  blocks: GuideBlock[]
}

export interface SiteManagementGuideContent {
  sections: GuideSection[]
}
