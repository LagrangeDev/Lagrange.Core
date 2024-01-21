FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine3.18 AS build-env
WORKDIR /App

COPY . ./
RUN dotnet publish Lagrange.OneBot/Lagrange.OneBot.csproj \
        -c Release \
        -o out \
        -v normal \
        --no-self-contained \
        -p:PublishSingleFile=true \
        -p:IncludeContentInSingleFile=true
		
FROM mcr.microsoft.com/dotnet/runtime:7.0-alpine3.18
WORKDIR /app
COPY --from=build-env /App/out .
COPY appsettings.onebot.json ./appsettings.json
ENTRYPOINT ["./Lagrange.OneBot"]
