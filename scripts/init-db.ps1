Param(
  [string]$ApiProjectPath = "../sge-api/SGE.Api/SGE.Api.csproj",
  [string]$InfraProjectPath = "../sge-api/SGE.Infrastructure/SGE.Infrastructure.csproj",
  [string]$MigrationName = "Initial"
)

Write-Host "==> Restaurando herramientas..." -ForegroundColor Cyan
dotnet tool restore

Write-Host "==> Compilando..." -ForegroundColor Cyan
dotnet build ../sge-api/SGE.sln

Write-Host "==> Creando migración '$MigrationName'..." -ForegroundColor Cyan
dotnet ef migrations add $MigrationName -p $InfraProjectPath -s $ApiProjectPath

Write-Host "==> Actualizando base de datos..." -ForegroundColor Cyan
dotnet ef database update -p $InfraProjectPath -s $ApiProjectPath

Write-Host "==> Listo." -ForegroundColor Green