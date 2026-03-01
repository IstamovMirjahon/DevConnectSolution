FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["DevConnect.Api/DevConnect.Api.csproj", "DevConnect.Api/"]
COPY ["DevConnect.Application/DevConnect.Application.csproj", "DevConnect.Application/"]
COPY ["DevConnect.Domain/DevConnect.Domain.csproj", "DevConnect.Domain/"]
COPY ["DevConnect.Infrastructure/DevConnect.Infrastructure.csproj", "DevConnect.Infrastructure/"]

# Restore
RUN dotnet restore "DevConnect.Api/DevConnect.Api.csproj"

# Copy the rest
COPY . .
WORKDIR "/src/DevConnect.Api"
# Build and publish
RUN dotnet publish "DevConnect.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port and start
EXPOSE 8080
ENTRYPOINT ["dotnet", "DevConnect.Api.dll"]
