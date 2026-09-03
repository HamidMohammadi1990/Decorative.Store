import { StrictMode } from 'react'
import { renderToReadableStream } from 'react-dom/server'
import {
  createStaticHandler,
  createStaticRouter,
  StaticRouterProvider,
} from 'react-router'
import { AppProviders } from '@/providers/AppProviders'
import { createAppRoutes } from '@/routes/routeConfig'
import { consumeSsrPageMeta, resetSsrPageMeta } from '@/seo/ssrPageMeta'
import { serializePageMetaToHtml, setPageMetaDefaults } from '@/seo/pageMetaManager'
import { buildPageTitle, DEFAULT_DESCRIPTION } from '@/config/site'
import { syncI18nLocale } from '@/i18n'
import { resolveLocale } from '@/ssr/resolveLocale'
import type { Locale } from '@/models/shared/locale.model'
import '@/i18n'

setPageMetaDefaults(buildPageTitle(undefined, 'en'), DEFAULT_DESCRIPTION.en)

function serializeRouterHydration(context: {
  loaderData?: unknown
  actionData?: unknown
  errors?: unknown
}) {
  return {
    loaderData: context.loaderData ?? {},
    actionData: context.actionData ?? null,
    errors: context.errors ?? null,
  }
}

function collectLazyRouteIds(context: {
  matches?: Array<{ route: { id?: string } }>
}): string[] {
  return (
    context.matches
      ?.map((match) => match.route.id)
      .filter((id): id is string => Boolean(id)) ?? []
  )
}

async function streamToString(stream: ReadableStream<Uint8Array>) {
  const reader = stream.getReader()
  const decoder = new TextDecoder()
  let html = ''
  while (true) {
    const { done, value } = await reader.read()
    if (done) break
    html += decoder.decode(value, { stream: true })
  }
  html += decoder.decode()
  return html
}

export async function render(request: Request) {
  resetSsrPageMeta()
  const locale = resolveLocale(request)
  syncI18nLocale(locale)
  setPageMetaDefaults(buildPageTitle(undefined, locale), DEFAULT_DESCRIPTION[locale])

  const routes = createAppRoutes()
  const handler = createStaticHandler(routes)
  const context = await handler.query(request)

  if (context instanceof Response) {
    return {
      html: '',
      headHtml: '',
      hydrationData: null,
      locale: null as Locale | null,
      lazyRouteIds: [] as string[],
      status: context.status,
      redirect: context.headers.get('Location'),
    }
  }

  const router = createStaticRouter(handler.dataRoutes, context)
  const stream = await renderToReadableStream(
    <StrictMode>
      <AppProviders>
        <StaticRouterProvider router={router} context={context} />
      </AppProviders>
    </StrictMode>,
  )

  await stream.allReady
  const html = await streamToString(stream)
  const meta = consumeSsrPageMeta()
  const headHtml = meta ? serializePageMetaToHtml(meta) : ''

  return {
    html,
    headHtml,
    hydrationData: serializeRouterHydration(context),
    locale,
    lazyRouteIds: collectLazyRouteIds(context),
    status: context.statusCode ?? 200,
    redirect: null as string | null,
  }
}
