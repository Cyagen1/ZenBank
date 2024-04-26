FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App
COPY ZenCore/ZenCore.csproj ZenCore/
COPY ZenReporting.Contracts/ZenReporting.Contracts.csproj ZenReporting.Contracts/
RUN dotnet restore "ZenCore/ZenCore.csproj"

# Copy the entire project and build
COPY . .
WORKDIR /App/ZenCore
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "ZenCore.dll"]