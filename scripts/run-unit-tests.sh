#! /usr/bin/env bash
set -euo pipefail

image_name="junkyardtests:1"

docker build . -t "${image_name}" --target "test"

docker run "${image_name}"