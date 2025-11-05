# Sistema de Gestión de Empleados (SGE)

MVP funcional para gestión de empleados con CRUD completo, búsqueda, paginación y ordenamiento.

## 📋 Descripción

Sistema completo con backend .NET 8 y frontend Ionic 7 + Angular 17 que permite:
- ✅ CRUD de empleados
- 🔍 Búsqueda por ID o nombre
- 📄 Paginación y ordenamiento
- 🏢 Gestión de departamentos
- 🎨 Interfaz responsive tipo escritorio

## 🏗️ Arquitectura

```
┌─────────────────────────────────────────────────────┐
│                   IONIC + ANGULAR                   │
│          (PWA Responsive - Puerto 8100)             │
└──────────────────┬──────────────────────────────────┘
                   │ HTTP/REST
                   ↓
┌─────────────────────────────────────────────────────┐
│              .NET 8 Web API (HTTPS)                 │
│                  Puerto 5001                        │
├─────────────────────────────────────────────────────┤
│  Controllers → Services → Repositories → EF Core    │
│                                                     │
│  ┌─────────────────────────────────────────────┐   │
│  │ SGE.Api (Controllers, DI, Swagger, CORS)    │   │
│  └─────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────┐   │
│  │ SGE.Application (Services, DTOs, Logic)     │   │
│  └─────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────┐   │
│  │ SGE.Infrastructure (EF Core, Repos, Seed)   │   │
│  └─────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────┐   │
│  │ SGE.Domain (Entities, Contracts)            │   │
│  └─────────────────────────────────────────────┘   │
└──────────────────┬──────────────────────────────────┘
                   │
                   ↓
┌─────────────────────────────────────────────────────┐
│           SQL Server LocalDB / Docker               │
│              Base de datos: SGE                     │
└─────────────────────────────────────────────────────┘
```

## 🛠️ Stack Tecnológico

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server (LocalDB o Docker)
- xUnit (tests)
- Swagger/OpenAPI

### Frontend
- Ionic 7
- Angular 17
- TypeScript
- RxJS
- Reactive Forms

## 📁 Estructura del Proyecto

```
sge/
├── sge-api/                          # Backend .NET
│   ├── SGE.Domain/                   # Entidades y contratos
│   │   ├── Entities/
│   │   │   ├── Employee.cs
│   │   │   └── Department.cs
│   │   └── Contracts/
│   │       └── IEmployeeRepository.cs
│   ├── SGE.Application/              # Lógica de negocio
│   │   ├── DTOs/
│   │   │   ├── EmployeeCreateDto.cs
│   │   │   ├── EmployeeUpdateDto.cs
│   │   │   ├── EmployeeListItemDto.cs
│   │   │   ├── DepartmentDto.cs
│   │   │   └── PagedResult.cs
│   │   └── Services/
│   │       ├── IEmployeeService.cs
│   │       └── EmployeeService.cs
│   ├── SGE.Infrastructure/           # Persistencia
│   │   ├── Persistence/
│   │   │   └── SgeDbContext.cs
│   │   ├── Repositories/
│   │   │   └── EmployeeRepository.cs
│   │   └── Seed/
│   │       └── DatabaseSeeder.cs
│   ├── SGE.Api/                      # API REST
│   │   ├── Controllers/
│   │   │   ├── EmployeesController.cs
│   │   │   └── DepartmentsController.cs
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   └── SGE.Tests/                    # Tests unitarios
│       └── EmployeeServiceTests.cs
├── sge-app/                          # Frontend Ionic
│   └── src/
│       ├── app/
│       │   ├── core/
│       │   │   └── models/
│       │   │       └── employee.model.ts
│       │   └── features/
│       │       └── employees/
│       │           ├── employees.service.ts
│       │           └── pages/
│       │               ├── list/
│       │               │   ├── list.page.ts
│       │               │   ├── list.page.html
│       │               │   └── list.page.scss
│       │               └── form/
│       │                   ├── form.page.ts
│       │                   ├── form.page.html
│       │                   └── form.page.scss
│       └── environments/
│           └── environment.ts
├── scripts/
│   └── init-db.ps1                   # Script inicialización DB
├── .gitignore                        # Ignorar artefactos
├── docker-compose.yml                # Docker setup completo
└── README.md
```

## 🚀 Instalación y Configuración

### Prerrequisitos

```powershell
# .NET 8 SDK
# Descargar desde: https://dotnet.microsoft.com/download/dotnet/8.0

# Node.js >= 18
# Descargar desde: https://nodejs.org/

# Herramientas globales
npm install -g @angular/cli @ionic/cli
dotnet tool install --global dotnet-ef

# SQL Server LocalDB (incluido en Visual Studio)
# O Docker Desktop para usar contenedor
```

### Opción 1: Instalación Rápida con LocalDB

```powershell
# 1. Clonar o descargar el proyecto
cd c:\Users\Deivis\source\repos\sge

# 2. Restaurar backend
cd sge-api
dotnet restore
dotnet build

# 3. Crear base de datos y aplicar migraciones
cd ..
.\scripts\init-db.ps1

# 4. Ejecutar API
cd sge-api
dotnet run --project .\SGE.Api\SGE.Api.csproj

# En otra terminal: Ejecutar frontend
cd ..\sge-app
npm install
ionic serve
```

**URLs:**
- API: `https://localhost:5001`
- Swagger: `https://localhost:5001/swagger`
- App: `http://localhost:8100`

### Opción 2: Con Docker Compose

#### Método A: Migraciones locales (Recomendado)

```powershell
# 1. Aplicar migraciones ANTES de Docker (recomendado)
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass -Force
pwsh -NoProfile -ExecutionPolicy Bypass -File .\scripts\init-db.ps1

# 2. Iniciar todos los servicios
docker compose up --build

# 3. Verificar servicios
docker compose ps
```

#### Método B: Migraciones dentro del contenedor

```powershell
# 1. Iniciar contenedores
docker-compose up -d

# 2. Esperar a que SQL Server esté listo (~30s)
docker compose logs -f sqlserver

# 3. Iniciar API y aplicar migraciones dentro del contenedor
docker compose up -d sge-api
docker compose exec sge-api dotnet ef database update

# 4. En otra terminal: Frontend
cd sge-app
npm install
ionic serve
```

**URLs:**
- API: `http://localhost:5000`
- API Swagger: `http://localhost:5000/swagger`
- App: `http://localhost:8100`
- SQL Server: `localhost,1433` (sa / ydQk3zrE3kBOLON4sw9a)

## Script de Inicialización de Base de Datos

### Uso de `.\scripts\init-db.ps1`

Este script **idempotente** crea la base de datos y aplica las migraciones de Entity Framework:

```powershell
# Ejecución básica (desde la raíz del proyecto)
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass -Force
pwsh -NoProfile -ExecutionPolicy Bypass -File .\scripts\init-db.ps1
```

### Parámetros Soportados

| Parámetro | Descripción | Valor por defecto |
|-----------|-------------|-------------------|
| `-ApiProjectPath` | Ruta al proyecto startup (.csproj) | `../sge-api/SGE.Api/SGE.Api.csproj` |
| `-InfraProjectPath` | Ruta al proyecto de infraestructura | `../sge-api/SGE.Infrastructure/SGE.Infrastructure.csproj` |
| `-MigrationName` | Nombre de la migración inicial | `Initial` |

### Ejemplos de Uso

```powershell
# Uso básico
.\scripts\init-db.ps1

# Con parámetros personalizados
.\scripts\init-db.ps1 -MigrationName "InitialMigration" -ApiProjectPath ".\sge-api\SGE.Api\SGE.Api.csproj"

# Desde otro directorio
pwsh -NoProfile -ExecutionPolicy Bypass -File "C:\ruta\completa\scripts\init-db.ps1"
```

### ¿Qué hace el script?

1. **Restaura herramientas**: `dotnet tool restore`
2. **Compila la solución**: `dotnet build ../sge-api/SGE.sln`
3. **Crea migración**: `dotnet ef migrations add [nombre]`
4. **Actualiza BD**: `dotnet ef database update`

### Troubleshooting del Script

#### Error: "No se puede ejecutar scripts"
```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass -Force
```

#### Error: "dotnet ef no encontrado"
```powershell
dotnet tool install --global dotnet-ef
# o desde el proyecto:
dotnet tool restore
```

#### Error de conexión SQL Server
- **Local**: Verificar que SQL Server esté ejecutándose
- **Docker**: `docker compose up -d sqlserver && docker compose logs -f sqlserver`

## 📊 Comandos Útiles

### Entity Framework

```powershell
# Crear migración
dotnet ef migrations add NombreMigracion `
  -p .\SGE.Infrastructure\SGE.Infrastructure.csproj `
  -s .\SGE.Api\SGE.Api.csproj

# Aplicar migraciones
dotnet ef database update `
  -p .\SGE.Infrastructure\SGE.Infrastructure.csproj `
  -s .\SGE.Api\SGE.Api.csproj

# Revertir migración
dotnet ef database update PreviousMigration `
  -p .\SGE.Infrastructure\SGE.Infrastructure.csproj `
  -s .\SGE.Api\SGE.Api.csproj

# Eliminar última migración
dotnet ef migrations remove `
  -p .\SGE.Infrastructure\SGE.Infrastructure.csproj `
  -s .\SGE.Api\SGE.Api.csproj

# Ver migraciones aplicadas
dotnet ef migrations list `
  -p .\SGE.Infrastructure\SGE.Infrastructure.csproj `
  -s .\SGE.Api\SGE.Api.csproj
```

### Tests

```powershell
cd sge-api

# Ejecutar todos los tests
dotnet test

# Con cobertura
dotnet test /p:CollectCoverage=true

# Solo un proyecto
dotnet test .\SGE.Tests\SGE.Tests.csproj
```

### Ionic/Angular

```powershell
cd sge-app

# Desarrollo
ionic serve
# o
ng serve

# Build producción
ionic build --prod

# Generar componentes
ionic g component shared/mi-componente
ionic g page features/nueva-seccion
ionic g service core/services/mi-servicio

# Tests
ng test
```

## 🔗 Endpoints API

### Empleados

| Método | Endpoint | Descripción | Parámetros |
|--------|----------|-------------|------------|
| GET | `/api/employees` | Lista paginada | `query`, `page`, `pageSize`, `orderBy`, `desc` |
| GET | `/api/employees/{id}` | Empleado por ID | - |
| POST | `/api/employees` | Crear empleado | Body: `EmployeeCreateDto` |
| PUT | `/api/employees/{id}` | Actualizar empleado | Body: `EmployeeUpdateDto` |
| DELETE | `/api/employees/{id}` | Eliminar empleado | - |

### Departamentos

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/departments` | Lista completa |

### Ejemplos cURL

```bash
# Listar empleados
curl -X GET "https://localhost:5001/api/employees?page=1&pageSize=10" -k

# Buscar por nombre
curl -X GET "https://localhost:5001/api/employees?query=Ana" -k

# Crear empleado
curl -X POST "https://localhost:5001/api/employees" \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Juan Pérez",
    "hireDate": "2024-01-15",
    "role": "Developer",
    "salary": 5000,
    "departmentId": 3
  }' -k

# Actualizar empleado
curl -X PUT "https://localhost:5001/api/employees/1" \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Juan Pérez Actualizado",
    "hireDate": "2024-01-15",
    "role": "Senior Developer",
    "salary": 6000,
    "departmentId": 3
  }' -k

# Eliminar empleado
curl -X DELETE "https://localhost:5001/api/employees/1" -k
```

## 🎯 Decisiones de Diseño (Principios SOLID)

### Single Responsibility Principle (SRP)
- **Entidades**: Solo representan el modelo de datos
- **Repositorios**: Solo acceso a datos
- **Servicios**: Solo lógica de negocio
- **Controllers**: Solo orquestación HTTP

### Open/Closed Principle (OCP)
- Uso de **interfaces** (`IEmployeeService`, `IEmployeeRepository`) permite extender sin modificar
- Nuevos servicios/repos se agregan sin tocar código existente

### Liskov Substitution Principle (LSP)
- Cualquier implementación de `IEmployeeRepository` es intercambiable
- Inyección de dependencias permite cambiar implementaciones

### Interface Segregation Principle (ISP)
- Contratos específicos por dominio (`IEmployeeRepository` ≠ `IDepartmentRepository`)
- Clientes no dependen de métodos que no usan

### Dependency Inversion Principle (DIP)
- Controllers dependen de **abstracciones** (interfaces), no de implementaciones
- Configuración en `Program.cs`:
  ```csharp
  builder.Services.AddScoped<IEmployeeService, EmployeeService>();
  builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
  ```

### Otros Patrones
- **Repository Pattern**: Abstrae acceso a datos
- **Service Layer**: Encapsula lógica de negocio
- **DTO Pattern**: Desacopla API de entidades
- **Problem Details**: Manejo estandarizado de errores (RFC 7807)

## 🔒 Configuración CORS

El backend permite requests desde `http://localhost:8100` (Ionic dev server).

Para producción, actualizar en `Program.cs`:

```csharp
builder.Services.AddCors(o => o.AddPolicy(cors, p =>
    p.WithOrigins("https://tu-dominio.com", "https://app.tu-dominio.com")
     .AllowAnyHeader()
     .AllowAnyMethod()
));
```

## 📸 Capturas de Pantalla

*(Agregar capturas en `/docs/screenshots/`)*

- `01-lista-empleados.png`: Vista principal con tabla
- `02-busqueda.png`: Búsqueda en acción
- `03-formulario-crear.png`: Formulario nuevo empleado
- `04-formulario-editar.png`: Formulario edición
- `05-swagger.png`: Documentación API

## 🐛 Troubleshooting

### Error: "Login failed for user"
**Causa**: Credenciales incorrectas en connection string  
**Solución**: Verificar `appsettings.Development.json` o variables Docker

### Error: "Cannot connect to LocalDB"
**Causa**: SQL Server LocalDB no instalado  
**Solución**: 
```powershell
sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
```

### Error: "CORS policy blocked"
**Causa**: Frontend en puerto diferente a 8100  
**Solución**: Actualizar origen en `Program.cs` línea 14

### Error: "No se encuentra el módulo @angular/core"
**Causa**: Dependencias no instaladas  
**Solución**: 
```powershell
cd sge-app
npm install
```

### Error 404 en API
**Causa**: API no ejecutándose  
**Solución**: Verificar que `dotnet run` esté activo en https://localhost:5001

## 📝 Datos de Prueba (Seed)

Al ejecutar por primera vez, se crean:

**Departamentos:**
- Recursos Humanos
- Finanzas
- Tecnología
- Operaciones

**Empleados:**
- Ana Pérez - Developer - Tecnología - $4,500
- Luis Díaz - QA - Tecnología - $3,800
- Marta Ríos - HR - Recursos Humanos - $4,200

## 🧪 Testing

```powershell
# Backend: xUnit
cd sge-api
dotnet test

# Frontend: Jasmine/Karma (opcional)
cd sge-app
ng test
```

**Tests incluidos:**
- `EmployeeServiceTests`: Casos de uso del servicio
- Validaciones de DTOs
- Búsqueda y paginación

## 📦 Producción

### Backend (.NET)

```powershell
dotnet publish sge-api/SGE.Api/SGE.Api.csproj -c Release -o ./publish
```

Configurar:
- Connection string en `appsettings.Production.json`
- Variables de entorno
- IIS / Kestrel / Docker

### Frontend (Ionic)

```powershell
cd sge-app
ionic build --prod

# Desplegar carpeta www/ a servidor web
# O generar APK/IPA con Capacitor
ionic capacitor add android
ionic capacitor build android
```

## 🔐 Seguridad (Para Producción)

⚠️ Este MVP **NO** incluye autenticación/autorización.

Para producción, agregar:
- **JWT Authentication** en backend
- **Guards** en rutas Angular
- **HTTPS** obligatorio
- **Rate limiting**
- **Validación de entrada** robusta
- **Secrets management** (Azure Key Vault, AWS Secrets)

## 📄 Licencia

Este proyecto es un MVP educativo. Usar bajo tu propia responsabilidad.

## 👥 Autor

Sistema de Gestión de Empleados - MVP 2024

---

**¿Preguntas?** Revisa la documentación de:
- [.NET 8](https://learn.microsoft.com/es-es/dotnet/core/whats-new/dotnet-8)
- [Entity Framework Core](https://learn.microsoft.com/es-es/ef/core/)
- [Ionic Framework](https://ionicframework.com/docs)
- [Angular](https://angular.io/docs)