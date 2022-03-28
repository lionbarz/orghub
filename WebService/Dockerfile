FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebService/WebService.csproj", "WebService/"]
COPY ["Databases/Databases.csproj", "Databases/"]
COPY ["InterfaceAdapters/InterfaceAdapters.csproj", "InterfaceAdapters/"]
RUN dotnet restore "WebService/WebService.csproj"
RUN dotnet restore "Databases/Databases.csproj"
RUN dotnet restore "InterfaceAdapters/InterfaceAdapters.csproj"
COPY . .
WORKDIR "/src/WebService"
RUN dotnet build "WebService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebService.dll"]
