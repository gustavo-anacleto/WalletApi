# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WalletApi.sln ./
COPY WalletApi/ ./WalletApi/

WORKDIR /src/WalletApi
RUN dotnet restore ../WalletApi.sln
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "WalletApi.dll"]
