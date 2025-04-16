#!/bin/bash

total_count=0
success_count=0

echo "[install.sh] Scanning folders for docker-compose.yaml..."

for dir in */; do
  ((total_count++))
  compose_file="${dir}docker-compose.yaml"

  if [[ -f "$compose_file" ]]; then
    echo "[install.sh] Found: $compose_file"
    (
      cd "$dir" && docker-compose up -d
    )
    echo "[install.sh] Services started in: $dir"
    ((success_count++))
  else
    echo "[install.sh] Skipping: $dir (no docker-compose.yaml)"
  fi
done

echo ""
echo "[install.sh] Installation Complete. (Success: $success_count, Failed: $((total_count - success_count)))"
