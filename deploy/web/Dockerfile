# syntax=docker/dockerfile:1

FROM node:22-bookworm-slim AS build
WORKDIR /app

COPY ReactUI-V2/package.json ReactUI-V2/package-lock.json* ./
RUN npm ci

COPY ReactUI-V2/ ./

ARG SSR_API_TARGET=http://api:8080
ARG VITE_SITE_URL=http://localhost
ARG VITE_BLOCK_SEARCH_INDEXING=true
ENV SSR_API_TARGET=${SSR_API_TARGET} \
    VITE_SITE_URL=${VITE_SITE_URL} \
    VITE_BLOCK_SEARCH_INDEXING=${VITE_BLOCK_SEARCH_INDEXING}

RUN npm run build:ssr

FROM node:22-bookworm-slim AS runtime
WORKDIR /app

RUN groupadd --system --gid 10001 nodeapp \
    && useradd --system --uid 10001 --gid nodeapp --home-dir /app nodeapp

COPY --from=build /app/dist ./dist
COPY --from=build /app/server ./server
COPY --from=build /app/scripts/configureSsrTls.mjs ./scripts/configureSsrTls.mjs
COPY --from=build /app/node_modules ./node_modules
COPY --from=build /app/package.json ./package.json

ENV NODE_ENV=production \
    HOST=0.0.0.0 \
    PORT=3000 \
    SSR_API_TARGET=http://api:8080

EXPOSE 3000

HEALTHCHECK --interval=30s --timeout=5s --start-period=40s --retries=3 \
  CMD node -e "fetch('http://127.0.0.1:3000/').then(r=>process.exit(r.ok?0:1)).catch(()=>process.exit(1))"

USER nodeapp

CMD ["node", "server/ssr.mjs"]
