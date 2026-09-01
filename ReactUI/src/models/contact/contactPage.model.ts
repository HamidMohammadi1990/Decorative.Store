export interface ContactHero {
  eyebrow?: string
  title: string
  subtitle?: string
}

export interface ContactMethod {
  id: string
  title: string
  description: string
  href: string
}

export interface ContactLocation {
  title: string
  address: string
  hours: string
  mapHref: string
}

export interface ContactFormIntro {
  heading: string
  lead?: string
  email: string
  note?: string
}

export interface ContactPageContent {
  slug: string
  title: string
  metaTitle?: string | null
  metaDescription?: string | null
  hero: ContactHero
  methods: { heading: string; items: ContactMethod[] }
  locations: { heading: string; items: ContactLocation[] }
  formIntro: ContactFormIntro
}

export interface ContactFormValues {
  name: string
  email: string
  subject: string
  message: string
}

export type ContactFormField = keyof ContactFormValues
