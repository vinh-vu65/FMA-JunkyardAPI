#!/usr/bin/env bash
set -euo pipefail

ktmpl deploy/template.yml -f deploy/default.yml --parameter imageTag "${BUILDKITE_COMMIT}" | kubectl apply -f -