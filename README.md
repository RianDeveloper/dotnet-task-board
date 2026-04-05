# TaskBoard

REST API для доски задач: проекты и задачи со статусами, пагинация и фильтрация. Слои: Domain, Application, Infrastructure, Api.

## Стек

- .NET 8, ASP.NET Core Minimal APIs
- MediatR (CQRS), FluentValidation
- EF Core 8, SQLite
- xUnit, FluentAssertions, `WebApplicationFactory`
- Swagger в среде Development

## Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Запуск локально

```bash
dotnet restore
dotnet build
```

API (по умолчанию см. `launchSettings.json`):

```bash
dotnet run --project src/SharpPet.Api
```

В Development доступен Swagger UI (например `http://localhost:5088/swagger`).

## Конфигурация

| Ключ | Назначение |
|------|------------|
| `ConnectionStrings:Database` | Строка подключения SQLite; если не задана, в коде используется `Data Source=app.db` |
| `ConnectionStrings__Database` | То же через переменную окружения (двойное подчёркивание) |
| User Secrets (`ConnectionStrings:Database`) | Локальная строка без коммита: в каталоге `src/SharpPet.Api` — `dotnet user-secrets set "ConnectionStrings:Database" "..."` |
| `appsettings.Development.local.json` | Опциональные переопределения в Development (файл в `.gitignore`) |

В репозитории в `appsettings*.json` нет секретов и продакшен-строк подключения.

## HTTP API

Префикс `/api`. Статусы задач в JSON — строки: `Todo`, `InProgress`, `Done`.

| Метод | Путь | Описание |
|--------|------|----------|
| `GET` | `/health` | Проверка доступности |
| `GET` | `/api/projects` | Список проектов (`page`, `pageSize`, по умолчанию 1 и 20) |
| `POST` | `/api/projects` | Тело: `{ "name": "..." }`. Ответ: `201` и `{ "id" }` |
| `GET` | `/api/projects/{projectId}` | Проект по id |
| `DELETE` | `/api/projects/{projectId}` | Удаление проекта (задачи каскадно) |
| `POST` | `/api/projects/{projectId}/tasks` | Создание задачи: `title`, опционально `description`, `dueDate`, `status` |
| `GET` | `/api/projects/{projectId}/tasks` | Список задач (`page`, `pageSize`, опционально `status`) |
| `PATCH` | `/api/tasks/{taskId}/status` | Тело: `{ "status": "Done" }` |
| `DELETE` | `/api/tasks/{taskId}` | Удаление задачи |

Ошибки: `404` для отсутствующих сущностей, `400` с полем `errors` при ошибках валидации, `500` для прочих сбоев. При старте (кроме окружения `Testing`) применяются миграции EF Core.

## Тесты

```bash
dotnet test
```
