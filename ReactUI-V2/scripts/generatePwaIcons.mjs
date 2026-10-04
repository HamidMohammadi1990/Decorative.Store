import fs from 'node:fs/promises'
import path from 'node:path'
import { fileURLToPath } from 'node:url'

let sharp
try {
  sharp = (await import('sharp')).default
} catch {
  console.warn('[pwa] sharp is not installed — skipping PNG generation. Run: npm install')
  process.exit(0)
}

const __dirname = path.dirname(fileURLToPath(import.meta.url))
const root = path.resolve(__dirname, '..')
const pwaDir = path.join(root, 'public', 'pwa')

const targets = [
  { file: 'icon-192.png', size: 192, source: 'icon-source.svg' },
  { file: 'icon-512.png', size: 512, source: 'icon-source.svg' },
  { file: 'apple-touch-icon.png', size: 180, source: 'icon-source.svg' },
  { file: 'icon-512-maskable.png', size: 512, source: 'icon-maskable.svg' },
]

await fs.mkdir(pwaDir, { recursive: true })

for (const { file, size, source } of targets) {
  const input = path.join(pwaDir, source)
  const output = path.join(pwaDir, file)

  await sharp(input)
    .resize(size, size, {
      fit: 'contain',
      background: { r: 154, g: 116, b: 72, alpha: 1 },
    })
    .png({ compressionLevel: 9, adaptiveFiltering: true })
    .toFile(output)

  console.log(`[pwa] ${file} (${size}x${size})`)
}

console.log('[pwa] Icons generated in public/pwa/')
