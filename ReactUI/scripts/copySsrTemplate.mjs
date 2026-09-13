import fs from 'node:fs/promises'
import path from 'node:path'
import { fileURLToPath } from 'node:url'

const __dirname = path.dirname(fileURLToPath(import.meta.url))
const root = path.resolve(__dirname, '..')
const clientIndex = path.resolve(root, 'dist/client/index.html')
const ssrTemplate = path.resolve(root, 'dist/client/index.ssr.html')

await fs.copyFile(clientIndex, ssrTemplate)
console.log('[ssr] Saved SSR HTML template → dist/client/index.ssr.html')
