# Inventory Management System

A production-minded inventory management application built with **ASP.NET Core**, **Blazor WebAssembly**, **Entity Framework Core**, and **ASP.NET Core Identity**.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 10 — Web API |
| Frontend | Blazor WebAssembly |
| ORM | Entity Framework Core |
| Database | SQLite (local) / SQL Server (optional) |
| Auth | ASP.NET Core Identity + JWT |
| Architecture | Clean Architecture + CQRS + MediatR |
| Logging | Serilog |

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) v17.x or higher
- [Git](https://git-scm.com/)

No additional tools required. SQLite is used by default — no database engine installation needed.

---

## Project Structure

```
InventoryManagementSystem.sln
│
├── src/
│   ├── Dominio/                  → Entities, enums
│   ├── Aplicacion/               → Use cases, DTOs, interfaces, MediatR handlers
│   ├── Persistencia/             → EF Core, repositories, Identity, seeder
│   └── Presentacion/
│       ├── InventoryApi/         → ASP.NET Core Web API
│       └── InventoryWeb/         → Blazor WebAssembly
```

---

## Running Locally

### 1. Clone the repository

```bash
git clone https://github.com/your-username/InventoryManagementSystem.git
```

### 2. Open the solution in Visual Studio

Open the file `InventoryManagementSystem.sln` with **Visual Studio 2022**.

### 3. Configure multiple startup projects

Both the API and the Blazor client need to run at the same time. To set this up:

1. Right-click the **Solution** in Solution Explorer
2. Select **"Set Startup Projects..."**
3. Choose **"Multiple startup projects"**
4. Set the following projects to **"Start"**:
   - `InventoryApi`
   - `InventoryWeb`
5. Make sure to select the **https** profile on both projects to use the development certificate provided by Visual Studio
6. Click **OK**

### 4. Run the application

Press **F5** or click the **▶ Start** button in Visual Studio.

> **That's it.** Database migrations run automatically on startup. No manual migration steps needed.

---

## Database

### Default — SQLite (no setup required)

SQLite is configured by default. The `inventory.db` file is created automatically on first run.

Configuration in `appsettings.json`:
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

### Optional — SQL Server

To switch to SQL Server, update `appsettings.json` in `InventoryApi`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=InventoryDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  },
  "ConfigDB": {
    "Db": "SqlServer"
  }
}
```

> Migrations run automatically on startup regardless of the database engine selected.

---

## Migrations

Migrations are applied **automatically** when the API starts. No manual commands needed.

---

## Default Credentials

The following admin user is created automatically on first run:

| Field | Value |
|---|---|
| Email | admin@inventory.com |
| Password | Admin123! |
| Role | Admin |

---

## API Endpoints

| Method | Route | Description | Auth |
|---|---|---|---|
| POST | `/api/auth/login` | Sign in — returns JWT token | Public |
| POST | `/api/auth/register` | Register new user | Public |
| GET | `/api/products` | List products (filter by category, low-stock) | Required |
| GET | `/api/products/{id}` | Get product by ID | Required |
| POST | `/api/products` | Create product | Required |
| PUT | `/api/products/{id}` | Update product | Required |
| DELETE | `/api/products/{id}` | Delete product | Required |
| POST | `/api/products/{id}/movements` | Register stock movement | Required |
| GET | `/api/products/{id}/movements` | Get movement history | Required |

---

## Features Implemented

### Core
- Product CRUD with SKU uniqueness validation
- Stock movement registration (Inbound / Outbound)
- Movement history per product
- Low-stock threshold row highlighting (< 10 units)
- JWT authentication with ASP.NET Core Identity
- Protected API endpoints and Blazor routes

### Optional Challenges
- **Structured logging** — Serilog with `MachineName` and `CorrelationId` enrichers, file and console sinks
- **Low-stock alert** — `IHostedService` checking stock every 30 minutes and logging warnings
- **Audit trail** — `CreatedBy` / `UpdatedBy` fields populated from authenticated user claims

---

## Logs

Application logs are written to:
- **Console** — with machine name and correlation ID per request
- **File** — `logs/inventory-YYYYMMDD.log` with daily rolling

---

## Notes

- The SQLite database file (`inventory.db`) is excluded from version control via `.gitignore`
- Log files (`logs/`) are excluded from version control
- Seed data (3 sample products + admin user) is inserted only if the tables are empty
