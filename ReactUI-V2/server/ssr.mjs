import fs from 'node:fs/promises'
import path from 'node:path'
import { fileURLToPath, pathToFileURL } from 'node:url'
import express from 'express'
import compression from 'compression'
import sirv from 'sirv'
import { createServer as createViteServer, loadEnv } from 'vite'
import { configureSsrTlsForLocalApi } from '../scripts/configureSsrTls.mjs'

const __dirname = path.dirname(fileURLToPath(import.meta.url))
const root = path.resolve(__dirname, '..')
const mode = process.env.NODE_ENV ?? 'development'
const env = loadEnv(mode, root, '')
if (!process.env.SSR_API_TARGET && env.SSR_API_TARGET) {
  process.env.SSR_API_TARGET = env.SSR_API_TARGET
}
const isProd = process.env.NODE_ENV === 'production'
const port = Number(process.env.PORT) || 5173
const host = process.env.HOST ?? '127.0.0.1'
const apiTarget = process.env.SSR_API_TARGET ?? 'https://localhost:60927'
configureSsrTlsForLocalApi(apiTarget)

function isBuiltStaticAsset(pathname) {
  if (!pathname || pathname.endsWith('.html')) return false
  if (
    pathname === '/manifest.webmanifest' ||
    pathname === '/sw.js' ||
    pathname.startsWith('/workbox-') ||
    pathname.startsWith('/assets/') ||
    pathname.startsWith('/pwa/') ||
    pathname === '/favicon.svg' ||
    pathname === '/offline.html'
  ) {
    return true
  }
  return /\.[a-z0-9]{2,8}$/i.test(pathname)
}

function applyHtmlDocumentAttrs(template, locale) {
  const lang = locale === 'fa' ? 'fa' : 'en'
  const dir = locale === 'fa' ? 'rtl' : 'ltr'
  return template.replace(/<html\b[^>]*>/i, `<html lang="${lang}" dir="${dir}">`)
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

async function createSsrServer() {
  const app = express()
  app.use(compression())

  if (!process.env.SSR_API_TARGET) {
    console.warn(
      `[ssr] SSR_API_TARGET is not set — defaulting to ${apiTarget}. Set it in .env for production.`,
    )
  }

  let vite
  if (!isProd) {
    vite = await createViteServer({
      root,
      server: { middlewareMode: true },
      appType: 'custom',
    })
    app.use(vite.middlewares)
  } else {
    const clientRoot = path.resolve(root, 'dist/client')
    const serveClientAssets = sirv(clientRoot, {
      extensions: [],
      gzip: true,
    })

    // Serve built static assets (PWA manifest/SW, JS/CSS, icons). HTML routes use SSR below.
    app.use((req, res, next) => {
      if (req.method !== 'GET' && req.method !== 'HEAD') return next()

      const pathname = req.path.split('?')[0]
      if (isBuiltStaticAsset(pathname)) {
        return serveClientAssets(req, res, next)
      }

      return next()
    })
  }

  // Same-origin API for browser requests in SSR mode.
  app.use('/api', async (req, res) => {
    await proxyRequest(req, res, `${apiTarget}${req.originalUrl}`)
  })
  app.use('/Uploads', async (req, res) => {
    await proxyRequest(req, res, `${apiTarget}${req.originalUrl}`)
  })
  app.use('/sitemap-dynamic.xml', async (req, res) => {
    await proxyRequest(req, res, `${apiTarget}/api/v1/seo/sitemap.xml`)
  })

  app.use('*', async (req, res, next) => {
    try {
      const url = req.originalUrl
      const templatePath = isProd
        ? path.resolve(root, 'dist/client/index.ssr.html')
        : path.resolve(root, 'index.html')

      let template
      try {
        template = await fs.readFile(templatePath, 'utf-8')
      } catch {
        if (!isProd) throw new Error(`SSR template not found: ${templatePath}`)
        template = await fs.readFile(path.resolve(root, 'dist/client/index.html'), 'utf-8')
      }

      if (!isProd && vite) {
        template = await vite.transformIndexHtml(url, template)
      }

      const { render } = !isProd && vite
        ? await vite.ssrLoadModule('/src/entry-server.tsx')
        : await import(
            pathToFileURL(path.resolve(root, 'dist/server/entry-server.js')).href
          )

      const request = new Request(`http://${req.headers.host ?? 'localhost'}${url}`, {
        headers: req.headers,
      })

      const result = await render(request)

      if (result.redirect) {
        res.redirect(result.status ?? 302, result.redirect)
        return
      }

      const ssrBootParts = []
      if (result.locale) {
        ssrBootParts.push(`window.__SSR_LOCALE__=${JSON.stringify(result.locale)}`)
      }
      if (result.lazyRouteIds?.length) {
        ssrBootParts.push(
          `window.__SSR_LAZY_ROUTES__=${JSON.stringify(result.lazyRouteIds).replace(/</g, '\\u003c')}`,
        )
      }
      if (result.hydrationData) {
        ssrBootParts.push(
          `window.__ROUTER_HYDRATION__=${JSON.stringify(result.hydrationData).replace(/</g, '\\u003c')}`,
        )
      }
      const hydrationScript = ssrBootParts.length
        ? `<script>${ssrBootParts.join(';')}</script>`
        : ''

      const html = applyHtmlDocumentAttrs(
        injectRenderedHtml(template, {
          html: result.html ?? '',
          headHtml: result.headHtml ?? '',
          hydrationScript,
        }),
        result.locale,
      )

      res
        .status(result.status ?? 200)
        .set({ 'Content-Type': 'text/html; charset=utf-8' })
        .end(html)
    } catch (error) {
      if (!isProd && vite) vite.ssrFixStacktrace(error)
      next(error)
    }
  })

  app.listen(port, host, () => {
    console.log(`SSR server running at http://${host}:${port}`)
    console.log(`[ssr] API target: ${apiTarget}`)
    if (isProd) {
      console.log('[ssr] HTML template: dist/client/index.ssr.html')
    }
  })
}

async function proxyRequest(req, res, targetUrl) {
  const target = new URL(targetUrl)
  const transport = target.protocol === 'https:' ? await import('node:https') : await import('node:http')

  const headers = { ...req.headers, host: target.host }
  delete headers.connection

  const proxyReq = transport.request(
    {
      protocol: target.protocol,
      hostname: target.hostname,
      port: target.port || (target.protocol === 'https:' ? 443 : 80),
      path: `${target.pathname}${target.search}`,
      method: req.method,
      headers,
      rejectUnauthorized: false,
    },
    (proxyRes) => {
      res.writeHead(proxyRes.statusCode ?? 502, proxyRes.headers)
      proxyRes.pipe(res)
    },
  )

  proxyReq.on('error', (error) => {
    console.error('[ssr] API proxy error:', error.message)
    if (!res.headersSent) {
      res.statusCode = 502
      res.end('Bad Gateway')
    }
  })

  req.pipe(proxyReq)
}

createSsrServer()
