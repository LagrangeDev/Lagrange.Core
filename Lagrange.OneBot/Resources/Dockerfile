FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.18 AS build-env
WORKDIR /root/build
ARG TARGETARCH

COPY . ./
RUN dotnet publish Lagrange.OneBot \
        -c Release \
        -a $TARGETARCH \
        --no-self-contained \
        -p:PublishSingleFile=true \
        --framework net8.0 \
        -o /root/out

FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine3.18

COPY --from=build-env /root/out/Lagrange.OneBot /root/bin/Lagrange.OneBot

COPY Lagrange.OneBot/Resources/docker-entrypoint.sh /root/bin/docker-entrypoint.sh

ENV UID=100
ENV GID=100
ENV UAMSK=0022

RUN apk --no-cache add shadow su-exec \
 && mkdir /root/data \
 && adduser -D user \
 && chmod +x /root/bin/docker-entrypoint.sh

WORKDIR /root/data
ENTRYPOINT ["/root/bin/docker-entrypoint.sh"]