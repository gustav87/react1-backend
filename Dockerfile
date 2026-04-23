FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY ./Citrus-Backend/Citrus-Backend.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish "./Citrus-Backend/Citrus-Backend.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS=http://*:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Citrus-Backend.dll"]

