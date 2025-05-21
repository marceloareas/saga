#!/bin/bash 

echo "[db.sh] Starting PostgreSQL container..."
docker exec -it pondoc-postgres-1 psql -U postgres
echo "[db.sh] PostgreSQL container started."
