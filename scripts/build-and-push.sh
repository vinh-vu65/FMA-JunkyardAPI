#!/usr/bin/env bash

local_image_name="docker.myob.com/future-makers-academy/vinh-junkyard"
image_suffix=${BUILDKITE_COMMIT}

echo "Building application"
docker build -t "$local_image_name:$image_suffix" .

echo "Pushing image to CloudSmith"
docker push "$local_image_name:$image_suffix"