FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ramendo-api.slnx .
COPY src/Ramendo.Domain/Ramendo.Domain.csproj src/Ramendo.Domain/
COPY src/Ramendo.Application/Ramendo.Application.csproj src/Ramendo.Application/
COPY src/Ramendo.Infrastructure/Ramendo.Infrastructure.csproj src/Ramendo.Infrastructure/
COPY src/Ramendo.Api/Ramendo.Api.csproj src/Ramendo.Api/

RUN dotnet restore ramendo-api.slnx

COPY . .
RUN dotnet publish src/Ramendo.Api/Ramendo.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Ramendo.Api.dll"]
