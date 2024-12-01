FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish "./React1-Backend.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS http://*:80
ENV VIRTUAL_HOST=React1-Backend.gstav.se
ENV LETSENCRYPT_HOST=React1-Backend.gstav.se
EXPOSE 80

ENTRYPOINT ["dotnet", "React1-Backend.dll"]

