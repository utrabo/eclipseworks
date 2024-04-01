# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use SDK image to build the solution file
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EclipseWorks.TasksManagementSystem.API/EclipseWorks.TasksManagementSystem.API.csproj", "./EclipseWorks.TasksManagementSystem.API/"]
COPY ["EclipseWorks.TasksManagementSystem.Domain/EclipseWorks.TasksManagementSystem.Domain.csproj", "./EclipseWorks.TasksManagementSystem.Domain/"]
COPY ["EclipseWorks.TasksManagementSystem.Application/EclipseWorks.TasksManagementSystem.Application.csproj", "./EclipseWorks.TasksManagementSystem.Application/"]
COPY ["EclipseWorks.TasksManagementSystem.Infrastructure/EclipseWorks.TasksManagementSystem.Infrastructure.csproj", "./EclipseWorks.TasksManagementSystem.Infrastructure/"]
RUN dotnet restore "EclipseWorks.TasksManagementSystem.API/EclipseWorks.TasksManagementSystem.API.csproj" --interactive
COPY . .
WORKDIR "/src/EclipseWorks.TasksManagementSystem.API"
RUN dotnet build "EclipseWorks.TasksManagementSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EclipseWorks.TasksManagementSystem.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EclipseWorks.TasksManagementSystem.API.dll"]
