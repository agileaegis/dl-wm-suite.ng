#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/interprocess/dl-wm-suite-interprocess.api/dl-wm-suite-interprocess.api.csproj", "src/interprocess/dl-wm-suite-interprocess.api/"]
COPY ["src/common/dl-wm-suite-common.messaging/dl-wm-suite-common.messaging.csproj", "src/common/dl-wm-suite-common.messaging/"]
RUN dotnet restore "src/interprocess/dl-wm-suite-interprocess.api/dl-wm-suite-interprocess.api.csproj"
COPY . .
WORKDIR "/src/src/interprocess/dl-wm-suite-interprocess.api"
RUN dotnet build "dl-wm-suite-interprocess.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dl-wm-suite-interprocess.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dl-wm-suite-interprocess.api.dll"]