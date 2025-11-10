FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY src/Uppbeat.Api/*.csproj ./Uppbeat.Api.csproj

RUN dotnet restore ./Uppbeat.Api.csproj

COPY src/Uppbeat.Api/. ./

RUN dotnet publish ./Uppbeat.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 5000
ENV DOTNET_ENVIRONMENT=Production
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Uppbeat.Api.dll"]
