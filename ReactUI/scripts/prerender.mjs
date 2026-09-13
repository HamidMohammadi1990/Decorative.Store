import fs from 'node:fs/promises'
import path from 'node:path'
import { fileURLToPath, pathToFileURL } from 'node:url'
import { loadEnv } from 'vite'
import { PRERENDER_ROUTES } from './prerender-routes.mjs'
import { configureSsrTlsForLocalApi } from './configureSsrTls.mjs'

const __dirname = path.dirname(fileURLToPath(import.meta.url))
const root = path.resolve(__dirname, '..')
const mode = process.env.NODE_ENV ?? 'production'
const env = loadEnv(mode, root, '')

if (!process.env.SSR_API_TARGET && env.SSR_API_TARGET) {
  process.env.SSR_API_TARGET = env.SSR_API_TARGET
}

configureSsrTlsForLocalApi(process.env.SSR_API_TARGET)

const siteOrigin = (env.VITE_SITE_URL || process.env.VITE_SITE_URL || 'http://127.0.0.1:5173').replace(
  /\/+$/,
  '',
)

async function readTemplate() {
  const ssrTemplatePath = path.resolve(root, 'dist/client/index.ssr.html')
  const clientIndexPath = path.resolve(root, 'dist/client/index.html')

  try {
    return await fs.readFile(ssrTemplatePath, 'utf-8')
  } catch {
    return fs.readFile(clientIndexPath, 'utf-8')
  }
}

function outputPathForRoute(route) {
  const clientDir = path.resolve(root, 'dist/client')
  if (route === '/') return path.join(clientDir, 'index.html')
  const segment = route.replace(/^\/+/, '')
  return path.join(clientDir, segment, 'index.html')
}

function applyHtmlDocumentAttrs(template, locale) {
  const lang = locale === 'fa' ? 'fa' : 'en'
  const dir = locale === 'fa' ? 'rtl' : 'ltr'
  return template.replace(/<html\b[^>]*>/i, `<html lang="${lang}" dir="${dir}">`)
}

function buildHydrationScript(result) {
  const parts = []
  if (result.locale) {
    parts.push(`window.__SSR_LOCALE__=${JSON.stringify(result.locale)}`)
  }
  if (result.lazyRouteIds?.length) {
    parts.push(
      `window.__SSR_LAZY_ROUTES__=${JSON.stringify(result.lazyRouteIds).replace(/</g, '\\u003c')}`,
    )
  }
  if (result.hydrationData) {
    parts.push(
      `window.__ROUTER_HYDRATION__=${JSON.stringify(result.hydrationData).replace(/</g, '\\u003c')}`,
    )
  }
  return parts.length ? `<script>${parts.join(';')}</script>` : ''
}

function injectRenderedHtml(template, { html = '', headHtml = '', hydrationScript = '' }) {
  let output = template

  if (output.includes('<!--ssr-outlet-->')) {
    output = output.replace('<!--ssr-outlet-->', html)
  } else {
    output = output.replace(
      /(<div id="root"[^>]*>)([\s\S]*?)(<\/div>)/i,
      `$1${html}$3`,
    )
  }

  if (output.includes('<!--ssr-head-->')) {
    output = output.replace('<!--ssr-head-->', headHtml)
  } else if (headHtml) {
    output = output.replace('</head>', `${headHtml}\n</head>`)
  }

  if (output.includes('<!--ssr-data-->')) {
    output = output.replace('<!--ssr-data-->', hydrationScript)
  } else if (hydrationScript) {
    output = output.replace(
      /(<script type="module"[^>]*><\/script>)/i,
      `${hydrationScript}\n$1`,
    )
  }

  return output
}

function buildHtml(template, result) {
  const hydrationScript = buildHydrationScript(result)

  return applyHtmlDocumentAttrs(
    injectRenderedHtml(template, {
      html: result.html ?? '',
      headHtml: result.headHtml ?? '',
      hydrationScript,
    }),
    result.locale,
  )
}

async function prerenderRoute(render, template, route) {
  const request = new Request(`${siteOrigin}${route}`, {
    headers: {
      'accept-language': 'en-US,en;q=0.9',
    },
  })

  const result = await render(request)
  const html = buildHtml(template, result)
  const outputPath = outputPathForRoute(route)

  await fs.mkdir(path.dirname(outputPath), { recursive: true })
  await fs.writeFile(outputPath, html, 'utf-8')

  console.log(`[prerender] ${route} → ${path.relative(root, outputPath)} (${result.status ?? 200})`)
}

async function main() {
  const serverEntry = path.resolve(root, 'dist/server/entry-server.js')
  const { render } = await import(pathToFileURL(serverEntry).href)
  const template = await readTemplate()

  console.log(`[prerender] site origin: ${siteOrigin}`)
  console.log(`[prerender] API target: ${process.env.SSR_API_TARGET ?? '(not set)'}`)
  console.log(`[prerender] routes: ${PRERENDER_ROUTES.length}`)

  for (const route of PRERENDER_ROUTES) {
    try {
      await prerenderRoute(render, template, route)
    } catch (error) {
      console.error(`[prerender] failed for ${route}:`, error)
      process.exitCode = 1
    }
  }
}

await main()
