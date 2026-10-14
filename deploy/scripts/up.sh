#!/usr/bin/env sh
set -eu
cd "$(dirname "$0")/.."

if [ ! -f .env ]; then
  echo "Missing deploy/.env — copy from .env.example and edit."
  exit 1
fi

docker compose --env-file .env up -d --build "$@"
