# Миграция базы данных для модуля конференции

## Создание миграции

Выполните следующие команды в каталоге проекта `RESTful.Infrastructure`:

```bash
# Добавить миграцию
dotnet ef migrations add InitialConferenceModule --startup-project ..\RESTful.API\RESTful.API.csproj

# Применить миграцию к базе данных
dotnet ef database update --startup-project ..\RESTful.API\RESTful.API.csproj
```

## Структура базы данных

### Таблица Participants (Участники)
- **Id** (int, PK) - Идентификатор участника
- **FirstName** (nvarchar(100)) - Имя
- **LastName** (nvarchar(100)) - Фамилия
- **MiddleName** (nvarchar(100)) - Отчество
- **WorkPlace** (nvarchar(200)) - Место работы
- **AcademicDegree** (nvarchar(100), nullable) - Ученая степень
- **AcademicTitle** (nvarchar(100), nullable) - Ученое звание
- **Email** (nvarchar(100), unique) - Email
- **Phone** (nvarchar(20)) - Телефон
- **Role** (int) - Роль (1=Слушатель, 2=Докладчик)
- **RegistrationDate** (datetime2) - Дата регистрации

### Таблица Presentations (Доклады)
- **Id** (int, PK) - Идентификатор доклада
- **Title** (nvarchar(300)) - Название доклада
- **Abstract** (nvarchar(2000), nullable) - Аннотация
- **MainSpeakerId** (int, FK) - ID основного докладчика
- **CreatedDate** (datetime2) - Дата создания

### Таблица CoSpeakers (Содокладчики)
- **Id** (int, PK) - Идентификатор записи
- **PresentationId** (int, FK) - ID доклада
- **ParticipantId** (int, FK) - ID участника-содокладчика
- **CanEdit** (bit) - Права на редактирование
- **AddedDate** (datetime2) - Дата добавления

### Связи
- Participant ? Presentations (1:N) - Один участник может быть основным докладчиком нескольких докладов
- Presentation ? CoSpeakers (1:N) - У одного доклада может быть несколько содокладчиков
- Participant ? CoSpeakers (1:N) - Один участник может быть содокладчиком в нескольких докладах

### Индексы
- UNIQUE на Participants.Email
- UNIQUE на CoSpeakers (PresentationId, ParticipantId) - предотвращает дублирование содокладчиков
