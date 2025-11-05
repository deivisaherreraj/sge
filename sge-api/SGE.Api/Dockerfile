FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["SGE.Api/SGE.Api.csproj", "SGE.Api/"]
COPY ["SGE.Application/SGE.Application.csproj", "SGE.Application/"]
COPY ["SGE.Infrastructure/SGE.Infrastructure.csproj", "SGE.Infrastructure/"]
COPY ["SGE.Domain/SGE.Domain.csproj", "SGE.Domain/"]

# Restaurar dependencias
RUN dotnet restore "SGE.Api/SGE.Api.csproj"

# Copiar todo el código
COPY . .

# Compilar y publicar
WORKDIR "/src/SGE.Api"
RUN dotnet build "SGE.Api.csproj" -c Release -o /app/build
RUN dotnet publish "SGE.Api.csproj" -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000

ENTRYPOINT ["dotnet", "SGE.Api.dll"]