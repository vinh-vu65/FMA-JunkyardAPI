FROM mcr.microsoft.com/dotnet/sdk:6.0 AS dotnetsdk

WORKDIR /src

COPY ./JunkyardWebApp.sln .
COPY ./JunkyardWebApp.API/JunkyardWebApp.API.csproj ./JunkyardWebApp.API/
COPY ./JunkyardWebApp.API.Tests/JunkyardWebApp.API.Tests.csproj ./JunkyardWebApp.API.Tests/
RUN ["dotnet", "restore"]

COPY . .

FROM dotnetsdk AS test
ENTRYPOINT ["dotnet", "test"]

FROM dotnetsdk AS publish
RUN dotnet publish "./JunkyardWebApp.API/JunkyardWebApp.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JunkyardWebApp.API.dll"]