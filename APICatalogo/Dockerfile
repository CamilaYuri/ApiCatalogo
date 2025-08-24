FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out
RUN mkdir ./out/db

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /runtime-app
COPY --from=build-env /app/out .

RUN apt-get update && apt-get install -y default-mysql-client && rm -rf /var/lib/apt/lists/*

EXPOSE 8080

ENTRYPOINT ["dotnet", "APICatalogo.dll"]