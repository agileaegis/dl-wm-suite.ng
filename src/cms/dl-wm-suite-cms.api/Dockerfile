FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/cms/dl-wm-suite-cms.api/dl-wm-suite-cms.api.csproj", "src/cms/dl-wm-suite-cms.api/"]
RUN dotnet restore "src/cms/dl-wm-suite-cms.api/dl-wm-suite-cms.api.csproj"
COPY . .
WORKDIR "/src/cms/dl-wm-suite-cms.api"
RUN dotnet build "dl-wm-suite-cms.api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "dl-wm-suite-cms.api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "dl-wm-suite-cms.api.dll"]