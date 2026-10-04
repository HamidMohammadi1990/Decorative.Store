import fs from 'node:fs'
import path from 'node:path'
import { fileURLToPath } from 'node:url'

const root = path.dirname(fileURLToPath(import.meta.url))
const statsPath = path.resolve(root, '../dist/stats.html')

if (!fs.existsSync(statsPath)) {
  console.error('Run npm run build:analyze first to generate dist/stats.html')
  process.exit(1)
}

console.log(`Bundle stats: ${statsPath}`)
