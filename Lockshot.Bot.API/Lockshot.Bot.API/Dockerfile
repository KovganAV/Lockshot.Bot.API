FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Lockshot.Bot.API.csproj", "Lockshot.User.API/"]
RUN dotnet restore "Lockshot.Bot.API\Lockshot.Bot.API.csproj"

COPY . .
WORKDIR "/src/Lockshot.Bot.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lockshot.Bot.API.dll"]
