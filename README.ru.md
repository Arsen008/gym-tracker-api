[English](README.md) | Русский

# SachkovTech

Бэкенд для приложения-трекера тренировок: управление упражнениями, видами упражнений и тренировками, медиафайлы хранятся в объектном хранилище. Учебный проект по курсу «.NET Fullstack», поэтому в нём намеренно используются явные подходы (Clean Architecture, паттерн Result, строго типизированные идентификаторы), а не сокращённые решения.

## Стек

- .NET 9, ASP.NET Core Web API, C#
- PostgreSQL 17 и Entity Framework Core 9 (Npgsql)
- Объектное хранилище MinIO (`Minio.AspNetCore`) для медиафайлов
- FluentValidation и SharpGrip AutoValidation
- CSharpFunctionalExtensions (паттерн `Result<T, Error>`)
- Логирование Serilog с приёмником Seq
- Swashbuckle / Swagger
- Docker Compose для локальной инфраструктуры
- Qodana для статического анализа

## Архитектура

Четыре проекта, у каждого своя зона ответственности:

- **SachkovTech.Domain** — агрегаты (`Exercise`, `ExerciseType`, `Workout` + `WorkoutExercise`), value-объекты, строго типизированные идентификаторы, общий тип `Error`.
- **SachkovTech.Application** — обработчики сценариев, DTO запросов/команд, валидаторы, интерфейсы репозиториев, абстракция `IFileProvider`.
- **SachkovTech.Infrastructure** — `ApplicationDbContext`, конфигурации EF, реализации репозиториев, миграции, `MinioProvider`, `SoftDeleteInterceptor`.
- **SachkovTech.API** — контроллеры, обёртка ответа, middleware обработки исключений, подключение Serilog и Swagger.

Направление зависимостей: **API → Application + Infrastructure**, **Infrastructure → Application + Domain**, **Application → Domain**. У Domain зависимостей на другие проекты нет.

В каждом слое есть статический `Inject.cs` с методом-расширением `Add…`, который вызывается из `Program.cs`. Обработчики сценариев — обычные классы, зарегистрированные как scoped-сервисы (без MediatR). Все обработчики возвращают `Result<T, Error>` / `UnitResult<Error>`; каждый HTTP-ответ оборачивается в `Envelope { Result, Errors, TimeGenerated }`.

## Запуск

Требования: .NET SDK 9 (зафиксирован в `global.json`) и Docker.

1. Поднять инфраструктуру (PostgreSQL на `5434`, Seq на `5341`, MinIO на `9000` / консоль `9001`):

   ```bash
   docker compose up -d
   ```

2. (Опционально) задать креды MinIO через user-secrets — иначе используются значения по умолчанию:

   ```bash
   dotnet user-secrets set "Minio:AccessKey" "minioadmin" --project SachkovTech.API
   dotnet user-secrets set "Minio:SecretKey" "minioadmin" --project SachkovTech.API
   ```

3. Запустить API (в окружении Development незапущенные миграции EF применяются автоматически при старте):

   ```bash
   dotnet run --project SachkovTech.API
   ```

4. Открыть Swagger UI: <https://localhost:7246/swagger> (или <http://localhost:5164/swagger>).

## Эндпоинты API

Маршруты обслуживаются по имени контроллера; ответы оборачиваются в стандартный envelope, если не указано иное.

| Метод | Маршрут | Описание |
|-------|---------|----------|
| POST | `/exercises` | Создать упражнение |
| PUT | `/exercises/{id}` | Обновить упражнение (название, группа мышц) |
| DELETE | `/exercises/{id}` | Мягко удалить упражнение |
| POST | `/exercises/{id}/media` | Загрузить медиафайл (`multipart/form-data`) |
| DELETE | `/exercises/{id}/media` | Удалить медиафайл |
| POST | `/workouts` | Создать тренировку |
| PUT | `/workouts/{id}` | Обновить тренировку (теги) |
| DELETE | `/workouts/{id}` | Мягко удалить тренировку |
| POST | `/file` | Список бакетов MinIO (диагностика) |
| GET | `/api/todoitems` | Список задач *(заготовка)* |
| GET | `/api/todoitems/{id}` | Задача по id *(заготовка)* |
| POST | `/api/todoitems` | Создать задачу *(заготовка)* |
| PUT | `/api/todoitems/{id}` | Обновить название задачи *(заготовка)* |
| DELETE | `/api/todoitems/{id}` | Удалить задачу *(заготовка)* |
