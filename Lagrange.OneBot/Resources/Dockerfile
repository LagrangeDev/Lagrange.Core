FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.18 AS build-env

COPY . /root/build

ARG TARGETARCH

WORKDIR /root/build
RUN dotnet publish Lagrange.OneBot \
        -c Release \
        -a $TARGETARCH \
        --no-self-contained \
        -p:PublishSingleFile=true \
        --framework net8.0 \
        -o /root/out

FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine3.18

COPY --from=build-env /root/out/Lagrange.OneBot /app/bin/Lagrange.OneBot

COPY Lagrange.OneBot/Resources/docker-entrypoint.sh /app/bin/docker-entrypoint.sh

ENV UID=100
ENV GID=100

RUN apk --no-cache add shadow su-exec \
 && mkdir /app/data \
 && adduser -D user \
 && chmod +x /app/bin/docker-entrypoint.sh

WORKDIR /app/data
ENTRYPOINT ["/app/bin/docker-entrypoint.sh"]