FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.18 AS build-env

WORKDIR /root/build
ARG TARGETARCH

COPY . /root/build

RUN dotnet publish -p:DebugType="none" -a $TARGETARCH -f "net8.0" -o "/root/out" "Lagrange.OneBot"

# ===

FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine3.18

WORKDIR /app/data

COPY --from=build-env /root/out /app/bin
COPY Lagrange.OneBot/Resources/docker-entrypoint.sh /app/bin/docker-entrypoint.sh

ENV UID=100
ENV GID=100

RUN apk --no-cache add shadow su-exec \
 && adduser -D user \
 && chmod +x /app/bin/docker-entrypoint.sh

ENTRYPOINT ["/app/bin/docker-entrypoint.sh"]