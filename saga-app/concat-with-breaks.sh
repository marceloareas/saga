#!/usr/bin/env bash
# concat-with-breaks.sh
# Usage: ./concat-with-breaks.sh [DIR]
# Recursively concatenates all regular files under DIR (default: .)
# into ./cone-analogy, inserting a newline before each file's content
# (except the first). Skips the output file itself.

set -euo pipefail

dir="${1:-.}"
out="cat-recursion"

# Recursively find files, sort for deterministic order, and handle weird filenames safely
# Exclude the output file itself if it's inside the tree.
find "$dir" -type f ! -name "$(basename -- "$out")" -print0 \
  | sort -z \
  | while IFS= read -r -d '' file; do
      # Insert a separating newline only if $out already has content
      if [ -s "$out" ]; then
        printf '\n' >> "$out"
      fi
      cat -- "$file" >> "$out"
    done

