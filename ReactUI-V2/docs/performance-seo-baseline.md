# Performance & SEO baseline

Run these checks before and after optimization work to measure impact.

## Frontend bundle

```bash
cd ReactUI
npm run build
npm run analyze
```

Open `dist/stats.html` after analyze (requires one-time build with visualizer).

## Lighthouse (manual)

Test in Chrome DevTools → Lighthouse (mobile, navigation):

| URL | Priority |
|-----|----------|
| `/` | Home, LCP hero |
| `/product/:slug` | PDP + Product schema |
| `/blog/:slug` | Article schema |
| `/search?q=sofa` | Search (noindex) |
| `/room-layout` | Room studio |

Record scores for Performance, SEO, Best Practices, Accessibility.

## Core Web Vitals targets

| Metric | Target |
|--------|--------|
| LCP | < 2.5s |
| INP | < 200ms |
| CLS | < 0.1 |

Optional: set `VITE_WEB_VITALS_ENDPOINT` in production to collect RUM metrics.

## SEO checklist

- [ ] `/robots.txt` allows public pages, blocks `/account/dashboard`
- [ ] `/sitemap.xml` index → static + dynamic sitemaps
- [ ] Dynamic sitemap: `/sitemap-dynamic.xml` (proxied to API in dev)
- [ ] Each public page sets title, description, canonical, OG tags
- [ ] PDP + blog have JSON-LD structured data
- [ ] Cart, checkout, dashboard use `noindex`

## Production config

```env
VITE_SITE_URL=https://your-domain.com
VITE_WEB_VITALS_ENDPOINT=https://your-analytics.example/vitals
```

Backend `appsettings.json`:

```json
"StorefrontSettings": {
  "BaseUrl": "https://your-domain.com"
}
```

## Nginx (production) — proxy dynamic sitemap

```nginx
location = /sitemap-dynamic.xml {
  proxy_pass https://api.example.com/api/v1/seo/sitemap.xml;
}
```

Serve `ReactUI/dist` static files with long cache for hashed assets.
