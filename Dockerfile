# ---- Build Stage ----

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies

COPY \*.csproj ./
RUN dotnet restore

# Copy everything else and publish

COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ---- Runtime Stage ----

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output (includes wwwroot automatically)

COPY --from=build /app/publish .

# Railway provides PORT env variable, fallback to 8080

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "backend.dll"]
