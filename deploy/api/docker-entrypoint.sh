#!/bin/sh
set -eu

APP_USER=appuser
APP_GROUP=appgroup
UPLOADS_ROOT=/app/wwwroot/Uploads

mkdir -p \
  "${UPLOADS_ROOT}/Products" \
  "${UPLOADS_ROOT}/BlogPosts" \
  "${UPLOADS_ROOT}/Users" \
  "${UPLOADS_ROOT}/Orders" \
  "${UPLOADS_ROOT}/Cms" \
  "${UPLOADS_ROOT}/Stories" \
  "${UPLOADS_ROOT}/MarketingPromos" \
  "${UPLOADS_ROOT}/RoomTypes"

chown -R "${APP_USER}:${APP_GROUP}" /app/wwwroot

if [ -n "${WAIT_FOR_SQL_HOST:-}" ] && [ -n "${WAIT_FOR_SQL_PORT:-}" ]; then
  echo "[entrypoint] waiting for SQL at ${WAIT_FOR_SQL_HOST}:${WAIT_FOR_SQL_PORT} ..."
  i=0
  max="${WAIT_FOR_SQL_TIMEOUT:-90}"
  while ! nc -z "${WAIT_FOR_SQL_HOST}" "${WAIT_FOR_SQL_PORT}" 2>/dev/null; do
    i=$((i + 1))
    if [ "$i" -ge "$max" ]; then
      echo "[entrypoint] SQL not reachable after ${max}s"
      exit 1
    fi
    sleep 1
  done
  echo "[entrypoint] SQL is reachable"
fi

if [ -n "${WAIT_FOR_REDIS_HOST:-}" ] && [ -n "${WAIT_FOR_REDIS_PORT:-}" ]; then
  echo "[entrypoint] waiting for Redis at ${WAIT_FOR_REDIS_HOST}:${WAIT_FOR_REDIS_PORT} ..."
  i=0
  max="${WAIT_FOR_REDIS_TIMEOUT:-60}"
  while ! nc -z "${WAIT_FOR_REDIS_HOST}" "${WAIT_FOR_REDIS_PORT}" 2>/dev/null; do
    i=$((i + 1))
    if [ "$i" -ge "$max" ]; then
      echo "[entrypoint] Redis not reachable after ${max}s"
      exit 1
    fi
    sleep 1
  done
  echo "[entrypoint] Redis is reachable"
fi

exec gosu "${APP_USER}" "$@"
