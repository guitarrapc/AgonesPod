FROM mcr.microsoft.com/dotnet/core/runtime:3.1.0-buster-slim AS base
WORKDIR /app

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