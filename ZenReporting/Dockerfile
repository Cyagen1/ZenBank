# Stage 1: Build the solution
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App
COPY ZenReporting/ZenReporting.csproj ZenReporting/
COPY ZenReporting.Contracts/ZenReporting.Contracts.csproj ZenReporting.Contracts/
RUN dotnet restore "ZenReporting/ZenReporting.csproj"

# Copy the entire project and build
COPY . .
WORKDIR /App/ZenReporting
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "ZenReporting.dll"]