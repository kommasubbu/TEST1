:: Copyright (c) Microsoft. All rights reserved.

@ECHO off & setlocal enableextensions enabledelayedexpansion

:: Note: use lowercase names for the Docker images
SET ORIGINAL_IMAGE="sensewire-storage-adapter"
SET DOCKER_IMAGE="cloud.canister.io:5000/dyocense"/%ORIGINAL_IMAGE%
:: "testing" is the latest dev build, usually matching the code in the "master" branch
SET DOCKER_TAG=0.1

docker tag %ORIGINAL_IMAGE%:%DOCKER_TAG% %DOCKER_IMAGE%:%DOCKER_TAG%
docker push %DOCKER_IMAGE%:%DOCKER_TAG%


endlocal
