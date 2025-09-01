# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ColourMemory/*.csproj ./ColourMemory/
COPY ColourMemory.Tests/*.csproj ./ColourMemory.Tests/
RUN dotnet restore ./ColourMemory/ColourMemory.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish ./ColourMemory/ColourMemory.csproj -c Release -o out

# Use official .NET runtime image for run
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out ./

# Run the app
ENTRYPOINT ["dotnet", "ColourMemory.dll"]