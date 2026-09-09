# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

Prerequisites: .NET SDK 9 (pinned via `global.json`, `rollForward: latestMinor`). Start backing services first: `docker compose up -d` (PostgreSQL 17 on host port 5434, Seq on 5341, MinIO on 9000/console 9001).

- Build: `dotnet build SachkovTech.sln`
- Run API: `dotnet run --project SachkovTech.API` (http `http://localhost:5164`, https `https://localhost:7246`, Swagger at `/swagger`, Development env by default)
- Tests: no test project exists yet. To add one: `dotnet new xunit -o SachkovTech.<Name>.Tests && dotnet sln add ...`. Run with `dotnet test`, a single test with `dotnet test --filter "FullyQualifiedName~<Namespace.Class.Method>"`.
- Static analysis: Qodana (`qodana.yaml`, profile `qodana.starter`, `QDNET`) — `qodana scan` if the CLI is installed; otherwise runs in CI.

### EF Core migrations

Migrations live in `SachkovTech.Infrastructure/Migrations`; startup project is the API. In Development, pending migrations are applied automatically on startup (`app.ApplyMigration()` in `Program.cs`, defined in `SachkovTech.API/AppExtensions.cs`).

- Add: `dotnet ef migrations add <Name> --project SachkovTech.Infrastructure --startup-project SachkovTech.API`
- Apply manually: `dotnet ef database update --project SachkovTech.Infrastructure --startup-project SachkovTech.API`

## Architecture

Four-project Clean Architecture solution. Dependency direction: **API → Application + Infrastructure**, **Infrastructure → Application + Domain**, **Application → Domain**, **Domain → (only CSharpFunctionalExtensions + EF Core)**.

- **SachkovTech.Domain** — aggregates (`Exercise`, `ExerciseType`, `Workout` + `WorkoutExercise` child), value objects, strongly-typed IDs, shared `Error` type.
- **SachkovTech.Application** — use-case handlers, request/command DTOs, FluentValidation validators, repository *interfaces*, `IFileProvider` abstraction.
- **SachkovTech.Infrastructure** — `ApplicationDbContext`, EF `IEntityTypeConfiguration` classes, repository implementations, `MinioProvider`, `SoftDeleteInterceptor`.
- **SachkovTech.API** — controllers, response envelope, exception middleware, Serilog + Swagger wiring.

### Composition / DI

Each layer exposes a static `Inject.cs` with an extension method (`AddApplication()`, `AddInfrastructure(configuration)`) called from `Program.cs`. **No MediatR** — use-case handlers are plain classes registered as `AddScoped<THandler>()` and injected into controller actions via `[FromServices]` alongside `IValidator<T>`.

### Result / Error pattern (pervasive)

- Everything returns `Result<T, Error>` or `UnitResult<Error>` (CSharpFunctionalExtensions). Exceptions are not used for control flow.
- `Error` (`Domain/Shared/Error.cs`) is a record with `Code`, `Message`, and `ErrorType` (`Validation | NotFound | Failure | Conflict`). Use the `Errors.General.*` factories for common cases.
- `Error.Serialize()` encodes as `Code||Message||Type`. FluentValidation failures carry this serialized string; `CustomResultFactory` (API/Validation) and `ResponseExtensions.ToResponse()` deserialize it and map `ErrorType` → HTTP status (Validation→400, NotFound→404, Conflict→409, Failure→500).
- Domain factories `Create(...)` return `Result`; value objects and aggregates have a private parameterless ctor for EF plus a static `Create`.

### API response contract

All responses are wrapped in `Envelope { Result, Errors[], TimeGenerated }`. Controllers derive from `ApplicationController` (base `[ApiController]`, route `[controller]`, overrides `Ok()` to wrap). Return via the `.ToResponse()` extensions on `Result<T,Error>` / `UnitResult<Error>` / `Error` / `ValidationResult`. Unhandled exceptions are caught by `ExceptionMiddleware` (`UseCustomExceptionHandler`) → 500 envelope.

### Use-case folder convention

`Application/<Aggregate>/<UseCase>/` contains `<UseCase>Handler`, `<UseCase>Request` or `<UseCase>Command`, `<UseCase>RequestValidator`, and sometimes a `Dto`. Follow this layout when adding a feature, and register the handler in `Application/Inject.cs`.

### Validation

FluentValidation. `AddFluentValidationAutoValidation` auto-validates request bodies at the MVC layer through `CustomResultFactory`; some controllers/handlers also run `IValidator<T>` manually. Custom helpers in `Application/Validation/CustomValidators.cs`: `MustBeValueObject(factoryMethod)` and `WithError(Error)`.

### Persistence

- EF mappings are `IEntityTypeConfiguration<T>` classes in `Infrastructure/Configurations/`, auto-applied via `ApplyConfigurationsFromAssembly`. snake_case tables/columns. Strongly-typed IDs and value objects are mapped with `ValueConverter`.
- Repository interfaces live in **Application** (`IExercisesRepository`, `IWorkoutsRepository`, `IExerciseTypesRepository`); implementations in `Infrastructure/Repositories/` and call `SaveChangesAsync` themselves (no separate Unit of Work). `GetBy*` methods return `Result<T, Error>`.

### Soft delete

`ISoftDeletable` (`Delete`/`Restore`) is implemented by `Exercise` and `Workout` via a private `_isDeleted` field. `SoftDeleteInterceptor` (SaveChanges interceptor, singleton, wired in `ApplicationDbContext.OnConfiguring`) rewrites `EntityState.Deleted` → `Modified` and calls `Delete()`. Each configuration adds `HasQueryFilter(e => !EF.Property<bool>(e, "_isDeleted"))` so soft-deleted rows are hidden by default. Note the two repos differ: `ExercisesRepository.DeleteAsync` uses `DbSet.Remove` (relies on the interceptor); `WorkoutsRepository.DeleteAsync` calls `workout.Delete()` directly.

### File storage

MinIO via `Minio.AspNetCore`. Abstraction `IFileProvider` (`Application/FileProvider/`, namespace `SachkovTech.Application.Providers`) with `MinioProvider` impl; payload is `FileData(Stream, BucketName, ObjectName)` (namespace `SachkovTech.Application.Models`). Exercise media goes to bucket `exercise-media` (auto-created on first upload); presigned URLs expire after 24h.

### Logging

Serilog → Console + Debug + Seq (Seq URL from the `Seq` config key). `UseSerilogRequestLogging` for request logs. Note: several handler log messages are written in Russian.

## Configuration

- `ConnectionStrings:Database` — PostgreSQL. `appsettings.json` default targets `localhost:5434` (matches the docker-compose port mapping).
- `Minio` section: `Endpoint`, `AccessKey`, `SecretKey`, `WithSsl`. The API project has a `UserSecretsId`; prefer user-secrets for real credentials.
- `Seq` — Seq ingestion URL (`http://localhost:5341` locally).

## Known inconsistencies (don't mimic; fix if touching)

- `ExerciseType.Create` returns `Result<ExerciseType>` with plain-string (Russian) errors instead of the standard `Result<T, Error>`.
- Strongly-typed IDs are split across namespaces: `ExerciseId` / `WorkoutId` are in `SachkovTech.Domain`, while `ExerciseTypeId` / `WorkoutExerciseId` are in `SachkovTech.Domain.Shared.Ids`.
- `IExerciseTypesRepository` is registered twice (in `Program.cs` and `Infrastructure/Inject.cs`).
- `MinioOptions.WithSsl` has no key in `appsettings.json` (falls back to the `false` default). Config binding is case-insensitive, so key casing in the `Minio` section is cosmetic, not a binding bug.
- The repo currently has **no commits** — everything is staged/untracked on `master` (default branch for PRs is `main`).

## Working with me

I'm learning this stack and write the code myself. Don't edit files unless I explicitly ask.

Default behavior:
- Point out where the problem is (file, line, what's wrong)
- Give a hint that leads me to the answer, not the finished code
- Explain the architectural reasoning behind a suggestion
- Show complete code only when I say "show me the code"

Answer in Russian.
