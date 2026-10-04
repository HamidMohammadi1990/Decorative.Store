# Homepage images

| File | Usage |
|------|--------|
| `living-room.jpg` | Hero, tiles, best sellers |
| `bedroom.jpg` | Hero slide |
| `velvet-sofa.jpg` | Hero, tiles |
| `dining.jpg` | Hero, tiles |
| `bedroom-set.jpg` | Promo tile |
| `new-arrivals.jpg` | Featured grid |
| `trade-interior.jpg` | Featured grid |
| `in-stock.jpg` | Featured grid |
| `collaboration.jpg` | Featured grid |
| `commercial-office.jpg` | Featured grid |

## Current source

Real `.jpg` files are stored here and served at `/images/home/*.jpg`.

They are **AI-generated lifestyle photos** styled for neutral contemporary interiors. Direct download from external sources may fail on some networks; the app falls back to matching `.svg` placeholders if a `.jpg` is missing.

## Regenerate placeholders (offline)

```bash
npm run download:images:placeholders
```

## Optional: download from Unsplash (requires internet)

```bash
npm run download:images
```

Until `.jpg` files exist, `LocalImage` shows matching `.svg` placeholders.
