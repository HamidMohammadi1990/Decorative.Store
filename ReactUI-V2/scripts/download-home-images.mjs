/**
 * Downloads homepage images from Unsplash into public/images/home/
 * Run: npm run download:images
 *
 * If you are offline, use: npm run download:images -- --placeholders
 */
import fs from 'node:fs/promises'
import path from 'node:path'
import { fileURLToPath } from 'node:url'

const __dirname = path.dirname(fileURLToPath(import.meta.url))
const root = path.resolve(__dirname, '..')
const outDir = path.join(root, 'public', 'images', 'home')

/** @type {Record<string, string>} */
const IMAGES = {
  'living-room.jpg':
    'https://images.unsplash.com/photo-1618220179428-22790b461013?w=1600&q=80&fm=jpg',
  'bedroom.jpg':
    'https://images.unsplash.com/photo-1616594039964-40865a69e977?w=1600&q=80&fm=jpg',
  'velvet-sofa.jpg':
    'https://images.unsplash.com/photo-1555041469-a586c61ea9bc?w=1600&q=80&fm=jpg',
  'dining.jpg':
    'https://images.unsplash.com/photo-1617806118233-18e1de247200?w=1600&q=80&fm=jpg',
  'bedroom-set.jpg':
    'https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?w=1200&q=80&fm=jpg',
  'new-arrivals.jpg':
    'https://images.unsplash.com/photo-1616486338812-3dadae4b4ace?w=1200&q=80&fm=jpg',
  'trade-interior.jpg':
    'https://images.unsplash.com/photo-1616137422498-4e0e2e1bffb0?w=1200&q=80&fm=jpg',
  'in-stock.jpg':
    'https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=1200&q=80&fm=jpg',
  'collaboration.jpg':
    'https://images.unsplash.com/photo-1615529328331-f8917597711f?w=1200&q=80&fm=jpg',
  'commercial-office.jpg':
    'https://images.unsplash.com/photo-1497366216548-37526070297c?w=1200&q=80&fm=jpg',
}

const URL_TO_FILE = [
  ['photo-1618220179428-22790b461013', '/images/home/living-room.jpg'],
  ['photo-1616594039964-40865a69e977', '/images/home/bedroom.jpg'],
  ['photo-1555041469-a586c61ea9bc', '/images/home/velvet-sofa.jpg'],
  ['photo-1617806118233-18e1de247200', '/images/home/dining.jpg'],
  ['photo-1522771739844-6a9f6d5f14af', '/images/home/bedroom-set.jpg'],
  ['photo-1616486338812-3dadae4b4ace', '/images/home/new-arrivals.jpg'],
  ['photo-1616137422498-4e0e2e1bffb0', '/images/home/trade-interior.jpg'],
  ['photo-1560448204-e02f11c3d0e2', '/images/home/in-stock.jpg'],
  ['photo-1615529328331-f8917597711f', '/images/home/collaboration.jpg'],
  ['photo-1497366216548-37526070297c', '/images/home/commercial-office.jpg'],
]

const PLACEHOLDER_COLORS = {
  'living-room.jpg': ['#e8e4df', '#c4bdb4'],
  'bedroom.jpg': ['#ebe6e1', '#bfb8af'],
  'velvet-sofa.jpg': ['#d4ddd6', '#7a8f82'],
  'dining.jpg': ['#ece8e3', '#b0a89e'],
  'bedroom-set.jpg': ['#e5e0da', '#a89f94'],
  'new-arrivals.jpg': ['#eae6e1', '#b5ada3'],
  'trade-interior.jpg': ['#e0e4e8', '#9aa3ad'],
  'in-stock.jpg': ['#e6e8e4', '#a8aea3'],
  'collaboration.jpg': ['#ebe7e2', '#b8b0a6'],
  'commercial-office.jpg': ['#dfe3e8', '#8f99a3'],
}

function svgPlaceholder(filename, colors) {
  const [a, b] = colors
  const label = filename.replace(/\.(jpg|svg)$/, '').replace(/-/g, ' ')
  return `<?xml version="1.0" encoding="UTF-8"?>
<svg xmlns="http://www.w3.org/2000/svg" width="1600" height="1000" viewBox="0 0 1600 1000">
  <defs>
    <linearGradient id="g" x1="0%" y1="0%" x2="100%" y2="100%">
      <stop offset="0%" style="stop-color:${a}"/>
      <stop offset="100%" style="stop-color:${b}"/>
    </linearGradient>
  </defs>
  <rect width="1600" height="1000" fill="url(#g)"/>
  <text x="800" y="500" text-anchor="middle" fill="#5c5c5c" font-family="system-ui,sans-serif" font-size="28" opacity="0.6">${label}</text>
</svg>`
}

async function writePlaceholders() {
  await fs.mkdir(outDir, { recursive: true })
  for (const [name, colors] of Object.entries(PLACEHOLDER_COLORS)) {
    const svgName = name.replace('.jpg', '.svg')
    await fs.writeFile(
      path.join(outDir, svgName),
      svgPlaceholder(name, colors),
      'utf8',
    )
    console.log(`Wrote placeholder ${svgName}`)
  }
}

async function downloadImages() {
  await fs.mkdir(outDir, { recursive: true })
  for (const [filename, url] of Object.entries(IMAGES)) {
    const dest = path.join(outDir, filename)
    console.log(`Downloading ${filename}...`)
    const res = await fetch(url)
    if (!res.ok) throw new Error(`Failed ${filename}: ${res.status}`)
    const buf = Buffer.from(await res.arrayBuffer())
    await fs.writeFile(dest, buf)
    console.log(`  → ${(buf.length / 1024).toFixed(0)} KB`)
  }
}

async function patchMockJson() {
  for (const locale of ['en', 'fa']) {
    const file = path.join(root, 'src', 'data', 'mock', `home.${locale}.json`)
    let text = await fs.readFile(file, 'utf8')
    for (const [id, localPath] of URL_TO_FILE) {
      text = text.replace(
        new RegExp(
          `https://images\\.unsplash\\.com/${id.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')}\\?[^"]+`,
          'g',
        ),
        localPath,
      )
      text = text.replace(
        new RegExp(
          `"/images/home/[^"]+\\.svg"`,
          'g',
        ),
        (match) => {
          if (match.includes(id.split('-').slice(-1)[0])) return `"${localPath}"`
          return match
        },
      )
    }
    // Replace any remaining unsplash URLs by photo id
    for (const [id, localPath] of URL_TO_FILE) {
      text = text.replaceAll(
        new RegExp(`https://images\\.unsplash\\.com/${id}[^"]*`, 'g'),
        localPath,
      )
    }
    await fs.writeFile(file, text, 'utf8')
    console.log(`Patched home.${locale}.json`)
  }
}

const args = process.argv.slice(2)
const placeholdersOnly = args.includes('--placeholders')

try {
  if (placeholdersOnly) {
    await writePlaceholders()
    console.log('\nSVG placeholders saved. Mock JSON keeps .jpg paths; LocalImage falls back to .svg until download.')
  } else {
    await downloadImages()
    await patchMockJson()
    console.log('\nAll images downloaded and mock JSON updated to local paths.')
  }
} catch (err) {
  console.error(err.message ?? err)
  process.exit(1)
}
