FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PetPortalAPI/PetPortalAPI.csproj", "PetPortalAPI/"]
COPY ["../PetPortalCore/PetPortalCore.csproj", "../PetPortalCore/"]
COPY ["../PetPortalApplication/PetPortalApplication.csproj", "../PetPortalApplication/"]
COPY ["../PetPortalDAL/PetPortalDAL.csproj", "../PetPortalDAL/"]
RUN dotnet restore "PetPortalAPI/PetPortalAPI.csproj"
COPY . .
WORKDIR "/src/PetPortalAPI"
RUN dotnet build "PetPortalAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PetPortalAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetPortalAPI.dll"]
