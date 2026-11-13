# Copilot Instructions for DynDyn

DynDyn is a personal financial control system with multiple server implementations and a mobile frontend.

## Repository Structure

### Server Projects

- **`server_v2/src/`** - Primary production API (.NET 6.0 with clean architecture)
  - `Api.Application/` - ASP.NET Core web API, versioned under `V1/`
  - `Api.Domain/` - Domain models, enums, and security configurations
  - `Api.Service/` - Business logic layer
  - `Api.Data/` - Data access layer with Entity Framework Core
  - `Api.CrossCutting/` - Dependency injection and shared utilities
  - `Worker.Service/` - Background worker service (.NET 8.0) for async tasks

- **`server_v3/`** - Alternative Node.js/TypeScript API (Express + Knex)
  - `src/api/` - API controllers and repositories
  - `src/middleware/` - Express middleware
  - `src/router/` - Route definitions
  - `src/utils/` - Utility functions

- **`server/`** - Legacy Somnia.API (.NET implementation, reference only)

### Frontend Projects

- **`mobile/`** - React Native application (Expo)
- **`web_mvp/`** - Web MVP

## Build & Test Commands

### Server V2 (.NET 6.0 / C#)

**Building:**
```bash
cd server_v2/src

# Build the entire solution
dotnet build

# Build specific project
dotnet build Api.Application/Application.csproj
```

**Testing:**
```bash
cd server_v2/src

# Run all tests
dotnet test

# Run tests for a specific test project
dotnet test Api.Data.Test/Api.Data.Test.csproj

# Run tests with code coverage
dotnet test /p:CollectCoverage=true

# Run a specific test class or method
dotnet test --filter "FullyQualifiedName~NamespaceOrClassName"
```

**Running Locally:**
```bash
cd server_v2/src/Api.Application
dotnet run
```

### Server V3 (Node.js/TypeScript)

**Development:**
```bash
cd server_v3

# Run with auto-reload
npm run dev

# Start with nodemon
npm start
```

**Testing:**
```bash
cd server_v3

# Run tests (currently placeholder)
npm test
```

### Database Migrations

**For server_v3** (using Knex):
```bash
cd server_v3

# Run migrations
npx knex migrate:latest

# Create a new migration
npx knex migrate:make migration_name
```

## Architecture & Patterns

### Clean Architecture Layers (server_v2)

The .NET API follows clean architecture with clear separation:

1. **Domain Layer** (`Api.Domain/`)
   - Contains entities, enums, and business rules
   - No external dependencies
   - Examples: `Enums/` folder contains OperationType, PortfolioType, AttributeDataType

2. **Service Layer** (`Api.Service/`)
   - Business logic implementation
   - Orchestrates domain logic

3. **Data Access Layer** (`Api.Data/`)
   - Entity Framework Core configurations
   - Database context and migrations
   - Test doubles for unit testing

4. **Application Layer** (`Api.Application/`)
   - ASP.NET Core web API
   - Controllers are versioned (V1/ for API v1)
   - Dependency injection setup in `Program.cs`

5. **Cross-Cutting Layer** (`Api.CrossCutting/`)
   - Shared utilities
   - Dependency injection configuration

### API Versioning

The API uses URL path versioning:
- Controllers are organized under `V1/` folder: `/api/v1/trash`, `/api/v1/portfolios`, etc.
- Use `Microsoft.AspNetCore.Mvc.Versioning` and `Versioning.ApiExplorer` packages
- Swagger documentation includes API Explorer integration

### Testing Strategy

**XUnit Framework** is used for all tests:
- Test projects named with `.Test` suffix
- Faker.NETCore for generating test data
- Code coverage via Coverlet collector
- Layer-specific tests: Data, Service, Application, and Integration

### Node.js API (server_v3)

- Express.js for routing
- Knex.js for database query builder and migrations
- TypeScript for type safety
- Middleware-based request handling (`src/middleware/`)
- Repository pattern for data access (`src/api/repositories/`)

## Key Conventions

### C# (.NET)

1. **Nullable Reference Types** - Enabled project-wide (`<Nullable>enable</Nullable>`)
2. **Implicit Usings** - Enabled; no need for explicit `using` statements for common namespaces
3. **Project Structure** - Layer-based organization with clear separation
4. **Async/Await** - Use async Task patterns for controllers and services
5. **Dependency Injection** - All dependencies registered in `Program.cs`

### TypeScript (Node.js)

1. **Controllers** - Handle HTTP requests, delegate to repositories
2. **Repositories** - Data access abstraction with base repository pattern
3. **Middleware** - Custom request handling and validation
4. **Utils** - Shared utility functions (date parsing, string manipulation, validation errors)
5. **Interfaces** - Type definitions for request/response contracts

## CI/CD & Deployment

### GitHub Actions Workflows

- **`deploy.yml`** - Builds and deploys API.Application (server_v2) to AWS EC2
  - Triggered on push to `main` with changes in `server_v2/**`
  - Uses .NET 6.0.x
  - Publishes as self-contained linux-x64 binary
  
- **`deploy-worker.yml`** - Builds and deploys Worker.Service to AWS EC2
  - Triggered on changes to `server_v2/src/Worker.Service/**`
  - Uses .NET 8.0.x

Both workflows use SCP to transfer published artifacts and SSH for deployment commands.

## Dependencies & Setup

### .NET Global SDK Version

Defined in `server_v2/src/_global.json`:
- SDK 6.0.428 for main API
- Worker Service uses .NET 8.0.x

### Key NuGet Packages

- **AspNetCore.Authentication.JwtBearer** - JWT authentication
- **EntityFrameworkCore** - ORM for database access
- **Swashbuckle.AspNetCore** - Swagger/OpenAPI documentation
- **AspNetCore.Mvc.Versioning** - API versioning support
- **xunit** - Testing framework

### npm Dependencies (server_v3)

- Express.js - Web framework
- Knex - Query builder and migrations
- TypeScript - Type-safe JavaScript
- pg - PostgreSQL driver
- celebrate - Input validation
- dotenv - Environment variables

## Important Notes

- Multiple server versions exist; ensure clarity about which version to modify
- Worker.Service is separate from the main API but shares some domain models
- API uses JWT authentication (see `Api.Domain/Security/SigningConfiguration.cs`)
- Integration tests exist in `Api.Integration.Test/` for end-to-end testing
- The codebase uses AWS EC2 for deployment; secrets are stored in GitHub
