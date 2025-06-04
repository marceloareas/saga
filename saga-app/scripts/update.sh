#!/bin/bash

found=0
path=""

echo "[install.sh] You are at '$(pwd)'."
echo "[install.sh] Will scan the current directory for docker-compose.yaml files and start the services."
echo "[install.sh] Scanning folders for docker-compose.yaml ..."
echo ""

for file in $(pwd)/*; do
  if [[ "$file" == *docker-compose.yaml ]]; then
    found=1
    path=$file
    echo "[install.sh] Found '$file'."
    echo "[install.sh] Starting services using '$file'."
    echo ""
    (docker-compose up -d)
  # else
    # echo "[install.sh] Skipping: $file"
  fi
done

echo ""
if [ $found -eq 0 ]; then
  echo "[install.sh] Unable to find YAML file. Services not started."
else
  echo "[install.sh] Services started. Check Docker for status."
fi