## Pet-проект: REST API «доски задач» на .NET 8 с разделением на слои (Clean Architecture), CQRS (MediatR), валидацией (FluentValidation), EF Core и SQLite.

## Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Сборка и тесты

```bash
dotnet restore SharpPet.sln
dotnet build SharpPet.sln
dotnet test SharpPet.sln
```

## Запуск API

```bash
dotnet run --project src/SharpPet.Api/SharpPet.Api.csproj
```

В режиме Development доступен Swagger UI: по умолчанию [http://localhost:5088/swagger](http://localhost:5088/swagger) (см. `src/SharpPet.Api/Properties/launchSettings.json`).

Если строка подключения к БД нигде не задана, используется значение по умолчанию в коде: файл `app.db` в рабочей директории процесса.

## Конфигурация

В репозитории **нет** строк подключения к продакшен-БД и других секретов: только общие настройки логирования и хоста (`appsettings*.json`).

**Строка подключения SQLite** задаётся через конфигурацию (приоритет выше — ниже по списку):

1. Переменные окружения, в т.ч. `ConnectionStrings__Database` (двойное подчёркивание для вложенного ключа).
2. [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) для проекта API (удобно в Development): в каталоге `src/SharpPet.Api` выполните, например:

```bash
dotnet user-secrets set "ConnectionStrings:Database" "Data Source=app.db"
```

Список секретов: `dotnet user-secrets list`.

3. Локальные переопределения без коммита: файл `appsettings.Development.local.json` (игнорируется Git, см. `.gitignore`), формат как у обычного `appsettings`.

Для продакшена задайте `ConnectionStrings__Database` (или секреты в панели хостинга) и не храните реальные значения в Git.

## HTTP API

Базовый префикс: `/api`. Перечисления в JSON передаются строками (`Todo`, `InProgress`, `Done`).

### Проекты

| Метод | Путь | Описание |
|--------|------|----------|
| `GET` | `/api/projects` | Список проектов с пагинацией (`page`, `pageSize`, по умолчанию 1 и 20) |
| `POST` | `/api/projects` | Создание проекта (`{ "name": "..." }`) |
| `GET` | `/api/projects/{projectId}` | Проект по идентификатору |
| `DELETE` | `/api/projects/{projectId}` | Удаление проекта (задачи каскадно) |

### Задачи

| Метод | Путь | Описание |
|--------|------|----------|
| `POST` | `/api/projects/{projectId}/tasks` | Создание задачи (`title`, опционально `description`, `dueDate`, `status`) |
| `GET` | `/api/projects/{projectId}/tasks` | Список задач проекта (`page`, `pageSize`, опционально `status`) |
| `PATCH` | `/api/tasks/{taskId}/status` | Смена статуса (`{ "status": "Done" }`) |
| `DELETE` | `/api/tasks/{taskId}` | Удаление задачи |

### Служебное

| Метод | Путь | Описание |
|--------|------|----------|
| `GET` | `/health` | Проверка доступности сервиса |

