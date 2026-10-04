# Diba Gallery — Frontend (Phase 1)

React storefront for Diba Gallery, with a flatter, modern UI.

## Stack

- Vite + React 19 + TypeScript
- Tailwind CSS v4
- React Router v7
- Zustand (cart + locale settings)
- i18next (EN / FA, RTL for Persian)
- Mock JSON + service layer (ready for backend swap)

## Scripts

```bash
npm install
npm run dev
npm run build
npm run download:images          # fetch photos into public/images/home/
npm run download:images:placeholders  # SVG fallbacks when offline
```

## Project structure

```
src/
  models/       # Per-section types (no shared Product model for list/detail yet)
  data/mock/    # home.en.json, home.fa.json
  services/     # homeService, currencyService, mockClient
  extensions/   # formatMoney, getDirection
  hooks/
  stores/       # cartStore, settingsStore
  components/   # ui, layout, home, header, cart
  pages/
```

## Connecting a backend later

Replace `homeService.getPage` to call your API and map the response with `mapHomePage`. Currency comes from `currencyService.getActive(locale)`.

## Phase 1 scope

Homepage sections: promo bars, header/nav, design services, hero carousel, promo tiles, category chips, featured shop grid, footer. Cart drawer is wired; product pages will add lines in a later phase.
