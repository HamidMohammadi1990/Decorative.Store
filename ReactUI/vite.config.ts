import path from 'node:path'
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'

export default defineConfig(({ mode }) => ({
  plugins: [react(), tailwindcss()],
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
        target: 'https://localhost:55274',
        changeOrigin: true,
        secure: false,
      },
      '/Uploads': {
        target: 'https://localhost:55274',
        changeOrigin: true,
        secure: false,
      },
      '/sitemap-dynamic.xml': {
        target: 'https://localhost:55274',
        changeOrigin: true,
        secure: false,
        rewrite: () => '/api/v1/seo/sitemap.xml',
      },
    },
  },
}))
