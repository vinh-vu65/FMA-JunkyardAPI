#!/usr/bin/env bash
set -euo pipefail

ktmpl deploy/jupiter.yml -f deploy/arguments.yml --parameter imageTag "${BUILDKITE_COMMIT}" | kubectl apply -f -