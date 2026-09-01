import { useState, type ReactNode } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type {
  ContactFormField,
  ContactFormIntro,
  ContactFormValues,
  ContactHero,
  ContactLocation,
  ContactMethod,
  ContactPageContent,
} from '@/models/contact/contactPage.model'
import { Button } from '@/components/ui/Button'
import { Container } from '@/components/ui/Container'
import { showToast } from '@/stores/toastStore'

const METHOD_ICONS: Record<string, string> = {
  phone: '☎',
  email: '✉',
  chat: '💬',
  design: '◈',
}

function isExternalHref(href: string) {
  return href.startsWith('http') || href.startsWith('mailto:') || href.startsWith('tel:')
}

function ContactLink({
  href,
  className,
  children,
}: {
  href: string
  className?: string
  children: ReactNode
}) {
  if (!href) return <span className={className}>{children}</span>

  if (isExternalHref(href)) {
    return (
      <a href={href} className={className}>
        {children}
      </a>
    )
  }

  return (
    <Link to={href} className={className}>
      {children}
    </Link>
  )
}

export function ContactHeroSection({ data }: { data: ContactHero }) {
  return (
    <section className="border-b border-border bg-surface-muted py-14 md:py-20">
      <Container className="max-w-3xl text-center">
        {data.eyebrow && (
          <p className="text-xs font-semibold uppercase tracking-[0.2em] text-warm">{data.eyebrow}</p>
        )}
        <h1 className="mt-3 font-display text-4xl font-semibold tracking-tight text-text md:text-5xl">
          {data.title}
        </h1>
        {data.subtitle && (
          <p className="mt-5 text-base leading-relaxed text-text-muted md:text-lg">{data.subtitle}</p>
        )}
      </Container>
    </section>
  )
}

export function ContactMethodsSection({
  heading,
  items,
}: {
  heading: string
  items: ContactMethod[]
}) {
  const { t } = useTranslation()
  if (items.length === 0) return null

  return (
    <section className="border-b border-border bg-surface py-14 md:py-16">
      <Container>
        <h2 className="text-center font-display text-2xl font-semibold tracking-tight text-text md:text-3xl">
          {heading}
        </h2>
        <div className="mt-10 grid gap-5 sm:grid-cols-2 lg:grid-cols-4">
          {items.map((item) => (
            <ContactLink
              key={item.id}
              href={item.href}
              className="group flex h-full flex-col rounded-sm border border-border bg-surface-muted/40 p-6 transition-colors hover:border-warm/40 hover:bg-surface-muted/70"
            >
              <span className="inline-flex size-11 items-center justify-center rounded-full bg-warm-soft text-lg text-warm transition-transform group-hover:scale-105">
                {METHOD_ICONS[item.id] ?? '◎'}
              </span>
              <h3 className="mt-4 text-base font-semibold text-text">{item.title}</h3>
              <p className="mt-2 flex-1 text-sm leading-relaxed text-text-muted">{item.description}</p>
              <span className="mt-4 text-xs font-semibold uppercase tracking-wider text-warm">
                {item.href.startsWith('tel:')
                  ? item.href.replace('tel:', '')
                  : t('contactPage.form.viewLink')}
              </span>
            </ContactLink>
          ))}
        </div>
      </Container>
    </section>
  )
}

export function ContactLocationsSection({
  heading,
  items,
}: {
  heading: string
  items: ContactLocation[]
}) {
  const { t } = useTranslation()
  if (items.length === 0) return null

  return (
    <section className="border-b border-border bg-surface-muted py-14 md:py-16">
      <Container>
        <h2 className="text-center font-display text-2xl font-semibold tracking-tight text-text md:text-3xl">
          {heading}
        </h2>
        <div className="mt-10 grid gap-5 md:grid-cols-3">
          {items.map((item) => (
            <article
              key={item.title}
              className="flex h-full flex-col rounded-sm border border-border bg-surface p-6 shadow-sm"
            >
              <h3 className="text-lg font-semibold text-text">{item.title}</h3>
              <p className="mt-3 text-sm leading-relaxed text-text-muted">{item.address}</p>
              <p className="mt-2 text-sm text-warm">{item.hours}</p>
              {item.mapHref && (
                <a
                  href={item.mapHref}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="mt-5 text-sm font-semibold text-warm hover:underline"
                >
                  {t('contactPage.form.mapLink')} ↗
                </a>
              )}
            </article>
          ))}
        </div>
      </Container>
    </section>
  )
}

const EMPTY_FORM: ContactFormValues = {
  name: '',
  email: '',
  subject: '',
  message: '',
}

export function ContactFormSection({ data }: { data: ContactFormIntro }) {
  const { t } = useTranslation()
  const [form, setForm] = useState<ContactFormValues>(EMPTY_FORM)
  const [errors, setErrors] = useState<Partial<Record<ContactFormField, string>>>({})
  const [submitting, setSubmitting] = useState(false)

  function updateField(field: ContactFormField, value: string) {
    setForm((prev) => ({ ...prev, [field]: value }))
    setErrors((prev) => ({ ...prev, [field]: undefined }))
  }

  function validate(): boolean {
    const next: Partial<Record<ContactFormField, string>> = {}
    if (!form.name.trim()) next.name = t('contactPage.form.required')
    if (!form.email.trim()) next.email = t('contactPage.form.required')
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email.trim())) {
      next.email = t('contactPage.form.invalidEmail')
    }
    if (!form.subject.trim()) next.subject = t('contactPage.form.required')
    if (!form.message.trim()) next.message = t('contactPage.form.required')
    setErrors(next)
    return Object.keys(next).length === 0
  }

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (!validate()) return

    setSubmitting(true)
    try {
      const recipient = data.email.trim() || 'support@dibagallery.com'
      const body = [
        `${t('contactPage.form.name')}: ${form.name.trim()}`,
        `${t('contactPage.form.email')}: ${form.email.trim()}`,
        '',
        form.message.trim(),
      ].join('\n')

      const mailto = `mailto:${encodeURIComponent(recipient)}?subject=${encodeURIComponent(form.subject.trim())}&body=${encodeURIComponent(body)}`
      window.location.href = mailto
      showToast(t('contactPage.form.success'), 'success')
      setForm(EMPTY_FORM)
    } finally {
      setSubmitting(false)
    }
  }

  const inputClass =
    'w-full rounded-sm border border-border bg-surface px-3 py-2.5 text-sm text-text placeholder:text-text-muted/70 focus:border-warm focus:outline-none focus:ring-1 focus:ring-warm/30'

  return (
    <section className="bg-surface py-14 md:py-20">
      <Container>
        <div className="mx-auto grid max-w-5xl gap-10 lg:grid-cols-[1fr_1.1fr] lg:gap-16">
          <div>
            <h2 className="font-display text-2xl font-semibold tracking-tight text-text md:text-3xl">
              {data.heading}
            </h2>
            {data.lead && (
              <p className="mt-4 text-base leading-relaxed text-text-muted">{data.lead}</p>
            )}
            {data.note && (
              <p className="mt-6 rounded-sm border border-warm/20 bg-warm-soft/50 px-4 py-3 text-sm leading-relaxed text-text-muted">
                {data.note}
              </p>
            )}
            {data.email && (
              <p className="mt-6 text-sm text-text-muted">
                {t('contactPage.form.directEmail')}{' '}
                <a href={`mailto:${data.email}`} className="font-semibold text-warm hover:underline">
                  {data.email}
                </a>
              </p>
            )}
          </div>

          <form
            onSubmit={handleSubmit}
            className="rounded-sm border border-border bg-surface-muted/30 p-6 md:p-8"
            noValidate
          >
            <div className="grid gap-4 sm:grid-cols-2">
              <div className="sm:col-span-1">
                <label htmlFor="contact-name" className="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-text-muted">
                  {t('contactPage.form.name')}
                </label>
                <input
                  id="contact-name"
                  type="text"
                  value={form.name}
                  onChange={(event) => updateField('name', event.target.value)}
                  className={inputClass}
                  autoComplete="name"
                  disabled={submitting}
                />
                {errors.name && <p className="mt-1 text-xs text-sale">{errors.name}</p>}
              </div>
              <div className="sm:col-span-1">
                <label htmlFor="contact-email" className="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-text-muted">
                  {t('contactPage.form.email')}
                </label>
                <input
                  id="contact-email"
                  type="email"
                  value={form.email}
                  onChange={(event) => updateField('email', event.target.value)}
                  className={inputClass}
                  autoComplete="email"
                  disabled={submitting}
                />
                {errors.email && <p className="mt-1 text-xs text-sale">{errors.email}</p>}
              </div>
            </div>

            <div className="mt-4">
              <label htmlFor="contact-subject" className="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-text-muted">
                {t('contactPage.form.subject')}
              </label>
              <input
                id="contact-subject"
                type="text"
                value={form.subject}
                onChange={(event) => updateField('subject', event.target.value)}
                className={inputClass}
                disabled={submitting}
              />
              {errors.subject && <p className="mt-1 text-xs text-sale">{errors.subject}</p>}
            </div>

            <div className="mt-4">
              <label htmlFor="contact-message" className="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-text-muted">
                {t('contactPage.form.message')}
              </label>
              <textarea
                id="contact-message"
                rows={5}
                value={form.message}
                onChange={(event) => updateField('message', event.target.value)}
                className={`${inputClass} resize-y min-h-[8rem]`}
                disabled={submitting}
              />
              {errors.message && <p className="mt-1 text-xs text-sale">{errors.message}</p>}
            </div>

            <div className="mt-6">
              <Button type="submit" variant="primary" disabled={submitting} className="min-w-[10rem]">
                {t('contactPage.form.submit')}
              </Button>
            </div>
          </form>
        </div>
      </Container>
    </section>
  )
}

export function ContactPageSections({ content }: { content: ContactPageContent }) {
  return (
    <>
      <ContactHeroSection data={content.hero} />
      <ContactMethodsSection heading={content.methods.heading} items={content.methods.items} />
      <ContactLocationsSection heading={content.locations.heading} items={content.locations.items} />
      <ContactFormSection data={content.formIntro} />
    </>
  )
}
