#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Auth/API/API.csproj", "Auth/API/"]
COPY ["Auth/Domain/Domain.csproj", "Auth/Domain/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Auth/Infrastructure/Infrastructure.csproj", "Auth/Infrastructure/"]
COPY ["AWS/AWS/AWS.csproj", "AWS/AWS/"]
RUN dotnet restore "Auth/API/API.csproj"
COPY . .
WORKDIR "/src/Auth/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY credentials /app/aws-credentials

ENTRYPOINT ["dotnet", "API.dll"]