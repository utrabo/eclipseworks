FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8000
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EclipseWorks.TaskManagementSystem.API/EclipseWorks.TaskManagementSystem.API.csproj", "EclipseWorks.TaskManagementSystem.API/"]
COPY ["EclipseWorks.TaskManagementSystem.Application/EclipseWorks.TaskManagementSystem.Application.csproj", "EclipseWorks.TaskManagementSystem.Application/"]
COPY ["EclipseWorks.TaskManagementSystem.Domain/EclipseWorks.TaskManagementSystem.Domain.csproj", "EclipseWorks.TaskManagementSystem.Domain/"]
COPY ["EclipseWorks.TaskManagementSystem.Infrastructure/EclipseWorks.TaskManagementSystem.Infrastructure.csproj", "EclipseWorks.TaskManagementSystem.Infrastructure/"]

COPY ["EclipseWorks.TaskManagementSystem.Database/001_DATABASE_CREATION.sql", "dbinit"]

RUN dotnet restore "EclipseWorks.TaskManagementSystem.API/EclipseWorks.TaskManagementSystem.API.csproj"

COPY . .
WORKDIR "/src/EclipseWorks.TaskManagementSystem.API"
RUN dotnet build "EclipseWorks.TaskManagementSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EclipseWorks.TaskManagementSystem.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EclipseWorks.TaskManagementSystem.API.dll"]