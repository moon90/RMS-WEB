# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy sln and csproj files first for caching
COPY RMS.sln ./
COPY RMS.WebApi/RMS.WebApi.csproj ./RMS.WebApi/
COPY RMS.Application/RMS.Application.csproj ./RMS.Application/
COPY RMS.Core/RMS.Core.csproj ./RMS.Core/
COPY RMS.Domain/RMS.Domain.csproj ./RMS.Domain/
COPY RMS.Infrastructure/RMS.Infrastructure.csproj ./RMS.Infrastructure/

RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish RMS.WebApi/RMS.WebApi.csproj -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose port and start
EXPOSE 80
ENTRYPOINT ["dotnet", "RMS.WebApi.dll"]
