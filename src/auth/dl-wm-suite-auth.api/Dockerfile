#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/auth/dl-wm-suite-auth.api/dl-wm-suite-auth.api.csproj", "src/auth/dl-wm-suite-auth.api/"]
COPY ["src/common/dl-wm-suite-common.dtos/dl-wm-suite-common.dtos.csproj", "src/common/dl-wm-suite-common.dtos/"]
COPY ["src/common/dl-wm-suite-common.infrastructure/dl-wm-suite-common.infrastructure.csproj", "src/common/dl-wm-suite-common.infrastructure/"]
RUN dotnet restore "src/auth/dl-wm-suite-auth.api/dl-wm-suite-auth.api.csproj"
COPY . .
WORKDIR "/src/src/auth/dl-wm-suite-auth.api"
RUN dotnet build "dl-wm-suite-auth.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dl-wm-suite-auth.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dl-wm-suite-auth.api.dll"]