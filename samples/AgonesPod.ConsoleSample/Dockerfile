FROM mcr.microsoft.com/dotnet/core/runtime:3.1.0-buster-slim AS base
WORKDIR /app

# Build-time metadata as defined at http://label-schema.org
ARG BUILD_DATE
ARG VCS_REF
ARG VERSION
LABEL org.label-schema.build-date=$BUILD_DATE \
    org.label-schema.name="guitarrapc/agonespod" \
    org.label-schema.description="kubernetes api to call agonespod via .NETCore" \
    org.label-schema.url="https://hub.docker.com/r/guitarrapc/agonespod" \
    org.label-schema.vcs-ref=$VCS_REF \
    org.label-schema.vcs-url="https://github.com/guitarrapc/AgonesPod" \
    org.label-schema.vendor="guitarrapc" \
    org.label-schema.version=$VERSION \
    org.label-schema.schema-version="1.0"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/AgonesPod/AgonesPod.csproj", "src/AgonesPod/"]
COPY ["samples/AgonesPod.ConsoleSample/AgonesPod.ConsoleSample.csproj", "samples/AgonesPod.ConsoleSample/"]
RUN dotnet restore "samples/AgonesPod.ConsoleSample/AgonesPod.ConsoleSample.csproj"
COPY . .
WORKDIR /src/samples/AgonesPod.ConsoleSample
RUN dotnet build "AgonesPod.ConsoleSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgonesPod.ConsoleSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgonesPod.ConsoleSample.dll"]