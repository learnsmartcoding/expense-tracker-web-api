FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ExpenseTracker.Web/ExpenseTracker.Web.csproj", "ExpenseTracker.Web/"]
COPY ["ExpenseTracker.Core/ExpenseTracker.Core.csproj", "ExpenseTracker.Core/"]
COPY ["ExpenseTracker.Data/ExpenseTracker.Data.csproj", "ExpenseTracker.Data/"]
COPY ["ExpenseTracker.Service/ExpenseTracker.Service.csproj", "ExpenseTracker.Service/"]
RUN dotnet restore "./ExpenseTracker.Web/ExpenseTracker.Web.csproj"
COPY . .
WORKDIR "/src/ExpenseTracker.Web"
RUN dotnet build "./ExpenseTracker.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ExpenseTracker.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpenseTracker.Web.dll"]
