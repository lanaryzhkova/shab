# Модуль регистрации на конференцию

## Описание

Модуль предоставляет функциональность для регистрации участников на конференцию в двух ролях:
- **Слушатель** - участник конференции без доклада
- **Докладчик** - участник с докладом

## Основные возможности

### 1. Регистрация участников
- Регистрация с полной информацией (ФИО, место работы, степень, звание, контакты)
- Выбор роли: слушатель или докладчик
- Для докладчиков - информация о докладе (название, аннотация)
- Возможность добавления содокладчиков

### 2. Управление докладами
- Основной докладчик имеет полный доступ к докладу
- Содокладчики могут просматривать информацию о докладе
- Возможность настройки прав доступа для содокладчиков (CanEdit)

### 3. Просмотр информации
- Список всех участников с фильтрацией по роли
- Детальная информация об участнике
- Список докладов конференции
- Просмотр докладов с учетом прав доступа

## Архитектура проекта

### Проекты решения

1. **RESTful.Domain** - Доменные модели и интерфейсы
   - `Entities/` - Сущности БД (Participant, Presentation, CoSpeaker)
   - `DTOs/` - DTO для передачи данных
   - `Interfaces/` - Интерфейсы репозиториев

2. **RESTful.Infrastructure** - Инфраструктурный слой
   - `Data/Context.cs` - Контекст Entity Framework
   - `Repositories/` - Реализация репозиториев

3. **RESTful.API** - RESTful API
   - `Controllers/ParticipantsController.cs` - API участников
   - `Controllers/PresentationsController.cs` - API докладов

4. **Frontend** - Blazor интерфейс
   - `Services/ConferenceService.cs` - Сервис для работы с API
   - `Components/Pages/` - Страницы приложения

## API Endpoints

### Участники

#### POST /api/participants/register
Регистрация нового участника

**Request Body:**
```json
{
  "firstName": "Иван",
  "lastName": "Иванов",
  "middleName": "Иванович",
  "workPlace": "МГУ",
  "academicDegree": "к.т.н.",
  "academicTitle": "доцент",
  "email": "ivanov@example.com",
  "phone": "+79991234567",
  "role": 2,
  "presentation": {
    "title": "Исследование методов машинного обучения",
    "abstract": "Доклад посвящен...",
"coSpeakerIds": [1, 2]
  }
}
```

**Response:**
```json
{
  "id": 1,
  "firstName": "Иван",
  "lastName": "Иванов",
  "middleName": "Иванович",
  "fullName": "Иванов Иван Иванович",
  "workPlace": "МГУ",
  "academicDegree": "к.т.н.",
  "academicTitle": "доцент",
  "email": "ivanov@example.com",
  "phone": "+79991234567",
  "role": 2,
  "registrationDate": "2024-01-15T10:30:00Z",
  "presentations": [...]
}
```

#### GET /api/participants
Получить всех участников

#### GET /api/participants/{id}
Получить участника по ID

#### GET /api/participants/{id}/presentations
Получить доклады участника (как основного докладчика и содокладчика)

### Доклады

#### GET /api/presentations
Получить все доклады

#### GET /api/presentations/{id}?participantId={participantId}
Получить доклад по ID с учетом прав доступа для указанного участника

## Blazor страницы

### /conference/register
Страница регистрации на конференцию
- Форма с валидацией
- Динамическое отображение полей для докладчиков
- Выбор содокладчиков из уже зарегистрированных участников

### /conference/participants
Список всех участников
- Таблица с информацией
- Фильтрация по роли
- Ссылки на детальную информацию

### /conference/participants/{id}
Детальная информация об участнике
- Полная информация об участнике
- Список докладов (как основного докладчика и содокладчика)
- Индикация прав доступа к докладам

### /conference/presentations
Список всех докладов конференции
- Карточки докладов
- Информация о докладчиках

## Настройка проекта

### 1. Настройка базы данных

Откройте `RESTful.API\appsettings.json` и настройте строку подключения:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ConferenceDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 2. Создание миграции

Выполните команды из терминала в каталоге `RESTful.Infrastructure`:

```bash
dotnet ef migrations add InitialConferenceModule --startup-project ..\RESTful.API\RESTful.API.csproj
dotnet ef database update --startup-project ..\RESTful.API\RESTful.API.csproj
```

### 3. Настройка URL API

В файле `Frontend\appsettings.json` укажите URL вашего API:

```json
{
  "ApiBaseUrl": "https://localhost:7001/"
}
```

### 4. Настройка CORS

В файле `RESTful.API\Program.cs` настройте CORS для вашего Blazor приложения. По умолчанию разрешены порты 7149 и 5149.

### 5. Запуск проектов

Запустите оба проекта:
- **RESTful.API** - API сервер (обычно https://localhost:7001)
- **Frontend** - Blazor приложение (обычно https://localhost:7149)

## Доменные модели

### Participant (Участник)
```csharp
public class Participant
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string WorkPlace { get; set; }
    public string? AcademicDegree { get; set; }
    public string? AcademicTitle { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public ParticipantRole Role { get; set; }
    public DateTime RegistrationDate { get; set; }
}

public enum ParticipantRole
{
    Listener = 1,
    Speaker = 2
}
```

### Presentation (Доклад)
```csharp
public class Presentation
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Abstract { get; set; }
    public int MainSpeakerId { get; set; }
    public DateTime CreatedDate { get; set; }
    public Participant MainSpeaker { get; set; }
  public ICollection<CoSpeaker> CoSpeakers { get; set; }
}
```

### CoSpeaker (Содокладчик)
```csharp
public class CoSpeaker
{
    public int Id { get; set; }
    public int PresentationId { get; set; }
    public int ParticipantId { get; set; }
    public bool CanEdit { get; set; }
    public DateTime AddedDate { get; set; }
    public Presentation Presentation { get; set; }
    public Participant Participant { get; set; }
}
```

## Права доступа к докладам

### Основной докладчик
- Полный доступ к докладу
- Может редактировать всю информацию

### Содокладчики
- Режим чтения по умолчанию (CanEdit = false)
- Могут просматривать название, аннотацию, список участников
- Если CanEdit = true - могут редактировать доклад

### Посторонние участники
- Не имеют доступа к детальной информации о докладе
- Могут видеть доклад в общем списке

## Валидация

Все формы имеют валидацию на стороне клиента и сервера:
- Обязательные поля отмечены (*)
- Email проверяется на корректность формата
- Телефон проверяется на корректность формата
- Уникальность email в базе данных

## Особенности реализации

1. **Режим просмотра для содокладчиков** - содокладчики могут только просматривать доклад, если не установлен флаг CanEdit
2. **Уникальность email** - один email = один участник
3. **Каскадное удаление** - при удалении доклада удаляются все связи с содокладчиками
4. **Защита от дублирования** - один участник не может быть добавлен содокладчиком дважды к одному докладу

## Возможные расширения

1. Добавление аутентификации и авторизации
2. Редактирование информации об участнике
3. Редактирование докладов основным докладчиком
4. Управление правами содокладчиков
5. Загрузка файлов (презентации, материалы)
6. Расписание конференции
7. Регистрация на секции
8. Email-уведомления
9. Экспорт списков участников и докладов
10. Поиск по участникам и докладам

## Технологии

- .NET 8
- ASP.NET Core Web API
- Blazor Server
- Entity Framework Core 9
- SQL Server (LocalDB)
- Bootstrap 5

## Структура файлов

```
WebApplication1/
??? RESTful.Domain/
?   ??? Entities/
?   ?   ??? Participant.cs
?   ?   ??? Presentation.cs
?   ?   ??? CoSpeaker.cs
?   ??? DTOs/
?   ?   ??? ConferenceDto.cs
?   ??? Interfaces/
?       ??? IRepositories.cs
??? RESTful.Infrastructure/
?   ??? Data/
?   ?   ??? Context.cs
???? Repositories/
?       ??? ParticipantRepository.cs
?    ??? PresentationRepository.cs
?       ??? CoSpeakerRepository.cs
??? RESTful.API/
?   ??? Controllers/
?   ?   ??? ParticipantsController.cs
?   ?   ??? PresentationsController.cs
?   ??? Program.cs
?   ??? appsettings.json
??? Frontend/
??? Components/
    ?   ??? Pages/
    ?   ?   ??? ConferenceRegistration.razor
    ?   ?   ??? ParticipantsList.razor
    ?   ?   ??? ParticipantDetail.razor
    ?   ?   ??? PresentationsList.razor
    ?   ??? Layout/
    ? ??? NavMenu.razor
 ??? Services/
    ?   ??? ConferenceService.cs
    ??? Program.cs
    ??? appsettings.json
```
