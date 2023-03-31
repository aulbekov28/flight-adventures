FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FlightAdventures/FlightAdventures.csproj", "FlightAdventures/"]
RUN dotnet restore "FlightAdventures/FlightAdventures.csproj"
COPY . .
WORKDIR "/src/FlightAdventures"
RUN dotnet build "FlightAdventures.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlightAdventures.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightAdventures.dll"]
