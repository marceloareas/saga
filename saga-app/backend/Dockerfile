# Start with the official .NET Core SDK image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app
COPY *.sln ./
COPY saga.csproj ./
RUN dotnet restore

# Copy the entire project and build the app
COPY . ./
RUN dotnet publish ./saga.csproj -c Release -o out
 
# Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose port 80 to the outside world
EXPOSE 80

# Run the app on container startup
ENTRYPOINT ["dotnet", "saga.dll"]