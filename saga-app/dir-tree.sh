#!/usr/bin/env bash
# dir-tree.sh
# Usage:
#   dir-tree.sh [DIR] [--no-hidden] [--follow-symlinks]
#
# Examples:
#   dir-tree.sh
#   dir-tree.sh /path/to/project
#   dir-tree.sh --no-hidden
#   dir-tree.sh /path --follow-symlinks

set -euo pipefail

# ---------- args ----------
root="${1:-.}"
shift || true

no_hidden=false
follow_flag=""

while (( "$#" )); do
  case "$1" in
    --no-hidden)        no_hidden=true ;;
    --follow-symlinks)  follow_flag="-L" ;;
    -h|--help)
      sed -n '1,80p' "$0"; exit 0 ;;
    *) ;;
  esac
  shift
done

# normalize & validate
root="${root%/}"
if [[ ! -d "$root" ]]; then
  echo "Error: '$root' is not a directory" >&2
  exit 1
fi

# ---------- helpers ----------
# Build a find command that (optionally) prunes hidden paths.
# $1 = type selector (-type d / -type f / empty for all)
build_find_cmd() {
  local type_sel="$1"
  if $no_hidden; then
    # prune any path component starting with a dot
    # find ... \( -path '*/.*' -prune -o <type_sel> -print \)
    if [[ -n "$type_sel" ]]; then
      printf "find %q %s \\( -path '*/.*' -prune -o %s -print \\)" "$root" "$follow_flag" "$type_sel"
    else
      printf "find %q %s \\( -path '*/.*' -prune -o -print \\)" "$root" "$follow_flag"
    fi
  else
    if [[ -n "$type_sel" ]]; then
      printf "find %q %s %s -print" "$root" "$follow_flag" "$type_sel"
    else
      printf "find %q %s -print" "$root" "$follow_flag"
    fi
  fi
}

# ---------- flat lists ----------
echo "Directories under: $root"
echo

echo "Flat list — directories:"
# shellcheck disable=SC2046
eval "$(build_find_cmd "-type d")" | sort

echo
echo "Flat list — files:"
# shellcheck disable=SC2046
eval "$(build_find_cmd "-type f")" | sort

# ---------- tree (indented; dirs + files) ----------
echo
echo "Directory tree:"
echo "📁 $(basename -- "$root")"

# We print both dirs and files, sorted lexicographically for stable grouping.
# Then indent by depth = number of slashes in the relative path.
# (No fancy ├──/└── connectors to stay maximally portable.)
# shellcheck disable=SC2046
eval "$(build_find_cmd "")" | sort | sed '1d' | while IFS= read -r path; do
  rel="${path#$root/}"
  # If find printed the root again (edge cases), skip it
  [[ "$rel" == "$root" || -z "$rel" ]] && continue

  # depth = count of slashes in rel
  # remove all non-slashes, length of remainder = depth
  slashes_only="${rel//[^\/]/}"
  depth="${#slashes_only}"

  indent=""
  for ((i=0; i<depth; i++)); do indent+="  "; done

  if [[ -d "$path" ]]; then
    printf "%s📁 %s\n" "$indent" "$(basename -- "$path")"
  else
    printf "%s📄 %s\n" "$indent" "$(basename -- "$path")"
  fi
done
