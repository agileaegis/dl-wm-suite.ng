FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/aegis-wm-suite-dms.api/aegis-wm-suite-dms.api.csproj", "src/aegis-wm-suite-dms.api.api/"]
RUN dotnet restore "src/aegis-wm-suite-dms.api/aegis-wm-suite-dms.api.csproj"
COPY . .
WORKDIR "/src/aegis-wm-suite-dms.api"
RUN dotnet build "aegis-wm-suite-dms.api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "aegis-wm-suite-dms.api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "aegis-wm-suite-dms.api.dll"]