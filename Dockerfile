FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /build

COPY . .

RUN dotnet publish -c Release -o /app PickAndEat.csproj

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "PickAndEat.dll"]
