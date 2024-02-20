FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.18 AS build-env
WORKDIR /App

COPY . ./
RUN dotnet publish Lagrange.OneBot/Lagrange.OneBot.csproj \
        -c Release \
        -o out \
        --no-self-contained \
        -p:PublishSingleFile=true \
        -p:IncludeContentInSingleFile=true \
	    --framework net8.0
		
FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine3.18
WORKDIR /app
COPY --from=build-env /App/out .
COPY Lagrange.OneBot/Resources/appsettings.onebot.json ./appsettings.json
ENTRYPOINT ["./Lagrange.OneBot"]
