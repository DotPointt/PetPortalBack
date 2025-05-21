#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./PetPortalBack/PetPortalAPI/PetPortalAPI/PetPortalAPI.csproj", "PetPortalAPI/PetPortalAPI/"]
COPY ["./PetPortalBack/PetPortalApplication/PetPortalApplication.csproj", "PetPortalApplication/"]
COPY ["./PetPortalBack/PetPortalCore/PetPortalCore.csproj", "PetPortalCore/"]
COPY ["./PetPortalBack/PetPortalDAL/PetPortalDAL.csproj", "PetPortalDAL/"]
RUN dotnet restore "PetPortalAPI/PetPortalAPI/PetPortalAPI.csproj"
COPY ./PetPortalBack /src/PetPortalBack
WORKDIR "/src/PetPortalBack/PetPortalAPI/PetPortalAPI"
RUN dotnet build "PetPortalAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PetPortalAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#COPY --from=publish /app/publish/*.xml ./
ENTRYPOINT ["dotnet", "PetPortalAPI.dll"]