import fs from 'node:fs/promises'
import path from 'node:path'
import { fileURLToPath, pathToFileURL } from 'node:url'
import { loadEnv } from 'vite'
import { PRERENDER_ROUTES } from './prerender-routes.mjs'

const __dirname = path.dirname(fileURLToPath(import.meta.url))
const root = path.resolve(__dirname, '..')
const mode = process.env.NODE_ENV ?? 'production'
const env = loadEnv(mode, root, '')

if (!process.env.SSR_API_TARGET && env.SSR_API_TARGET) {
  process.env.SSR_API_TARGET = env.SSR_API_TARGET
}

const siteOrigin = (env.VITE_SITE_URL || process.env.VITE_SITE_URL || 'http://127.0.0.1:5173').replace(
  /\/+$/,
  '',
)

async function readTemplate() {
  return fs.readFile(path.resolve(root, 'dist/client/index.html'), 'utf-8')
}

function outputPathForRoute(route) {
  const clientDir = path.resolve(root, 'dist/client')
  if (route === '/') return path.join(clientDir, 'index.html')
  const segment = route.replace(/^\/+/, '')
  return path.join(clientDir, segment, 'index.html')
}

function buildHtml(template, result) {
  const hydrationScript = result.hydrationData
    ? `<script>window.__ROUTER_HYDRATION__=${JSON.stringify(result.hydrationData).replace(/</g, '\\u003c')}</script>`
    : ''

  return template
    .replace('<!--ssr-outlet-->', result.html ?? '')
    .replace('<!--ssr-head-->', result.headHtml ?? '')
    .replace('<!--ssr-data-->', hydrationScript)
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
