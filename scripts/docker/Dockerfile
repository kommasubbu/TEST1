FROM mcr.microsoft.com/dotnet/runtime:5.0

MAINTAINER dyocense (https://dyocense.com)

LABEL Tags="DyoCense,Sensewire,IoT,Solutions,IoT Hub,storage,.NET"

WORKDIR /app

COPY ./scripts/docker/ .

RUN cp content/run.sh .

RUN \
    # Ensures the entry point is executable
    chmod ugo+x /app/run.sh && \
    # Clean up destination folder
    rm -f /app/Dockerfile /app/.dockerignore

ENTRYPOINT ["/bin/bash", "/app/run.sh"]
