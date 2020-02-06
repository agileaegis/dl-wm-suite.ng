FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/fleet/dl-wm-suite-fleet.api/dl-wm-suite-fleet.api.csproj", "src/fleet/dl-wm-suite-fleet.api/"]
COPY ["src/fleet/dl-wm-suite-fleet.contracts/dl-wm-suite-fleet.contracts.csproj", "src/fleet/dl-wm-suite-fleet.contracts/"]
COPY ["src/common/dl-wm-suite-common.dtos/dl-wm-suite-common.dtos.csproj", "src/common/dl-wm-suite-common.dtos/"]
COPY ["src/common/dl-wm-suite-common.infrastructure/dl-wm-suite-common.infrastructure.csproj", "src/common/dl-wm-suite-common.infrastructure/"]
COPY ["src/fleet/dl-wm-suite-fleet.model/dl-wm-suite-fleet.model.csproj", "src/fleet/dl-wm-suite-fleet.model/"]
COPY ["src/fleet/dl-wm-suite-fleet.services/dl-wm-suite-fleet.services.csproj", "src/fleet/dl-wm-suite-fleet.services/"]
COPY ["src/fleet/dl-wm-suite-fleet.repository/dl-wm-suite-fleet.repository.csproj", "src/fleet/dl-wm-suite-fleet.repository/"]
COPY ["src/common/dl-wm-suite-common.messaging/dl-wm-suite-common.messaging.csproj", "src/common/dl-wm-suite-common.messaging/"]
RUN dotnet restore "src/fleet/dl-wm-suite-fleet.api/dl-wm-suite-fleet.api.csproj"
COPY . .
WORKDIR "/src/src/fleet/dl-wm-suite-fleet.api"
RUN dotnet build "dl-wm-suite-fleet.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dl-wm-suite-fleet.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dl-wm-suite-fleet.api.dll"]