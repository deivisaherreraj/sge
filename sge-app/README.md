# SGE App - Frontend

Aplicación web desarrollada con **Ionic 7 + Angular 20** para el Sistema de Gestión de Empleados.

## 🛠️ Tecnologías

- **Ionic 7** - Framework híbrido multiplataforma
- **Angular 20** - Framework frontend de Google
- **TypeScript** - Superset tipado de JavaScript
- **Bootstrap 5** - Framework CSS (integrado en `angular.json`)
- **RxJS** - Programación reactiva
- **Capacitor** - Bridge nativo para móviles

## 🚀 Ejecución Local

### Prerrequisitos
- Node.js 20+ LTS
- npm 10+
- @ionic/cli global:
  ```bash
  npm install -g @ionic/cli
  ```

### Pasos

1. **Instalar dependencias:**
   ```bash
   npm ci
   ```

2. **Configurar API backend:**
   
   Editar `src/environments/environment.ts`:
   ```typescript
   export const environment = {
     production: false,
     apiBaseUrl: 'http://localhost:5000/api'  // Puerto del backend
   };
   ```

3. **Ejecutar en modo desarrollo:**
   ```bash
   # Ionic CLI (recomendado)
   ionic serve
   
   # o con Angular CLI
   ng serve --host=localhost --port=8100
   ```

4. **Abrir navegador:**
   - App: http://localhost:8100

## 📱 Build Producción

```bash
# Build optimizado
ionic build --prod

# Los archivos se generan en: ./www/
```

## 🔧 Scripts Disponibles

| Comando | Descripción | Uso |
|---------|-------------|-----|
| `npm start` | Desarrollo con ng serve | Para desarrollo |
| `npm run build` | Build de producción | Para deployment |
| `npm test` | Ejecutar tests Jasmine/Karma | Testing |
| `npm run lint` | Linter del código | Code quality |

### Scripts Ionic

```bash
# Generar componentes
ionic g component shared/mi-componente
ionic g page features/nueva-pagina
ionic g service core/services/mi-servicio

# Previsualizar en dispositivos
ionic serve --lab

# Build para móviles
ionic cap build ios
ionic cap build android
```

## 🏗️ Arquitectura del Proyecto

```
src/
├── app/
│   ├── core/                    # Servicios core y modelos
│   │   ├── models/
│   │   │   └── employee.model.ts
│   │   └── services/
│   ├── features/                # Características principales
│   │   └── employees/
│   │       ├── employees.service.ts
│   │       └── pages/
│   │           ├── list/        # Página de listado
│   │           └── form/        # Página de formulario
│   ├── shared/                  # Componentes compartidos
│   └── app-routing.module.ts    # Rutas principales
├── environments/                # Configuración de entornos
│   ├── environment.ts
│   └── environment.prod.ts
├── theme/                       # Variables SCSS e Ionic
└── assets/                      # Recursos estáticos
```

## 🎨 Estilos y Theming

### Bootstrap 5
Configurado en `angular.json`:
```json
"styles": [
  "node_modules/bootstrap/dist/css/bootstrap.min.css",
  "src/global.scss",
  "src/theme/variables.scss"
]
```

### Variables Ionic
Personalizar en `src/theme/variables.scss`:
```scss
:root {
  --ion-color-primary: #3880ff;
  --ion-color-secondary: #0cd1e8;
  // ... más variables
}
```

## 🔌 Configuración de API

### Cambiar URL del backend

1. **Desarrollo:** `src/environments/environment.ts`
2. **Producción:** `src/environments/environment.prod.ts`

```typescript
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5000/api' // Cambiar aquí
};
```

### Servicios HTTP

Los servicios utilizan `HttpClient` de Angular:
```typescript
constructor(private http: HttpClient) {}

getEmployees(): Observable<Employee[]> {
  return this.http.get<Employee[]>(`${environment.apiBaseUrl}/employees`);
}
```

## 🧪 Testing

```bash
# Ejecutar tests una vez
npm test

# Modo watch (desarrollo)
ng test --watch

# Con cobertura
ng test --code-coverage
```

## 📱 Capacitor (Móviles)

```bash
# Agregar plataformas
ionic cap add ios
ionic cap add android

# Sincronizar cambios
ionic cap sync

# Abrir en IDE nativo
ionic cap open ios
ionic cap open android
```

## 🐛 Troubleshooting

### Error de CORS
Verificar que el backend tenga configurado CORS para `http://localhost:8100`:
```csharp
// En .NET API Program.cs
p => p.WithOrigins("http://localhost:8100")
```

### Puerto ocupado
```bash
ionic serve --port=8101
# o
ng serve --port=8101
```

### Error de dependencias
```bash
# Limpiar caché
npm ci
rm -rf node_modules package-lock.json
npm install
```

### Problemas con Ionic CLI
```bash
npm uninstall -g @ionic/cli
npm install -g @ionic/cli@latest
```