#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["server/src/IssueTracker/IssueTracker.csproj", "server/src/IssueTracker/"]
COPY ["server/src/IssueTracker.Application/IssueTracker.Application.csproj", "server/src/IssueTracker.Application/"]
COPY ["server/src/IssueTracker.Domain/IssueTracker.Domain.csproj", "server/src/IssueTracker.Domain/"]
COPY ["server/src/IssueTracker.Infrastructure/IssueTracker.Infrastructure.csproj", "server/src/IssueTracker.Infrastructure/"]
RUN dotnet restore "server/src/IssueTracker/IssueTracker.csproj"
COPY . .
WORKDIR "/src/server/src/IssueTracker"
RUN dotnet build "IssueTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IssueTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IssueTracker.dll"]