# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build the solution
dotnet build

# Run the API (starts on https://localhost:7127)
# In dev, HTTPS redirection is skipped — use http://localhost:5062 instead:
dotnet run --project ClinicFlow --urls "http://localhost:5062"

# Apply database migrations
dotnet ef database update --project ClinicFlow.Infrastructure/ClinicFlow.Infrastructure.csproj --startup-project ClinicFlow.csproj

# Add a new EF Core migration
dotnet ef migrations add <MigrationName> --project ClinicFlow.Infrastructure/ClinicFlow.Infrastructure.csproj --startup-project ClinicFlow.csproj

# Scaffold Azure AD app registration (dotnet tool)
dotnet msidentity --tenant-id <id> --client-id <id>
```

There are no test projects yet.

`PROGRESS.txt` (repo root) is a living, actively-maintained log of implementation status, the full API reference, and known issues/fixes — check its "Last updated" date and read it before making non-trivial changes.

## Architecture

Clean Architecture with four layers:

- **ClinicFlow.Domain** — Entities, enums, repository interfaces (`ICustomerRepository`, `IUnitOfWork`, etc.). No external dependencies.
- **ClinicFlow.Application** — CQRS features (MediatR), DTOs, FluentValidation validators, pipeline behaviors, and custom exceptions. Depends only on Domain.
- **ClinicFlow.Infrastructure** — EF Core `ClinicFlowDbContext`, entity configurations, repository implementations, migrations. Depends on Domain.
- **ClinicFlow** (API) — ASP.NET Core controllers, middleware, `ICurrentClinicService` implementation. Wires up all layers.

### CQRS / MediatR pattern

Every feature lives under `ClinicFlow.Application/Features/<Domain>/`:
```
Commands/CreateFoo/
  CreateFooCommand.cs       # record with properties
  CreateFooCommandHandler.cs # returns Guid or void
  CreateFooCommandValidator.cs # FluentValidation rules
Queries/GetFoos/
  GetFoosQuery.cs
  GetFoosQueryHandler.cs    # returns DTO or list
DTOs/
  FooDto.cs
```

Validators run automatically via `ValidationBehavior` (MediatR pipeline) before any handler executes. Never call validators manually.

### Multi-tenancy

Every entity has a `ClinicId` (Guid). All repository queries filter by `ClinicId` — this must be maintained on any new repository method.

`TenantMiddleware` resolves the current clinic and stores it in `HttpContext.Items["ClinicId"]`. `CurrentClinicService` (scoped) reads it. Resolution order:
1. Look up the `preferred_username`/email claim from the JWT, then `IStaffRepository.GetByEmailAsync(email)` (global lookup, no `ClinicId` — this is how the middleware discovers which clinic a user belongs to) to get the Staff row's `ClinicId`.
2. If there's no email claim and the host is Development, fall back to `DevClinicId` from `appsettings.Development.json` (no JWT needed) — remove this bypass before shipping to production.

`TenantMiddleware` takes `IStaffRepository` as an `InvokeAsync` parameter (not constructor-injected) since constructor injection of a scoped service into middleware creates a singleton-scoped instance.

**Not yet implemented:** role-based authorization. `Staff.Role` (`StaffRole.Owner`/`StaffRole.Receptionist`) is not injected into the `ClaimsPrincipal`, and no controller/action has `[Authorize(Roles = ...)]` — every endpoint is currently reachable by any authenticated (or dev-bypassed) caller. See `PROGRESS.txt` for the planned approach and per-controller access matrix.

### Domain entities

Six entities: `Clinic` (tenant root, no `ClinicId` of its own), `Staff`, `Customer`, `Treatment`, `Appointment`, `AppointmentTreatment` (join entity bridging Appointment↔Treatment for multi-treatment appointments). All use factory methods (`Customer.Create(...)`, `Treatment.Create(...)`) rather than public constructors, and mutation methods (`Update(...)`, `Deactivate()`, `Cancel()`, `Complete()`, `MarkNoShow()`) rather than public setters. `BaseEntity` provides `Id` (Guid), `CreatedAt`, and `UpdatedAt`.

Note: the `ClinicFlow.Application.Features.Staff` namespace shadows the `Staff` entity class — inside that namespace, refer to the entity as `global::ClinicFlow.Domain.Entities.Staff`.

### Error handling

Controllers return raw results — error translation is handled entirely by `ExceptionHandlingMiddleware`:

| Exception | HTTP status |
|---|---|
| `ValidationException` (FluentValidation) | 400 |
| `NotFoundException` | 404 |
| `ConflictException` | 409 |
| `UnauthorizedAccessException` | 401 |
| anything else | 500 |

Throw the appropriate custom exception from handlers; never return error HTTP codes from handlers or controllers.

### Dependency injection entry points

- `services.AddApplication()` — MediatR, FluentValidation, ValidationBehavior
- `services.AddInfrastructure(config)` — DbContext, repositories, UnitOfWork
- `Program.cs` — Azure AD JWT Bearer auth, middleware pipeline, `ICurrentClinicService`

### Database

SQL Server via EF Core 10. Local connection: `Server=(localdb)\mssqllocaldb;Database=ClinicFlowDb`. Entity type configurations live in `ClinicFlow.Infrastructure/Persistence/Configurations/`. All entities use composite indexes on `(ClinicId, <lookup field>)`.

### Authentication

Azure AD / Microsoft Identity Web with JWT Bearer. Token claims are parsed in `CurrentClinicService` (`preferred_username` or `email` claim). **`appsettings.json` still has placeholder `AzureAd` values** (`TenantId`/`ClientId` are dummy GUIDs) — real Entra ID app registration has not been done yet. Use the dev bypass (`DevClinicId` in `appsettings.Development.json`) for local testing until that's configured; see `PROGRESS.txt` for the full setup walkthrough.

### EF Core gotcha: collection navigation updates

Never call `context.Update(entity)` on an entity with a collection navigation (e.g. `Appointment.Treatments`) — it traverses the full graph and marks new children as `Modified`, which tries to `UPDATE` non-existent rows and throws a concurrency exception. Use explicit `RemoveRange` + `AddRange` on the child `DbSet` instead (see `AppointmentRepository.ReplaceTreatments`).
