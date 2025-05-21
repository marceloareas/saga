#!/bin/bash


set -e


if [ -z "$1" ]; then
  echo "Please provide the name of the entity as a parameter."
  exit 1
fi


name=$1


if [ -z "$2" ]; then
  echo "Please provide the name of the entity to copy as a second parameter."
  exit 1
fi


template_name=$2
name_to_find=$2


if [ -z "$3" ]; then
  name_to_find=$template_name
else
  name_to_find=$3
fi


# Add help option
if [ "$1" == "--help" ]; then
  echo "Usage: $0 <name> <template_name>"
  echo "Copies files with names containing <template_name> and replaces it with <name>."
  exit 0
fi


# Find destination paths
paths=($(find . -name "*${name_to_find}**.cs"))


# Create empty files if there were no files found
if [ ${#paths[@]} -eq 0 ]; then
  echo "No files found with name containing '$template_name' in the current directory and its subdirectories."
  exit 1
fi


# Copy and update files
for path in "${paths[@]}"; do
  new="${path//$template_name/$name}"
  mkdir -p "$(dirname $new)"
  if [ ! -f "$path" ]; then
    echo "Error: $path does not exist."
    exit 1
  fi
 
  if [ -f "$new" ]; then
    echo "Warning: $new already exists. Skipping creation of this file."
  else
    cp "$path" "$new"
    formatted_string=$(echo "$template_name" | sed -E 's/([a-z])([A-Z])/\1 \2/g')
    sed -i "s/$template_name/$name/g" "$new"
    sed -i "s/$formatted_string/$name/g" "$new"
    sed -i "s/${formatted_string,,}/${name,}/g"  "$new"
    sed -i "s/${template_name,}/${name,}/g" "$new"
    echo "Debug: $new created."
  fi
done


echo "Files copied and updated successfully."

