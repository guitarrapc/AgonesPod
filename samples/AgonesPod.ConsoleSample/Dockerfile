FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["AgonesPod.ConsoleSample/AgonesPod.ConsoleSample.csproj", "AgonesPod.ConsoleSample/"]
RUN dotnet restore "AgonesPod.ConsoleSample/AgonesPod.ConsoleSample.csproj"
COPY . .
WORKDIR "/src/AgonesPod.ConsoleSample"
RUN dotnet build "AgonesPod.ConsoleSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgonesPod.ConsoleSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgonesPod.ConsoleSample.dll"]