English | [Русский](README.ru.md)

# SachkovTech

Backend for a workout-tracking application: manage exercises, exercise types and workouts, with media files stored in object storage. This is a learning project built for the ".NET Fullstack" course, so it favours explicit patterns (Clean Architecture, the Result pattern, strongly-typed IDs) over shortcuts.

## Tech stack

- .NET 9, ASP.NET Core Web API, C#
- PostgreSQL 17 with Entity Framework Core 9 (Npgsql)
- MinIO object storage (`Minio.AspNetCore`) for media files
- FluentValidation with SharpGrip AutoValidation
- CSharpFunctionalExtensions (`Result<T, Error>` pattern)
- Serilog logging with a Seq sink
- Swashbuckle / Swagger
- Docker Compose for local infrastructure
- Qodana for static analysis

## Architecture

Four projects, one responsibility each:

- **SachkovTech.Domain** — aggregates (`Exercise`, `ExerciseType`, `Workout` + `WorkoutExercise`), value objects, strongly-typed IDs, the shared `Error` type.
- **SachkovTech.Application** — use-case handlers, request/command DTOs, validators, repository interfaces, the `IFileProvider` abstraction.
- **SachkovTech.Infrastructure** — `ApplicationDbContext`, EF configurations, repository implementations, migrations, `MinioProvider`, `SoftDeleteInterceptor`.
- **SachkovTech.API** — controllers, the response envelope, exception middleware, Serilog and Swagger wiring.

Dependency direction: **API → Application + Infrastructure**, **Infrastructure → Application + Domain**, **Application → Domain**. Domain has no project dependencies.

Each layer exposes a static `Inject.cs` with an `Add…` extension method that is called from `Program.cs`. Use-case handlers are plain classes registered as scoped services (no MediatR). All handlers return `Result<T, Error>` / `UnitResult<Error>`; every HTTP response is wrapped in an `Envelope { Result, Errors, TimeGenerated }`.

## Getting Started

Prerequisites: .NET SDK 9 (pinned in `global.json`) and Docker.

1. Start infrastructure (PostgreSQL on `5434`, Seq on `5341`, MinIO on `9000` / console `9001`):

   ```bash
   docker compose up -d
   ```

2. (Optional) provide MinIO credentials via user-secrets — otherwise the MinIO defaults are used:

   ```bash
   dotnet user-secrets set "Minio:AccessKey" "minioadmin" --project SachkovTech.API
   dotnet user-secrets set "Minio:SecretKey" "minioadmin" --project SachkovTech.API
   ```

3. Run the API (in Development, pending EF migrations are applied automatically on startup):

   ```bash
   dotnet run --project SachkovTech.API
   ```

4. Open Swagger UI: <https://localhost:7246/swagger> (or <http://localhost:5164/swagger>).

## API endpoints

Routes are served under the controller name; responses are wrapped in the standard envelope unless noted.

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/exercises` | Create an exercise |
| PUT | `/exercises/{id}` | Update an exercise (name, muscle group) |
| DELETE | `/exercises/{id}` | Soft-delete an exercise |
| POST | `/exercises/{id}/media` | Upload a media file (`multipart/form-data`) |
| DELETE | `/exercises/{id}/media` | Remove the media file |
| POST | `/workouts` | Create a workout |
| PUT | `/workouts/{id}` | Update a workout (tags) |
| DELETE | `/workouts/{id}` | Soft-delete a workout |
| POST | `/file` | List MinIO buckets (diagnostic) |
| GET | `/api/todoitems` | List to-do items *(scaffold)* |
| GET | `/api/todoitems/{id}` | Get a to-do item by id *(scaffold)* |
| POST | `/api/todoitems` | Create a to-do item *(scaffold)* |
| PUT | `/api/todoitems/{id}` | Update a to-do item title *(scaffold)* |
| DELETE | `/api/todoitems/{id}` | Delete a to-do item *(scaffold)* |
