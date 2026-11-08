# Multi-stage
# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["DotNetSkeleton.csproj", "./"]
RUN dotnet restore "DotNetSkeleton.csproj"

COPY . .
RUN dotnet publish "DotNetSkeleton.csproj" -c Release -o /app/publish --no-restore

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DotNetSkeleton.dll"]
