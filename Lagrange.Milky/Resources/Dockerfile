FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine-aot AS build

WORKDIR /app/src

COPY --link Lagrange.Milky/*.csproj             Lagrange.Milky/
COPY --link Lagrange.Core/*.csproj              Lagrange.Core/
COPY --link Lagrange.Core.Generator/*.csproj    Lagrange.Core.Generator/
COPY --link Lagrange.Proto.Generator/*.csproj   Lagrange.Proto.Generator/
COPY --link Lagrange.Proto/*.csproj             Lagrange.Proto/
COPY --link Lagrange.Milky.Generator/*.csproj   Lagrange.Milky.Generator/
RUN dotnet restore Lagrange.Milky

COPY --link . .
RUN dotnet publish Lagrange.Milky -c Release -p:DebugType=none -o /app/bin


FROM alpine

WORKDIR /root

COPY --from=build /app/bin/ /usr/local/bin/
CMD ["Lagrange.Milky"]