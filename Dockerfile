FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish "./React1-backend.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS http://*:80
ENV VIRTUAL_HOST=react1-backend.gstav.se
ENV LETSENCRYPT_HOST=react1-backend.gstav.se
EXPOSE 80

ENTRYPOINT ["dotnet", "React1-backend.dll"]

