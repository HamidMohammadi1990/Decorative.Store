import path from 'node:path'
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import { VitePWA } from 'vite-plugin-pwa'
import { offlineFallbackPlugin } from './scripts/pwaOfflineFallbackPlugin'
import { PWA } from './pwa.config'

export default defineConfig(({ mode }) => ({
  plugins: [
    react(),
    tailwindcss(),
    VitePWA({
      registerType: 'prompt',
      injectRegister: false,
      // Do not fail the build when large public assets are excluded from precache.
      showMaximumFileSizeToCacheInBytesWarning: true,
      integration: {
        beforeBuildServiceWorker(options) {
          options.throwMaximumFileSizeToCacheInBytes = false
          options.workbox.globFollow = false
          options.workbox.globPatterns = [
            'assets/**/*.js',
            'assets/**/*.css',
            'favicon.svg',
            'pwa/apple-touch-icon.png',
            'pwa/icon-192.png',
            'pwa/icon-512.png',
            'pwa/icon-512-maskable.png',
            'pwa/icon-source.svg',
            'pwa/icon-maskable.svg',
          ]
          options.workbox.globIgnores = [
            '**/index.html',
            '**/index.ssr.html',
            '**/images/**',
            '**/*.{jpg,jpeg,webp,gif}',
            'sw.js',
            'workbox-*.js',
          ]
        },
      },
      includeAssets: [
        'favicon.svg',
        'offline.html',
        'pwa/apple-touch-icon.png',
        'pwa/icon-192.png',
        'pwa/icon-512.png',
        'pwa/icon-512-maskable.png',
      ],
      manifest: {
        id: PWA.scope,
        name: PWA.name,
        short_name: PWA.shortName,
        description: PWA.description,
        theme_color: PWA.themeColor,
        background_color: PWA.backgroundColor,
        display: 'standalone',
        display_override: ['standalone', 'browser'],
        orientation: 'portrait-primary',
        scope: PWA.scope,
        start_url: PWA.startUrl,
        lang: 'en',
        dir: 'ltr',
        categories: [...PWA.categories],
        icons: [
          {
            src: '/pwa/icon-192.png',
            sizes: '192x192',
            type: 'image/png',
            purpose: 'any',
          },
          {
            src: '/pwa/icon-512.png',
            sizes: '512x512',
            type: 'image/png',
            purpose: 'any',
          },
          {
            src: '/pwa/icon-512-maskable.png',
            sizes: '512x512',
            type: 'image/png',
            purpose: 'maskable',
          },
          {
            src: '/pwa/icon-source.svg',
            sizes: '512x512',
            type: 'image/svg+xml',
            purpose: 'any',
          },
          {
            src: '/pwa/icon-maskable.svg',
            sizes: '512x512',
            type: 'image/svg+xml',
            purpose: 'maskable',
          },
        ],
      },
      includeManifestIcons: false,
      workbox: {
        navigateFallback: null,
        cleanupOutdatedCaches: true,
        clientsClaim: true,
        skipWaiting: false,
        runtimeCaching: [
          {
            urlPattern: ({ request }) => request.mode === 'navigate',
            handler: 'NetworkFirst',
            options: {
              cacheName: 'pages-network',
              networkTimeoutSeconds: 8,
              expiration: {
                maxEntries: 40,
                maxAgeSeconds: 60 * 60 * 24,
              },
              cacheableResponse: {
                statuses: [0, 200],
              },
              plugins: [offlineFallbackPlugin()],
            },
          },
          {
            urlPattern: ({ url }) => url.pathname.startsWith('/api/'),
            handler: 'NetworkOnly',
          },
          {
            urlPattern: ({ url }) => url.pathname.startsWith('/account/dashboard'),
            handler: 'NetworkOnly',
          },
          {
            urlPattern: /^https:\/\/fonts\.googleapis\.com\/.*/i,
            handler: 'CacheFirst',
            options: {
              cacheName: 'google-fonts-stylesheets',
              expiration: {
                maxEntries: 8,
                maxAgeSeconds: 60 * 60 * 24 * 365,
              },
            },
          },
          {
            urlPattern: /^https:\/\/fonts\.gstatic\.com\/.*/i,
            handler: 'CacheFirst',
            options: {
              cacheName: 'google-fonts-webfonts',
              expiration: {
                maxEntries: 24,
                maxAgeSeconds: 60 * 60 * 24 * 365,
              },
            },
          },
          {
            urlPattern: /^https:\/\/cdn\.jsdelivr\.net\/.*/i,
            handler: 'CacheFirst',
            options: {
              cacheName: 'cdn-jsdelivr',
              expiration: {
                maxEntries: 12,
                maxAgeSeconds: 60 * 60 * 24 * 365,
              },
            },
          },
          {
            urlPattern: ({ url }) => url.pathname.startsWith('/Uploads/'),
            handler: 'StaleWhileRevalidate',
            options: {
              cacheName: 'uploaded-media',
              expiration: {
                maxEntries: 120,
                maxAgeSeconds: 60 * 60 * 24 * 14,
              },
            },
          },
          {
            urlPattern: ({ request, url }) =>
              request.destination === 'image' && url.pathname.startsWith('/images/'),
            handler: 'StaleWhileRevalidate',
            options: {
              cacheName: 'static-images',
              expiration: {
                maxEntries: 80,
                maxAgeSeconds: 60 * 60 * 24 * 30,
              },
              cacheableResponse: {
                statuses: [0, 200],
              },
            },
          },
        ],
      },
      devOptions: {
        enabled: false,
      },
    }),
  ],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
      // Force ESM build — CJS dist/index.js breaks Vite SSR (module is not defined).
      'react-router-dom': path.resolve(
        __dirname,
        'node_modules/react-router-dom/dist/index.mjs',
      ),
    },
    conditions: ['import', 'module-sync', 'module', 'browser', 'default'],
  },
  build: {
    outDir: 'dist/client',
    target: 'es2020',
    sourcemap: mode === 'analyze',
    cssMinify: true,
    chunkSizeWarningLimit: 700,
    rollupOptions: {
      output: {
        manualChunks(id) {
          if (!id.includes('node_modules')) return undefined
          if (id.includes('react-router')) return 'router'
          if (id.includes('react-dom') || id.includes('react/')) return 'react-vendor'
          if (id.includes('i18next') || id.includes('react-i18next')) return 'i18n'
          if (id.includes('zustand')) return 'state'
          return 'vendor'
        },
      },
    },
  },
  ssr: {
    resolve: {
      conditions: ['react-server', 'import', 'module-sync', 'module', 'default'],
    },
  },
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:60927',
        changeOrigin: true,
        secure: false,
      },
      '/Uploads': {
        target: 'https://localhost:60927',
        changeOrigin: true,
        secure: false,
      },
      '/sitemap-dynamic.xml': {
        target: 'https://localhost:60927',
        changeOrigin: true,
        secure: false,
        rewrite: () => '/api/v1/seo/sitemap.xml',
      },
    },
  },
}))
