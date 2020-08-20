#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Shares.Users/Shares.Users.csproj", "Shares.Users/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "Shares.Users/Shares.Users.csproj"
COPY . .
WORKDIR "/src/Shares.Users"
RUN dotnet build "Shares.Users.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shares.Users.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shares.Users.dll"]