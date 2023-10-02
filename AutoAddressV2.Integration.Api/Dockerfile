FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AutoAddressV2.Integration.Api/AutoAddressV2.Integration.Api.csproj", "AutoAddressV2.Integration.Api/"]
RUN dotnet restore "AutoAddressV2.Integration.Api/AutoAddressV2.Integration.Api.csproj"
COPY . .
WORKDIR "/src/AutoAddressV2.Integration.Api"
RUN dotnet build "AutoAddressV2.Integration.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutoAddressV2.Integration.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoAddressV2.Integration.Api.dll"]
