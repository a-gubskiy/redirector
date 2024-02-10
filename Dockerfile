FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . ./
WORKDIR /app/src/Redirector

RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Build runtime image
FROM  mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "/app/Redirector.dll"]