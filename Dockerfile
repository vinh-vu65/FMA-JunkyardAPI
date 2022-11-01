FROM mcr.microsoft.com/dotnet/sdk:6.0 AS dotnetsdk

WORKDIR /src

COPY .JunkyardWebApp.sln .
COPY ./JunkyardWebApp.API/JunkyardWebApp.API.csproj ./JunkyardWebApp.API/
COPY ./JunkyardWebApp.API.Tests/JunkyardWebApp.API.Tests.csproj ./JunkyardWebApp.API.Tests/
RUN ["dotnet", "restore"]

COPY . .

FROM dotnetsdk AS TEST
ENTRYPOINT ["dotnet", "test"]