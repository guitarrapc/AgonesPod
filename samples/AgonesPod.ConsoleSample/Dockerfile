FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["samples/AgonesPod.ConsoleSample/AgonesPod.ConsoleSample.csproj", "samples/AgonesPod.ConsoleSample/"]
RUN dotnet restore "samples/AgonesPod.ConsoleSample/AgonesPod.ConsoleSample.csproj"
COPY ["src/AgonesPod/AgonesPod.csproj", "src/AgonesPod/"]
RUN dotnet restore "src/AgonesPod/AgonesPod.csproj"
COPY . .
WORKDIR "/src/samples/AgonesPod.ConsoleSample"
RUN dotnet build "AgonesPod.ConsoleSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgonesPod.ConsoleSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgonesPod.ConsoleSample.dll"]