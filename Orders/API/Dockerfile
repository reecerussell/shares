#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Orders/API/API.csproj", "Orders/API/"]
COPY ["Orders/Domain/Domain.csproj", "Orders/Domain/"]
COPY ["Shares.Core/Shares.Core.csproj", "Shares.Core/"]
COPY ["Orders/Infrastructure/Infrastructure.csproj", "Orders/Infrastructure/"]
COPY ["AWS/AWS/AWS.csproj", "AWS/AWS/"]
RUN dotnet restore "Orders/API/API.csproj"
COPY . .
WORKDIR "/src/Orders/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orders.API.dll"]