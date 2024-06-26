FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /csproj-src
COPY ["src/Infrastructure/Hosys.WebAPI/Hosys.WebAPI.csproj", "Infrastructure/Hosys.WebAPI/"]
RUN dotnet restore "Infrastructure/Hosys.WebAPI/Hosys.WebAPI.csproj"
WORKDIR /src
COPY /src .

RUN dotnet build "Infrastructure/Hosys.WebAPI/Hosys.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Infrastructure/Hosys.WebAPI/Hosys.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Remove source code
RUN rm -rf /src

# Add python3 and pdf2image
RUN apk update
RUN apk add python3 py3-pdf2image

ENTRYPOINT [ "dotnet", "Hosys.WebAPI.dll" ]