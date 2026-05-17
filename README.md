# Sistema de Gestión de Inventario

Aplicación de gestión de inventario construida con **ASP.NET Core**, **Blazor WebAssembly**, **Entity Framework Core** y **ASP.NET Core Identity**.

---

## Stack Tecnológico

| Capa | Tecnología |
|---|---|
| Backend | ASP.NET Core 10 — Web API |
| Frontend | Blazor WebAssembly |
| ORM | Entity Framework Core |
| Base de datos | SQLite (local) / SQL Server (opcional) |
| Autenticación | ASP.NET Core Identity + JWT |
| Arquitectura | Clean Architecture + CQRS + MediatR |
| Logging | Serilog |

---

## Prerrequisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) v17.x o superior
- [Git](https://git-scm.com/)

No se requieren herramientas adicionales. SQLite se usa por defecto — no es necesario instalar ningún motor de base de datos.

---

## Estructura del Proyecto

```
InventoryManagementSystem.sln
│
├── src/
│   ├── Dominio/                  → Entidades, enums
│   ├── Aplicacion/               → Casos de uso, DTOs, interfaces, handlers MediatR
│   ├── Persistencia/             → EF Core, repositorios, Identity, seeder
│   └── Presentacion/
│       ├── InventoryApi/         → ASP.NET Core Web API
│       └── InventoryWeb/         → Blazor WebAssembly
```

---

## Cómo Ejecutar Localmente

### 1. Clonar el repositorio

```bash
git clone https://github.com/your-username/InventoryManagementSystem.git
```

### 2. Abrir la solución en Visual Studio

Abrir el archivo `InventoryManagementSystem.sln` con **Visual Studio 2022**.

### 3. Configurar múltiples proyectos de inicio

Es necesario ejecutar la API y el cliente Blazor al mismo tiempo. Para configurarlo:

1. Click derecho en la **Solución** en el Solution Explorer
2. Seleccionar **"Set Startup Projects..."**
3. Elegir la opción **"Multiple startup projects"**
4. Establecer los siguientes proyectos en **"Start"**:
   - `InventoryApi`
   - `InventoryWeb`
5. Asegurarse de seleccionar el perfil **https** en ambos proyectos para usar el certificado de desarrollo que provee Visual Studio
6. Click en **OK**

### 4. Ejecutar la aplicación

Presionar **F5** o el botón **▶ Start** en Visual Studio.

> **Eso es todo.** Las migraciones de base de datos se ejecutan automáticamente al iniciar. No se requieren pasos manuales de migración.

---

## Base de Datos

### Por defecto — SQLite (no requiere configuración)

SQLite está configurado por defecto. El archivo `inventory.db` se crea automáticamente en el primer inicio.

Configuración en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=inventory.db"
  },
  "ConfigDB": {
    "Db": "Sqlite"
  }
}
```

### Opcional — SQL Server

Para cambiar a SQL Server, actualizar `appsettings.json` en `InventoryApi`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=InventoryDb;User Id=sa;Password=TuPassword;TrustServerCertificate=True;"
  },
  "ConfigDB": {
    "Db": "SqlServer"
  }
}
```

> Las migraciones se aplican automáticamente al iniciar la aplicación independientemente del motor de base de datos seleccionado.

---

## Migraciones

Las migraciones se aplican **automáticamente** cuando la API inicia. No se requieren comandos manuales.

---

## Credenciales de Prueba

El siguiente usuario administrador se crea automáticamente en el primer inicio:

| Campo | Valor |
|---|---|
| Email | admin@inventory.com |
| Contraseña | Admin123! |
| Rol | Admin |

---

## Endpoints de la API

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| POST | `/api/auth/login` | Iniciar sesión — retorna token JWT | Público |
| POST | `/api/auth/register` | Registrar nuevo usuario | Público |
| GET | `/api/products` | Listar productos (filtrar por categoría, bajo stock) | Requerida |
| GET | `/api/products/{id}` | Obtener producto por ID | Requerida |
| POST | `/api/products` | Crear producto | Requerida |
| PUT | `/api/products/{id}` | Actualizar producto | Requerida |
| DELETE | `/api/products/{id}` | Eliminar producto | Requerida |
| POST | `/api/products/{id}/movements` | Registrar movimiento de stock | Requerida |
| GET | `/api/products/{id}/movements` | Obtener historial de movimientos | Requerida |

---

## Funcionalidades Implementadas

### Requeridas
- CRUD de productos con validación de SKU único
- Registro de movimientos de stock (Entrada / Salida)
- Historial de movimientos por producto
- Resaltado de filas con bajo stock (< 10 unidades)
- Autenticación JWT con ASP.NET Core Identity
- Endpoints protegidos y rutas de Blazor protegidas

### Opcionales
- **Logging estructurado** — Serilog con enrichers `MachineName` y `CorrelationId`, sinks de archivo y consola
- **Alerta de bajo stock** — `IHostedService` que verifica el stock cada 30 minutos y registra advertencias
- **Audit trail** — campos `CreatedBy` / `UpdatedBy` poblados desde los claims del usuario autenticado

---

## Logs

Los logs de la aplicación se escriben en:
- **Consola** — con nombre de máquina y correlation ID por request
- **Archivo** — `logs/inventory-YYYYMMDD.log` con rotación diaria

---

## Notas

- El archivo de base de datos SQLite (`inventory.db`) está excluido del control de versiones via `.gitignore`
- Los archivos de log (`logs/`) están excluidos del control de versiones
- Los datos iniciales (3 productos de ejemplo + usuario admin) se insertan solo si las tablas están vacías
