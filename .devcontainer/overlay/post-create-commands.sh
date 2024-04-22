#!/bin/bash -e

# Instal dependencies
npm install

# Invoke nx directly
npm i -g nx

# Workaround: https://github.com/nrwl/nx/issues/14126
npm run enable-nx-docker-cache

echo
echo "Post-Create Commands Complete!"
echo
