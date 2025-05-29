# Coderland - Gestión de Marcas de Automóviles

Proyecto .NET Core 8.0 con arquitectura hexagonal para gestionar marcas de automóviles, utilizando Entity Framework Core, PostgreSQL y Docker.

## Estructura del Proyecto

El proyecto sigue una arquitectura hexagonal (también conocida como arquitectura de puertos y adaptadores):

- **Coderland.API**: Proyecto principal de la API REST
  - **Domain**: Contiene las entidades y interfaces del dominio
    - **Entities**: Clases de dominio (MarcaAuto)
    - **Interfaces**: Interfaces de repositorios
  - **Application**: Contiene la lógica de aplicación
    - **Interfaces**: Interfaces de servicios
    - **Services**: Implementaciones de servicios
  - **Infrastructure**: Contiene implementaciones concretas
    - **Persistence**: Configuración de EF Core y DbContext
    - **Repositories**: Implementaciones de repositorios
  - **Presentation**: Contiene los controladores API
    - **Controllers**: Controladores REST

- **Coderland.Test**: Proyecto de pruebas unitarias con XUnit

## Requisitos

- .NET Core SDK 8.0
- Docker y Docker Compose
- PostgreSQL (o usar la versión en contenedor)

## Configuración y Ejecución

### Configuración de la Base de Datos

La aplicación utiliza PostgreSQL con las siguientes configuraciones:
- Nombre de la base de datos: `nombre_db`
- Usuario: `tu_usuario`
- Contraseña: `tu_password`

### Ejecución Local

1. Clonar el repositorio
2. Restaurar los paquetes NuGet:
   ```
   dotnet restore
   ```
3. Ejecutar las migraciones de la base de datos:
   ```
   cd Coderland.API
   dotnet ef database update
   ```
4. Ejecutar la aplicación:
   ```
   dotnet run --project Coderland.API
   ```

### Ejecución con Docker Compose

1. Construir y ejecutar los contenedores:
   ```
   docker-compose up -d
   ```
2. La API estará disponible en: http://localhost:8080

## Endpoints API

- `GET /api/MarcasAutos`: Obtiene todas las marcas de automóviles
- `GET /api/MarcasAutos/{id}`: Obtiene una marca de automóvil por su ID

## Pruebas Unitarias

Para ejecutar las pruebas unitarias:

```
dotnet test Coderland.Test
```

## Características Principales

- Arquitectura hexagonal para separación de responsabilidades
- Entity Framework Core con PostgreSQL
- Migraciones y Data Seeding
- Pruebas unitarias con base de datos en memoria
- Configuración Docker para desarrollo y despliegue
